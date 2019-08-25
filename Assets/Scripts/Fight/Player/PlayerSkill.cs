using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSkill : MonoBehaviour
{
    [SerializeField]
    private List<int> skillIds;
    private List<SkillBase> skills = new List<SkillBase>();
    // Start is called before the first frame update
    void Start()
    {

    }

    public void Init()
    {
        for (int i = 0; i < skillIds.Count; ++i)
        {
            skills.Add(addSkill(skillIds[i]));
        }
        PlayerAttribute attr = GetComponent<PlayerAttribute>();
        if (attr.seat == RoomData.seat)
        {
            for (int i = 0; i < skillIds.Count; ++i)
            {
                FightScene.instance.skillBtns[i].transform.Find("Text").GetComponent<Text>().text = skills[i].skillName;
            }
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
        switch (skillNum)
        {
            case (int)SkillEunm.SkillBtn.ack: NormalAck(); break;
            case (int)SkillEunm.SkillBtn.skill: Skill(); break;
        }
    }
    public void NormalAck()
    {
        skills[(int)SkillEunm.SkillBtn.ack].Release();
    }
    public void Skill()
    {
        skills[(int)SkillEunm.SkillBtn.skill].Release();
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
