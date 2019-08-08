using UnityEngine;
using System.Collections;

public class NetMessagetUtil : MonoBehaviour
{

    IHandler main;
    IHandler fight;
    void Start()
    {
        main = GetComponent<MainHandler>();
        fight = GetComponent<FightHandler>();
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

        }
    }


}
