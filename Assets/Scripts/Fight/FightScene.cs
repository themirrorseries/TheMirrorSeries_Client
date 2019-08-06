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
    }
    // Update is called once per frame
    void Update()
    {

    }
    public bool isEnd()
    {
        int count = glasses.Length;
        for (int i = 0; i < glasses.Length; ++i)
        {
            HPManager hP = glasses[i].GetComponent<HPManager>();
            if (hP.isDead())
            {
                count--;
            }
        }
        return count == 1;
    }
}
