using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Google.Protobuf;

public class FrameActions : MonoBehaviour
{
    public static FrameActions instance = null;
    // 是否开始,加载场景完成后,发送一个请求,收到回复后才开始
    public bool isStart = false;
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
        FightLoadDTO fight = new FightLoadDTO();
        fight.Roomid = RoomData.room.Roomid;
        fight.Seat = RoomData.seat;
        clientMove = new ClientMoveDTO();
        clientMove.Roomid = RoomData.room.Roomid;
        clientMove.Seat = RoomData.seat;
        clientMove.Bagid = bagid;
        this.WriteMessage((int)MsgTypes.TypeFight, (int)FightTypes.LoadUpCreq, fight.ToByteArray());
    }
    void Start()
    {
        emptyFrame = new FrameInfo();
        emptyFrame.Skillid = (int)SkillEunm.SkillID.empty;
        needAdd = true;
    }
    void Update()
    {
        if (needAdd && isStart)
        {
            emptyFrame.DeltaTime = Time.deltaTime;
            Add(emptyFrame);
        }
    }

    public void Init()
    {
        isLock = true;
        clientMove.Msg.Clear();
        isLock = false;
    }
    public void SendCache()
    {
        isLock = false;
        if (IsFull())
        {
            Send();
        }
    }
    public void CacheNext()
    {
        clientMove.Msg.Clear();
        ++bagid;
    }
    public void Add(FrameInfo frame)
    {
        if (!isStart) return;
        if (clientMove.Msg.Count < frameCount)
        {
            frame.Frame = clientMove.Msg.Count;
            clientMove.Msg.Add(frame);
            if (IsFull())
            {
                if (!isLock)
                {
                    Send();
                }
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
        CacheNext();
    }

    public int FrameCount
    {
        get
        {
            return frameCount;
        }
    }
}
