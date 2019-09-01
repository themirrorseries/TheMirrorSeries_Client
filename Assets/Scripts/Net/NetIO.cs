using UnityEngine;
using System.Collections;
using System.Net.Sockets;
using System;
using System.Collections.Generic;
using System.IO;
using Google.Protobuf;
using System.Net;
using System.Text;
public class NetIO
{

    public static NetIO instance;
    private Socket socket;
    private string ip = "127.0.0.1";
    private string domain = "http://mirror.murmur.top/";
    private int port = 9700;
    private byte[] readBuff = new byte[1024];
    List<byte> cache = new List<byte>();
    public List<SocketModel> messagesList = new List<SocketModel>();
    bool isReading;
    public static NetIO Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new NetIO();
            }
            return instance;
        }
    }
    private NetIO()
    {
        ip = SendRequest(domain, Encoding.UTF8);
        try
        {
            //创建客户端连接
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            //连接服务器
            socket.Connect(ip, port);
            //开启异步消息接受，消息到达后会直接写入缓存区
            socket.BeginReceive(readBuff, 0, 1024, SocketFlags.None, ReceiveCallBack, readBuff);
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
        }
    }
    //收到消息回调函数
    public void ReceiveCallBack(IAsyncResult ar)
    {
        try
        {
            // 结束异步接收，返回当前收到的消息的长度
            int length = socket.EndReceive(ar);
            byte[] message = new byte[length];
            Buffer.BlockCopy(readBuff, 0, message, 0, length);
            cache.AddRange(message);

            if (!isReading)
            {
                isReading = true;
                OnDate();
            }
            // 尾递归，再次开启异步消息接收， 消息到达后会直接写入缓存区
            socket.BeginReceive(readBuff, 0, 1024, SocketFlags.None, ReceiveCallBack, readBuff);
        }
        catch (Exception e)
        {
            Debug.Log("远程服务器断开连接" + e.Message);
        }
    }
    // 缓存中数据处理
    public void OnDate()
    {
        // 有数据的话就调用长度解码
        byte[] result = Decode(ref cache);

        if (result == null)
        {
            isReading = false;
            return;
        }
        // 消息体解码
        SocketModel messages = ModelDecode(result);
        if (messages == null)
        {
            isReading = false;
            return;
        }

        // 将消息体存储 等待Unity调用
        messagesList.Add(messages);

        // 尾递归防止在消息处理过程中有其他的消息而没有经过处理
        OnDate();

    }
    // 消息体长度解码
    private byte[] Decode(ref List<byte> cache)
    {
        // 消息体的长度识别数据都不够，肯定没数据
        if (cache.Count < 4) return null;

        // 创建内存流对象，并将缓存数据写进去
        MemoryStream ms = new MemoryStream(cache.ToArray());
        // 二进制读取流
        BinaryReader br = new BinaryReader(ms);
        // 从缓存中读取int型消息体长度
        int length = br.ReadInt32();
        // 如果消息体长度大于缓存中数据长度，说明消息没有读取完成 ，等待下次消息到达后再次处理。
        if (length > ms.Length - ms.Position)
        {
            return null;
        }
        // 取正确长度的数据
        byte[] result = br.ReadBytes(length);
        // 清空缓存
        cache.Clear();

        // 将读取后剩余的数据写入缓存
        cache.AddRange(br.ReadBytes((int)(ms.Length - ms.Position)));
        br.Close();
        ms.Close();
        return result;

    }
    // 消息解码
    private SocketModel ModelDecode(byte[] value)
    {
        ByteArray ba = new ByteArray(value);
        SocketModel socketModel = new SocketModel();
        int type;
        int command;
        // 从数据中读取两层协议,读取数据顺序必须和写入顺序保持一致
        ba.read(out type);
        ba.read(out command);
        socketModel.type = type;
        socketModel.command = command;
        // 判断读取完协议后,是否还有消息需要读取,是则说明有消息体,进行消息体读取
        if (ba.Readble)
        {
            byte[] message;
            ba.read(out message, ba.Length - ba.Position);
            socketModel.message = message;
        }
        ba.Close();
        return socketModel;
    }
    // 发送消息 NetIO.instance.write();
    public void Write(int type, int command, byte[] message)
    {
        // 消息体编码
        ByteArray ba = new ByteArray();
        ba.Write(type);
        ba.Write(command);
        if (message != null)
        {
            ba.Write(message);
        }
        // 长度编码
        ByteArray arr = new ByteArray();
        arr.Write(ba.Length);
        arr.Write(ba.GetBuffer());
        try
        {
            socket.Send(arr.GetBuffer());
        }
        catch (Exception e)
        {
            Debug.Log("网络错误，请重新登陆" + e.Message);
        }

    }
    public string SendRequest(string url, Encoding encoding)
    {
        HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(url);
        webRequest.Method = "GET";
        HttpWebResponse webResponse = (HttpWebResponse)webRequest.GetResponse();
        StreamReader sr = new StreamReader(webResponse.GetResponseStream(), encoding);
        return sr.ReadToEnd();
    }

}