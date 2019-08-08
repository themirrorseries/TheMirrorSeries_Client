using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoginHandler : MonoBehaviour, IHandler
{
    public void MessageReceive(SocketModel model)
    {
        switch (model.command)
        {
            case (int)LoginTypes.LoginSres:
                Login(model.message); break;
        }
    }
    public void Login(byte[] message)
    {
        UserDTO user = UserDTO.Parser.ParseFrom(message);
        GameData.user = user;
    }
}
