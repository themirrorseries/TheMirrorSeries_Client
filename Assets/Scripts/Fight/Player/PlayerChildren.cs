using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerChildren : MonoBehaviour
{
    [SerializeField]
    // 头顶
    public PlayerTitle title;
    [SerializeField]
    // 闪电
    public GameObject thunder;
    [SerializeField]
    private GameObject arrowhead;
    // Start is called before the first frame update
    void Start()
    {

    }
    public void Init(int seat)
    {
        title.Init(seat);
        if (RoomData.isMainRole(seat))
        {
            arrowhead.SetActive(true);
        }
        else
        {
            arrowhead.SetActive(false);
        }
    }
}
