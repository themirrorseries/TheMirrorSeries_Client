using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttribute : MonoBehaviour
{
    public int seat;
    public int bounces = 0;
    public float hp = 5;
    // 血量上限
    public float hpMax = 5;
    public float mp = 100;
    // 蓝量上限
    public float mpMax = 100;
    // 单次反射能量值增量
    public float bounceAddMp = 50;
    // 光线正面击中伤害
    public float damage_normal = -1;
    // 光线背面击中伤害
    public float damage_hit = -3;
    // 击退撞墙伤害
    public float damage_repel = -3;
    // 击退状态
    public bool isRepulse = false;
    // 保护罩状态
    public bool hasProtection = false;
    // 混乱计数
    public int inChaosCount = 0;
    private PlayerChildren playerChildren;
    // 死亡后会有一段位置,用于区别血量为0的死亡和移动之后的死亡
    private bool isDeathMoveEnd = false;
    // 自动回血计时
    private float autoAddHpTime;
    // 自动回血间隔
    private float autoAddHpSpace = 1f;
    // 自动回血数值
    private float autoAddHpNum = 1f;
    // 被释放黑夜降临
    public int inNightCount = 0;
    // 主动释放黑夜降临
    public bool inSelfNight = false;
    // Start is called before the first frame update
    void Start()
    {
        autoAddHpTime = 0;
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
    public void UpdateState(float deltaTime)
    {
        AutoAddHp(deltaTime);
    }
    public void AutoAddHp(float deltaTime)
    {
        autoAddHpTime += deltaTime;
        if (autoAddHpTime >= autoAddHpSpace)
        {
            autoAddHpTime = 0;
            ChangeHp(autoAddHpNum);
        }
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
    public bool isEnd
    {
        get
        {
            return isDeathMoveEnd;
        }
        set
        {
            isDeathMoveEnd = value;
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
