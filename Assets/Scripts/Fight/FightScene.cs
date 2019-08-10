﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FightScene : MonoBehaviour
{
    public static FightScene instance;
    [SerializeField]
    private LightManager lightMgr;
    private List<GameObject> players = new List<GameObject>();
    [SerializeField]
    private GameObject playerPrefab;
    // 当前死亡人数
    public int deathCount;
    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        InitPlayers();
        InitLight();
    }
    public void InitPlayers()
    {
        Vector3[] location = {

        };
        for (int i = 0; i < GameData.room.Players.Count; ++i)
        {
            GameObject player = GameObject.Instantiate(playerPrefab, Vector3.zero, Quaternion.identity);
            PlayerControl playerControl = player.GetComponent<PlayerControl>();
            playerControl.Init(GameData.room.Players[i].Seat);
            players.Add(player);
        }
    }
    public void InitLight()
    {
        lightMgr.Init(GameData.room.Speed, GameData.room.Count, GameData.room.X, GameData.room.Z);
    }
    public void Refresh(MoveDTO move)
    {
        for (int i = 0; i < players.Count; ++i)
        {
            PlayerControl playerControl = players[i].GetComponent<PlayerControl>();
            if (playerControl.seat == move.Seat)
            {
                playerControl.onMoveMsgHandler(move.X, move.Y, move.DeltaTime);
                break;
            }
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
