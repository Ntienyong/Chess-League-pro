using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class NetworkUIManager : MonoBehaviour
{
    public static NetworkUIManager Instance { set; get; }

    public Server server;
    public Client client;

    public const int PORT = 8007;
    public const string URL = "127.0.0.1";

    private void Awake()
    {
        Instance = this;
    }

    public void CreateOnlineHost()
    {
        server.Init(PORT);
        client.Init(URL, PORT);
    }
    public void JoinOnlineHost(string URL)
    {
        client.Init(URL, PORT);
    }
}
