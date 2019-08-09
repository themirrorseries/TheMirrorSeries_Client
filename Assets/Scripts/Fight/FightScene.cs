using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FightScene : MonoBehaviour
{
    public static FightScene instance;
    [SerializeField]
    private GameObject[] glasses;
    [SerializeField]
    private List<GameObject> players;
    [SerializeField]
    private GameObject playerPrefab;
    private float speed = 0.1f;
    // 当前死亡人数
    public int deathCount;
    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        InitPlayers();
    }
    public void InitPlayers()
    {
        for (int i = 0; i < GameData.room.Players.Count; ++i)
        {
            GameObject player = GameObject.Instantiate(playerPrefab, Vector3.zero, Quaternion.identity);
            PlayerControl playerControl = player.GetComponent<PlayerControl>();
            playerControl.Init(GameData.room.Players[i].Seat);
            players.Add(player);
        }
    }
    public void Refresh(MoveDTO move)
    {
        for (int i = 0; i < players.Count; ++i)
        {
            PlayerControl playerControl = players[i].GetComponent<PlayerControl>();
            if (playerControl.seat == move.Seat)
            {
                float angle = Mathf.Atan2(move.X, move.Y) * Mathf.Rad2Deg;
                players[i].transform.rotation = Quaternion.Euler(new Vector3(0, angle, 0));
                players[i].transform.Translate(Vector3.forward * speed, Space.Self);
                break;
            }
        }
    }
    public void Init()
    {
    }
    // Update is called once per frame
    void Update()
    {

    }
    public void AddDeathCount()
    {
        if ((deathCount + 1) <= glasses.Length)
        {
            ++deathCount;
        }
    }
    // 游戏是否结束
    public bool isEnd()
    {
        return deathCount == glasses.Length;
    }
}
