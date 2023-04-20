using System.Collections;
using System.Collections.Generic;
using Unity.Networking.Transport;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance { set; get; }

    //Settings logic
    public int pieceVariationIndex;
    public int homeTeam, awayTeam, opponentTeam;

    //Multiplayer logic
    private int playerCount = -1;
    private int currentTeam = -1;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }

        RegisterEvents();

        //Settings Logic
        homeTeam = 0;
        awayTeam = 1;
    }

    private void RegisterEvents()
    {
        NetUtility.S_WELCOME += OnWelcomeServer;
        NetUtility.C_WELCOME += OnWelcomeClient;

        NetUtility.C_START_GAME += OnStartGameClient;
    }

    private void UnRegisterEvents()
    {
        NetUtility.S_WELCOME -= OnWelcomeServer;
        NetUtility.C_WELCOME -= OnWelcomeClient;
        NetUtility.C_START_GAME -= OnStartGameClient;
    }

    private void OnWelcomeServer(NetMessage msg, NetworkConnection cnn)
    {
        //Client has connected, assign a team and return the message back to him
        NetWelcome nw = msg as NetWelcome;

        //Assign a team
        nw.AssignedTeam = ++playerCount;

        //Return back to client
        Server.Instance.SendToClient(cnn, nw);

        if(playerCount == 1)
        {
            Server.Instance.Broadcast(new NetStartGame());
        }

    }

    private void OnWelcomeClient(NetMessage msg)
    {
        //Recieve the connection message
        NetWelcome nw = msg as NetWelcome;

        currentTeam = nw.AssignedTeam;

        Debug.Log($"My assigned team is {nw.AssignedTeam}");
    }

    private void OnStartGameClient(NetMessage obj)
    {
        //We just need to change the camera
        //if (MenuUI.Instance.homeAndAwayButtonClicked == 1)
        //{
        //    GameUI.Instance.ChangeCamera(CameraAngle.blackTeam);

        //}
        //else if (MenuUI.Instance.homeAndAwayButtonClicked == 0)
        //{
        //    GameUI.Instance.ChangeCamera(CameraAngle.whiteTeam);
        //}
        SceneManager.LoadScene("GameMatch");
        GameUI.Instance.ChangeCamera((currentTeam == 0) ? CameraAngle.whiteTeam : CameraAngle.blackTeam);
        //this.gameObject.SetActive(false);
    }
}
