using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Rank : MonoBehaviour
{
    [SerializeField]
    // 头像
    private Image head;
    [SerializeField]
    // 皇冠
    private Image imperialCrown;
    [SerializeField]
    // 姓名
    private Text nickname;
    [SerializeField]
    // 光线数量
    private Text lightCount;
    [SerializeField]
    // 生存时间
    private Text time;
    [SerializeField]
    // 反弹次数
    private Text bounce;
    public void View(bool isFirst, Death death)
    {
        imperialCrown.gameObject.SetActive(isFirst);
        nickname.text = RoomData.seat2PlayerName(death.seat);
        lightCount.text = death.light.ToString();
        time.text = TimeTools.Num2TimeString(death.time);
        bounce.text = death.bounces.ToString();
    }
}
