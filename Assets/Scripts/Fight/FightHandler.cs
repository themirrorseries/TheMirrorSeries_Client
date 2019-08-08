using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Google.Protobuf;

public class FightHandler : MonoBehaviour, IHandler
{
    public void MessageReceive(SocketModel model)
    {
        switch (model.command)
        {

        }
    }
}
