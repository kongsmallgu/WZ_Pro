using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapShuffle
{
    public static RoomCoord[] Knuth(RoomCoord[] roomArray) 
    {
        for (int i = roomArray.Length-1; i>=0;i--) 
        {
            int randNum = Random.Range(0,i);
            RoomCoord temp = roomArray[i];
            roomArray[randNum] = roomArray[i];
            roomArray[i] = temp;
        }
        return roomArray;
    }
}
