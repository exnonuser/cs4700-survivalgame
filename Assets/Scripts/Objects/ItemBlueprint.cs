using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBlueprint 
{
    public string itemName;

    public int numOfRequirements;

    public string Req1;
    public string Req2;

    public int Req1Amount;
    public int Req2Amount;

    public ItemBlueprint(string name, int reqNum, string R1, int R1num, string R2, int R2num)
    {
        itemName = name;

        numOfRequirements = reqNum; 

        Req1 = R1;
        Req2 = R2;

        Req1Amount = R1num;
        Req2Amount = R2num; 
    }

    public ItemBlueprint(string name, int reqNum, string R1, int R1num)
    {
        itemName = name;

        numOfRequirements = reqNum; 

        Req1 = R1;

        Req1Amount = R1num;
    }
}
