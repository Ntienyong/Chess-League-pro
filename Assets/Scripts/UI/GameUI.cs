using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public enum CameraAngle
{
    menu = 0,
    whiteTeam = 1,
    blackTeam = 2
}



public class GameUI : MonoBehaviour
{
    public static GameUI Instance { set; get; }

    public Server server;
    public Client client;

    [SerializeField] private Animator menuAnimator;
    [SerializeField] private TMP_InputField addressInput;
    [SerializeField] private GameObject[] cameraAngles;

    [Header("Timer Test")]
    [SerializeField] public float whiteTimeLeft;
    [SerializeField] public bool whiteTimerOn;
    [SerializeField] public float blackTimeLeft;
    [SerializeField] public bool blackTimerOn;
    [SerializeField] public TextMeshProUGUI whiteTimerText;
    [SerializeField] public TextMeshProUGUI blackTimerText;

    public Action<bool> SetLocalGame;

    private void Awake()
    {
        Instance = this;
        RegisterEvents();
        menuAnimator.SetTrigger("InGameMenu");
        whiteTimeLeft = MenuUI.Instance.timeLeft;
        blackTimeLeft = MenuUI.Instance.timeLeft;
        whiteTimerText.text = "00 : 00";
        blackTimerText.text = "00 : 00";

        if(MenuUI.Instance.isMatchLocal == true)
        {
            menuAnimator.SetTrigger("InGameMenu");
            SetLocalGame?.Invoke(true);
            server.Init(8007);
            client.Init("127.0.0.1", 8007);
        }
        
    }

    //Cameras
    public void ChangeCamera(CameraAngle index)
    {
        for(int i = 0; i < cameraAngles.Length; i++)
            cameraAngles[i].SetActive(false);

        cameraAngles[(int)index].SetActive(true);
    }

    //Buttons
    public void OnLocalGameButton()
    {

        menuAnimator.SetTrigger("InGameMenu");
        SetLocalGame?.Invoke(true);
        server.Init(8007);
        client.Init("127.0.0.1", 8007);


        SceneManager.LoadScene("GameMatch");
    }

    public void OnOnlineGameButton()
    {
        menuAnimator.SetTrigger("OnlineMenu");
    }

    public void OnOnlineHostButton()
    {
        SetLocalGame?.Invoke(false);
        server.Init(8007);
        client.Init("127.0.0.1", 8007);
        menuAnimator.SetTrigger("HostMenu");
    }

    public void OnOnlineConnectButton()
    {
        SetLocalGame?.Invoke(false);
        client.Init(addressInput.text, 8007);
    }

    public void OnOnlineBackButton()
    {
        menuAnimator.SetTrigger("StartMenu");
    }

    public void OnHostBackButton()
    {
        server.Shutdown();
        client.Shutdown();
        menuAnimator.SetTrigger("OnlineMenu");
    }

    public void OnLeaveFromGameMenu()
    {
        //ChangeCamera(CameraAngle.menu);
        //menuAnimator.SetTrigger("StartMenu");
        SceneManager.LoadScene("MenuScene");
    }

    #region
    private void RegisterEvents()
    {
        NetUtility.C_START_GAME += OnStartGameClient;
    }
    private void UnRegisterEvents()
    {
        NetUtility.C_START_GAME -= OnStartGameClient;
    }

    private void OnStartGameClient(NetMessage obj)
    {
        menuAnimator.SetTrigger("InGameMenu");
    }
    #endregion
}
