using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour
{
    [SerializeField]
    // 进度条
    private Image progress;
    [SerializeField]
    // 个数显示
    private Text count;
    private int curCount = 1;
    // 模数
    private int mod;
    // Start is called before the first frame update
    void Start()
    {
        mod = 100 / RoomData.room.Players.Count - 1;
        count.text = "(1/" + RoomData.room.Players.Count + ")";
        StartCoroutine(Load());
    }
    IEnumerator Load()
    {
        while (progress.fillAmount < 1)
        {
            progress.fillAmount += 0.01f;
            curCount++;
            if (curCount % mod == 0)
            {
                count.text = "(" + (curCount / mod).ToString() + "/" + RoomData.room.Players.Count + ")";
            }
            yield return null;
        }
        SceneManager.LoadScene(SceneEunm.FIGHT);
    }
}
