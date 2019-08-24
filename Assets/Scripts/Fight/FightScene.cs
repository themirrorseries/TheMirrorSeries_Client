using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Google.Protobuf;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class FightScene : MonoBehaviour
{
    public static FightScene instance;
    [SerializeField]
    private GameObject[] seats;
    private Dictionary<int, int> seat2Player = new Dictionary<int, int>();
    private List<GameObject> players = new List<GameObject>();
    private GameObject myself;
    private PlayerControl myselfControl;
    private List<GameObject> lights = new List<GameObject>();
    // 战斗进行时间
    public float gameTime;
    // 死亡玩家记录
    public List<Death> deaths = new List<Death>();
    [SerializeField]
    private Image settlementPlane;
    [SerializeField]
    private GameObject[] ranklist;
    // 墙壁距离
    public float wallDistance = 2f;
    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        gameTime = 0;
        settlementPlane.gameObject.SetActive(false);
        InitPlayers();
        InitLight();
    }
    public void ShowRankList()
    {
        settlementPlane.gameObject.SetActive(true);
        int index = 0;
        for (; index < deaths.Count; ++index)
        {
            ranklist[index].SetActive(true);
            ranklist[index].GetComponent<Rank>().View(index == 0, deaths[index]);
        }
        for (; index < ranklist.Length; ++index)
        {
            ranklist[index].SetActive(false);
        }
    }
    public void InitPlayers()
    {
        for (int i = 0; i < RoomData.room.Players.Count; ++i)
        {
            GameObject player = Instantiate(ResourcesTools.getMirror(RoomData.room.Players[i].Roleid),
                        seats[RoomData.room.Players[i].Seat - 1].transform.position, Quaternion.identity);
            PlayerControl playerControl = player.GetComponent<PlayerControl>();
            playerControl.Init(RoomData.room.Players[i].Seat);
            players.Add(player);
            seat2Player.Add(RoomData.room.Players[i].Seat, i);
            if (RoomData.seat == RoomData.room.Players[i].Seat)
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
        lightMgr.Init(RoomData.room.Speed, RoomData.room.Count, RoomData.room.X, RoomData.room.Z);
        lights.Add(light);
    }
    public void Refresh(ServerMoveDTO move)
    {
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
            if (count == 0)
            {
                continue;
            }
            deltaTime /= count;
            gameTime += deltaTime;
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
                if (playerControl.attr.isEnd)
                {
                    return;
                }
                for (int q = 0; q < move.ClientInfo.Count; ++q)
                {
                    if (move.ClientInfo[q].Seat == playerControl.attr.seat)
                    {
                        index = q;
                        break;
                    }
                }
                if (index != -1 && !playerControl.attr.isDied)
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
    public void skill4()
    {
        if (myselfControl)
        {
            myselfControl.Skill4();
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
    public void AddDeath(int seat, int bounces)
    {
        Death death;
        death.seat = seat;
        death.bounces = bounces;
        death.time = gameTime;
        death.light = lights.Count;
        if (seat == RoomData.seat)
        {
            FrameActions.instance.needAdd = true;
        }
        deaths.Add(death);
        if (isEnd)
        {
            beforeShow();
            ShowRankList();
        }
    }
    public void beforeShow()
    {
        for (int i = 0; i < RoomData.room.Players.Count; ++i)
        {
            bool isDeath = false;
            for (int j = 0; j < deaths.Count; ++j)
            {
                if (RoomData.room.Players[i].Seat == deaths[j].seat)
                {
                    isDeath = true;
                    break;
                }
            }
            if (!isDeath)
            {
                Death death;
                death.seat = RoomData.room.Players[i].Seat;
                death.time = gameTime;
                death.bounces = players[seat2Player[death.seat]].GetComponent<PlayerAttribute>().bounces;
                death.light = lights.Count;
                deaths.Add(death);
                deaths.Reverse();
                return;
            }
        }
    }
    // 游戏是否结束
    public bool isEnd
    {
        get
        {
            return deaths.Count == players.Count - 1;
        }
    }
    public void BackToMainScene()
    {
        GameData.match = false;
        SceneManager.LoadScene(SceneEunm.MAIN);
    }
    public void Again()
    {
        GameData.match = true;
        SceneManager.LoadScene(SceneEunm.MAIN);
    }
}
