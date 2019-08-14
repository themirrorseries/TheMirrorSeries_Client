using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillBase : MonoBehaviour
{
    // 名称
    public string skillName;
    // CD
    public float cd;
    // 释放方式
    public int releaseType;
    // 延迟时间
    public float delayTime;
    // 技能对象
    public GameObject[] skillAims;
    // 技能范围_宽度
    public float skillScope_width;
    // 技能范围_长度
    public float skillScope_height;

    public virtual void beforeSkill()
    {

    }

    public virtual void onSkill()
    {

    }

    public virtual void afterSkill()
    {

    }

}
