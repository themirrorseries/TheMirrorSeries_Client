using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSkill : MonoBehaviour
{
    [SerializeField]
    private List<int> skillIds;
    private List<Button> skillBtns;
    private List<Image> cdMasks = new List<Image>();
    private List<SkillBase> skills = new List<SkillBase>();
    // Start is called before the first frame update
    void Start()
    {

    }

    public void Init()
    {
        skillBtns = FightScene.instance.skillBtns;
        for (int i = 0; i < skillIds.Count; ++i)
        {
            skills.Add(addSkill(skillIds[i]));
        }
        PlayerAttribute attr = GetComponent<PlayerAttribute>();
        if (RoomData.isMainRole(attr.seat))
        {
            for (int i = 0; i < skillIds.Count; ++i)
            {
                skillBtns[i].GetComponent<Image>().sprite = ResourcesTools.getSkillIcon(skills[i].skillId);
                if (i > 0)
                {
                    cdMasks.Add(skillBtns[i].transform.Find("CD").gameObject.GetComponent<Image>());
                }
            }
        }
    }

    // 更新技能cd遮罩
    public void UpdateSkills(float deltaTime)
    {
        for (int i = 0; i < skills.Count; ++i)
        {
            skills[i].UpdateState(deltaTime);
        }
        for (int i = 0; i < cdMasks.Count; ++i)
        {
            if (skills[i + 1].isEndCd())
            {
                if (cdMasks[i].gameObject.activeInHierarchy)
                {
                    cdMasks[i].gameObject.SetActive(false);
                }
            }
            else
            {
                if (!cdMasks[i].gameObject.activeInHierarchy)
                {
                    cdMasks[i].gameObject.SetActive(true);
                }
                cdMasks[i].fillAmount = skills[i + 1].cdPercentage();
            }
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
    public void ChangeCd(int skillNum, float cd)
    {
        skills[skillNum].diffCd(cd);
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
