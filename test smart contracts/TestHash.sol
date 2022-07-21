// SPDX-License-Identifier: MIT
pragma solidity ^0.8.0;


contract TestHash
{
    string[] hashlist;

    function setHashList(string memory hash) public
    {
        hashlist.push(hash);
    }

    function gethashList(uint256 index) public view returns(string memory)
    {
        return hashlist[index];
    }
}