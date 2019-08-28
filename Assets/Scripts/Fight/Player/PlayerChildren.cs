using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerChildren : MonoBehaviour
{
    [SerializeField]
    public PlayerTitle title;
    [SerializeField]
    // 闪电
    public GameObject thunder;
    // Start is called before the first frame update
    void Start()
    {

    }
    public void Init(int seat)
    {
        title.Init(seat);
    }
}
