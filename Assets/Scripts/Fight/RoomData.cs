using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomData
{
    public static MatchSuccessDTO room = null;
    public static int seat = -1;
    public static bool isDeath = false;
    public static int seat2PlayerId(int seat)
    {
        if (room == null) return -1;
        for (int i = 0; i < room.Players.Count; ++i)
        {
            if (room.Players[i].Seat == seat)
            {
                return room.Players[i].Playerid;
            }
        }
        return -1;
    }
    public static string seat2PlayerName(int seat)
    {
        if (room == null) return string.Empty;
        for (int i = 0; i < room.Players.Count; ++i)
        {
            if (room.Players[i].Seat == seat)
            {
                return room.Players[i].Name;
            }
        }
        return string.Empty;
    }
    public static bool isMainRole(int seatId)
    {
        return seat == seatId;
    }
}
