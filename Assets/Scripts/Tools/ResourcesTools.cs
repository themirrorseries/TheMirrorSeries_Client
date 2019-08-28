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
    // UI路径
    public static string UI = "UI/";
    // 战斗场景路径
    public static string FightUrl = "Fight/";
    // 技能Icon路径
    public static string SkillUrl = "Skill/";
    // 血条预制体前缀
    public static string hpPre = "Hp";
    public static string getPrefabUrl(string url)
    {
        return prefabUrl + url;
    }
    public static string getUIUrl(string url)
    {
        return UI + url;
    }
    public static GameObject getResource(string url)
    {
        return Resources.Load<GameObject>(url);
    }

    public static GameObject getMirror(int id)
    {
        return getResource(getPrefabUrl(mirrorUrl) + mirrorPre + id.ToString());
    }
    public static GameObject getLight(int id)
    {
        return getResource(getPrefabUrl(lightUrl) + lightPre + id.ToString());
    }
    public static Sprite getSkillIcon(int id)
    {
        return Resources.Load<Sprite>(UI + FightUrl + SkillUrl + id.ToString());
    }
    public static Sprite getHpBar(int id)
    {
        return Resources.Load<Sprite>(UI + FightUrl + hpPre + id.ToString());
    }
}
