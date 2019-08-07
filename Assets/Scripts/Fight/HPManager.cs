using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HPManager : MonoBehaviour
{
    public float hpMax;
    public float hpCur;
    [SerializeField]
    private TextMesh countText;
    [SerializeField]
    private GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        countText.gameObject.SetActive(true);
        // 实际应该为收到消息后调用
        Init();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Init()
    {
        hpCur = hpMax = 2f;
        countText.text = hpCur.ToString();
    }
    public void HpChange(float value)
    {
        if (hpCur - value > 0)
        {
            hpCur -= value;
            countText.text = hpCur.ToString();
        }
        else
        {
            hpCur = 0;
            countText.gameObject.SetActive(false);
            player.SetActive(false);
            FightScene.instance.AddDeathCount();
        }
    }
    public bool isDead()
    {
        return hpCur == 0;
    }
}
