// SPDX-License-Identifier: MIT
pragma solidity ^0.8.0;

contract CheckGasSimple
{
    string[]  RESULT;
    int256[] flexofferedlist;
    uint256 flexofferedlistCount;

    
    mapping (string => int256) accepted_offers;


    //for flexrequest
    
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


    constructor()  {}


    //for all offers from a user
    struct Flexoffer
    {
        string userid;
        string bidpricectpeulist;
        string endflexshifttimeout;
        string requestid;
        string startflexshifttimeout;
        int256 totalflexofferedeu;
    }

    uint flexofferCount;
    mapping (uint => Flexoffer) flexoffers;
    


    //setup requests
    function setflexrequests(string memory _requestid, string memory _mode, int256 _fullfillmentfactor,
    string memory _ifflexrequested, string[] memory _loc, string memory _markettype, string memory _maxpricectpeu,
    string memory _priceofferctpeu, string memory _referencepricectpeu, string memory _timeslot, int256 _totalflexrequestedeu) public
    {
        
        requestid = _requestid;
        mode = _mode;
        fullfillmentfactor = _fullfillmentfactor;
        ifflexrequested = _ifflexrequested;
        loc = _loc;
        markettype = _markettype;
        maxpricectpeu = _maxpricectpeu;
        priceofferctpeu = _priceofferctpeu;
        referencepricectpeu = _referencepricectpeu;
        timeslot = _timeslot;
        totalflexrequestedeu = _totalflexrequestedeu;


    }


    

    //setup all ofers from a user
    function setoffer(string memory _userid, string memory _bidpricectpeulist, string memory _endflexshifttimeout, string memory _requestid,
    string memory _startflexshifttimeout, int256 _totalflexofferedeu) public
    {

        flexoffers[flexofferCount].userid = _userid;
        flexoffers[flexofferCount].bidpricectpeulist = _bidpricectpeulist;
        flexoffers[flexofferCount].endflexshifttimeout = _endflexshifttimeout;
        flexoffers[flexofferCount].requestid = _requestid;
        flexoffers[flexofferCount].startflexshifttimeout = _startflexshifttimeout;
        flexoffers[flexofferCount].totalflexofferedeu = _totalflexofferedeu;

        flexofferCount++;

               
    }



    function matching(int256 TOTALFLEXREQUESTED) private 
    {
        int256 SIGN =1;


        if(TOTALFLEXREQUESTED<0)
        {
            SIGN = -1;

            TOTALFLEXREQUESTED = -TOTALFLEXREQUESTED;
        }

        uint256 count =0;

        for(uint j=0; j<loc.length; j++)
        {
            count++;

            for(uint256 k=0; k<flexofferCount; k++)
            {
                if(keccak256(abi.encodePacked((flexoffers[k].userid))) == keccak256(abi.encodePacked((loc[j]))))
                {
                    if(keccak256(abi.encodePacked((flexoffers[k].requestid))) == keccak256(abi.encodePacked((requestid))))
                    {
                        if((SIGN>0 && (flexoffers[k].totalflexofferedeu > 0)) || (SIGN<0 && (flexoffers[k].totalflexofferedeu < 0)))
                        {
                            if(SIGN<0 && flexoffers[k].totalflexofferedeu < 0)
                                {
                                    flexoffers[k].totalflexofferedeu = -flexoffers[k].totalflexofferedeu;
                                }

                                if(TOTALFLEXREQUESTED - flexoffers[k].totalflexofferedeu >0)
                                {
                                    TOTALFLEXREQUESTED -= flexoffers[k].totalflexofferedeu;
                                    
                                    accepted_offers[flexoffers[k].userid] = flexoffers[k].totalflexofferedeu;

                                    RESULT.push(flexoffers[k].userid);

                                    flexofferedlist[flexofferedlistCount++] = flexoffers[k].totalflexofferedeu;
                                }

                                else
                                {

                                    accepted_offers[flexoffers[k].userid] = TOTALFLEXREQUESTED;

                                    RESULT.push(flexoffers[k].userid);


                                    flexofferedlist[flexofferedlistCount++] = TOTALFLEXREQUESTED;
                                }
                        }

                    }
                }

            }

        }

        if(count == loc.length && TOTALFLEXREQUESTED>0)
        {
            return ;
        }

        return ;


    }


    function checkfulfillmentfactor(int256 _fullfillmentfactor, int256 _totalflexrequestedeu) private view returns(bool)
    {
        int256 val = 0;

        for(uint a=0; a<flexofferedlistCount; a++)
        {
            val+=flexofferedlist[a];
        }

        if((val/ _totalflexrequestedeu * 100) >= _fullfillmentfactor)
        {
            return true;
        }
        return false;

    }



    function fcfs() public
    {
        if((keccak256(abi.encodePacked((markettype))) == keccak256(abi.encodePacked(("fixedprice"))))) 
        {
            if((keccak256(abi.encodePacked((ifflexrequested))) == keccak256(abi.encodePacked(("false")))))  
                {
                    

                    accepted_offers[string(abi.encodePacked(requestid, "ifflexrequested false"))] = 0;
                }
                else
                {
                    
                    matching(totalflexrequestedeu);

                    if(checkfulfillmentfactor(fullfillmentfactor, totalflexrequestedeu))
                    {
                        accepted_offers[string(abi.encodePacked("fullment factor"))] = 1;
                    }
                    else
                    {
                        
                        accepted_offers[string(abi.encodePacked("fullment factor"))] = 0;
                    }

                }
        }
    }


    function returnresult() public view returns(string memory)
    {
        if(keccak256(abi.encodePacked((ifflexrequested))) == keccak256(abi.encodePacked(("false"))))
        {
            return "flexrequested false";
        }
        
        if( accepted_offers["fullment factor"]==1)
        {
            string memory finalresult;

            for(uint i=0; i<RESULT.length; i++)
            {
                string(abi.encodePacked(finalresult," ", accepted_offers[RESULT[i]]));
                
            }
            return finalresult;
        }
        else
        {
            return "fullment factor did not match";
        }
    }




}