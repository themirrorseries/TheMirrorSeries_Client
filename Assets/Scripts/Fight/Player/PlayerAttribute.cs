using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttribute : MonoBehaviour
{
    public float hp = 20;
    // 血量上限
    public float hpMax = 20;
    public float mp = 0;
    // 蓝量上限
    public float mpMax = 5;
    [SerializeField]
    private TextMesh hpText;
    [SerializeField]
    private TextMesh mpText;
    // 击退状态
    public bool isRepulse = false;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    // 初始化函数为多个参数,暂时写死
    public void Init()
    {
        hpText.text = "hp:" + hp.ToString();
        mpText.text = "mp:" + mp.ToString();
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
        hpText.text = "hp:" + hp.ToString();
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
        mpText.text = "mp:" + mp.ToString();
    }
    public bool canMove()
    {
        return (isRepulse == false);
    }
}
