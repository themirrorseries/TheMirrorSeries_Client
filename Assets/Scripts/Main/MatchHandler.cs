using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MatchHandler : MonoBehaviour, IHandler
{
    public void MessageReceive(SocketModel model)
    {
        switch (model.command)
        {
            case (int)MatchTypes.EnterSelectBro:
                Match(model.message);
                break;
            case (int)MatchTypes.EnterSres:
                MatchMsgHandler(model.message);
                break;
            case (int)MatchTypes.LeaveSres:
                CancelMatch(model.message);
                break;
        }
    }
    public void Match(byte[] message)
    {
        MatchSuccessDTO match = MatchSuccessDTO.Parser.ParseFrom(message);
        RoomData.room = match;
        for (int i = 0; i < match.Players.Count; ++i)
        {
            if (match.Players[i].Playerid == GameData.user.Id)
            {
                RoomData.seat = match.Players[i].Seat;
                break;
            }
        }
        MainScene.instance.isMatch = false;
        SceneManager.LoadScene(SceneEunm.LOAD);
    }
    public void MatchMsgHandler(byte[] message)
    {
        MatchRtnDTO matchRtn = new MatchRtnDTO();
        MainScene.instance.SetRoomID(matchRtn.Cacheroomid);
    }
    public void CancelMatch(byte[] message)
    {
        MainScene.instance.CancelMatch();
    }
}
