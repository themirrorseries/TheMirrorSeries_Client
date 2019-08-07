using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
            // this.SendMessage();
            // 暂时处理,先直接存储UUID
            LocalStorage.SetString("UUID", uuid);
            UserDTO user = new UserDTO();
            user.Id = 1;
            user.Uuid = uuid;
            GameData.user = user;
        }
        else
        {
            UserDTO user = new UserDTO();
            user.Id = 1;
            user.Uuid = LocalStorage.GetString("UUID");
            GameData.user = user;
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
        Text text = matchBtn.transform.Find("Text").GetComponent<Text>();
        if (isMatch)
        {
            isMatch = !isMatch;
            text.text = "开始游戏";
            // 给服务端发送消息
        }
        else
        {
            isMatch = !isMatch;
            text.text = "取消匹配";
            // 给服务端发送消息
        }
    }
}
