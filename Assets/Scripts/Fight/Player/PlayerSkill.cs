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
        int[] skillIds = { SkillEunm.normalAck };
        for (int i = 0; i < skillIds.Length; ++i)
        {
            skills.Add(addSkill(skillIds[i]));
        }
    }

    public void Release(int skillNum, DeltaDirection direction)
    {
        // 这里还应该有一个 判断这个技能编号是否大于了技能数组的长度
        switch (skillNum)
        {
            case SkillEunm.ack: NormalAck(direction); break;
            case SkillEunm.skill1: Skill1(direction); break;
            case SkillEunm.skill2: Skill2(direction); break;
            case SkillEunm.skill3: Skill3(direction); break;
        }
    }
    public void NormalAck(DeltaDirection direction)
    {
        skills[SkillEunm.ack].Release(direction);
    }
    public void Skill1(DeltaDirection direction)
    {
        skills[SkillEunm.skill1].Release(direction);
    }
    public void Skill2(DeltaDirection direction)
    {
        skills[SkillEunm.skill2].Release(direction);
    }
    public void Skill3(DeltaDirection direction)
    {
        skills[SkillEunm.skill3].Release(direction);
    }
    public SkillBase addSkill(int skillId)
    {
        switch (skillId)
        {
            case SkillEunm.normalAck: return gameObject.AddComponent<NormalAttack>();
            default:
                return gameObject.AddComponent<NormalAttack>();
        }
    }
}
