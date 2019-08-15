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
    }
    public void Refresh(ServerMoveDTO move)
    {
        for (int i = 0; i < move.ClientInfo.Count; ++i)
        {
            // -1=>丢包
            if (move.ClientInfo[i].Seat != -1)
            {
                PlayerControl playerControl = players[seat2Player[move.ClientInfo[i].Seat]].GetComponent<PlayerControl>();
                playerControl.onMsgHandler(move.ClientInfo[i].Msg);
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
    // Update is called once per frame
    void Update()
    {

    }
    // 游戏是否结束
    public bool isEnd()
    {
        return deathCount == players.Count;
    }
}
