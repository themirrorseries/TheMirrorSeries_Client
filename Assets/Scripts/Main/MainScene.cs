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
    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        if (GameData.user == null)
        {
            // 没有UUID,第一次登陆
            if (LocalStorage.GetString("UUID") == string.Empty)
            {
                // 生成UUID,发送给服务端,服务端返回UUID以及ID,然后记录下来
                string uuid = System.Guid.NewGuid().ToString();
                LocalStorage.SetString("UUID", uuid);
                UserDTO user = new UserDTO();
                user.Id = -1;
                user.Uuid = uuid;
                this.WriteMessage((int)MsgTypes.TypeLogin, (int)LoginTypes.LoginCreq, user.ToByteArray());
            }
            else
            {
                UserDTO user = new UserDTO();
                user.Id = -1;
                user.Uuid = LocalStorage.GetString("UUID");
                this.WriteMessage((int)MsgTypes.TypeLogin, (int)LoginTypes.LoginCreq, user.ToByteArray());
            }
        }
        else
        {
            if (GameData.match)
            {
                Match();
            }
        }
        nameInput.onEndEdit.AddListener(EditEndHandler);
        if (LocalStorage.GetString("name") == string.Empty)
        {
            string name = MakeName();
            nameInput.text = name;
            LocalStorage.SetString("name", name);
        }
        else
        {
            nameInput.text = LocalStorage.GetString("name");
        }
    }
    private void EditEndHandler(string str)
    {
        LocalStorage.SetString("name", str);
    }
    public void ReName()
    {
        string name = MakeName();
        nameInput.text = name;
    }
    private string MakeName()
    {
        int randomNum = Random.Range(0, 100);
        LocalStorage.SetString("name", "好名都被狗取了" + randomNum.ToString());
        return "好名都被狗取了" + randomNum.ToString();
    }

    public void SetRoomID(int value)
    {
        roomId = value;
        Text text = matchBtn.transform.Find("Text").GetComponent<Text>();
        text.text = "取消匹配";
        if (!isMatch)
        {
            isMatch = !isMatch;
        }
    }

    public void CancelMatch()
    {
        roomId = -1;
        Text text = matchBtn.transform.Find("Text").GetComponent<Text>();
        text.text = "开始游戏";
        if (isMatch)
        {
            isMatch = !isMatch;
        }
    }

    public void Match()
    {
        // 防止未登陆的情况
        if (GameData.user == null) return;
        Text text = matchBtn.transform.Find("Text").GetComponent<Text>();
        if (isMatch)
        {
            isMatch = !isMatch;
            text.text = "开始游戏";
            MatchRtnDTO matchRtn = new MatchRtnDTO();
            matchRtn.Id = GameData.user.Id;
            matchRtn.Cacheroomid = roomId;
            this.WriteMessage((int)MsgTypes.TypeMatch, (int)MatchTypes.LeaveCreq, matchRtn.ToByteArray());
        }
        else
        {
            isMatch = !isMatch;
            text.text = "取消匹配";
            MatchDTO match = new MatchDTO();
            match.Id = GameData.user.Id;
            match.Name = nameInput.text;
            // 选择角色id,暂无
            match.RoleID = Random.Range(1, 5);
            this.WriteMessage((int)MsgTypes.TypeMatch, (int)MatchTypes.EnterCreq, match.ToByteArray());
        }
    }
}
