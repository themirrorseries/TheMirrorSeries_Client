using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttribute : MonoBehaviour
{
    public int seat;
    public int bounces = 0;
    public float hp = 20;
    // 血量上限
    public float hpMax = 20;
    public float mp = 5;
    // 蓝量上限
    public float mpMax = 5;
    // 击退状态
    public bool isRepulse = false;
    // 保护罩状态
    public bool hasProtection = false;
    // 混乱计数
    public int inChaosCount = 0;
    private PlayerChildren playerChildren;
    // Start is called before the first frame update
    void Start()
    {
    }

    // 初始化函数为多个参数,暂时写死
    public void Init()
    {
        children.hpText.text = "hp:" + hp.ToString();
        children.mpText.text = "mp:" + mp.ToString();
    }

    // 加/减血函数,正数加血,负数减血
    public void ChangeHp(float value)
    {
        if (hp + value > hpMax)
        {
            hp = hpMax;
        }
        else if (hp + value < 0)
        {
            hp = 0;
        }
        else
        {
            hp += value;
        }
        children.hpText.text = "hp:" + hp.ToString();
    }
    // 加/减蓝函数,正数加蓝,负数减蓝
    public void ChangeMp(float value)
    {
        if (mp + value > mpMax)
        {
            mp = mpMax;
        }
        else if (mp + value < 0)
        {
            mp = 0;
        }
        else
        {
            mp += value;
        }
        children.mpText.text = "mp:" + mp.ToString();
    }
    public bool canMove
    {
        get
        {
            return (isRepulse == false);
        }
    }
    // 是否混乱
    public bool isChaos
    {
        get
        {
            return (inChaosCount > 0);
        }
    }

    // 是否死亡
    public bool isDied
    {
        get
        {
            return (hp == 0);
        }
    }
    public PlayerChildren children
    {
        get
        {
            if (playerChildren == null)
            {
                playerChildren = GetComponent<PlayerChildren>();
            }
            return playerChildren;
        }
    }
}
