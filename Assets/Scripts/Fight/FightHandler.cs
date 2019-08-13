﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Google.Protobuf;

public class FightHandler : MonoBehaviour, IHandler
{
    public void MessageReceive(SocketModel model)
    {
        switch (model.command)
        {
            case (int)FightTypes.InformSres:
                MoveHandler(model.message);
                break;
        }
    }
    private void MoveHandler(byte[] message)
    {
        ServerMoveDTO serverMove = ServerMoveDTO.Parser.ParseFrom(message);
        FightScene.instance.Refresh(serverMove);
        FrameActions.Instance().Remove();
    }
}
