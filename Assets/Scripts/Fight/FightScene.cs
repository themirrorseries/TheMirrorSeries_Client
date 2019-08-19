using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Google.Protobuf;

public class FightScene : MonoBehaviour
{
    public static FightScene instance;
    [SerializeField]
    private GameObject[] seats;
    // 当前死亡人数
    public int deathCount;
    private Dictionary<int, int> seat2Player = new Dictionary<int, int>();
    private List<GameObject> players = new List<GameObject>();
    private GameObject myself;
    private PlayerControl myselfControl;
    private List<GameObject> lights = new List<GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        InitPlayers();
        InitLight();
    }
    public void InitPlayers()
    {
        for (int i = 0; i < GameData.room.Players.Count; ++i)
        {
            // GameObject player = Instantiate(ResourcesTools.getMirror(GameData.room.Players[i].Roleid),
            //             seats[GameData.room.Players[i].Seat - 1].transform.position, Quaternion.identity);
            GameObject player = Instantiate(ResourcesTools.getMirror(1),
                        seats[GameData.room.Players[i].Seat - 1].transform.position, Quaternion.identity);

            PlayerControl playerControl = player.GetComponent<PlayerControl>();
            playerControl.Init(GameData.room.Players[i].Seat);
            players.Add(player);
            seat2Player.Add(GameData.room.Players[i].Seat, i);
            if (GameData.seat == GameData.room.Players[i].Seat)
            {
                myself = player;
                myselfControl = playerControl;
            }
        }
    }
    public void InitLight()
    {
        GameObject light = Instantiate(ResourcesTools.getLight(1));
        LightManager lightMgr = light.GetComponent<LightManager>();
        lightMgr.Init(GameData.room.Speed, GameData.room.Count, GameData.room.X, GameData.room.Z);
        lights.Add(light);
    }
    public void Refresh(ServerMoveDTO move)
    {
        List<float> deltaTimes = new List<float>();
        for (int i = 0; i < FrameActions.instance.FrameCount; ++i)
        {
            int count = 0;
            float deltaTime = 0;
            for (int j = 0; j < move.ClientInfo.Count; ++j)
            {
                if (move.ClientInfo[j].Seat != -1)
                {
                    ++count;
                    deltaTime += move.ClientInfo[j].Msg[i].DeltaTime;
                }
            }
            deltaTimes.Add(deltaTime / count);
            deltaTime /= count;

            for (int k = 0; k < lights.Count; ++k)
            {
                LightManager lightManager = lights[k].GetComponent<LightManager>();
                lightManager.Move(deltaTime);
            }

            for (int p = 0; p < players.Count; ++p)
            {
                // 是否丢包
                int index = -1;
                PlayerControl playerControl = players[p].GetComponent<PlayerControl>();
                for (int q = 0; q < move.ClientInfo.Count; ++q)
                {
                    if (move.ClientInfo[q].Seat == playerControl.attr.seat)
                    {
                        index = q;
                        break;
                    }
                }
                if (index != -1)
                {
                    playerControl.onMsgHandler(move.ClientInfo[index].Msg[i], deltaTime);
                }
                playerControl.UpdateState(deltaTime);
            }
        }
    }
    public void NormalAck()
    {
        if (myselfControl)
        {
            myselfControl.Ack();
        }
    }
    public void Skill1()
    {
        if (myselfControl)
        {
            myselfControl.Skill1();
        }
    }
    public void skill2()
    {
        if (myselfControl)
        {
            myselfControl.Skill2();
        }
    }
    public void skill3()
    {
        if (myselfControl)
        {
            myselfControl.Skill3();
        }
    }
    public List<GameObject> Players
    {
        get
        {
            return players;
        }
    }
    public List<GameObject> Lights
    {
        get
        {
            return lights;
        }
    }
    public void RomoveLight(GameObject light)
    {
        for (int i = 0; i < lights.Count; ++i)
        {
            if (lights[i] == light)
            {
                lights.Remove(light);
                return;
            }
        }
    }
    // 游戏是否结束
    public bool isEnd()
    {
        return deathCount == players.Count;
    }
}
