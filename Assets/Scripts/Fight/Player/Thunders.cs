﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thunders : MonoBehaviour
{
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
}
