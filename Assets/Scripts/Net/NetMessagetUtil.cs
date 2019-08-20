using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class NetMessagetUtil : MonoBehaviour
{
    IHandler login;
    IHandler fight;
    IHandler match;
    void Start()
    {
        login = GetComponent<LoginHandler>();
        fight = GetComponent<FightHandler>();
        match = GetComponent<MatchHandler>();
    }

    void Update()
    {
        while (NetIO.Instance.messagesList.Count > 0)
        {
            SocketModel model = NetIO.Instance.messagesList[0];
            StartCoroutine("MessageReceive", model);
            NetIO.Instance.messagesList.RemoveAt(0);
        }
    }

    void MessageReceive(SocketModel model)
    {
        switch (model.type)
        {
            case (int)MsgTypes.TypeLogin:
                {
                    if (SceneManager.GetActiveScene().name == SceneEunm.MAIN)
                    {
                        login.MessageReceive(model);
                    }
                }
                break;
            case (int)MsgTypes.TypeMatch:
                if (SceneManager.GetActiveScene().name == SceneEunm.MAIN)
                {
                    match.MessageReceive(model);
                }
                break;
            case (int)MsgTypes.TypeFight:
                if (SceneManager.GetActiveScene().name == SceneEunm.FIGHT)
                {
                    fight.MessageReceive(model);
                }
                break;
        }
    }


}
