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
        }
    }
    public void Match(byte[] message)
    {
        MatchSuccessDTO match = MatchSuccessDTO.Parser.ParseFrom(message);
        GameData.room = match;
        for (int i = 0; i < match.Players.Count; ++i)
        {
            if (match.Players[i].Playerid == GameData.user.Id)
            {
                GameData.seat = match.Players[i].Seat;
                break;
            }
        }
        // 加载匹配场景,加载完成后进入战斗场景
        SceneManager.LoadScene(SceneTypes.FIGHT);
    }
    public void CancelMatch(byte[] message)
    {

    }
}
