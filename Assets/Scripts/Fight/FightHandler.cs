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
            case (int)FightTypes.InformSres:
                MoveHandler(model.message);
                break;
            case (int)FightTypes.LoadUpSres:
                StartHandler(model.message);
                break;
            case (int)FightTypes.DeathSres:
                DeathHandler(model.message);
                break;
        }
    }
    private void MoveHandler(byte[] message)
    {
        ServerMoveDTO serverMove = ServerMoveDTO.Parser.ParseFrom(message);
        FightScene.instance.Refresh(serverMove);
        FrameActions.instance.SendCache();
    }
    private void StartHandler(byte[] message)
    {
        FrameActions.instance.isStart = true;
    }
    private void DeathHandler(byte[] message)
    {

    }
}
