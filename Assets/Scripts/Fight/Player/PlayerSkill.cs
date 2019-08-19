using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkill : MonoBehaviour
{
    public List<SkillBase> skills;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    // 根据使用角色添加技能组件
    public void Init()
    {
        int[] skillIds = { SkillEunm.normalAck, SkillEunm.protectiveCover, SkillEunm.groupChaos, SkillEunm.fiveThunder };
        for (int i = 0; i < skillIds.Length; ++i)
        {
            skills.Add(addSkill(skillIds[i]));
        }
    }

    public void Release(int skillNum)
    {
        // 这里还应该有一个 判断这个技能编号是否大于了技能数组的长度
        switch (skillNum)
        {
            case SkillEunm.ack: NormalAck(); break;
            case SkillEunm.skill1: Skill1(); break;
            case SkillEunm.skill2: Skill2(); break;
            case SkillEunm.skill3: Skill3(); break;
        }
    }
    public void NormalAck()
    {
        skills[SkillEunm.ack].Release();
    }
    public void Skill1()
    {
        skills[SkillEunm.skill1].Release();
    }
    public void Skill2()
    {
        skills[SkillEunm.skill2].Release();
    }
    public void Skill3()
    {
        skills[SkillEunm.skill3].Release();
    }
    public SkillBase addSkill(int skillId)
    {
        switch (skillId)
        {
            case SkillEunm.normalAck: return gameObject.AddComponent<NormalAttack>();
            case SkillEunm.protectiveCover: return gameObject.AddComponent<ProtectiveCover>();
            case SkillEunm.groupChaos: return gameObject.AddComponent<GroupChaos>();
            case SkillEunm.fiveThunder: return gameObject.AddComponent<FiveThunder>();
            default:
                return gameObject.AddComponent<SkillBase>();
        }
    }
}
