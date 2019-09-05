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
    public int bagid = 1;
    public int serverBagId = 0;
    private int frameCount = 3;
    public bool isLock = false;
    public bool needAdd = false;
    public List<ServerMoveDTO> caches = new List<ServerMoveDTO>();
    // 空帧
    private FrameInfo emptyFrame;
    void Awake()
    {
        instance = this;
        clientMove = new ClientMoveDTO();
        clientMove.Roomid = RoomData.room.Roomid;
        clientMove.Seat = RoomData.seat;
        clientMove.Bagid = bagid;
        Init();
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
    public void AddCache(ServerMoveDTO moveDTO)
    {
        caches.Add(moveDTO);
        caches.Sort((a, b) => a.Bagid.CompareTo(b.Bagid));
        PlayCache();
    }
    public void PlayCache()
    {
        if (caches.Count == 0) return;
        if (serverBagId > caches[0].Bagid)
        {
            return;
        }
        FightScene.instance.Refresh(caches[0]);
        ++serverBagId;
        caches.RemoveAt(0);
        if (serverBagId == bagid)
        {
            FrameActions.instance.Clear();
        }
        else
        {
            PlayCache();
        }
    }
    public void Init()
    {
        isLock = true;
        clientMove.Msg.Clear();
        isLock = false;
    }
    public void Clear()
    {
        clientMove.Msg.Clear();
        ++bagid;
        isLock = false;
    }
    public void Add(FrameInfo frame)
    {
        if (!isStart) return;
        if (RoomData.isDeath) return;
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
        clientMove.Bagid = bagid;
        isLock = true;
        this.WriteMessage((int)MsgTypes.TypeFight, (int)FightTypes.MoveCreq, clientMove.ToByteArray());
    }

    public int FrameCount
    {
        get
        {
            return frameCount;
        }
    }
}
