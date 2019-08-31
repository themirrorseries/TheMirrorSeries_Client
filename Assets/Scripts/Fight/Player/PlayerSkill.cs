﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSkill : MonoBehaviour
{
    [SerializeField]
    private List<int> skillIds;
    private List<Button> skillBtns;
    // cd遮罩
    private List<Image> cdMasks = new List<Image>();
    // cd文字
    private List<Text> cdTexts = new List<Text>();
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
                cdMasks.Add(skillBtns[i].transform.Find("CDMask").gameObject.GetComponent<Image>());
                cdTexts.Add(skillBtns[i].transform.Find("Text").gameObject.GetComponent<Text>());
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
            if (skills[i].isEndCd())
            {
                if (cdMasks[i].gameObject.activeInHierarchy)
                {
                    cdMasks[i].gameObject.SetActive(false);
                    cdTexts[i].gameObject.SetActive(false);
                }
            }
            else
            {
                if (!cdMasks[i].gameObject.activeInHierarchy)
                {
                    cdMasks[i].gameObject.SetActive(true);
                    cdTexts[i].gameObject.SetActive(true);
                }
                cdMasks[i].fillAmount = skills[i].cdPercentage();
                cdTexts[i].text = skills[i].remainCd().ToString();
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
