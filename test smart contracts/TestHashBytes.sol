// SPDX-License-Identifier: MIT
pragma solidity ^0.8.0;


contract TestHashBytes
{
    bytes[] hashlist;

    function setHashList(bytes memory hash) public
    {
        hashlist.push(hash);
    }

    function gethashList(uint256 index) public view returns(bytes memory)
    {
        return hashlist[index];
    }
}