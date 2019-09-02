using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Rank : MonoBehaviour
{
    [SerializeField]
    // 排名图片
    private Image rankImg;
    [SerializeField]
    // 皇冠
    private Image imperialCrown;
    [SerializeField]
    // 姓名
    private Text nickname;
    [SerializeField]
    // 生存时间
    private Text time;
    [SerializeField]
    // 反弹次数
    private Text bounce;
    public void View(bool isLast, bool is321, bool isFirst, Death death)
    {
        rankImg.gameObject.SetActive(isLast && is321);
        imperialCrown.gameObject.SetActive(isFirst);
        nickname.text = RoomData.seat2PlayerName(death.seat);
        time.text = TimeTools.Num2TimeString(death.time);
        bounce.text = death.bounces.ToString();
    }
}
