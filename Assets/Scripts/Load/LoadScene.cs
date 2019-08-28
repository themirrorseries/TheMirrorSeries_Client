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
    private float progressNum = 0;
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
        while (progressNum < 1)
        {
            progressNum += 0.01f;
            progress.transform.localScale = new Vector3(progressNum, 1, 1);
            progress.transform.localPosition = new Vector3(5 - 450 * (1 - progressNum), 3, 0);
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
