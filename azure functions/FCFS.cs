using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlexchainFunction
{
    public class FCFS
    {
        List<FlexibilityRequest> flexRequest;
        List<FlexibilityOffer> flexOffer;

        public FCFS(List<FlexibilityRequest> flexRequest, List<FlexibilityOffer> flexOffer)
        {
            this.flexRequest = flexRequest;
            this.flexOffer = flexOffer;
        }


        public Dictionary<string, int> Matching(List<string> userlist, FlexibilityRequest flexibilityRequest, int TOTALFLEXREQUESTED)
        {
            var accepted_offers = new Dictionary<string, int>();

            var SIGN = 1;

            if (TOTALFLEXREQUESTED<0)
            {
                SIGN = -1;
                TOTALFLEXREQUESTED = Math.Abs(TOTALFLEXREQUESTED);
            }

            int count = 0;

            foreach (var user in userlist)
            {
                count++;

                for (int j = 0; j < flexOffer.Count; j++)
                {
                    FlexibilityOffer flexibilityOffer = flexOffer[j];

                    if(flexibilityOffer.UserId==user)
                    {                        

                        foreach(var uservalue in flexibilityOffer.FlexOfferList)
                        {
                            if(uservalue.RequestID==flexibilityRequest.RequestID)
                            {
                                if((SIGN>0 && uservalue.TotalFlexOfferedEU > 0) || (SIGN<0 && uservalue.TotalFlexOfferedEU < 0))
                                {
                                    if(SIGN<0 && uservalue.TotalFlexOfferedEU < 0)
                                    {
                                        uservalue.TotalFlexOfferedEU = Math.Abs(uservalue.TotalFlexOfferedEU);
                                    }

                                    if(TOTALFLEXREQUESTED - uservalue.TotalFlexOfferedEU > 0)
                                    {
                                        TOTALFLEXREQUESTED-= uservalue.TotalFlexOfferedEU;

                                        accepted_offers[flexibilityOffer.UserId] = uservalue.TotalFlexOfferedEU;
                                    }
                                    else
                                    {
                                        accepted_offers[flexibilityOffer.UserId] = TOTALFLEXREQUESTED;

                                        return accepted_offers;
                                    }
                                }
                            }
                        }
                    }
                }
            }

            if(count==userlist.Count && TOTALFLEXREQUESTED>0)
            {
                return accepted_offers;
            }

            return accepted_offers;


        }


        public bool CheckFulfillmentFactor(Dictionary<string, int> accepted_offers, int fullfillmentFactor, int TOTALFLEXREQUESTED)
        {
            var val = 0;
            foreach(KeyValuePair<string, int> pair in accepted_offers)
            {
                val+=pair.Value;
            }

            if((int)(val/Math.Abs(TOTALFLEXREQUESTED)*100) >= fullfillmentFactor)
            {
                return true;
            }
            return false;
        }




        public Flex_MatchingAlgo fcfsCalculation()
        {
            var accepted_offers = new Dictionary<string, int>();
            var final_accepted_offers = new Dictionary<string, Dictionary<string, int>> ();


            for (int i = 0; i < flexRequest.Count; i++)
            {
                FlexibilityRequest flexibilityRequest = flexRequest[i];
                
                if (flexibilityRequest.MarketType== "fixedPrice")
                {
                    if (flexibilityRequest.IfFlexRequested == false)
                    {
                        var unreachable_accepted_offers = new Dictionary<string, int>();
                        unreachable_accepted_offers.Add("ifflexrequested false", 0);

                        final_accepted_offers[flexibilityRequest.RequestID] = unreachable_accepted_offers;
                    }
                    else
                    {
                        var TOTALFLEXREQUESTED = flexibilityRequest.TotalFlexRequestedEU;

                        List<string> userlist = new List<string>(flexibilityRequest.Loc.Keys);

                        accepted_offers = Matching(userlist, flexibilityRequest, Int32.Parse(TOTALFLEXREQUESTED));

                        if (CheckFulfillmentFactor(accepted_offers, Int32.Parse(flexibilityRequest.FullfillmentFactor), Int32.Parse(TOTALFLEXREQUESTED)))
                        {
                            final_accepted_offers[flexibilityRequest.RequestID] = accepted_offers;
                        }
                        else
                        {
                            var unreachable_accepted_offers = new Dictionary<string, int>();
                            unreachable_accepted_offers.Add("fullfillment factor did not reach", 0);
                            final_accepted_offers[flexibilityRequest.RequestID] = unreachable_accepted_offers;
                        }

                    }


                }
            }
            Flex_MatchingAlgo flex_MatchingAlgo = new Flex_MatchingAlgo(final_accepted_offers);

            return flex_MatchingAlgo;
        }
    }
}
