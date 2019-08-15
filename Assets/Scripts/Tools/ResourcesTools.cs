using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourcesTools
{
    // 预制体路径
    public static string prefabUrl = "prefabs/";
    // 角色模型路径
    public static string mirrorUrl = "Mirror/";
    // 光线预制体路径
    public static string lightUrl = "Light/";
    // 光线预制体前缀
    public static string lightPre = "Light";
    // 角色模型前缀
    public static string mirrorPre = "Mirror";
    public static string getResourceUrl(string url)
    {
        return prefabUrl + url;
    }
    public static GameObject getResource(string url)
    {
        return Resources.Load<GameObject>(url);
    }

    public static GameObject getMirror(int id)
    {
        return getResource(getResourceUrl(mirrorUrl) + mirrorPre + id.ToString());
    }
    public static GameObject getLight(int id)
    {
        return getResource(getResourceUrl(lightUrl) + lightPre + id.ToString());
    }
}
