using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerTitle : MonoBehaviour
{
    [SerializeField]
    private HpSlider hp;
    [SerializeField]
    private ProgressSlider progress;
    [SerializeField]
    private TextMesh nameText;
    private GameObject parent;
    void Start()
    {
        parent = transform.parent.gameObject;
    }
    void Update()
    {
        if (gameObject.activeInHierarchy)
        {
            transform.localEulerAngles = new Vector3(0, 360 - parent.transform.localEulerAngles.y, 0);
        }
    }
    public void Init(int seat)
    {
        hp.Init(seat);
        nameText.text = RoomData.seat2PlayerName(seat);
    }
    public void hpView(float value)
    {
        hp.Value = value;
    }
    public void progressView(float value)
    {
        progress.Value = value;
    }
    public void progressActive(bool active)
    {
        progress.gameObject.SetActive(active);
    }
}
