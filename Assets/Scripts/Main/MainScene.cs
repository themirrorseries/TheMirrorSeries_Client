using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Google.Protobuf;

public class MainScene : MonoBehaviour
{
    [SerializeField]
    private InputField nameInput;
    [SerializeField]
    private Button matchBtn;
    private bool isMatch = false;
    // Start is called before the first frame update
    void Start()
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

    public void Match()
    {
        // 防止未登陆的情况
        if (GameData.user == null) return;
        Text text = matchBtn.transform.Find("Text").GetComponent<Text>();
        if (isMatch)
        {
            isMatch = !isMatch;
            text.text = "开始游戏";
            CancelMatchDTO cancel = new CancelMatchDTO();
            cancel.Id = GameData.user.Id;
            cancel.RoomID = GameData.room.Roomid;
            this.WriteMessage((int)MsgTypes.TypeMatch, (int)MatchTypes.LeaveCreq, cancel.ToByteArray());
        }
        else
        {
            isMatch = !isMatch;
            text.text = "取消匹配";
            MatchDTO match = new MatchDTO();
            match.Name = nameInput.text;
            // 选择角色id,暂无
            match.RoleID = -1;
            this.WriteMessage((int)MsgTypes.TypeMatch, (int)MatchTypes.EnterCreq, match.ToByteArray());
        }
    }
}
