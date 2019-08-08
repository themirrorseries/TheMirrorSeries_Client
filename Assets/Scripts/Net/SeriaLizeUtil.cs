using UnityEngine;
using System.Collections;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System;
using Google.Protobuf;

public class SerializeUtil
{
    public static ByteString Encode(object value)
    {
        MemoryStream ms = new MemoryStream();//创建编码解码的内存流对象
        BinaryFormatter bw = new BinaryFormatter();//二进制流序列化对象
        //将obj对象序列化成二进制数据 写入到内存流
        bw.Serialize(ms, value);
        // result = AESCode.AesEncrypt(result, AESCode.key);
        // 第一种方法,直接从流,这里需要测试下MemoryStream和Stream时候能够转化
        ByteString byteString = ByteString.FromStream(ms);
        ms.Close();
        return byteString;
        // 第二种,从bytes[]数组进行拷贝
        byte[] result = new byte[ms.Length];
        //将流数据 拷贝到结果数组
        Buffer.BlockCopy(ms.GetBuffer(), 0, result, 0, (int)ms.Length);
        byteString = ByteString.CopyFrom(result, 0, result.Length);
        return byteString;
    }

    public static object Decode(ByteString value)
    {
        byte[] bytes = value.ToByteArray();
        // value = AESCode.AesDecrypt(value, AESCode.key);
        MemoryStream ms = new MemoryStream(bytes);//创建编码解码的内存流对象 并将需要反序列化的数据写入其中
        BinaryFormatter bw = new BinaryFormatter();//二进制流序列化对象
        //将流数据反序列化为obj对象
        object result = bw.Deserialize(ms);
        ms.Close();
        return result;

    }
}
