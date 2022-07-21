// SPDX-License-Identifier: MIT
pragma solidity ^0.8.0;


contract CheckGas
{

    string[]  RESULT;
    int256[] flexofferedlist;
    uint256 flexofferedlistCount;


    struct Final_accepted_offer
    {
        string requestid;
        string[] result;
    }
    uint final_accepted_offerCount;
    mapping (uint => Final_accepted_offer) final_accepted_offers;
    
    //for flexrequest
    struct Flexrequest
    {
        string requestid;
        string mode;
        int256 fullfillmentfactor;
        string ifflexrequested;
        string[] loc;
        string markettype;
        string maxpricectpeu;
        string priceofferctpeu;
        string referencepricectpeu;
        string timeslot;
        int256 totalflexrequestedeu;
    }
    uint flexrequestCount;
    mapping (uint => Flexrequest) flexrequests;


    //for singler offer under a user
    struct Singleofferformat
    {
        string bidpricectpeulist;
        string endflexshifttimeout;
        string requestid;
        string startflexshifttimeout;
        int256 totalflexofferedeu;
    }

    Singleofferformat[] public singleofferformats;


    //for all offers from a user
    struct Flexoffer
    {
        string userid;
        Singleofferformat[] singleofferformats;
    }

    uint flexofferCount;
    mapping (uint => Flexoffer) flexoffers;




    //setup requests
    function setflexrequests(string memory requestid, string memory mode, int256 fullfillmentfactor,
    string memory ifflexrequested, string[] memory loc, string memory markettype, string memory maxpricectpeu,
    string memory priceofferctpeu, string memory referencepricectpeu, string memory timeslot, int256 totalflexrequestedeu) public
    {
        

        flexrequests[flexrequestCount].requestid = requestid;
        flexrequests[flexrequestCount].mode = mode;
        flexrequests[flexrequestCount].fullfillmentfactor = fullfillmentfactor;
        flexrequests[flexrequestCount].ifflexrequested = ifflexrequested;
        flexrequests[flexrequestCount].loc = loc;
        flexrequests[flexrequestCount].markettype = markettype;
        flexrequests[flexrequestCount].maxpricectpeu =maxpricectpeu;
        flexrequests[flexrequestCount].priceofferctpeu = priceofferctpeu;
        flexrequests[flexrequestCount].referencepricectpeu = referencepricectpeu;
        flexrequests[flexrequestCount].timeslot = timeslot;
        flexrequests[flexrequestCount].totalflexrequestedeu = totalflexrequestedeu;

        flexrequestCount++;
    
        

    }


    //setup singleroffer
    function setsingleuserflexoffer(string memory bidpricectpeulist, string memory endflexshifttimeout, string memory requestid,
    string memory startflexshifttimeout, int256 totalflexofferedeu) public
    {
        

        Singleofferformat memory singleofferformat = Singleofferformat(bidpricectpeulist, endflexshifttimeout,
        requestid, startflexshifttimeout, totalflexofferedeu);

        singleofferformats.push(singleofferformat);

    }


    //setup all ofers from a user
    function setoffer(string memory userid) public
    {

        flexoffers[flexofferCount].userid = userid;
        flexoffers[flexofferCount].singleofferformats = singleofferformats;

        flexofferCount++;

        for(uint i=0; i<singleofferformats.length; i++)
        {
            singleofferformats.pop();
        }
        

    }

    
    function matching(uint256 i, int256 TOTALFLEXREQUESTED) private returns(string[] memory)
    {
        

        int256 SIGN =1;

        

        uint256 resultCount;

        if(TOTALFLEXREQUESTED<0)
        {
            SIGN = -1;

            TOTALFLEXREQUESTED = -TOTALFLEXREQUESTED;
        }

        uint256 count =0;

        for(uint j=0; j<flexrequests[i].loc.length; j++)
        {
            count++;

            for(uint256 k=0; k<flexofferCount; k++)
            {
                if(keccak256(abi.encodePacked((flexoffers[k].userid))) == keccak256(abi.encodePacked((flexrequests[i].loc[j]))))
                {
                    for(uint256 s=0; s< flexoffers[k].singleofferformats.length; s++)
                    {
                        if(keccak256(abi.encodePacked((flexoffers[k].singleofferformats[s].requestid))) == keccak256(abi.encodePacked((flexrequests[i].requestid))))
                        {
                            if((SIGN>0 && (flexoffers[k].singleofferformats[s].totalflexofferedeu > 0)) || (SIGN<0 && (flexoffers[k].singleofferformats[s].totalflexofferedeu < 0)))
                            {
                                if(SIGN<0 && flexoffers[k].singleofferformats[s].totalflexofferedeu < 0)
                                {
                                    flexoffers[k].singleofferformats[s].totalflexofferedeu = -flexoffers[k].singleofferformats[s].totalflexofferedeu;
                                }

                                if(TOTALFLEXREQUESTED - flexoffers[k].singleofferformats[s].totalflexofferedeu >0)
                                {
                                    TOTALFLEXREQUESTED -= flexoffers[k].singleofferformats[s].totalflexofferedeu;
                                    string memory subRESULT = string(bytes.concat(bytes(flexoffers[k].userid), " ", bytes(abi.encodePacked(flexoffers[k].singleofferformats[s].totalflexofferedeu))));
                                    
                                    RESULT[resultCount++] = subRESULT;

                                    flexofferedlist[flexofferedlistCount++] = flexoffers[k].singleofferformats[s].totalflexofferedeu;
                                }
                                else
                                {
                                    string memory subRESULT= string(bytes.concat(bytes(flexoffers[k].userid), " ", bytes(abi.encodePacked(TOTALFLEXREQUESTED))));
                                    RESULT[resultCount++] = subRESULT;

                                    flexofferedlist[flexofferedlistCount++] = TOTALFLEXREQUESTED;
                                }

                                
                                
                            }
                        }
                    }
                }
            }
            
        }

        if(count == flexrequests[i].loc.length && TOTALFLEXREQUESTED>0)
        {
            return RESULT;
        }

        return RESULT;


    }


    function CheckFulfillmentFactor(int256[] memory _flexofferedlist, int256 fullfillmentfactor, int256 TOTALFLEXREQUESTED) private view returns(bool)
    {
        int256 val = 0;

        for(uint a=0; a<flexofferedlistCount; a++)
        {
            val+=_flexofferedlist[a];
        }

        if((val/ TOTALFLEXREQUESTED * 100) >= fullfillmentfactor)
        {
            return true;
        }
        return false;


    }



    function fcfs() public  
    {
        
        for(uint i=0; i<flexrequestCount; i++)
        {
            if((keccak256(abi.encodePacked((flexrequests[i].markettype))) == keccak256(abi.encodePacked(("fixedprice"))))) 
            {
                if((keccak256(abi.encodePacked((flexrequests[i].ifflexrequested))) == keccak256(abi.encodePacked(("false")))))  
                {
                    

                    final_accepted_offers[final_accepted_offerCount++].requestid = flexrequests[i].requestid;
                    final_accepted_offers[final_accepted_offerCount++].result = ["ifflexrequested false"];
                }
                else
                {
                    int256 TOTALFLEXREQUESTED = flexrequests[i].totalflexrequestedeu;
                    

                    if(CheckFulfillmentFactor(flexofferedlist, flexrequests[i].fullfillmentfactor, TOTALFLEXREQUESTED))
                    {
                        final_accepted_offers[final_accepted_offerCount++].requestid = flexrequests[i].requestid;
                        final_accepted_offers[final_accepted_offerCount++].result = matching(i, TOTALFLEXREQUESTED);
                    }
                    else
                    {
                        final_accepted_offers[final_accepted_offerCount++].requestid = flexrequests[i].requestid;
                        final_accepted_offers[final_accepted_offerCount++].result = ["fullfillment factor did not reach"];
                    }

                }
            }
        }



    }

    function createhash() public view returns(string memory)
    {
        string memory RESULTCONCAT;
        string memory REQFINALRESULT;

        string memory _RESULT;

        for(uint i=0; i<final_accepted_offerCount; i++)
        {
            string memory requestid = final_accepted_offers[i].requestid;
            string[] memory result = final_accepted_offers[i].result;


            for(uint j=0; j<result.length; j++)
            {
                RESULTCONCAT= string(abi.encodePacked(RESULTCONCAT," ",result[j]));
            }

            REQFINALRESULT= string(abi.encodePacked(requestid," ",RESULTCONCAT));

            _RESULT= string(abi.encodePacked(_RESULT," ",REQFINALRESULT));
        }

        return _RESULT;
    }


}