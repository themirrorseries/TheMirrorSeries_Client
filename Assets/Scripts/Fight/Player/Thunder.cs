﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thunder : MonoBehaviour
{
    private GameObject parent;
    // Start is called before the first frame update
    void Start()
    {
        parent = transform.parent.gameObject;
    }

    public void HideThunder()
    {
        if (parent.activeInHierarchy)
        {
            parent.SetActive(false);
        }
    }
}
