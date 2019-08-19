using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Google.Protobuf;

public class FrameActions : MonoBehaviour
{
    public static FrameActions instance = null;
    private ClientMoveDTO clientMove;
    private int bagid = 1;
    private int frameCount = 3;
    public bool isLock = false;
    public bool needAdd = false;
    // 空帧
    private FrameInfo emptyFrame;
    void Awake()
    {
        instance = this;
    }
    void Start()
    {
        emptyFrame = new FrameInfo();
        emptyFrame.Skillid = (int)SkillEunm.SkillID.empty;
        needAdd = true;
    }
    void Update()
    {
        if (needAdd)
        {
            emptyFrame.DeltaTime = Time.deltaTime;
            Add(emptyFrame);
        }
    }

    public void Init()
    {
        isLock = true;
        clientMove = new ClientMoveDTO();
        clientMove.Roomid = GameData.room.Roomid;
        clientMove.Seat = GameData.seat;
        clientMove.Bagid = bagid;
        clientMove.Msg.Clear();
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
            clientMove.Msg.Add(frame);
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
        Remove();
    }

    public int FrameCount
    {
        get
        {
            return frameCount;
        }
    }
}
