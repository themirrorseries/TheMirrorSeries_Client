using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NameTool : MonoBehaviour
{
    public string[] names;
    private string file = "names";
    void Awake()
    {
        TextAsset txt = Resources.Load(file) as TextAsset;
        names = txt.text.Split('\n');
    }

    public string getName()
    {
        return names[Random.Range(0, names.Length)];
    }
}
