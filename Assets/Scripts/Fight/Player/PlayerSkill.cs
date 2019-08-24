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
    // 根据使用角色添加技能组件
    public void Init()
    {
        int[] skillIds = { (int)SkillEunm.SkillID.normalAck,
                            (int)SkillEunm.SkillID.protectiveCover,
                            (int)SkillEunm.SkillID.groupChaos,
                            (int)SkillEunm.SkillID.fiveThunder ,
                            (int)SkillEunm.SkillID.nightBringer};
        for (int i = 0; i < skillIds.Length; ++i)
        {
            skills.Add(addSkill(skillIds[i]));
        }
    }

    public void UpdateSkills(float deltaTime)
    {
        for (int i = 0; i < skills.Count; ++i)
        {
            skills[i].UpdateState(deltaTime);
        }
    }

    public void Release(int skillNum)
    {
        // 这里还应该有一个 判断这个技能编号是否大于了技能数组的长度
        switch (skillNum)
        {
            case (int)SkillEunm.SkillBtn.ack: NormalAck(); break;
            case (int)SkillEunm.SkillBtn.skill1: Skill1(); break;
            case (int)SkillEunm.SkillBtn.skill2: Skill2(); break;
            case (int)SkillEunm.SkillBtn.skill3: Skill3(); break;
        }
    }
    public void NormalAck()
    {
        skills[(int)SkillEunm.SkillBtn.ack].Release();
    }
    public void Skill1()
    {
        skills[(int)SkillEunm.SkillBtn.skill1].Release();
    }
    public void Skill2()
    {
        skills[(int)SkillEunm.SkillBtn.skill2].Release();
    }
    public void Skill3()
    {
        skills[(int)SkillEunm.SkillBtn.skill3].Release();
    }
    public void Skill4()
    {
        skills[(int)SkillEunm.SkillBtn.skill4].Release();
    }
    public SkillBase addSkill(int skillId)
    {
        switch (skillId)
        {
            case (int)SkillEunm.SkillID.normalAck: return gameObject.AddComponent<NormalAttack>();
            case (int)SkillEunm.SkillID.protectiveCover: return gameObject.AddComponent<ProtectiveCover>();
            case (int)SkillEunm.SkillID.groupChaos: return gameObject.AddComponent<GroupChaos>();
            case (int)SkillEunm.SkillID.fiveThunder: return gameObject.AddComponent<FiveThunder>();
            case (int)SkillEunm.SkillID.nightBringer: return gameObject.AddComponent<NightBringer>();
            default:
                return gameObject.AddComponent<SkillBase>();
        }
    }
}
