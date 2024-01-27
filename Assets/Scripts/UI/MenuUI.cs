using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuUI : MonoBehaviour
{
    public static MenuUI Instance;

    [SerializeField] private Animator MenuAnimator;
    [SerializeField] private TextMeshProUGUI countryTextUI;
    [SerializeField] public bool isMatchLocal;
    [SerializeField] public int numberOfTimesButtonClicked;
    [SerializeField] public int homeAndAwayButtonClicked;
    [SerializeField] public int homeTeam = 0;
    [SerializeField] public int awayTeam = 1;
    [SerializeField] public int opponentTeam;
    [SerializeField] public float timeLeft;
    [SerializeField] private TextMeshProUGUI timeGivenText;
    [SerializeField] public TextMeshProUGUI homeOrAwayText;
    [SerializeField] public TextMeshProUGUI homeTeamColorIndexText;
    [SerializeField] public TextMeshProUGUI awayTeamColorIndexText;
    [SerializeField] public TextMeshProUGUI pieceTypeText, homePieceTypeText, awayPieceTypeText;
    [SerializeField] public TextMeshProUGUI arenaText, weatherText, timeOfDayText, difficultyText;
    [SerializeField] public TextMeshProUGUI teamSelectionText;
    [SerializeField] public Color[] pieceColor;
    [SerializeField] public Color[] homePiecesColor;
    [SerializeField] public Color[] awayPiecesColor;
    [SerializeField] public int homeTeamColorIndex;
    [SerializeField] public int awayTeamColorIndex;
    [SerializeField] public int pieceTypeIndex, homePieceTypeIndex, awayPieceTypeIndex;
    [SerializeField] public int arenaIndex, weatherIndex, timeOfDayIndex, difficultyIndex;
    [SerializeField] public bool homeTeamSelected;
    [SerializeField] public bool awayTeamSelected;
    [SerializeField] public Button homeColorButton, awayColorButton;
    [SerializeField] public Button[] englishClubButtons, spanishClubButtons, germanClubButtons, italianClubButtons, frenchClubButtons;
    [SerializeField] public Sprite[] countries;
    [SerializeField] public Sprite[] spritePieceColor;
    [SerializeField] public Image original;
    [SerializeField] public Image homeTeamVersus;
    [SerializeField] public Image awayTeamVersus;
    [SerializeField] public bool clubSelectionPanelActive;

    [SerializeField]
    public Clubs[] englishClubs;
    public Clubs[] spanishClubs;
    public Clubs[] germanClubs;
    public Clubs[] italianClubs;
    public Clubs[] frenchClubs;
    public Countries[] allCountries;

    [Header("New UI")]
    [SerializeField] private TextMeshProUGUI modeSelectionTextUI;
    [SerializeField] private TextMeshProUGUI randomSelectionTextUI;
    [SerializeField] public GameObject mainMenu;
    [SerializeField] private GameObject selectPlayMode;
    [SerializeField] public GameObject selectRuleSetSP, selectServerOrClient, waitingForClient, setMultiplayerRules;
    [SerializeField] private GameObject customModeRules;
    [SerializeField] private GameObject RandomModeRules;
    [SerializeField] private GameObject selectBothTeamsColor, selectPiecesType;
    [SerializeField] private GameObject selectTeam;
    [SerializeField] private GameObject selectCountry;
    [SerializeField] private GameObject selectEnglishClub;
    [SerializeField] private GameObject selectSpanishClub;
    [SerializeField] private GameObject selectGermanClub;
    [SerializeField] private GameObject selectItalianClub;
    [SerializeField] private GameObject selectFrenchClub;
    [SerializeField] private GameObject kitSelectionPanel, kitBackButton;
    [SerializeField] private GameObject homePiece, homePieceI, awayPiece, awayPieceI, homeTypePieceI, homeTypePieceII, awayTypePieceI, awayTypePieceII;
    [SerializeField] public GameObject homeColorPiece, awayColorPiece;

    [Header("Network UI Reference")]
    [SerializeField] private TMP_InputField addressInput;

    [Header("Career")]

    [SerializeField] public int countrySpriteIndex, clubSpriteIndex;                                                                                                                                                                                                       



    private void Start()
    {
        Instance = this;

        numberOfTimesButtonClicked = 2;
        timeGivenText.text = "10 Minute";
        GameManager.instance.timeLeft = 600f;

        homeAndAwayButtonClicked = 0;
        opponentTeam = 1;
        homeOrAwayText.text = "Home";

        pieceTypeText.text = "Classic";

        arenaIndex = 0;
        GameManager.instance.stadiumIndex = 0;
        arenaText.text = "Stadium 1";

        timeOfDayIndex = 0;
        difficultyIndex = 0;
        weatherIndex = 0;

        timeOfDayText.text = "Noon";
        difficultyText.text = "Easy";
        weatherText.text = "Sunny";

        //Career
        #region
        countrySpriteIndex = 0;
        clubSpriteIndex = 0;
        //englishClubsActive = true;
        //localTeamsClub.sprite = englishClubs[clubSpriteIndex].clubLogo;
        //localCareerOriginal.sprite = countries[countrySpriteIndex];
        //domesticSuccess.text = englishClubs[clubSpriteIndex].domesticExpectations;
        //continentalSuccess.text = englishClubs[clubSpriteIndex].continentalExpectations;
        //financialSuccess.text = englishClubs[clubSpriteIndex].financialExpectations;
        //firstKit.color = englishClubs[clubSpriteIndex].piecesColor[0];
        //secondKit.color = englishClubs[clubSpriteIndex].piecesColor[1];
        //thirdKit.color = englishClubs[clubSpriteIndex].piecesColor[2];


        //Test
        //for (int i = 0; i < 8; i++)
        //{
        //    GameObject menuButton = Instantiate(menuCard);
        //    menuButton.transform.SetParent(localMenuList.transform, false);
        //    string[] localCareerMenuButtons = new string[] {"Advance", "Squad", "Transfers", "My Office", "Season Stats", "Calender", "Settings", "Quit"};
        //    menuButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = localCareerMenuButtons[i];

        //    int copy = i;
        //    menuButton.GetComponent<Button>().onClick.AddListener(delegate { LocalMenuButtonsCallBack(copy); });
        //}
        #endregion

    }
    private void Update()
    {
        RotatePieces();
    }
    private void RotatePieces()
    {
        homePiece.transform.Rotate(0, 10 * Time.deltaTime, 0);
        homePieceI.transform.Rotate(0, 10 * Time.deltaTime, 0);
        awayPiece.transform.Rotate(0, -10 * Time.deltaTime, 0);
        awayPieceI.transform.Rotate(0, -10 * Time.deltaTime, 0);
    }
    public void OnAdvanceButton()
    {
        MenuAnimator.SetTrigger("TeamSelectAnim");
    }
    #region TeamSelect
    public void OnTeamSelectBackButton()
    {
        MenuAnimator.SetTrigger("PlayerProfileAnim");
    }
    public void OnCountrySelectbutton()
    {
        MenuAnimator.SetTrigger("CountrySelectAnim");
    }
    public void OnCountrySelectedButton()
    {
        MenuAnimator.SetTrigger("TeamSelectAnim");
        //countryTextUI.text =  
    }
    #endregion
    public void OnPlayerProfileBackButton()
    {
        MenuAnimator.SetTrigger("StartAnim");
    }


    //EXHIBITION
    #region
    public void OnPlayButton()
    {
        selectPlayMode.SetActive(true);
        mainMenu.SetActive(false);
    }
    public void OnSelectModeBackButton()
    {
        mainMenu.SetActive(true);
        selectPlayMode.SetActive(false);
    }

    public void OnSinglePlayerButton()
    {
        selectRuleSetSP.SetActive(true);
        modeSelectionTextUI.text = "Single Player";
        selectPlayMode.SetActive(false);
    }
    public void OnSinglePlayerModeBackButton()
    {
        selectPlayMode.SetActive(true);
        selectRuleSetSP.SetActive(false);
    }

    public void OnCustomButton()
    {
        customModeRules.SetActive(true);
        randomSelectionTextUI.text = "Custom Rules";
        selectRuleSetSP.SetActive(false);
    }
    public void OnCustomBackButton()
    {
        selectRuleSetSP.SetActive(true);
        customModeRules.SetActive(false);
    }
    public void OnRandomButton()
    {
        customModeRules.SetActive(true);
        randomSelectionTextUI.text = "Random Rules";
        selectRuleSetSP.SetActive(false);
    }
    public void OnRandomBackButton()
    {
        selectRuleSetSP.SetActive(true);
        customModeRules.SetActive(false);
    }
    public void OnPnPButton()
    {
        selectRuleSetSP.SetActive(true);
        modeSelectionTextUI.text = "Pass & Play";
        selectPlayMode.SetActive(false);
    }
    public void OnOMPButton()
    {
        selectRuleSetSP.SetActive(true);
        modeSelectionTextUI.text = "Online Multiplayer";
        selectPlayMode.SetActive(false);
    }
    public void OnMWFButton()
    {
        selectServerOrClient.SetActive(true);
        selectPlayMode.SetActive(false);
    }
    public void OnHostButton()
    {
        //GameManager.instance.isLocalGame = false;
        GameManager.instance.SetLocalGame?.Invoke(false);
        waitingForClient.SetActive(true);
        NetworkUIManager.Instance.CreateOnlineHost();
        setMultiplayerRules.SetActive(false);
    }
    public void OnHostButtonUpdated()
    {
        setMultiplayerRules.SetActive(true);
        selectServerOrClient.SetActive(false);

    }
    public void OnConnectPlayersBackButton()
    {
        selectServerOrClient.SetActive(false);
        selectPlayMode.SetActive(true);
    }
    public void OnWaitingForPlayerBackButton()
    {
        waitingForClient.SetActive(false);
        NetworkUIManager.Instance.ShutDownServerAndClient();
        selectServerOrClient.SetActive(true);
    }
    public void OnJoinButton()
    {
        GameManager.instance.SetLocalGame?.Invoke(false);
        NetworkUIManager.Instance.JoinOnlineHost(addressInput.text);
    }

    public void OnTimerButton()
    {
        numberOfTimesButtonClicked++;
        Debug.Log(numberOfTimesButtonClicked);
        if (numberOfTimesButtonClicked == 1)
        {
            timeGivenText.text = "5 Minutes";
            GameManager.instance.timeLeft = 300f;
        }
        else if (numberOfTimesButtonClicked == 2)
        {
            timeGivenText.text = "10 Minutes";
            GameManager.instance.timeLeft = 600f;
        }
        else if (numberOfTimesButtonClicked == 3)
        {
            timeGivenText.text = "15 Minutes";
            GameManager.instance.timeLeft = 900f;
        }
        else if (numberOfTimesButtonClicked == 4)
        {
            timeGivenText.text = "20 Minutes";
            GameManager.instance.timeLeft = 1200f;
        }

        if (numberOfTimesButtonClicked > 4)
        {
            numberOfTimesButtonClicked = 0;
            timeGivenText.text = "1 Minute";
            GameManager.instance.timeLeft = 60f;
        }
    }
    public void OnSelectSideButton()
    {
        homeAndAwayButtonClicked++;
        if(homeAndAwayButtonClicked == 1)
        {
            homeOrAwayText.text = "Away";
            //opponentTeam = homeTeam;
            GameManager.instance.opponentTeam = GameManager.instance.homeTeam;
            GameManager.instance.assignedTeam = 1;
            //GameUI.Instance.ChangeCamera(CameraAngle.blackTeam);
        }
        if (homeAndAwayButtonClicked > 1)
        {
            homeAndAwayButtonClicked = 0;
            homeOrAwayText.text = "Home";
            //opponentTeam = awayTeam;
            GameManager.instance.opponentTeam = GameManager.instance.awayTeam;
            GameManager.instance.assignedTeam = 0;
            //GameUI.Instance.ChangeCamera(CameraAngle.whiteTeam);
        }
    }

    public void OnSelectPieceTypeButton()
    {
        homeTypePieceI.GetComponent<MeshRenderer>().material.color = homeColorPiece.GetComponent<MeshRenderer>().material.color;
        homeTypePieceII.GetComponent<MeshRenderer>().material.color = homeColorPiece.GetComponent<MeshRenderer>().material.color;
        awayTypePieceI.GetComponent<MeshRenderer>().material.color = awayColorPiece.GetComponent<MeshRenderer>().material.color;
        awayTypePieceII.GetComponent<MeshRenderer>().material.color = awayColorPiece.GetComponent<MeshRenderer>().material.color;

        selectPiecesType.SetActive(true);
        homePiece.SetActive(false);
        awayPiece.SetActive(false);
        homePieceI.SetActive(false);
        awayPieceI.SetActive(false);
        kitBackButton.SetActive(false);
    }
    public void OnConfirmPieceTypeButton()
    {
        kitBackButton.SetActive(true);
        selectPiecesType.SetActive(false);
        awayPiece.GetComponent<MeshRenderer>().material.color = awayColorPiece.GetComponent<MeshRenderer>().material.color;
        awayPieceI.GetComponent<MeshRenderer>().material.color = awayColorPiece.GetComponent<MeshRenderer>().material.color;
        homePieceI.GetComponent<MeshRenderer>().material.color = homeColorPiece.GetComponent<MeshRenderer>().material.color;
        homePiece.GetComponent<MeshRenderer>().material.color = homeColorPiece.GetComponent<MeshRenderer>().material.color;
        if (awayPieceTypeIndex == 0)
        {
            GameManager.instance.awayPieceTypeIndex = 0;
            awayPiece.SetActive(true);
            awayPieceI.SetActive(false);
        }
        else if(awayPieceTypeIndex == 1)
        {
            GameManager.instance.awayPieceTypeIndex = 1;
            awayPiece.SetActive(false);
            awayPieceI.SetActive(true);
        }
        if(homePieceTypeIndex == 0)
        {
            GameManager.instance.homePieceTypeIndex = 0;
            homePiece.SetActive(true);
            homePieceI.SetActive(false);
        }
        else if(homePieceTypeIndex == 1)
        {
            GameManager.instance.homePieceTypeIndex = 1;
            homePiece.SetActive(false);
            homePieceI.SetActive(true);
        }
    }
    public void OnSelectHomePieceTypeButton()
    {
        homePieceTypeIndex++;
        if (homePieceTypeIndex == 1)
        {
            homePieceTypeText.text = "Classic II";
            homeTypePieceII.SetActive(true);
            homeTypePieceI.SetActive(false);
        }
        if(homePieceTypeIndex > 1)
        {
            homePieceTypeIndex = 0;
            homePieceTypeText.text = "Classic";
            homeTypePieceII.SetActive(false);
            homeTypePieceI.SetActive(true);
        }
    }
    public void OnSelectAwayPieceTypeButton()
    {
        awayPieceTypeIndex++;
        if (awayPieceTypeIndex == 1)
        {
            awayPieceTypeText.text = "Classic II";
            awayTypePieceII.SetActive(true);
            awayTypePieceI.SetActive(false);
        }
        if (awayPieceTypeIndex > 1)
        {
            awayPieceTypeIndex = 0;
            awayPieceTypeText.text = "Classic";
            awayTypePieceII.SetActive(false);
            awayTypePieceI.SetActive(true);
        }
    }

    public void OnSelectArenaButton()
    {
        arenaIndex++;
        if (arenaIndex == 2)
        {
            GameManager.instance.stadiumIndex = 2;
            arenaText.text = "Stadium 3";
        }

        if (arenaIndex == 1)
        {
            GameManager.instance.stadiumIndex = 1;
            arenaText.text = "Stadium 2"; 
        }
        if(arenaIndex > 2)
        {
            arenaIndex = 0;
            GameManager.instance.stadiumIndex = 0;
            arenaText.text = "Stadium 1";
        }
    }

    public void OnSelectDifficultyButton()
    {
        difficultyIndex++;
        if (difficultyIndex == 2)
        {
            difficultyText.text = "Hard";
        }

        if (difficultyIndex == 1)
        {
            difficultyText.text = "Medium";
        }
        if (difficultyIndex > 2)
        {
            difficultyIndex = 0;
            difficultyText.text = "Easy";
        }
    }

    public void OnTimeOfDayButton()
    {
        timeOfDayIndex++;
        if (timeOfDayIndex == 2)
        {
            timeOfDayText.text = "Night";
        }

        if (timeOfDayIndex == 1)
        {
            timeOfDayText.text = "Evening";
        }
        if (timeOfDayIndex > 2)
        {
            timeOfDayIndex = 0;
            timeOfDayText.text = "Noon";
        }
    }

    public void OnWeatherConditionButton()
    {
        weatherIndex++;
        if (weatherIndex == 2)
        {
            weatherText.text = "Snowy";
        }

        if (weatherIndex == 1)
        {
            weatherText.text = "Rainy";
        }
        if (weatherIndex > 2)
        {
            weatherIndex = 0;
            weatherText.text = "Sunny";
        }
    }
    public void OnConfirmSettingsButton()
    {
        customModeRules.SetActive(false);
        selectTeam.SetActive(true);
        teamSelectionText.text = "Select Home Team";
    }

    public void OnSelectCountryBackButton()
    {
        if(homeTeamSelected == false)
        {
            teamSelectionText.text = "Select Home Team";
            customModeRules.SetActive(true);
            selectTeam.SetActive(false);
            //selectEnglishClub.SetActive(false);
            selectCountry.SetActive(true);
        }

        if(homeTeamSelected == true)
        {
            teamSelectionText.text = "Select Home Team";
            //selectEnglishClub.SetActive(true);
            selectCountry.SetActive(true);
            homeTeamSelected = false;
        }
    }

    public void OnFwdArrowButton()
    {
        countrySpriteIndex++;
        if(countrySpriteIndex == 1)
        {
            original.sprite = countries[1];
        }
        else if(countrySpriteIndex == 2)
        {
            original.sprite = countries[2];
        }
        else if(countrySpriteIndex == 3)
        {
            original.sprite= countries[3];
        }

        else if(countrySpriteIndex == 4)
        {
            original.sprite = countries[4];
        }

        if(countrySpriteIndex > 4)
        {
            countrySpriteIndex = 0;
            original.sprite = countries[0];
        }

    }

    public void OnBwdArrowButton()
    {
        countrySpriteIndex--;
        if (countrySpriteIndex == 1)
        {
            original.sprite = countries[1];
        }
        else if (countrySpriteIndex == 2)
        {
            original.sprite = countries[2];
        }
        else if (countrySpriteIndex == 3)
        {
            original.sprite = countries[3];
        }

        else if (countrySpriteIndex == 0)
        {
            original.sprite = countries[0];
        }

        if (countrySpriteIndex < 0)
        {
            countrySpriteIndex = 4;
            original.sprite = countries[4];
        }

    }

    public void OnTeamCountrySelectButton()
    {
        if (countrySpriteIndex == 0)
        {
            selectCountry.SetActive(false);
            selectEnglishClub.SetActive(true);
            clubSelectionPanelActive = true;
        }

        else if (countrySpriteIndex == 1)
        {
            selectCountry.SetActive(false);
            selectSpanishClub.SetActive(true);
            clubSelectionPanelActive = true;
        }

        else if(countrySpriteIndex == 2)
        {
            selectCountry.SetActive(false);
            selectGermanClub.SetActive(true);
            clubSelectionPanelActive = true;
        }

        else if (countrySpriteIndex == 3)
        {
            selectCountry.SetActive(false);
            selectItalianClub.SetActive(true);
            clubSelectionPanelActive = true;
        }

        else if(countrySpriteIndex == 4)
        {
            selectCountry.SetActive(false);
            selectFrenchClub.SetActive(true);
            clubSelectionPanelActive = true;
        }
    }

    public void OnSelectTeamPieceColor()
    {
        homeTeamColorIndex++;
        if (homeTeamColorIndex == 1)
        {
            homeTeamColorIndexText.text = "Second";
            GameManager.instance.homePiecesColor = homePiecesColor[homeTeamColorIndex];
            homeColorPiece.GetComponent<MeshRenderer>().material.color = homePiecesColor[homeTeamColorIndex];
            homePiece.GetComponent<MeshRenderer>().material.color = homePiecesColor[homeTeamColorIndex];
            homePieceI.GetComponent<MeshRenderer>().material.color = homePiecesColor[homeTeamColorIndex];
        }

        if (homeTeamColorIndex == 2)
        {
            homeTeamColorIndexText.text = "Third";
            GameManager.instance.homePiecesColor = homePiecesColor[homeTeamColorIndex];
            homeColorPiece.GetComponent<MeshRenderer>().material.color = homePiecesColor[homeTeamColorIndex];
            homePiece.GetComponent<MeshRenderer>().material.color = homePiecesColor[homeTeamColorIndex];
            homePieceI.GetComponent<MeshRenderer>().material.color = homePiecesColor[homeTeamColorIndex];
        }

        if (homeTeamColorIndex > 2)
        {
            homeTeamColorIndexText.text = "First";
            homeTeamColorIndex = 0;
            GameManager.instance.homePiecesColor = homePiecesColor[homeTeamColorIndex];
            homeColorPiece.GetComponent<MeshRenderer>().material.color = homePiecesColor[homeTeamColorIndex];
            homePiece.GetComponent<MeshRenderer>().material.color = homePiecesColor[homeTeamColorIndex];
            homePieceI.GetComponent<MeshRenderer>().material.color = homePiecesColor[homeTeamColorIndex];
        }

        Debug.Log(GameManager.instance.homePiecesColor);
    }

    public void OnSelectOppPieceColor()
    {
        awayTeamColorIndex++;
        if (awayTeamColorIndex == 1)
        {
            awayTeamColorIndexText.text = "Second";
            GameManager.instance.awayPiecesColor = awayPiecesColor[awayTeamColorIndex];
            
            awayColorPiece.GetComponent<MeshRenderer>().material.color = awayPiecesColor[awayTeamColorIndex];
            awayPiece.GetComponent<MeshRenderer>().material.color = awayPiecesColor[awayTeamColorIndex];
            awayPieceI.GetComponent<MeshRenderer>().material.color = awayPiecesColor[awayTeamColorIndex];
        }

        if (awayTeamColorIndex == 2)
        {
            awayTeamColorIndexText.text = "Third";
            GameManager.instance.awayPiecesColor = awayPiecesColor[awayTeamColorIndex];
            awayColorPiece.GetComponent<MeshRenderer>().material.color = awayPiecesColor[awayTeamColorIndex];
            awayPiece.GetComponent<MeshRenderer>().material.color = awayPiecesColor[awayTeamColorIndex];
            awayPieceI.GetComponent<MeshRenderer>().material.color = awayPiecesColor[awayTeamColorIndex];
        }

        if (awayTeamColorIndex > 2)
        {
            awayTeamColorIndexText.text = "First";
            awayTeamColorIndex = 0;
            GameManager.instance.awayPiecesColor = awayPiecesColor[awayTeamColorIndex];
            awayColorPiece.GetComponent<MeshRenderer>().material.color = awayPiecesColor[awayTeamColorIndex];
            awayPiece.GetComponent<MeshRenderer>().material.color = awayPiecesColor[awayTeamColorIndex];
            awayPieceI.GetComponent<MeshRenderer>().material.color = awayPiecesColor[awayTeamColorIndex];
        }

        Debug.Log(GameManager.instance.awayPiecesColor);
    }

    public void OnSelectColorButton()
    {
        kitBackButton.SetActive(false);
        selectBothTeamsColor.SetActive(true);
        homePiece.SetActive(false);
        awayPiece.SetActive(false);
        homePieceI.SetActive(false);
        awayPieceI.SetActive(false);
    }

    public void OnConfirmColorButton()
    {
        kitBackButton.SetActive(true);
        selectBothTeamsColor.SetActive(false);
        if (awayPieceTypeIndex == 0)
        {
            awayPiece.SetActive(true);
            awayPieceI.SetActive(false);
        }
        else if (awayPieceTypeIndex == 1)
        {
            awayPiece.SetActive(false);
            awayPieceI.SetActive(true);
        }
        if (homePieceTypeIndex == 0)
        {
            homePiece.SetActive(true);
            homePieceI.SetActive(false);
        }
        else if (homePieceTypeIndex == 1)
        {
            homePiece.SetActive(false);
            homePieceI.SetActive(true);
        }
    }

    private void OnEnable()
    {
        for (int i = 0; i < englishClubButtons.Length; i++)
        {
            AddEventEngland(englishClubButtons[i], i);
            //englishClubButtons[i].onClick.AddListener(() => ButtonCallBack(englishClubButtons[i]));
        }

        for (int j = 0; j < spanishClubButtons.Length; j++)
        {
            AddEventSpain(spanishClubButtons[j], j);
        }

        for (int i = 0; i < germanClubButtons.Length; i++)
        {
            AddEventGermany(germanClubButtons[i], i);
        }

        for (int i = 0; i < italianClubButtons.Length; i++) 
        {
            AddEventItaly(italianClubButtons[i], i);
        }

        for (int i = 0; i < frenchClubButtons.Length; i++)
        {
            AddEventFrance(frenchClubButtons[i], i);
        }
    }

    private void AddEventEngland(Button b, int i)
    {
        b.onClick.AddListener(() => ButtonCallBack(englishClubButtons[i]));
    }
    private void AddEventSpain(Button b, int i)
    {
        b.onClick.AddListener(() => ButtonCallBack(spanishClubButtons[i]));
    }
    private void AddEventGermany(Button b, int i)
    {
        b.onClick.AddListener(() => ButtonCallBack(germanClubButtons[i]));
    }
    private void AddEventItaly(Button b, int i)
    {
        b.onClick.AddListener(() => ButtonCallBack(italianClubButtons[i]));
    }
    private void AddEventFrance(Button b, int i)
    {
        b.onClick.AddListener(() => ButtonCallBack(frenchClubButtons[i]));
    }
    private void ButtonCallBack(Button buttonPressed)
    {
        if(homeTeamSelected == false)
        {
            for (int i = 0; i < englishClubButtons.Length; i++)
            {
                if (buttonPressed == englishClubButtons[i])
                {
                    selectEnglishClub.SetActive(false);
                    selectCountry.SetActive(true);
                    homeTeamVersus.sprite = englishClubs[i].clubLogo;
                    homePiecesColor = new Color[] { englishClubs[i].piecesColor[0], englishClubs[i].piecesColor[1], englishClubs[i].piecesColor[2] };
                    homeTeamSelected = true;
                    teamSelectionText.text = "Select Away Team";

                    homeTeamColorIndexText.text = "First";
                    homeTeamColorIndex = 0;
                    homeColorPiece.GetComponent<MeshRenderer>().material.color = homePiecesColor[homeTeamColorIndex];
                    homePiece.GetComponent<MeshRenderer>().material.color = homePiecesColor[homeTeamColorIndex];
                    GameManager.instance.homePiecesColor = homePiecesColor[homeTeamColorIndex];
                }
            }
        }
        else if(homeTeamSelected == true)
        {
            for (int i = 0; i < englishClubButtons.Length; i++)
            {
                if (buttonPressed == englishClubButtons[i])
                {
                    kitSelectionPanel.SetActive(true);
                    selectEnglishClub.SetActive(false);
                    awayTeamVersus.sprite = englishClubs[i].clubLogo;
                    awayPiecesColor = new Color[] { englishClubs[i].piecesColor[0], englishClubs[i].piecesColor[1], englishClubs[i].piecesColor[2] };
                    clubSelectionPanelActive = false;
                    awayTeamSelected = true;

                    awayTeamColorIndexText.text = "First";
                    awayTeamColorIndex = 0;
                    awayColorPiece.GetComponent<MeshRenderer>().material.color = awayPiecesColor[awayTeamColorIndex];
                    awayPiece.GetComponent<MeshRenderer>().material.color = awayPiecesColor[awayTeamColorIndex];
                    GameManager.instance.awayPiecesColor = awayPiecesColor[homeTeamColorIndex];
                }
            }
        }

        if (homeTeamSelected == false)
        {
            for (int i = 0; i < spanishClubButtons.Length; i++)
            {
                if (buttonPressed == spanishClubButtons[i])
                {
                    selectSpanishClub.SetActive(false);
                    selectCountry.SetActive(true);
                    homeTeamVersus.sprite = spanishClubs[i].clubLogo;
                    homePiecesColor = new Color[] { spanishClubs[i].piecesColor[0], spanishClubs[i].piecesColor[1], spanishClubs[i].piecesColor[2] };
                    homeTeamSelected = true;
                    teamSelectionText.text = "Select Away Team";

                    homeTeamColorIndexText.text = "First";
                    homeTeamColorIndex = 0;
                    homeColorPiece.GetComponent<MeshRenderer>().material.color = homePiecesColor[homeTeamColorIndex];
                    homePiece.GetComponent<MeshRenderer>().material.color = homePiecesColor[homeTeamColorIndex];
                    GameManager.instance.homePiecesColor = homePiecesColor[homeTeamColorIndex];
                }
            }
        }
        else if (homeTeamSelected == true)
        {
            for (int i = 0; i < spanishClubButtons.Length; i++)
            {
                if (buttonPressed == spanishClubButtons[i])
                {
                    kitSelectionPanel.SetActive(true);
                    selectSpanishClub.SetActive(false);
                    awayTeamVersus.sprite = spanishClubs[i].clubLogo;
                    awayPiecesColor = new Color[] { spanishClubs[i].piecesColor[0], spanishClubs[i].piecesColor[1], spanishClubs[i].piecesColor[2] };
                    clubSelectionPanelActive = false;
                    awayTeamSelected = true;

                    awayTeamColorIndexText.text = "First";
                    awayTeamColorIndex = 0;
                    awayColorPiece.GetComponent<MeshRenderer>().material.color = awayPiecesColor[awayTeamColorIndex];
                    awayPiece.GetComponent<MeshRenderer>().material.color = awayPiecesColor[awayTeamColorIndex];
                    GameManager.instance.awayPiecesColor = awayPiecesColor[homeTeamColorIndex];
                }
            }
        }

        if (homeTeamSelected == false)
        {
            for (int i = 0; i < germanClubButtons.Length; i++)
            {
                if (buttonPressed == germanClubButtons[i])
                {
                    selectGermanClub.SetActive(false);
                    selectCountry.SetActive(true);
                    homeTeamVersus.sprite = germanClubs[i].clubLogo;
                    homePiecesColor = new Color[] { germanClubs[i].piecesColor[0], germanClubs[i].piecesColor[1], germanClubs[i].piecesColor[2] };
                    homeTeamSelected = true;
                    teamSelectionText.text = "Select Away Team";

                    homeTeamColorIndexText.text = "First";
                    homeTeamColorIndex = 0;
                    homeColorPiece.GetComponent<MeshRenderer>().material.color = homePiecesColor[homeTeamColorIndex];
                    homePiece.GetComponent<MeshRenderer>().material.color = homePiecesColor[homeTeamColorIndex];
                    GameManager.instance.homePiecesColor = homePiecesColor[homeTeamColorIndex];
                }
            }
        }
        else if (homeTeamSelected == true)
        {
            for (int i = 0; i < germanClubButtons.Length; i++)
            {
                if (buttonPressed == germanClubButtons[i])
                {
                    kitSelectionPanel.SetActive(true);
                    selectGermanClub.SetActive(false);
                    awayTeamVersus.sprite = germanClubs[i].clubLogo;
                    awayPiecesColor = new Color[] { germanClubs[i].piecesColor[0], germanClubs[i].piecesColor[1], germanClubs[i].piecesColor[2] };
                    clubSelectionPanelActive = false;
                    awayTeamSelected = true;

                    awayTeamColorIndexText.text = "First";
                    awayTeamColorIndex = 0;
                    awayColorPiece.GetComponent<MeshRenderer>().material.color = awayPiecesColor[awayTeamColorIndex];
                    awayPiece.GetComponent<MeshRenderer>().material.color = awayPiecesColor[awayTeamColorIndex];
                    GameManager.instance.awayPiecesColor = awayPiecesColor[homeTeamColorIndex];
                }
            }
        }

        if (homeTeamSelected == false)
        {
            for (int i = 0; i < italianClubButtons.Length; i++)
            {
                if (buttonPressed == italianClubButtons[i])
                {
                    selectItalianClub.SetActive(false);
                    selectCountry.SetActive(true);
                    homeTeamVersus.sprite = italianClubs[i].clubLogo;
                    homePiecesColor = new Color[] { italianClubs[i].piecesColor[0], italianClubs[i].piecesColor[1], italianClubs[i].piecesColor[2] };
                    homeTeamSelected = true;
                    teamSelectionText.text = "Select Away Team";

                    homeTeamColorIndexText.text = "First";
                    homeTeamColorIndex = 0;
                    homeColorPiece.GetComponent<MeshRenderer>().material.color = homePiecesColor[homeTeamColorIndex];
                    homePiece.GetComponent<MeshRenderer>().material.color = homePiecesColor[homeTeamColorIndex];
                    GameManager.instance.homePiecesColor = homePiecesColor[homeTeamColorIndex];

                }
            }
        }
        else if (homeTeamSelected == true)
        {
            for (int i = 0; i < italianClubButtons.Length; i++)
            {
                if (buttonPressed == italianClubButtons[i])
                {
                    kitSelectionPanel.SetActive(true);
                    selectItalianClub.SetActive(false);
                    awayTeamVersus.sprite = italianClubs[i].clubLogo;
                    awayPiecesColor = new Color[] { italianClubs[i].piecesColor[0], italianClubs[i].piecesColor[1], italianClubs[i].piecesColor[2] };
                    clubSelectionPanelActive = false;
                    awayTeamSelected = true;

                    awayTeamColorIndexText.text = "First";
                    awayTeamColorIndex = 0;
                    awayColorPiece.GetComponent<MeshRenderer>().material.color = awayPiecesColor[awayTeamColorIndex];
                    awayPiece.GetComponent<MeshRenderer>().material.color = awayPiecesColor[awayTeamColorIndex];
                    GameManager.instance.awayPiecesColor = awayPiecesColor[homeTeamColorIndex];
                }
            }
        }

        if (homeTeamSelected == false)
        {
            for (int i = 0; i < frenchClubButtons.Length; i++)
            {
                if (buttonPressed == frenchClubButtons[i])
                {
                    selectFrenchClub.SetActive(false);
                    selectCountry.SetActive(true);
                    homeTeamVersus.sprite = frenchClubs[i].clubLogo;
                    homePiecesColor = new Color[] { frenchClubs[i].piecesColor[0], frenchClubs[i].piecesColor[1], frenchClubs[i].piecesColor[2] };
                    homeTeamSelected = true;
                    teamSelectionText.text = "Select Away Team";

                    homeTeamColorIndexText.text = "First";
                    homeTeamColorIndex = 0;
                    homeColorPiece.GetComponent<MeshRenderer>().material.color = homePiecesColor[homeTeamColorIndex];
                    homePiece.GetComponent<MeshRenderer>().material.color = homePiecesColor[homeTeamColorIndex];
                    GameManager.instance.homePiecesColor = homePiecesColor[homeTeamColorIndex];
                }
            }
        }
        else if (homeTeamSelected == true)
        {
            for (int i = 0; i < frenchClubButtons.Length; i++)
            {
                if (buttonPressed == frenchClubButtons[i])
                {
                    kitSelectionPanel.SetActive(true);
                    selectFrenchClub.SetActive(false);
                    awayTeamVersus.sprite = frenchClubs[i].clubLogo;
                    awayPiecesColor = new Color[] { frenchClubs[i].piecesColor[0], frenchClubs[i].piecesColor[1], frenchClubs[i].piecesColor[2] };
                    clubSelectionPanelActive = false;
                    awayTeamSelected = true;

                    awayTeamColorIndexText.text = "First";
                    awayTeamColorIndex = 0;
                    awayColorPiece.GetComponent<MeshRenderer>().material.color = awayPiecesColor[awayTeamColorIndex];
                    awayPiece.GetComponent<MeshRenderer>().material.color = awayPiecesColor[awayTeamColorIndex];
                    GameManager.instance.awayPiecesColor = awayPiecesColor[homeTeamColorIndex];
                }
            }
        }
    }
    public void OnClubSelectBackButton()
    {
        selectEnglishClub.SetActive(false);
        selectFrenchClub.SetActive(false);
        selectGermanClub.SetActive(false);
        selectItalianClub.SetActive(false);
        selectSpanishClub.SetActive(false);
        selectCountry.SetActive(true);
        clubSelectionPanelActive = false;
    }
    public void OnKitSelectionBackButton()
    {
        selectCountry.SetActive(true);
        kitSelectionPanel.SetActive(false);
        awayTeamSelected = false;
        selectEnglishClub.SetActive(false);
        selectSpanishClub.SetActive(false);
        selectGermanClub.SetActive(false);
        selectItalianClub.SetActive(false);
        selectFrenchClub.SetActive(false);

        
    }
    public void OnHomeTeamSelectedButton()
    {

    }

    public void OnAwayTeamSelectedButton()
    {

    }
    public IEnumerator ChangeToGameScene()
    {
        yield return new WaitForSeconds(0.2f);
        SceneManager.LoadScene("Multiplayer");

    }
    public void OnClassicButton()
    {
        NetworkUIManager.Instance.CreateOnlineHost();

        GameManager.instance.SetLocalGame?.Invoke(true);
    }
    #endregion

    //CAREER
 
}
