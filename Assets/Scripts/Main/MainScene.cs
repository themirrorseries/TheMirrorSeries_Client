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
    private Button matchBtn;
    public bool isMatch = false;
    // 临时房间id,用于取消匹配用
    private int roomId = -1;
    [SerializeField]
    // 匹配按钮精灵图片
    private Sprite matchSprite;
    [SerializeField]
    // 取消匹配按钮精灵图片
    private Sprite cancelMatchSprite;
    [SerializeField]
    private Sprite[] roles;
    private int selectIndex = 1;
    [SerializeField]
    private Button[] roleBtns;
    private Vector3 prescale;
    private NameTool nameTool;
    private AudioController audioController;
    // Start is called before the first frame update
    void Start()
    {
        nameTool = GetComponent<NameTool>();
        audioController = GetComponent<AudioController>();
        audioController.BGMPlay(AudioEunm.mainBGM, 0.8f);
        instance = this;
        for (int i = 0; i < roleBtns.Length; ++i)
        {
            int index = i;
            roleBtns[i].onClick.AddListener(
                delegate ()
                {
                    onClickHandler(index);
                }
            );
            if (index == 0)
            {
                prescale = roleBtns[i].transform.localScale;
            }
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
                // 生成UUID,发送给服务端,服务端返回UUID以及ID,然后记录下来
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
        audioController.SoundPlay(AudioEunm.btnClick);
        roleBtns[selectIndex].transform.localScale = prescale;
        roleBtns[selectIndex].transform.SetSiblingIndex(index);
        selectIndex = index;
        roleBtns[selectIndex].transform.localScale = prescale * 1.5f;
        roleBtns[selectIndex].transform.SetSiblingIndex(roleBtns.Length);
        LocalStorage.SetInt("ROLE", selectIndex);
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
