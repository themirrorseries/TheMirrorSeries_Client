using UnityEngine;
using System.Collections;
using System.IO;
using System;


public class ByteArray
{

    //创建内存流对象，并将缓存数据写进去
    MemoryStream ms = new MemoryStream();
    BinaryReader br;
    BinaryWriter wr;

    public ByteArray()
    {
        br = new BinaryReader(ms);
        wr = new BinaryWriter(ms);
    }

    public ByteArray(byte[] buff)
    {
        ms = new MemoryStream(buff);
        br = new BinaryReader(ms);
        wr = new BinaryWriter(ms);
    }
    public void Close()
    {
        br.Close();
        wr.Close();
        ms.Close();
    }
    public bool Readble
    {
        get { return ms.Length > ms.Position; }
    }
    public int Length
    {
        get { return (int)ms.Length; }
    }
    public int Position
    {
        get { return (int)ms.Position; }
    }

    public void read(out int value)
    {
        value = br.ReadInt32();
    }

    public void read(out byte value)
    {
        value = br.ReadByte();
    }

    public void read(out bool value)
    {
        value = br.ReadBoolean();
    }
    public void read(out string value)
    {
        value = br.ReadString();
    }
    public void read(out byte[] value, int length)
    {
        value = br.ReadBytes(length);
    }
    public void read(out double value)
    {
        value = br.ReadDouble();
    }
    public void read(out float value)
    {
        value = br.ReadSingle();
    }
    public void read(out long value)
    {
        value = br.ReadInt64();
    }

    public void Write(int value)
    {
        wr.Write(value);
    }
    public void Write(byte value)
    {
        wr.Write(value);
    }
    public void Write(byte[] value)
    {
        wr.Write(value);
    }
    public void Write(bool value)
    {
        wr.Write(value);
    }
    public void Write(float value)
    {
        wr.Write(value);
    }
    public void Write(long value)
    {
        wr.Write(value);
    }
    public void Write(double value)
    {
        wr.Write(value);
    }
    public void Write(string value)
    {
        wr.Write(value);
    }

    public void reposition()
    {
        ms.Position = 0;
    }

    public byte[] GetBuffer()
    {
        byte[] result = new byte[ms.Length];
        Buffer.BlockCopy(ms.GetBuffer(), 0, result, 0, (int)ms.Length);
        return result;

    }
}