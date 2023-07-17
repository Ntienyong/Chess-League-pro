using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Networking.Transport;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{
    public static GameManager instance { set; get; }

    //Settings logic
    public float timeLeft;
    public int homeTeam, awayTeam, opponentTeam;
    public int homePieceTypeIndex, awayPieceTypeIndex;
    public Color homePiecesColor, awayPiecesColor;
    public int stadiumIndex;

    //Multiplayer logic
    public int playerCount = -1;
    public int currentTeam = -1;

    //LocalPlayer Logic
    public Action<bool> SetLocalGame;
    public bool localGame = false;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        
        RegisterEvents();

        //Settings Logic
        //homeTeam = 0;
        //awayTeam = 1;
    }

    private void RegisterEvents()
    {
        NetUtility.S_WELCOME += OnWelcomeServer;
        NetUtility.C_WELCOME += OnWelcomeClient;

        NetUtility.C_START_GAME += OnStartGameClient;

        //NetUtility.S_CHANGE_TIME += OnChangeTimeServer;

        SetLocalGame += OnSetLocalGame;
    }

    private void UnRegisterEvents()
    {
        NetUtility.S_WELCOME -= OnWelcomeServer;
        NetUtility.C_WELCOME -= OnWelcomeClient;
        NetUtility.C_START_GAME -= OnStartGameClient;

        SetLocalGame -= OnSetLocalGame;
    }

    private void OnWelcomeServer(NetMessage msg, NetworkConnection cnn)
    {
        //Client has connected, assign a team and return the message back to him
        NetWelcome nw = msg as NetWelcome;

        Debug.Log(playerCount);
        //Assign a team
        nw.AssignedTeam = ++playerCount;
        

        //Return back to client
        Server.Instance.SendToClient(cnn, nw);

        if (playerCount == 1)
        {
            Server.Instance.Broadcast(new NetStartGame());
        }

    }

    private void OnWelcomeClient(NetMessage msg)
    {
        //Recieve the connection message
        Debug.Log("called by client");

        NetWelcome nw = msg as NetWelcome;

       currentTeam = nw.AssignedTeam;

        Debug.Log($"My assigned team is {nw.AssignedTeam}");
        Debug.Log("Local game is "  + localGame);

        if (localGame && (currentTeam == 0))
            Server.Instance.Broadcast(new NetStartGame());
    }

    private void OnStartGameClient(NetMessage obj)
    {
        MenuUI.Instance.StartCoroutine(MenuUI.Instance.ChangeToGameScene());
        //SceneManager.LoadScene("GameMatch");
    }

    private void OnSetLocalGame(bool v)
    {
        Debug.Log("setting local called");
        playerCount = -1;
        currentTeam = -1;
        localGame = v;
    }
}