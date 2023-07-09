using System.Collections;
using System.Collections.Generic;
using Unity.Networking.Transport;
using UnityEngine;

public class NetChangeTime : NetMessage
{
    public int timeAssigned;

    public NetChangeTime()
    {
        Code = OpCode.CHANGE_TIME;
    }
    public NetChangeTime(DataStreamReader reader)
    {
        Code = OpCode.CHANGE_TIME;
        Deserialize(reader);
    }
    public override void Serialize(ref DataStreamWriter writer)
    {
        writer.WriteByte((byte)Code);
        writer.WriteInt(timeAssigned);
    }
    public override void Deserialize(DataStreamReader reader)
    {
        timeAssigned = reader.ReadInt();
    }

    public override void ReceivedOnClient()
    {
        NetUtility.C_CHANGE_TIME?.Invoke(this);
    }
    public override void ReceivedOnServer(NetworkConnection cnn)
    {
        NetUtility.S_CHANGE_TIME?.Invoke(this, cnn);
    }
}
