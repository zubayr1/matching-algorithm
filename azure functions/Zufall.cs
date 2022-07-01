using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlexchainFunction
{

    

    public class Zufall
    {
        List<FlexibilityRequest> flexRequest;
        List<FlexibilityOffer> flexOffer;



        public Zufall(List<FlexibilityRequest> flexRequest, List<FlexibilityOffer> flexOffer)
        {
            this.flexRequest = flexRequest;
            this.flexOffer = flexOffer;
        }


        public Dictionary<string, int> Matching(List<string> userlist, FlexibilityRequest flexibilityRequest, int TOTALFLEXREQUESTED)
        {
            var accepted_offers = new Dictionary<string, int>();

            var POTENTIALOFFER = new List<string>();

            var SIGN = 1;

            if (TOTALFLEXREQUESTED < 0)
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

                    if (flexibilityOffer.UserId == user)
                    {
                        foreach (var uservalue in flexibilityOffer.FlexOfferList)
                        {
                            if (uservalue.RequestID == flexibilityRequest.RequestID)
                            {
                                if ((SIGN > 0 && uservalue.totalFlexOfferedEU > 0) || (SIGN < 0 && uservalue.totalFlexOfferedEU < 0))
                                {
                                    if (SIGN < 0 && uservalue.totalFlexOfferedEU < 0)
                                    {
                                        uservalue.totalFlexOfferedEU = Math.Abs(uservalue.totalFlexOfferedEU);
                                    }

                                    var usertotalflexrequested = uservalue.totalFlexOfferedEU;

                                    while (usertotalflexrequested != 0)
                                    {
                                        POTENTIALOFFER.Add(flexibilityOffer.UserId);
                                        usertotalflexrequested--;
                                    }

                                }
                            }
                        }
                    }

                }
            }



            POTENTIALOFFER.Shuffle();

            if (POTENTIALOFFER.Count() > TOTALFLEXREQUESTED)
            {
                while (TOTALFLEXREQUESTED != 0)
                {
                    if (accepted_offers.ContainsKey(POTENTIALOFFER[0]))
                    {
                        accepted_offers[POTENTIALOFFER[0]] = accepted_offers[POTENTIALOFFER[0]] + 1;
                    }
                    else
                    {
                        accepted_offers[POTENTIALOFFER[0]] = 1;
                    }
                    TOTALFLEXREQUESTED--;
                    POTENTIALOFFER.RemoveAt(0);
                }
            }
            else
            {
                foreach (var offer in POTENTIALOFFER)
                {
                    if (accepted_offers.ContainsKey(offer))
                    {
                        accepted_offers[offer] = accepted_offers[offer] + 1;
                    }
                    else
                    {
                        accepted_offers[offer] = 1;
                    }
                }
            }


            return accepted_offers;


        }


        public bool CheckFulfillmentFactor(Dictionary<string, int> accepted_offers, int fullfillmentFactor, int TOTALFLEXREQUESTED)
        {
            var val = 0;
            foreach (KeyValuePair<string, int> pair in accepted_offers)
            {
                val += pair.Value;
            }

            if ((int)(val / Math.Abs(TOTALFLEXREQUESTED) * 100) >= fullfillmentFactor)
            {
                return true;
            }
            return false;
        }




        public Flex_MatchingAlgo ZufallCalculation()
        {
            var accepted_offers = new Dictionary<string, int>();
            var final_accepted_offers = new Dictionary<string, Dictionary<string, int>>();



            for (int i = 0; i < flexRequest.Count; i++)
            {
                FlexibilityRequest flexibilityRequest = flexRequest[i];

                if (flexibilityRequest.MarketType == "fixedPrice")
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
