using System.Collections;
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
    // cd光环
    private List<Image> cDHalos = new List<Image>();
    public List<Coroutine> haloCoroutines = new List<Coroutine>();
    // cd文字
    private List<Text> cdTexts = new List<Text>();
    private List<SkillBase> skills = new List<SkillBase>();
    private List<Animation> anims = new List<Animation>();
    private PlayerAttribute playerAttribute;
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
        if (RoomData.isMainRole(attr.seat))
        {
            for (int i = 0; i < skillIds.Count; ++i)
            {
                skillBtns[i].GetComponent<Image>().sprite = ResourcesTools.getSkillIcon(skills[i].skillId);
                cdMasks.Add(skillBtns[i].transform.Find("CDMask").gameObject.GetComponent<Image>());
                cdTexts.Add(skillBtns[i].transform.Find("Text").gameObject.GetComponent<Text>());
                if (i > 0)
                {
                    haloCoroutines.Add(null);
                    cDHalos.Add(skillBtns[i].transform.Find("CDHalo").gameObject.GetComponent<Image>());
                    anims.Add(skillBtns[i].GetComponent<Animation>());
                }
            }
        }
    }

    // 更新技能cd遮罩
    public void UpdateSkills(float deltaTime)
    {
        // 更新技能状态
        for (int i = 0; i < skills.Count; ++i)
        {
            skills[i].UpdateState(deltaTime);
        }
        // 更新技能UI表现
        if (!RoomData.isMainRole(attr.seat)) return;
        for (int i = 0; i < cdMasks.Count; ++i)
        {
            if (skills[i].isEndCd())
            {
                if (cdMasks[i].gameObject.activeInHierarchy)
                {
                    cdMasks[i].gameObject.SetActive(false);
                    if (skills[i].skillId != (int)SkillEunm.SkillID.normalAck)
                    {
                        cdTexts[i].gameObject.SetActive(false);
                    }
                }
                if (skills[i].skillId != (int)SkillEunm.SkillID.normalAck)
                {
                    if (!cDHalos[i - 1].gameObject.activeInHierarchy)
                    {
                        cDHalos[i - 1].gameObject.SetActive(true);
                    }
                    if (haloCoroutines[i - 1] == null)
                    {
                        haloCoroutines[i - 1] = StartCoroutine(Halo(cDHalos[i - 1]));
                    }
                }
            }
            else
            {
                if (!cdMasks[i].gameObject.activeInHierarchy)
                {
                    cdMasks[i].gameObject.SetActive(true);
                    if (skills[i].skillId != (int)SkillEunm.SkillID.normalAck)
                    {
                        cdTexts[i].gameObject.SetActive(true);
                    }
                }
                if (skills[i].skillId != (int)SkillEunm.SkillID.normalAck)
                {
                    if (cDHalos[i - 1].gameObject.activeInHierarchy)
                    {
                        cDHalos[i - 1].gameObject.SetActive(false);
                    }
                    if (haloCoroutines[i - 1] != null)
                    {
                        StopCoroutine(haloCoroutines[i - 1]);
                    }
                    haloCoroutines[i - 1] = null;
                }
                cdMasks[i].fillAmount = skills[i].cdPercentage();
                if (skills[i].skillId != (int)SkillEunm.SkillID.normalAck)
                {
                    cdTexts[i].text = skills[i].remainCd().ToString();
                }
            }
        }
    }

    private IEnumerator Halo(Image haloImg)
    {
        int count = 0;
        int total = 21;
        while (true)
        {
            haloImg.sprite = ResourcesTools.getHalo(count + 1);
            count = (count + 1) % total;
            yield return new WaitForSeconds(0.1f);
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
        if (!skills[skillNum].isEndCd() && RoomData.isMainRole(attr.seat))
        {
            anims[skillNum - 1].Play();
        }
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
    private PlayerAttribute attr
    {
        get
        {
            if (playerAttribute == null)
            {
                playerAttribute = GetComponent<PlayerAttribute>();
            }
            return playerAttribute;
        }
    }
}
