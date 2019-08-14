using UnityEngine;
using System.Collections;
using Google.Protobuf;
using System;
using System.Reflection;

public class SocketModel
{

    //一级协议 用于区分所属模块
    public int type { get; set; }
    //二级协议 用于区分当前处理的逻辑
    public int command { get; set; }
    //消息体 当前需要处理主体数据
    public byte[] message { get; set; }

    public SocketModel()
    {

    }

    public SocketModel(int t, int a, int c, byte[] o)
    {
        this.type = t;
        this.command = c;
        this.message = o;
    }
}
