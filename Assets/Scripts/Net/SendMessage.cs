using UnityEngine;
using System.Collections;

public static class SendMessage
{
    public static void WriteMessage(this MonoBehaviour mono, int type, int command, byte[] message)
    {
        NetIO.Instance.Write(type, command, message);
    }
}
