using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FightScene : MonoBehaviour
{
    public static FightScene instance;
    [SerializeField]
    private GameObject[] glasses;
    [SerializeField]
    private GameObject[] players;
    // 当前死亡人数
    public int deathCount;
    // Start is called before the first frame update
    void Start()
    {
        instance = this;
    }
    public void Init()
    {
        for (int i = 0; i < players.Length; ++i)
        {
            players[i].SetActive(true);
        }
        deathCount = 0;
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
