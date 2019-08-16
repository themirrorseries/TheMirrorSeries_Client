using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillBase : MonoBehaviour
{
    // 名称
    public string skillName;
    // 技能ID(从1开始计数)
    public int skillId;
    // CD(单位为秒)
    public int cd;
    // 释放方式
    public int releaseType;
    // 延迟时间
    public float delayTime;
    // 技能对象(层级名称)
    public string[] skillAims;
    // 技能范围
    public float skillScope;
    private long lastSkillTime;
    // 角色属性
    public PlayerAttribute playerAttribute;
    void Start()
    {
        lastSkillTime = TimeStamp.addTimeStamp(cd);
        playerAttribute = GetComponent<PlayerAttribute>();
    }
    public virtual void Release(DeltaDirection direction)
    {

    }
    // 判断蓝量是否足够
    public virtual bool isMpEnough(float mp)
    {
        return (playerAttribute.mp >= mp);
    }
    public virtual void AddMp(float mp)
    {
        playerAttribute.ChangeMp(mp);
    }
    // 判断是否冷却完成
    public virtual bool isEndCd()
    {
        return (TimeStamp.addTimeStamp(-cd) >= lastSkillTime);
    }
    // 寻找攻击目标(默认为圆形)
    public virtual List<GameObject> findAckAims()
    {
        // 需要寻找的Layers层
        LayerMask mask = 0;
        for (int i = 0; i < skillAims.Length; ++i)
        {
            mask |= LayerMask.GetMask(skillAims[i]);
        }
        List<GameObject> objects = new List<GameObject>();
        // 球形射线检测
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, skillScope, mask);
        for (int i = 0; i < hitColliders.Length; ++i)
        {
            objects.Add(hitColliders[i].gameObject);
        }
        return objects;
    }
    public virtual void beforeSkill()
    {
        lastSkillTime = TimeStamp.getTimeStamp();
    }

    public virtual void onSkill(DeltaDirection direction)
    {

    }

    public virtual void afterSkill()
    {

    }

}
