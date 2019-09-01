using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Google.Protobuf;

public class MainScene : MonoBehaviour
{
    public static MainScene instance;
    [SerializeField]
    private InputField nameInput;
    [SerializeField]
    private Button renameBtn;
    [SerializeField]
    private Button matchBtn;
    [SerializeField]
    // 角色描述
    private Image introduction;
    // 初始不暂时角色描述面板,需要点击一次才显示
    private bool isFirstIntroduction;
    [SerializeField]
    // 角色名
    private Text RoleName;
    [SerializeField]
    // 技能Icon
    private Image skillIcon;
    [SerializeField]
    // 技能描述
    private Text skillDescribe;
    [SerializeField]
    // 普通攻击Icon
    private Image ackIcon;
    [SerializeField]
    // 普通攻击描述
    private Text ackDescribe;
    public bool isMatch = false;
    // 临时房间id,用于取消匹配用
    private int roomId = -1;
    [SerializeField]
    // 匹配按钮精灵图片
    private Sprite matchSprite;
    [SerializeField]
    // 取消匹配按钮精灵图片
    private Sprite cancelMatchSprite;
    private List<Role> roles = new List<Role>();
    private int selectIndex = 1;
    [SerializeField]
    private Button[] roleBtns;
    private NameTool nameTool;
    private AudioController audioController;
    private List<int> indexs = new List<int>();
    // Start is called before the first frame update
    void Start()
    {
        introduction.gameObject.SetActive(false);
        // 读表
        Role role;
        role.roleId = 1;
        role.name = "调皮镜";
        role.ackId = (int)SkillEunm.SkillID.normalAck;
        role.ackDescribe = "发射光线";
        role.skillId = (int)SkillEunm.SkillID.groupChaos;
        role.skillDescribe = "颠倒方向";
        roles.Add(role);

        role.roleId = 2;
        role.name = "智慧镜";
        role.ackId = (int)SkillEunm.SkillID.normalAck;
        role.ackDescribe = "发射光线";
        role.skillId = (int)SkillEunm.SkillID.fiveThunder;
        role.skillDescribe = "直接伤害";
        roles.Add(role);

        role.roleId = 3;
        role.name = "活泼镜";
        role.ackId = (int)SkillEunm.SkillID.normalAck;
        role.ackDescribe = "发射光线";
        role.skillId = (int)SkillEunm.SkillID.protectiveCover;
        role.skillDescribe = "护盾防御";
        roles.Add(role);

        role.roleId = 4;
        role.name = "邪恶镜";
        role.ackId = (int)SkillEunm.SkillID.normalAck;
        role.ackDescribe = "发射光线";
        role.skillId = (int)SkillEunm.SkillID.nightBringer;
        role.skillDescribe = "限制视野";
        roles.Add(role);

        nameTool = GetComponent<NameTool>();
        audioController = GetComponent<AudioController>();
        audioController.BGMPlay(AudioEunm.mainBGM, 0.8f);
        instance = this;
        for (int i = 0; i < roleBtns.Length; ++i)
        {
            int index = i;
            indexs.Add(index);
            roleBtns[i].onClick.AddListener(
                delegate ()
                {
                    onClickHandler(index);
                }
            );
        }
        nameInput.onEndEdit.AddListener(EditEndHandler);
        if (LocalStorage.GetString("name") == string.Empty)
        {
            string name = nameTool.getName();
            nameInput.text = name;
            LocalStorage.SetString("name", name);
        }
        else
        {
            nameInput.text = LocalStorage.GetString("name");
        }
        if (GameData.user == null)
        {
            // 没有UUID,第一次登陆
            if (LocalStorage.GetString("UUID") == string.Empty)
            {
                // 生成UUID,发射给服务端,服务端返回UUID以及ID,然后记录下来
                string uuid = System.Guid.NewGuid().ToString();
                LocalStorage.SetString("UUID", uuid);
                LocalStorage.SetInt("ROLE", selectIndex);
                UserDTO user = new UserDTO();
                user.Id = -1;
                user.Uuid = uuid;
                this.WriteMessage((int)MsgTypes.TypeLogin, (int)LoginTypes.LoginCreq, user.ToByteArray());
            }
            else
            {
                selectIndex = LocalStorage.GetInt("ROLE");
                UserDTO user = new UserDTO();
                user.Id = -1;
                user.Uuid = LocalStorage.GetString("UUID");
                this.WriteMessage((int)MsgTypes.TypeLogin, (int)LoginTypes.LoginCreq, user.ToByteArray());
            }
        }
        else
        {
            selectIndex = LocalStorage.GetInt("ROLE");
            if (GameData.match)
            {
                Match();
            }
        }
        onClickHandler(selectIndex);
    }
    private void onClickHandler(int index)
    {
        selectIndex = indexs[index];
        audioController.SoundPlay(AudioEunm.btnClick);
        for (int i = 0; i < roleBtns.Length; ++i)
        {
            indexs[i] = getIndex(selectIndex + i - 1);
            roleBtns[i].GetComponent<Image>().sprite = ResourcesTools.getRole(roles[indexs[i]].roleId);
        }
        RoleName.text = roles[selectIndex].name;
        skillIcon.sprite = ResourcesTools.getSkillIcon(roles[selectIndex].skillId);
        skillDescribe.text = roles[selectIndex].skillDescribe;
        ackIcon.sprite = ResourcesTools.getSkillIcon(roles[selectIndex].ackId);
        ackDescribe.text = roles[selectIndex].ackDescribe;
        if (isFirstIntroduction)
        {
            if (!introduction.gameObject.activeInHierarchy)
            {
                introduction.gameObject.SetActive(true);
            }
        }
        if (!isFirstIntroduction)
        {
            isFirstIntroduction = true;
        }
        LocalStorage.SetInt("ROLE", selectIndex);
    }
    private int getIndex(int index)
    {
        if (index == -1)
        {
            index = roles.Count - 1;
        }
        index %= roles.Count;
        return index;
    }
    private void EditEndHandler(string str)
    {
        if (str == string.Empty)
        {
            str = nameTool.getName();
            nameInput.text = str;
        }
        LocalStorage.SetString("name", str);
    }
    public void ReName()
    {
        audioController.SoundPlay(AudioEunm.dice);
        string name = nameTool.getName();
        nameInput.text = name;
        LocalStorage.SetString("name", name);
    }
    public void SetRoomID(int value)
    {
        roomId = value;
        matchBtn.GetComponent<Image>().sprite = cancelMatchSprite;
        if (!isMatch)
        {
            isMatch = !isMatch;
        }
    }

    public void CancelMatch()
    {
        roomId = -1;
        matchBtn.GetComponent<Image>().sprite = matchSprite;
        if (isMatch)
        {
            isMatch = !isMatch;
        }
    }

    public void Match()
    {
        audioController.SoundPlay(AudioEunm.btnClick);
        // 防止未登陆的情况
        if (GameData.user == null) return;
        if (isMatch)
        {
            for (int i = 0; i < roleBtns.Length; ++i)
            {
                roleBtns[i].enabled = true;
            }
            renameBtn.enabled = true;
            isMatch = !isMatch;
            matchBtn.GetComponent<Image>().sprite = matchSprite;
            MatchRtnDTO matchRtn = new MatchRtnDTO();
            matchRtn.Id = GameData.user.Id;
            matchRtn.Cacheroomid = roomId;
            this.WriteMessage((int)MsgTypes.TypeMatch, (int)MatchTypes.LeaveCreq, matchRtn.ToByteArray());
        }
        else
        {
            for (int i = 0; i < roleBtns.Length; ++i)
            {
                roleBtns[i].enabled = false;
            }
            renameBtn.enabled = false;
            isMatch = !isMatch;
            matchBtn.GetComponent<Image>().sprite = cancelMatchSprite;
            MatchDTO match = new MatchDTO();
            match.Id = GameData.user.Id;
            match.Name = nameInput.text;
            match.RoleID = selectIndex + 1;
            this.WriteMessage((int)MsgTypes.TypeMatch, (int)MatchTypes.EnterCreq, match.ToByteArray());
        }
    }
}
