using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public enum CameraAngle
{
    menu = 0,
    whiteTeam = 1,
    blackTeam = 2
}



public class GameUI : MonoBehaviour
{
    public static GameUI Instance { set; get; }

    //public Server server;
    //public Client client;

    [SerializeField] private Animator menuAnimator;
    //[SerializeField] private TMP_InputField addressInput;
    [SerializeField] public GameObject[] cameraAngles;

    [Header("Timer Test")]
    [SerializeField] public float whiteTimeLeft;
    [SerializeField] public bool whiteTimerOn;
    [SerializeField] public float blackTimeLeft;
    [SerializeField] public bool blackTimerOn;
    [SerializeField] public TextMeshProUGUI whiteTimerText;
    [SerializeField] public TextMeshProUGUI blackTimerText;

    private void Awake()
    {
        Instance = this;
        //RegisterEvents();
        //menuAnimator.SetTrigger("InGameMenu");
        whiteTimeLeft = GameManager.instance.timeLeft;
        blackTimeLeft = GameManager.instance.timeLeft;
        whiteTimerText.text = "00 : 00";
        blackTimerText.text = "00 : 00";

        //ChangeCamera((GameManager.instance.currentTeam == 0) ? CameraAngle.whiteTeam : CameraAngle.blackTeam);

    }

    private void Start()
    {
        //Debug.Log("started game");
        //ChangeCamera(CameraAngle.blackTeam);

        StartCoroutine(ChangeTheCameras());
    }

    private IEnumerator ChangeTheCameras()
    {
        yield return new WaitForSeconds(0.5f);

        ChangeCamera((GameManager.instance.currentTeam == 0) ? CameraAngle.whiteTeam : CameraAngle.blackTeam);
    }

    private void OnEnable()
    {
        
    }

    //Cameras
    public void ChangeCamera(CameraAngle index)
    {
        for (int i = 0; i < cameraAngles.Length; i++)
        {
            cameraAngles[i].SetActive(false);
            
        }

        cameraAngles[(int)index].SetActive(true);
    }


    public void OnOnlineBackButton()
    {
        menuAnimator.SetTrigger("StartMenu");
    }

    //public void OnHostBackButton()
    //{
    //    server.Shutdown();
    //    client.Shutdown();
    //    menuAnimator.SetTrigger("OnlineMenu");
    //}

    public IEnumerator OnLeaveFromGameMenu()
    {
        //ChangeCamera(CameraAngle.menu);
        //menuAnimator.SetTrigger("StartMenu");
        yield return new WaitForSeconds(0.2f);
        SceneManager.LoadScene("MenuScene");
    }

    public void StartCouroutineLoadScene()
    {
        StartCoroutine(OnLeaveFromGameMenu());
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
