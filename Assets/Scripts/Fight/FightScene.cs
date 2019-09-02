using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Google.Protobuf;
using UnityEngine.UI;

public class FightScene : MonoBehaviour
{
    public static FightScene instance;
    [SerializeField]
    private GameObject[] seats;
    [SerializeField]
    public List<Button> skillBtns;
    private Dictionary<int, int> seat2Player = new Dictionary<int, int>();
    private List<GameObject> players = new List<GameObject>();
    public PlayerControl myselfControl;
    private List<GameObject> lights = new List<GameObject>();
    // 战斗进行时间
    public float gameTime;
    // 死亡玩家记录
    public List<Death> deaths = new List<Death>();
    [SerializeField]
    private Image settlementPlane;
    [SerializeField]
    private GameObject[] ranklist;
    // 掉血UI
    [SerializeField]
    public Image bleedingImg;
    [SerializeField]
    // 引导箭头
    private Image guideImg;
    [SerializeField]
    // 倒计时
    private Image countdownImg;
    // 倒计时协程
    private Coroutine countdownCoroutine;
    // 墙壁距离
    public float wallDistance = 2f;
    public AudioController audioController;
    public float nightScope = -1;
    // 是否显示过结算面板
    private bool isShowRank = false;
    private bool isShowGuide = true;
    // Start is called before the first frame update
    void Start()
    {
        bleedingImg.gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(Screen.width - 150, Screen.height);
        countdownCoroutine = StartCoroutine(Countdown());
        audioController = GetComponent<AudioController>();
        audioController.BGMPlay(AudioEunm.fightBGM, 0.8f);
        instance = this;
        gameTime = 0;
        settlementPlane.gameObject.SetActive(false);
        InitPlayers();
        InitLight();
    }

    private IEnumerator Countdown()
    {
        int count = 3;
        while (count > 0)
        {
            countdownImg.sprite = ResourcesTools.getCountdown(count);
            --count;
            yield return new WaitForSeconds(1f);
        }
        countdownImg.gameObject.SetActive(false);
        isShowGuide = true;
        guideImg.gameObject.GetComponent<Guide>().StartGuide();
        FightLoadDTO fight = new FightLoadDTO();
        fight.Roomid = RoomData.room.Roomid;
        fight.Seat = RoomData.seat;
        this.WriteMessage((int)MsgTypes.TypeFight, (int)FightTypes.LoadUpCreq, fight.ToByteArray());
        StopCoroutine(countdownCoroutine);
    }
    public void ShowOtherList()
    {
        settlementPlane.gameObject.SetActive(true);
        for (int i = 1; i < ranklist.Length; ++i)
        {
            ranklist[i].SetActive(false);
        }
        ranklist[0].SetActive(true);
        ranklist[0].GetComponent<Rank>().View(false, false, false, deaths[deaths.Count - 1]);
    }
    public void ShowFirstList()
    {
        settlementPlane.gameObject.SetActive(true);
        int index = 0;
        for (; index < deaths.Count; ++index)
        {
            ranklist[index].SetActive(true);
            ranklist[index].GetComponent<Rank>().View(true, index < 3, index == 0, deaths[index]);
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
                        seats[RoomData.room.Players[i].Seat - 1].transform.position,
                        seats[RoomData.room.Players[i].Seat - 1].transform.rotation);
            PlayerControl playerControl = player.GetComponent<PlayerControl>();
            playerControl.Init(RoomData.room.Players[i]);
            players.Add(player);
            seat2Player.Add(RoomData.room.Players[i].Seat, i);
            if (RoomData.isMainRole(RoomData.room.Players[i].Seat))
            {
                myselfControl = playerControl;
            }
        }
    }
    public void InitLight()
    {
        for (int i = 0; i < RoomData.room.Lights.Count; ++i)
        {
            GameObject light = Instantiate(ResourcesTools.getLight(1));
            LightManager lightMgr = light.GetComponent<LightManager>();
            lightMgr.Init(RoomData.room.Speed, RoomData.room.Count, RoomData.room.Lights[0].X, RoomData.room.Lights[0].Z);
            lights.Add(light);
        }
    }
    public void Refresh(ServerMoveDTO move)
    {
        for (int i = 0; i < FrameActions.instance.FrameCount; ++i)
        {
            int count = 0;
            float deltaTime = 0;
            for (int j = 0; j < move.ClientInfo.Count; ++j)
            {
                if (move.ClientInfo[j].Seat != -1 && !isInDeath(move.ClientInfo[j].Seat))
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
                playerControl.UpdateState(deltaTime);
                if (playerControl.attr.isEnd)
                {
                    continue;
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
            }
        }
    }
    public void NormalAck()
    {
        if (isShowGuide)
        {
            isShowGuide = false;
            guideImg.gameObject.GetComponent<Guide>().StopGuide();
        }
        if (myselfControl)
        {
            myselfControl.Ack();
        }
    }
    public void Skill()
    {
        if (myselfControl)
        {
            myselfControl.Skill();
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
        deaths.Add(death);
        if (isShowRank) return;
        if (RoomData.isMainRole(seat))
        {
            isShowRank = true;
            RoomData.isDeath = true;
            FrameActions.instance.needAdd = false;
            FightLeaveDTO leaveDTO = new FightLeaveDTO();
            leaveDTO.Roomid = RoomData.room.Roomid;
            leaveDTO.Seat = RoomData.seat;
            this.WriteMessage((int)MsgTypes.TypeFight, (int)FightTypes.DeathCreq, leaveDTO.ToByteArray());
        }
        if (isEnd)
        {
            if (isInDeath(RoomData.seat))
            {
                ShowOtherList();
            }
            else
            {
                beforeShow();
                ShowFirstList();
            }
        }
        else
        {
            if (RoomData.isMainRole(seat))
            {
                ShowOtherList();
            }
        }
    }

    public void beforeShow()
    {
        for (int i = 0; i < RoomData.room.Players.Count; ++i)
        {
            if (!isInDeath(RoomData.room.Players[i].Seat))
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
    private void StopGuide()
    {
        isShowGuide = false;
        guideImg.gameObject.GetComponent<Guide>().StopGuide();
    }
    private void LeaveRoom()
    {
        if (isShowGuide)
        {
            StopGuide();
        }
        FightLeaveDTO leaveDTO = new FightLeaveDTO();
        leaveDTO.Roomid = RoomData.room.Roomid;
        leaveDTO.Seat = RoomData.seat;
        this.WriteMessage((int)MsgTypes.TypeFight, (int)FightTypes.LaeveCreq, leaveDTO.ToByteArray());
    }
    public void BackToMainScene()
    {
        if (isShowGuide)
        {
            StopGuide();
        }
        RoomData.isDeath = false;
        GameData.match = false;
        LeaveRoom();
    }
    public void Again()
    {
        RoomData.isDeath = false;
        GameData.match = true;
        LeaveRoom();
    }
    public bool isInDeath(int seat)
    {
        for (int i = 0; i < deaths.Count; ++i)
        {
            if (deaths[i].seat == seat)
            {
                return true;
            }
        }
        return false;
    }
}
