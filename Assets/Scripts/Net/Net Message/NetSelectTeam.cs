using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Networking.Transport;
using UnityEngine;

public class NetSelectTeam : NetMessage
{
    public int idNumber;

    public int teamId;
    public string clubName { set; get; }
    public string clubShortName;

    public float r;
    public float g;
    public float b;

    //public string league;
    //public string country;
    //public Sprite clubLogo;
    //public Color pieceColor;

    public NetSelectTeam()
    {
        Code = OpCode.SELECT_TEAM;
    }

    public NetSelectTeam(DataStreamReader reader)
    {
        Code = OpCode.SELECT_TEAM;
        Deserialize(reader);
    }

    public override void Serialize(ref DataStreamWriter writer)
    {
        writer.WriteByte((byte)Code);
        writer.WriteInt(idNumber);
        writer.WriteInt(teamId);
        writer.WriteFixedString128(clubName);
        writer.WriteFixedString128(clubShortName);
        writer.WriteFloat(r);
        writer.WriteFloat(g);
        writer.WriteFloat(b);
    }

    public override void Deserialize(DataStreamReader reader)
    {
        idNumber = reader.ReadInt();
        teamId = reader.ReadInt();
        clubName = reader.ReadFixedString128().ToString();
        clubShortName = reader.ReadFixedString128().ToString();
        r = reader.ReadFloat();
        g = reader.ReadFloat();
        b = reader.ReadFloat();

    }

    public override void ReceivedOnClient()
    {
        NetUtility.C_SELECT_TEAM?.Invoke(this);
    }
    public override void ReceivedOnServer(NetworkConnection cnn)
    {
        NetUtility.S_SELECT_TEAM?.Invoke(this, cnn);
    }

}
