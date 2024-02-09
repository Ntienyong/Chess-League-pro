using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using Unity.Networking.Transport;

public class MultiplayerUI : MonoBehaviour
{

    public static MultiplayerUI Instance;

    [SerializeField] private GameObject selectEnglishClub;
    [SerializeField] private GameObject selectSpanishClub;
    [SerializeField] private GameObject selectGermanClub;
    [SerializeField] private GameObject selectItalianClub;
    [SerializeField] private GameObject selectFrenchClub;
    [SerializeField] private GameObject selectCountry;
    [SerializeField] private GameObject selectBothTeamsColor, selectPiecesType;
    [SerializeField] public bool clubSelectionPanelActive;
    [SerializeField] public int countrySpriteIndex, clubSpriteIndex;
    [SerializeField] private GameObject kitSelectionPanel, kitBackButton;
    [SerializeField] public Button[] englishClubButtons, spanishClubButtons, germanClubButtons, italianClubButtons, frenchClubButtons;

    public Clubs[] englishClubs;
    public Clubs[] spanishClubs;
    public Clubs[] germanClubs;
    public Clubs[] italianClubs;
    public Clubs[] frenchClubs;
    public Clubs teamSelected;
    public Countries[] allCountries;

    [SerializeField] public Sprite[] countries;
    [SerializeField] public Image original;

    [SerializeField] public Image homeTeamVersus;
    public TextMeshProUGUI[] teamsVersusTextUI;

    public GameObject[] homeVariants;
    public GameObject[] awayVariants;
    public GameObject homePieces;
    public GameObject awayPieces;
    public GameObject colorTestPiece;

    public Color colorSelected;

    public int homePieceTypeIndex, awayPieceTypeIndex;
    public int TeamColorIndex, pieceTypeIndex;
    public TextMeshProUGUI TeamColorIndexText, PieceTypeIndexText;

    public GameObject[] testPrefabs;




    // Start is called before the first frame update
    void Start()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        RegisterEvents();

        TeamColorIndexText.text = "First";
        TeamColorIndex = 0;

        pieceTypeIndex = 0;
        PieceTypeIndexText.text = "Classic I";


    }

    private void Update()
    {
        RotatePieces();
    }

    //MULTIPLAYER
    #region

    private void RotatePieces()
    {
        for(int i = 0; i < homeVariants.Length; i++)
        {
            homeVariants[i].transform.Rotate(0, 10 * Time.deltaTime, 0);
            awayVariants[i].transform.Rotate(0, -10 * Time.deltaTime, 0);

        }
    }

    private void PiecesColor()
    {
        for (int j = 0; j < awayVariants.Length; j++)
        {
            homeVariants[j].GetComponent<Renderer>().material.color = teamSelected.piecesColor[0];
            awayVariants[j].GetComponent<Renderer>().material.color = teamSelected.piecesColor[0];
        }
    }
    public void OnFwdArrowButton()
    {
        countrySpriteIndex++;
        Debug.Log(countrySpriteIndex);
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

        else if (countrySpriteIndex == 4)
        {
            original.sprite = countries[4];
        }

        else if (countrySpriteIndex > 4)
        {
            countrySpriteIndex = 0;
            original.sprite = countries[0];
        }

    }

    public void OnBwdArrowButton()
    {
        countrySpriteIndex--;
        Debug.Log(countrySpriteIndex);
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

        else if (countrySpriteIndex < 0)
        {
            countrySpriteIndex = 4;
            original.sprite = countries[4];
        }

    }

    public void OnMultiplayerCountrySelectButton()
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

        else if (countrySpriteIndex == 2)
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

        else if (countrySpriteIndex == 4)
        {
            selectCountry.SetActive(false);
            selectFrenchClub.SetActive(true);
            clubSelectionPanelActive = true;
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
        for (int i = 0; i < englishClubButtons.Length; i++)
        {
            if (buttonPressed == englishClubButtons[i])
            {
                kitSelectionPanel.SetActive(true);
                selectEnglishClub.SetActive(false);

                teamSelected.idNumber = englishClubs[i].idNumber;
                teamSelected.clubName = englishClubs[i].clubName;
                teamSelected.clubShortName = englishClubs[i].clubShortName;
                teamSelected.piecesColor = englishClubs[i].piecesColor;

                colorTestPiece.GetComponent<MeshRenderer>().material.color = teamSelected.piecesColor[TeamColorIndex];

                colorSelected = teamSelected.piecesColor[TeamColorIndex];


                RunOnlineVerifications();
                

                //homeTeamVersus.sprite = englishClubs[i].clubLogo;
                //homePiecesColor = new Color[] { englishClubs[i].piecesColor[0], englishClubs[i].piecesColor[1], englishClubs[i].piecesColor[2] };
                //homeTeamSelected = true;
                //teamSelectionText.text = "Select Away Team";

                //homeTeamColorIndexText.text = "First";
                //homeTeamColorIndex = 0;
                //homeColorPiece.GetComponent<MeshRenderer>().material.color = homePiecesColor[homeTeamColorIndex];
                //homePiece.GetComponent<MeshRenderer>().material.color = homePiecesColor[homeTeamColorIndex];
                //GameManager.instance.homePiecesColor = homePiecesColor[homeTeamColorIndex];
            }
        }
    }

    public void RunOnlineVerifications()
    {
        NetSelectTeam st = new NetSelectTeam();
        st.idNumber = teamSelected.idNumber;
        st.teamId = GameManager.instance.currentTeam;
        st.clubName = teamSelected.clubName;
        st.clubShortName = teamSelected.clubShortName;

        st.r = colorSelected.r;
        st.g = colorSelected.g;
        st.b = colorSelected.b;

        Client.Instance.SendToServer(st);
    }

    //Select & Set Pieces Color
    public void OnSelectColorButton()
    {
        //kitBackButton.SetActive(false);
        selectBothTeamsColor.SetActive(true);
        homeVariants[0].SetActive(false);
        awayVariants[0].SetActive(false);
    }

    public void OnConfirmColorButton()
    {
        //kitBackButton.SetActive(true);
        selectBothTeamsColor.SetActive(false);


        for (int i = 0; i < homeVariants.Length; i++)
        {
            homeVariants[pieceTypeIndex].SetActive(true);
        }

        for (int i = 0; i < awayVariants.Length; i++)
        {
            awayVariants[pieceTypeIndex].SetActive(true);
        }


        colorSelected = teamSelected.piecesColor[TeamColorIndex];

        RunOnlineVerifications();
    }

    public void OnSelectTeamPieceColor()
    {
        TeamColorIndex++;

        if (TeamColorIndex == 1)
        {
            TeamColorIndexText.text = "Second";
            colorTestPiece.GetComponent<MeshRenderer>().material.color = teamSelected.piecesColor[TeamColorIndex];
            //colorSelected = teamSelected.piecesColor[TeamColorIndex];
        }

        if (TeamColorIndex == 2)
        {
            TeamColorIndexText.text = "Third";
            colorTestPiece.GetComponent<MeshRenderer>().material.color = teamSelected.piecesColor[TeamColorIndex];
            //colorSelected = teamSelected.piecesColor[TeamColorIndex];
        }

        if (TeamColorIndex > 2)
        {
            TeamColorIndexText.text = "First";
            TeamColorIndex = 0;
            colorTestPiece.GetComponent<MeshRenderer>().material.color = teamSelected.piecesColor[TeamColorIndex];
            //colorSelected = teamSelected.piecesColor[TeamColorIndex];
        }
    }

    //Select & Set Pieces Type
    public void OnSelectPiecesButton()
    {
        //kitBackButton.SetActive(false);
        selectPiecesType.SetActive(true);
        homeVariants[0].SetActive(false);
        awayVariants[0].SetActive(false);
    }

    public void OnConfirmPiecesButton()
    {
        //kitBackButton.SetActive(true);
        selectPiecesType.SetActive(false);
        awayPieces.SetActive(true);
        homePieces.SetActive(true);
        //if (GameManager.instance.currentTeam == 0)
        //{
        //    for (int i = 0; i < homeVariants.Length; i++)
        //    {
        //        homeVariants[pieceTypeIndex].SetActive(true);
        //    }
        //}

        //if (GameManager.instance.currentTeam != 0)
        //{
        //    for (int i = 0; i < awayVariants.Length; i++)
        //    {
        //        awayVariants[pieceTypeIndex].SetActive(true);
        //    }
        //}


    }

    public void OnSelectTeamPieceType()
    {
        pieceTypeIndex++;

        if (pieceTypeIndex == 1)
        {
            PieceTypeIndexText.text = "Classic II";
            for (int i = 0; i < testPrefabs.Length; i++)
            {
                testPrefabs[i].SetActive(false);
            }
            testPrefabs[pieceTypeIndex].SetActive(true);
        }

        if (pieceTypeIndex > 1)
        {
            PieceTypeIndexText.text = "Classic I";
            pieceTypeIndex = 0;
            for(int i = 0; i < testPrefabs.Length; i++)
            {
                testPrefabs[i].SetActive(false);
            }
            testPrefabs[pieceTypeIndex].SetActive(true);
        }
    }



    public void OnAdvanceToGameButton()
    {
        StartCoroutine(ChangeToMatchScene());
    }

    public IEnumerator ChangeToMatchScene()
    {
        yield return new WaitForSeconds(0.2f);
        SceneManager.LoadScene("GameMatch");

    }
    #endregion

    #region
    private void RegisterEvents()
    {

        NetUtility.S_SELECT_TEAM += OnSelectTeamServer;
        NetUtility.C_SELECT_TEAM += OnSelectTeamClient;

        Debug.Log("got here");
    }

    private void UnRegisterEvents()
    {
        NetUtility.S_SELECT_TEAM -= OnSelectTeamServer;
        NetUtility.C_SELECT_TEAM -= OnSelectTeamClient;
    }

    private void OnSelectTeamServer(NetMessage msg, NetworkConnection cnn)
    {
        NetSelectTeam st = msg as NetSelectTeam;

        Server.Instance.Broadcast(st);
        
    }

    private void OnSelectTeamClient(NetMessage msg)
    {
        NetSelectTeam st = msg as NetSelectTeam;

        teamsVersusTextUI[st.teamId].text = st.clubName;      

        if(st.teamId != 0)
        {
            for(int i = 0; i < awayVariants.Length; i++)
            {
                awayVariants[i].GetComponent<Renderer>().material.color = new Color(st.r, st.g, st.b, 255);

            }
        }

        if(st.teamId == 0)
        {
            for (int i = 0; i < homeVariants.Length; i++)
            {
                homeVariants[i].GetComponent<Renderer>().material.color = new Color(st.r, st.g, st.b, 255);

            }
        }




    }
    #endregion
}