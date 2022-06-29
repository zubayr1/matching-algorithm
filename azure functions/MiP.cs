using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlexchainFunction
{
    public class MiP
    {
        List<FlexibilityRequest> flexRequest;
        List<FlexibilityOffer> flexOffer;

        public MiP(List<FlexibilityRequest> flexRequest, List<FlexibilityOffer> flexOffer)
        {
            this.flexRequest = flexRequest;
            this.flexOffer = flexOffer;
        }


        public Dictionary<string, int> Matching(List<string> userlist, FlexibilityRequest flexibilityRequest, int TOTALFLEXREQUESTED)
        {
            var accepted_offers = new Dictionary<string, int>();
            var POTENTIALOFFER = new Dictionary<string, int>();

            var BIDS = new Dictionary<string, string>();

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
                                    BIDS[user] = uservalue.BidPriceCtpEUList;
                                    
                                }
                            }
                        }
                    }
                }
            }

            foreach(var username in BIDS.Keys)
            {
                var COUNT = 0;

                var userbids = BIDS[username].Split(", ");

                foreach (var userbid in userbids)
                {
                    if (Int32.Parse(userbid) <= Int32.Parse(flexibilityRequest.MaxPriceCtpEU))
                    {
                        POTENTIALOFFER[username + " " + (COUNT).ToString()] = Int32.Parse(userbid);

                        COUNT+=1;
                    }
                }

            }

            var sortedDict = POTENTIALOFFER.OrderBy(x => x.Value).ToDictionary(x => x.Key, x => x.Value);


            foreach (var key in sortedDict.Keys)
            {
                var name = key.Split(' ')[0];

                if(TOTALFLEXREQUESTED>1)
                {
                    TOTALFLEXREQUESTED--;

                    if (accepted_offers.ContainsKey(name))
                    {
                        accepted_offers[name] += POTENTIALOFFER[key];
                    }
                    else
                    {
                        accepted_offers[name] = POTENTIALOFFER[key];
                    }
                }
                else
                {
                    if (accepted_offers.ContainsKey(name))
                    {
                        accepted_offers[name] += POTENTIALOFFER[key];
                    }
                    else
                    {
                        accepted_offers[name] = POTENTIALOFFER[key];
                    }

                    return accepted_offers;
                }
                
            }
            return accepted_offers;

        }


        public bool CheckFulfillmentFactor(Dictionary<string, int> accepted_offers, int fullfillmentFactor, int TOTALFLEXREQUESTED)
        {
            var val = 0;
            var SUM = 0;

            foreach (KeyValuePair<string, int> pair in accepted_offers)
            {
                val += pair.Value;

                SUM+=pair.Value.length();
            }

            if ((int)(val / Math.Abs(TOTALFLEXREQUESTED) * 100) >= fullfillmentFactor)
            {
                return true;
            }
            return false;
        }


        public Flex_MatchingAlgo MiPCalculation()
        {
            var accepted_offers = new Dictionary<string, int>();
            var final_accepted_offers = new Dictionary<string, Dictionary<string, int>>();

            for (int i = 0; i < flexRequest.Count; i++)
            {
                FlexibilityRequest flexibilityRequest = flexRequest[i];

                if (flexibilityRequest.MarketType == "auction")
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
