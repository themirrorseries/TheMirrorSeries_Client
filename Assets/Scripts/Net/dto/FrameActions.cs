using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Google.Protobuf;

public class FrameActions : MonoBehaviour
{
    private static FrameActions instance = null;
    private ClientMoveDTO clientMove;
    private int bagid = 1;
    private int frameCount = 5;
    public bool isLock = false;
    private FrameActions()
    {
    }

    public void Init(int seat)
    {
        isLock = true;
        clientMove = new ClientMoveDTO();
        clientMove.Roomid = GameData.room.Roomid;
        clientMove.Seat = seat;
        clientMove.Bagid = GameData.bagid;
        clientMove.Msg.Clear();
        bagid = 1;
        isLock = false;
    }

    public void Remove()
    {
        clientMove.Msg.Clear();
        ++bagid;
        isLock = false;
    }

    public void Add(FrameInfo frame)
    {
        if (clientMove.Msg.Count < frameCount)
        {
            frame.Frame = clientMove.Msg.Count;
            if (IsFull())
            {
                Send();
            }
        }
    }

    private bool IsFull()
    {
        return clientMove.Msg.Count == frameCount;
    }

    private void Send()
    {
        isLock = true;
        this.WriteMessage((int)MsgTypes.TypeFight, (int)FightTypes.MoveCreq, clientMove.ToByteArray());
    }

    public static FrameActions Instance()
    {
        if (instance == null)
        {
            instance = new FrameActions();
        }
        return instance;
    }
}
