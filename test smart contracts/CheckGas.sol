// SPDX-License-Identifier: MIT
pragma solidity ^0.8.0;


contract CheckGas
{
    //for flexrequest
    struct Flexrequest
    {
        string requestid;
        string mode;
        string fullfillmentfactor;
        string ifflexrequested;
        string loc;
        string markettype;
        string maxpricectpeu;
        string priceofferctpeu;
        string referencepricectpeu;
        string timeslot;
        string totalflexrequestedeu;
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
        string totalflexofferedeu;
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
    function setflexrequests(string memory requestid, string memory mode, string memory fullfillmentfactor,
    string memory ifflexrequested, string memory loc, string memory markettype, string memory maxpricectpeu,
    string memory priceofferctpeu, string memory referencepricectpeu, string memory timeslot, string memory totalflexrequestedeu) public
    {
        
        Flexrequest memory fr = flexrequests[flexrequestCount];

        fr.requestid = requestid;
        fr.mode = mode;
        fr.fullfillmentfactor = fullfillmentfactor;
        fr.ifflexrequested = ifflexrequested;
        fr.loc = loc;
        fr.markettype = markettype;
        fr.maxpricectpeu =maxpricectpeu;
        fr.priceofferctpeu = priceofferctpeu;
        fr.referencepricectpeu = referencepricectpeu;
        fr.timeslot = timeslot;
        fr.totalflexrequestedeu = totalflexrequestedeu;

        flexrequestCount++;
    
        

    }


    //setup singleroffer
    function setsingleuserflexoffer(string memory bidpricectpeulist, string memory endflexshifttimeout, string memory requestid,
    string memory startflexshifttimeout, string memory totalflexofferedeu) public
    {
        

        Singleofferformat memory singleofferformat = Singleofferformat(bidpricectpeulist, endflexshifttimeout,
        requestid, startflexshifttimeout, totalflexofferedeu);

        singleofferformats.push(singleofferformat);

    }


    //setup all ofers from a user
    function setoffer(string memory userid) public
    {
        Flexoffer storage fo = flexoffers[flexofferCount];

        fo.userid = userid;
        fo.singleofferformats = singleofferformats;

        flexofferCount++;

        for(uint i=0; i<singleofferformats.length; i++)
        {
            singleofferformats.pop();
        }
        

    }


}