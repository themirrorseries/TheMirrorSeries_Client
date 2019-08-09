﻿using System.Collections;
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
        Debug.Log("匹配成功");
        // 加载匹配场景,加载完成后进入战斗场景
        SceneManager.LoadScene(SceneTypes.FIGHT);
    }
    public void CancelMatch(byte[] message)
    {

    }
}
