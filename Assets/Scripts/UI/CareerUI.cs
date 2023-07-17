using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CareerUI : MonoBehaviour
{
    //public Clubs[] englishClubs;
    //public Clubs[] spanishClubs;
    //public Clubs[] germanClubs;
    //public Clubs[] italianClubs;
    //public Clubs[] frenchClubs;
    //public Countries[] allCountries;
    [SerializeField] public Sprite[] countries;
    [SerializeField] public bool englishClubsActive, spanishClubsActive, germanClubsActive, italianClubsActive, frenchClubsActive;
    [SerializeField] public TextMeshProUGUI domesticSuccess, continentalSuccess, financialSuccess;

    [SerializeField] private GameObject careerSelectType;
    [SerializeField] private GameObject selectLocalCareerTeam, selectNewOrLoadCareer, setPlayerProfile, nationalityPanel;
    [SerializeField] private GameObject nationalityPrefabParent;
    [SerializeField] private bool nationalityPicked;
    [SerializeField] private GameObject nationalityPrefab, favouriteTeamPrefab, favouriteTeamPrefabParent, favouriteTeamPanel, nationalityArrow, profilePageBackButton;
    [SerializeField] private TextMeshProUGUI nationalityText, favouriteTeamText;
    [SerializeField] private GameObject clubCard, leagueTable, menuCard, localMenuList, localCareerSubMenuList, calenderCard;
    [SerializeField] private bool matchDay, advanceBool, squadBool, transferBool, myOfficeBool, seasonStatsBool, calenderBool, settingsBool, quitBool;
    [SerializeField] bool advanceButtonPressedAlready;
    [SerializeField] public Image firstKit, secondKit, thirdKit;
    [SerializeField] public int /*countrySpriteIndex,*/ clubSpriteIndex;
    [SerializeField] public Image localCareerOriginal, localTeamsClub;
    [SerializeField] public int  numberofMonthDays, years;
    [SerializeField] public string[] weekDays, months;


    //CareerProfile
    [SerializeField] private string localCareerClubName;
    [SerializeField] private Image localCareerClubLogo;
    [SerializeField] private string localCareermanagername;

    //SimCalender
    public GameObject calender;
    public ScrollRect calenderScrollRect;
    public RectTransform calenderContentTransform;
    public float scrollSpeed = 10f;
    public float delayBetweenItems = 1f;

    private int currentIndex = 0;
    private int numItems;
    private Vector2 lastContentPosition;

    // Start is called before the first frame update

    private void Start()
    {
        //Career
        MenuUI.Instance.countrySpriteIndex = 0;
        clubSpriteIndex = 0;
        englishClubsActive = true;
        localTeamsClub.sprite = MenuUI.Instance.englishClubs[clubSpriteIndex].clubLogo;
        localCareerOriginal.sprite = MenuUI.Instance.countries[MenuUI.Instance.countrySpriteIndex];
        domesticSuccess.text = MenuUI.Instance.englishClubs[clubSpriteIndex].domesticExpectations;
        continentalSuccess.text = MenuUI.Instance.englishClubs[clubSpriteIndex].continentalExpectations;
        financialSuccess.text = MenuUI.Instance.englishClubs[clubSpriteIndex].financialExpectations;

        firstKit.color = MenuUI.Instance.englishClubs[clubSpriteIndex].piecesColor[0];
        secondKit.color = MenuUI.Instance.englishClubs[clubSpriteIndex].piecesColor[1];
        thirdKit.color = MenuUI.Instance.englishClubs[clubSpriteIndex].piecesColor[2];

        LocalCareerLeagueTable();

        //Calender imulation
        // Get the number of items in the scroll list
        numItems = calenderContentTransform.childCount;
        // Set the lastContentPosition to the current content position
        lastContentPosition = calenderScrollRect.content.anchoredPosition;


        //Test
        for (int i = 0; i < 8; i++)
        {
            GameObject menuButton = Instantiate(menuCard);
            menuButton.transform.SetParent(localMenuList.transform, false);
            string[] localCareerMenuButtons = new string[] { "Advance", "Squad", "Transfers", "My Office", "Season Stats", "Calender", "Settings", "Quit" };
            menuButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = localCareerMenuButtons[i];

            int copy = i;
            menuButton.GetComponent<Button>().onClick.AddListener(delegate { LocalMenuButtonsCallBack(copy); });
        }

        //Calender Calculation
        //Number of days in a month

        //for (int i = 0; i < months.Length; i++)
        //{
        //    Debug.Log(months[i]);
        //    if(i == 9 || i == 4 || i == 6 || i == 11)
        //    {
        //        numberofMonthDays = 30;
        //    }
        //    else if (i == 1 || i == 3 || i ==5 || i == 7 || i == 8 || i == 10 || i == 120)
        //    {
        //        numberofMonthDays = 31;
        //    }
        //    else if(i == 2)
        //    {
        //        numberofMonthDays = 28;
        //    }
        //}
        ////Week simulation
        //for (int i = 0; i < weekDays.Length; i++)
        //{
        //    if(i > weekDays.Length)
        //    {
        //        i = 1;
        //    }
        //}

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnCareerSelectButton()
    {
        MenuUI.Instance.mainMenu.SetActive(false);
        careerSelectType.SetActive(true);
    }
    public void OnSelectCareerModeBackButton()
    {
        MenuUI.Instance.mainMenu.SetActive(true);
        careerSelectType.SetActive(false);
    }
    public void OnLocalCareerButton()
    {
        selectNewOrLoadCareer.SetActive(true);
        careerSelectType.SetActive(false);
    }
    public void OnLocalCareerBackButton()
    {
        selectNewOrLoadCareer.SetActive(false);
        careerSelectType.SetActive(true);
    }
    public void OnNewLocalCareerButton()
    {
        selectNewOrLoadCareer.SetActive(false);
        selectLocalCareerTeam.SetActive(true);
    }

    public void OnSelectNeworLoadPageBackButton()
    {
        selectNewOrLoadCareer.SetActive(false);
        careerSelectType.SetActive(true);
    }

    public void OnTeamSelectPageBackButton()
    {
        selectNewOrLoadCareer.SetActive(true);
        selectLocalCareerTeam.SetActive(false);
    }

    //Team select page

    public void OnCountryFowardButton()
    {
        MenuUI.Instance.countrySpriteIndex++;
        clubSpriteIndex = 0;
        if (MenuUI.Instance.countrySpriteIndex == 1)
        {
            spanishClubsActive = true;
            englishClubsActive = false;
            germanClubsActive = false;
            italianClubsActive = false;
            frenchClubsActive = false;
            localTeamsClub.sprite = MenuUI.Instance.spanishClubs[0].clubLogo;
            domesticSuccess.text = MenuUI.Instance.spanishClubs[clubSpriteIndex].domesticExpectations;
            continentalSuccess.text = MenuUI.Instance.spanishClubs[clubSpriteIndex].continentalExpectations;
            financialSuccess.text = MenuUI.Instance.spanishClubs[clubSpriteIndex].financialExpectations;
            firstKit.color = MenuUI.Instance.spanishClubs[clubSpriteIndex].piecesColor[0];
            secondKit.color = MenuUI.Instance.spanishClubs[clubSpriteIndex].piecesColor[1];
            thirdKit.color = MenuUI.Instance.spanishClubs[clubSpriteIndex].piecesColor[2];
        }
        else if (MenuUI.Instance.countrySpriteIndex == 2)
        {
            germanClubsActive = true;
            englishClubsActive = false;
            spanishClubsActive = false;
            italianClubsActive = false;
            frenchClubsActive = false;
            localTeamsClub.sprite = MenuUI.Instance.germanClubs[0].clubLogo;
            domesticSuccess.text = MenuUI.Instance.germanClubs[clubSpriteIndex].domesticExpectations;
            continentalSuccess.text = MenuUI.Instance.germanClubs[clubSpriteIndex].continentalExpectations;
            financialSuccess.text = MenuUI.Instance.germanClubs[clubSpriteIndex].financialExpectations;
            firstKit.color = MenuUI.Instance.germanClubs[clubSpriteIndex].piecesColor[0];
            secondKit.color = MenuUI.Instance.germanClubs[clubSpriteIndex].piecesColor[1];
            thirdKit.color = MenuUI.Instance.germanClubs[clubSpriteIndex].piecesColor[2];
        }
        else if (MenuUI.Instance.countrySpriteIndex == 3)
        {
            italianClubsActive = true;
            englishClubsActive = false;
            spanishClubsActive = false;
            germanClubsActive = false;
            frenchClubsActive = false;
            localTeamsClub.sprite = MenuUI.Instance.italianClubs[0].clubLogo;
            domesticSuccess.text = MenuUI.Instance.italianClubs[clubSpriteIndex].domesticExpectations;
            continentalSuccess.text = MenuUI.Instance.italianClubs[clubSpriteIndex].continentalExpectations;
            financialSuccess.text = MenuUI.Instance.italianClubs[clubSpriteIndex].financialExpectations;
            firstKit.color = MenuUI.Instance.italianClubs[clubSpriteIndex].piecesColor[0];
            secondKit.color = MenuUI.Instance.italianClubs[clubSpriteIndex].piecesColor[1];
            thirdKit.color = MenuUI.Instance.italianClubs[clubSpriteIndex].piecesColor[2];
        }
        else if (MenuUI.Instance.countrySpriteIndex == 4)
        {
            frenchClubsActive = true;
            englishClubsActive = false;
            spanishClubsActive = false;
            germanClubsActive = false;
            italianClubsActive = false;
            localTeamsClub.sprite = MenuUI.Instance.frenchClubs[0].clubLogo;
            domesticSuccess.text = MenuUI.Instance.frenchClubs[clubSpriteIndex].domesticExpectations;
            continentalSuccess.text = MenuUI.Instance.frenchClubs[clubSpriteIndex].continentalExpectations;
            financialSuccess.text = MenuUI.Instance.frenchClubs[clubSpriteIndex].financialExpectations;
            firstKit.color = MenuUI.Instance.frenchClubs[clubSpriteIndex].piecesColor[0];
            secondKit.color = MenuUI.Instance.frenchClubs[clubSpriteIndex].piecesColor[1];
            thirdKit.color = MenuUI.Instance.frenchClubs[clubSpriteIndex].piecesColor[2];
        }
        else if (MenuUI.Instance.countrySpriteIndex > 4)
        {
            MenuUI.Instance.countrySpriteIndex = 0;
            englishClubsActive = true;
            spanishClubsActive = false;
            germanClubsActive = false;
            italianClubsActive = false;
            frenchClubsActive = false;
            localTeamsClub.sprite = MenuUI.Instance.englishClubs[0].clubLogo;
            domesticSuccess.text = MenuUI.Instance.englishClubs[clubSpriteIndex].domesticExpectations;
            continentalSuccess.text = MenuUI.Instance.englishClubs[clubSpriteIndex].continentalExpectations;
            financialSuccess.text = MenuUI.Instance.englishClubs[clubSpriteIndex].financialExpectations;
            firstKit.color = MenuUI.Instance.englishClubs[clubSpriteIndex].piecesColor[0];
            secondKit.color = MenuUI.Instance.englishClubs[clubSpriteIndex].piecesColor[1];
            thirdKit.color = MenuUI.Instance.englishClubs[clubSpriteIndex].piecesColor[2];
            //localCareerOriginal.sprite = countries[MenuUI.Instance.countrySpriteIndex];
        }
        localCareerOriginal.sprite = countries[MenuUI.Instance.countrySpriteIndex];

    }
    public void OnCountryBackwardButton()
    {
        MenuUI.Instance.countrySpriteIndex--;
        clubSpriteIndex = 0;
        if (MenuUI.Instance.countrySpriteIndex == 1)
        {
            spanishClubsActive = true;
            englishClubsActive = false;
            germanClubsActive = false;
            italianClubsActive = false;
            frenchClubsActive = false;
            localTeamsClub.sprite = MenuUI.Instance.spanishClubs[0].clubLogo;
            domesticSuccess.text = MenuUI.Instance.spanishClubs[clubSpriteIndex].domesticExpectations;
            continentalSuccess.text = MenuUI.Instance.spanishClubs[clubSpriteIndex].continentalExpectations;
            financialSuccess.text = MenuUI.Instance.spanishClubs[clubSpriteIndex].financialExpectations;
            firstKit.color = MenuUI.Instance.spanishClubs[clubSpriteIndex].piecesColor[0];
            secondKit.color = MenuUI.Instance.spanishClubs[clubSpriteIndex].piecesColor[1];
            thirdKit.color = MenuUI.Instance.spanishClubs[clubSpriteIndex].piecesColor[2];
        }
        else if (MenuUI.Instance.countrySpriteIndex == 2)
        {
            germanClubsActive = true;
            englishClubsActive = false;
            spanishClubsActive = false;
            italianClubsActive = false;
            frenchClubsActive = false;
            localTeamsClub.sprite = MenuUI.Instance.germanClubs[0].clubLogo;
            domesticSuccess.text = MenuUI.Instance.germanClubs[clubSpriteIndex].domesticExpectations;
            continentalSuccess.text = MenuUI.Instance.germanClubs[clubSpriteIndex].continentalExpectations;
            financialSuccess.text = MenuUI.Instance.germanClubs[clubSpriteIndex].financialExpectations;
            firstKit.color = MenuUI.Instance.germanClubs[clubSpriteIndex].piecesColor[0];
            secondKit.color = MenuUI.Instance.germanClubs[clubSpriteIndex].piecesColor[1];
            thirdKit.color = MenuUI.Instance.germanClubs[clubSpriteIndex].piecesColor[2];
        }
        else if (MenuUI.Instance.countrySpriteIndex == 3)
        {
            italianClubsActive = true;
            englishClubsActive = false;
            spanishClubsActive = false;
            germanClubsActive = false;
            frenchClubsActive = false;
            localTeamsClub.sprite = MenuUI.Instance.italianClubs[0].clubLogo;
            domesticSuccess.text = MenuUI.Instance.italianClubs[clubSpriteIndex].domesticExpectations;
            continentalSuccess.text = MenuUI.Instance.italianClubs[clubSpriteIndex].continentalExpectations;
            financialSuccess.text = MenuUI.Instance.italianClubs[clubSpriteIndex].financialExpectations;
            firstKit.color = MenuUI.Instance.italianClubs[clubSpriteIndex].piecesColor[0];
            secondKit.color = MenuUI.Instance.italianClubs[clubSpriteIndex].piecesColor[1];
            thirdKit.color = MenuUI.Instance.italianClubs[clubSpriteIndex].piecesColor[2];
        }
        else if (MenuUI.Instance.countrySpriteIndex == 0)
        {
            frenchClubsActive = false;
            englishClubsActive = true;
            spanishClubsActive = false;
            germanClubsActive = false;
            italianClubsActive = false;
            localTeamsClub.sprite = MenuUI.Instance.englishClubs[0].clubLogo;
            domesticSuccess.text = MenuUI.Instance.englishClubs[clubSpriteIndex].domesticExpectations;
            continentalSuccess.text = MenuUI.Instance.englishClubs[clubSpriteIndex].continentalExpectations;
            financialSuccess.text = MenuUI.Instance.englishClubs[clubSpriteIndex].financialExpectations;
            firstKit.color = MenuUI.Instance.englishClubs[clubSpriteIndex].piecesColor[0];
            secondKit.color = MenuUI.Instance.englishClubs[clubSpriteIndex].piecesColor[1];
            thirdKit.color = MenuUI.Instance.englishClubs[clubSpriteIndex].piecesColor[2];
        }
        else if (MenuUI.Instance.countrySpriteIndex < 0)
        {
            MenuUI.Instance.countrySpriteIndex = 4;
            englishClubsActive = false;
            spanishClubsActive = false;
            germanClubsActive = false;
            italianClubsActive = false;
            frenchClubsActive = true;
            localTeamsClub.sprite = MenuUI.Instance.frenchClubs[0].clubLogo;
            domesticSuccess.text = MenuUI.Instance.frenchClubs[clubSpriteIndex].domesticExpectations;
            continentalSuccess.text = MenuUI.Instance.frenchClubs[clubSpriteIndex].continentalExpectations;
            financialSuccess.text = MenuUI.Instance.frenchClubs[clubSpriteIndex].financialExpectations;
            firstKit.color = MenuUI.Instance.frenchClubs[clubSpriteIndex].piecesColor[0];
            secondKit.color = MenuUI.Instance.frenchClubs[clubSpriteIndex].piecesColor[1];
            thirdKit.color = MenuUI.Instance.frenchClubs[clubSpriteIndex].piecesColor[2];
        }
        localCareerOriginal.sprite = countries[MenuUI.Instance.countrySpriteIndex];

    }
    public void OnClubFowardButton()
    {
        clubSpriteIndex++;
        if (spanishClubsActive)
        {
            if (clubSpriteIndex > 19)
            {
                clubSpriteIndex = 0;
            }
            localTeamsClub.sprite = MenuUI.Instance.spanishClubs[clubSpriteIndex].clubLogo;
            domesticSuccess.text = MenuUI.Instance.spanishClubs[clubSpriteIndex].domesticExpectations;
            continentalSuccess.text = MenuUI.Instance.spanishClubs[clubSpriteIndex].continentalExpectations;
            financialSuccess.text = MenuUI.Instance.spanishClubs[clubSpriteIndex].financialExpectations;
            firstKit.color = MenuUI.Instance.spanishClubs[clubSpriteIndex].piecesColor[0];
            secondKit.color = MenuUI.Instance.spanishClubs[clubSpriteIndex].piecesColor[1];
            thirdKit.color = MenuUI.Instance.spanishClubs[clubSpriteIndex].piecesColor[2];
        }
        else if (englishClubsActive)
        {
            if (clubSpriteIndex > 19)
            {
                clubSpriteIndex = 0;
            }
            localTeamsClub.sprite = MenuUI.Instance.englishClubs[clubSpriteIndex].clubLogo;
            domesticSuccess.text = MenuUI.Instance.englishClubs[clubSpriteIndex].domesticExpectations;
            continentalSuccess.text = MenuUI.Instance.englishClubs[clubSpriteIndex].continentalExpectations;
            financialSuccess.text = MenuUI.Instance.englishClubs[clubSpriteIndex].financialExpectations;
            firstKit.color = MenuUI.Instance.englishClubs[clubSpriteIndex].piecesColor[0];
            secondKit.color = MenuUI.Instance.englishClubs[clubSpriteIndex].piecesColor[1];
            thirdKit.color = MenuUI.Instance.englishClubs[clubSpriteIndex].piecesColor[2];
        }
        else if (germanClubsActive)
        {
            if (clubSpriteIndex > 17)
            {
                clubSpriteIndex = 0;
            }
            localTeamsClub.sprite = MenuUI.Instance.germanClubs[clubSpriteIndex].clubLogo;
            domesticSuccess.text = MenuUI.Instance.germanClubs[clubSpriteIndex].domesticExpectations;
            continentalSuccess.text = MenuUI.Instance.germanClubs[clubSpriteIndex].continentalExpectations;
            financialSuccess.text = MenuUI.Instance.germanClubs[clubSpriteIndex].financialExpectations;
            firstKit.color = MenuUI.Instance.germanClubs[clubSpriteIndex].piecesColor[0];
            secondKit.color = MenuUI.Instance.germanClubs[clubSpriteIndex].piecesColor[1];
            thirdKit.color = MenuUI.Instance.germanClubs[clubSpriteIndex].piecesColor[2];
        }
        else if (italianClubsActive)
        {
            if (clubSpriteIndex > 19)
            {
                clubSpriteIndex = 0;
            }
            localTeamsClub.sprite = MenuUI.Instance.italianClubs[clubSpriteIndex].clubLogo;
            domesticSuccess.text = MenuUI.Instance.italianClubs[clubSpriteIndex].domesticExpectations;
            continentalSuccess.text = MenuUI.Instance.italianClubs[clubSpriteIndex].continentalExpectations;
            financialSuccess.text = MenuUI.Instance.italianClubs[clubSpriteIndex].financialExpectations;
            firstKit.color = MenuUI.Instance.italianClubs[clubSpriteIndex].piecesColor[0];
            secondKit.color = MenuUI.Instance.italianClubs[clubSpriteIndex].piecesColor[1];
            thirdKit.color = MenuUI.Instance.italianClubs[clubSpriteIndex].piecesColor[2];
        }
        else if (frenchClubsActive)
        {
            if (clubSpriteIndex > 19)
            {
                clubSpriteIndex = 0;
            }
            localTeamsClub.sprite = MenuUI.Instance.frenchClubs[clubSpriteIndex].clubLogo;
            domesticSuccess.text = MenuUI.Instance.frenchClubs[clubSpriteIndex].domesticExpectations;
            continentalSuccess.text = MenuUI.Instance.frenchClubs[clubSpriteIndex].continentalExpectations;
            financialSuccess.text = MenuUI.Instance.frenchClubs[clubSpriteIndex].financialExpectations;
            firstKit.color = MenuUI.Instance.frenchClubs[clubSpriteIndex].piecesColor[0];
            secondKit.color = MenuUI.Instance.frenchClubs[clubSpriteIndex].piecesColor[1];
            thirdKit.color = MenuUI.Instance.frenchClubs[clubSpriteIndex].piecesColor[2];
        }


    }

    public void OnClubBackwardButton()
    {
        clubSpriteIndex--;
        if (spanishClubsActive)
        {
            if (clubSpriteIndex < 0)
            {
                clubSpriteIndex = 19;
            }
            localTeamsClub.sprite = MenuUI.Instance.spanishClubs[clubSpriteIndex].clubLogo;
            domesticSuccess.text = MenuUI.Instance.spanishClubs[clubSpriteIndex].domesticExpectations;
            continentalSuccess.text = MenuUI.Instance.spanishClubs[clubSpriteIndex].continentalExpectations;
            financialSuccess.text = MenuUI.Instance.spanishClubs[clubSpriteIndex].financialExpectations;
            firstKit.color = MenuUI.Instance.spanishClubs[clubSpriteIndex].piecesColor[0];
            secondKit.color = MenuUI.Instance.spanishClubs[clubSpriteIndex].piecesColor[1];
            thirdKit.color = MenuUI.Instance.spanishClubs[clubSpriteIndex].piecesColor[2];
        }
        else if (englishClubsActive)
        {
            if (clubSpriteIndex < 0)
            {
                clubSpriteIndex = 19;
            }
            localTeamsClub.sprite = MenuUI.Instance.englishClubs[clubSpriteIndex].clubLogo;
            domesticSuccess.text = MenuUI.Instance.englishClubs[clubSpriteIndex].domesticExpectations;
            continentalSuccess.text = MenuUI.Instance.englishClubs[clubSpriteIndex].continentalExpectations;
            financialSuccess.text = MenuUI.Instance.englishClubs[clubSpriteIndex].financialExpectations;
            firstKit.color = MenuUI.Instance.englishClubs[clubSpriteIndex].piecesColor[0];
            secondKit.color = MenuUI.Instance.englishClubs[clubSpriteIndex].piecesColor[1];
            thirdKit.color = MenuUI.Instance.englishClubs[clubSpriteIndex].piecesColor[2];

        }
        else if (germanClubsActive)
        {
            if (clubSpriteIndex < 0)
            {
                clubSpriteIndex = 17;
            }
            localTeamsClub.sprite = MenuUI.Instance.germanClubs[clubSpriteIndex].clubLogo;
            domesticSuccess.text = MenuUI.Instance.germanClubs[clubSpriteIndex].domesticExpectations;
            continentalSuccess.text = MenuUI.Instance.germanClubs[clubSpriteIndex].continentalExpectations;
            financialSuccess.text = MenuUI.Instance.germanClubs[clubSpriteIndex].financialExpectations;
            firstKit.color = MenuUI.Instance.germanClubs[clubSpriteIndex].piecesColor[0];
            secondKit.color = MenuUI.Instance.germanClubs[clubSpriteIndex].piecesColor[1];
            thirdKit.color = MenuUI.Instance.germanClubs[clubSpriteIndex].piecesColor[2];
        }
        else if (italianClubsActive)
        {
            if (clubSpriteIndex < 0)
            {
                clubSpriteIndex = 19;
            }
            localTeamsClub.sprite = MenuUI.Instance.italianClubs[clubSpriteIndex].clubLogo;
            domesticSuccess.text = MenuUI.Instance.italianClubs[clubSpriteIndex].domesticExpectations;
            continentalSuccess.text = MenuUI.Instance.italianClubs[clubSpriteIndex].continentalExpectations;
            financialSuccess.text = MenuUI.Instance.italianClubs[clubSpriteIndex].financialExpectations;
            firstKit.color = MenuUI.Instance.italianClubs[clubSpriteIndex].piecesColor[0];
            secondKit.color = MenuUI.Instance.italianClubs[clubSpriteIndex].piecesColor[1];
            thirdKit.color = MenuUI.Instance.italianClubs[clubSpriteIndex].piecesColor[2];
        }
        else if (frenchClubsActive)
        {
            if (clubSpriteIndex < 0)
            {
                clubSpriteIndex = 19;
            }
            localTeamsClub.sprite = MenuUI.Instance.frenchClubs[clubSpriteIndex].clubLogo;
            domesticSuccess.text = MenuUI.Instance.frenchClubs[clubSpriteIndex].domesticExpectations;
            continentalSuccess.text = MenuUI.Instance.frenchClubs[clubSpriteIndex].continentalExpectations;
            financialSuccess.text = MenuUI.Instance.frenchClubs[clubSpriteIndex].financialExpectations;
            firstKit.color = MenuUI.Instance.frenchClubs[clubSpriteIndex].piecesColor[0];
            secondKit.color = MenuUI.Instance.frenchClubs[clubSpriteIndex].piecesColor[1];
            thirdKit.color = MenuUI.Instance.frenchClubs[clubSpriteIndex].piecesColor[2];

        }
    }
    public void OnClubAdvaceButton()
    {
        setPlayerProfile.SetActive(true);
        selectLocalCareerTeam.SetActive(false);
    }

    //Profile Page
    public void OnNationalityButton()
    {
        for (int i = 0; i <= 20; i++)
        {
            if (!nationalityPicked)
            {
                GameObject cell = Instantiate(nationalityPrefab);
                cell.transform.SetParent(nationalityPrefabParent.transform, false);

                int copy = i;
                cell.GetComponent<Button>().onClick.AddListener(delegate { NationalityButtonCallBack(copy); });
                cell.transform.GetComponentInChildren<TextMeshProUGUI>().text = MenuUI.Instance.allCountries[i].countryName;
                profilePageBackButton.SetActive(false);
                nationalityArrow.SetActive(false);
            }
            nationalityPanel.SetActive(true);

            if (nationalityPicked)
            {
                nationalityPanel.SetActive(true);
                profilePageBackButton.SetActive(false);
                nationalityArrow.SetActive(false);
            }
        }
    }
    private void NationalityButtonCallBack(int buttonPressed)
    {
        nationalityPanel.SetActive(false);
        nationalityText.text = MenuUI.Instance.allCountries[buttonPressed].countryName;
        nationalityPicked = true;
        profilePageBackButton.SetActive(true);
        nationalityArrow.SetActive(true);
    }

    public void OnFavouriteTeamButton()
    {
        for (int i = 0; i <= 19; i++)
        {
            GameObject cell = Instantiate(favouriteTeamPrefab);
            cell.transform.SetParent(favouriteTeamPrefabParent.transform, false);

            int copy = i;
            cell.GetComponent<Button>().onClick.AddListener(delegate { FavouriteTeamButtonCallBack(copy); });
            cell.transform.GetComponentInChildren<TextMeshProUGUI>().text = MenuUI.Instance.spanishClubs[i].clubName;
        }
        favouriteTeamPanel.SetActive(true);
    }

    private void FavouriteTeamButtonCallBack(int buttonPressed)
    {
        favouriteTeamPanel.SetActive(false);
        favouriteTeamText.text = MenuUI.Instance.spanishClubs[buttonPressed].clubName;
    }

    public void OnProfilePageBackButton()
    {
        setPlayerProfile.SetActive(false);
        selectLocalCareerTeam.SetActive(true);
        nationalityPanel.SetActive(false);
    }



    //Local Career Page
    public void SimCalender()
    {
        if(currentIndex < numItems)
        {
            // Get the position of the current item
            RectTransform currentRectTransform = calenderContentTransform.GetChild(currentIndex).GetComponent<RectTransform>();

            // Calculate the target position for scrolling
            Vector2 targetPosition = new Vector2(currentRectTransform.anchoredPosition.x, 0f);

            // If the scroll view is not scrolling
            if (calenderScrollRect.content.anchoredPosition == lastContentPosition || calenderScrollRect.velocity == Vector2.zero)
            {
                // If the current position is not equal to the target position, scroll towards the target position
                if (calenderScrollRect.content.anchoredPosition != targetPosition)
                {
                    calenderScrollRect.content.anchoredPosition = Vector2.Lerp(calenderScrollRect.content.anchoredPosition, targetPosition, scrollSpeed * Time.deltaTime);
                }
                // If the current position is equal to the target position, move to the next item
                else
                {
                    currentIndex++;
                    // Delay before scrolling to the next item
                    Invoke("ScrollToNextCard", delayBetweenItems);
                }
            }
            // Update the lastContentPosition
            lastContentPosition = calenderScrollRect.content.anchoredPosition;
        }
    }

    void ScrollToNextCard()
    {
        // Get the position of the next item
        RectTransform nextRectTransform = calenderContentTransform.GetChild(currentIndex).GetComponent<RectTransform>();

        // Calculate the target position for scrolling
        Vector2 targetPosition = new Vector2(nextRectTransform.anchoredPosition.x, 0f);

        // Scroll towards the target position
        calenderScrollRect.content.anchoredPosition = targetPosition;
    }

    //local Career MenuButtons
    private void LocalCareerMenuButtons()
    {

    }
    private void LocalMenuButtonsCallBack(int buttonPressed)
    {

        //ADVANCE BUTTON
        if (buttonPressed == 0)
        {
            //if there is no match
            if (!matchDay)
            {
                calender.SetActive(true);
                //Spawn stop imulation button
                for (int i = 0; i < 1; i++)
                {
                    GameObject advaceChildren = Instantiate(menuCard);
                    advaceChildren.transform.SetParent(localCareerSubMenuList.transform, false);
                    advaceChildren.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "Stop Simulation";
                    advanceBool = true;
                    //SIM CALENDER!
                    for (int j = 0; j < 12; j++)
                    {
                        GameObject simDatesChildren = Instantiate(calenderCard);
                        simDatesChildren.transform.SetParent(calenderContentTransform.transform, false);
                        SimCalender();
                        //simDatesChildren.transform.Translate(new Vector2(3, transform.position.y) * 3 * Time.deltaTime);

                    }

                    int copy = i;
                    advaceChildren.GetComponent<Button>().onClick.AddListener(delegate { MenuSubButtonsCallBack(copy); });

                }
                localMenuList.SetActive(false);
                localCareerSubMenuList.SetActive(true);
            }

            else if (matchDay)
            {
                for (int i = 0; i < 3; i++)
                {
                    GameObject advaceChildren = Instantiate(menuCard);
                    advaceChildren.transform.SetParent(localCareerSubMenuList.transform, false);
                    string[] subMenuList = new string[] { "Play Match", "Sim Match", "Back" };
                    advaceChildren.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = subMenuList[i];
                    advanceBool = true;

                    int copy = i;
                    advaceChildren.GetComponent<Button>().onClick.AddListener(delegate { MenuSubButtonsCallBack(copy); });

                }
                localMenuList.SetActive(false);
                localCareerSubMenuList.SetActive(true);

            }

        }

        //SQUAD BUTTON
        else if (buttonPressed == 1)
        {

            for (int i = 0; i < 3; i++)
            {
                GameObject advaceChildren = Instantiate(menuCard);
                advaceChildren.transform.SetParent(localCareerSubMenuList.transform, false);
                string[] subMenuList = new string[] { "Team Management", "Squad Report", "Back" };
                advaceChildren.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = subMenuList[i];
                squadBool = true;

                int copy = i;
                advaceChildren.GetComponent<Button>().onClick.AddListener(delegate { MenuSubButtonsCallBack(copy); });

            }
            localMenuList.SetActive(false);
            localCareerSubMenuList.SetActive(true);
            //Spawn new buttons
        }

        //TRANSFER BUTTON
        else if (buttonPressed == 2)
        {
            localMenuList.SetActive(false);

            for (int i = 0; i < 6; i++)
            {
                GameObject advaceChildren = Instantiate(menuCard);
                advaceChildren.transform.SetParent(localCareerSubMenuList.transform, false);
                string[] subMenuList = new string[] { "Buy Pieces", "Sell Pieces", "Shortlist", "Negotiations", "Budget Allocation", "Back" };
                advaceChildren.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = subMenuList[i];
                transferBool = true;

                int copy = i;
                advaceChildren.GetComponent<Button>().onClick.AddListener(delegate { MenuSubButtonsCallBack(copy); });

            }
            localMenuList.SetActive(false);
            localCareerSubMenuList.SetActive(true);
        }

        //MY OFFICE
        else if (buttonPressed == 3)
        {
            localMenuList.SetActive(false);

            for (int i = 0; i < 5; i++)
            {
                GameObject advaceChildren = Instantiate(menuCard);
                advaceChildren.transform.SetParent(localCareerSubMenuList.transform, false);
                string[] subMenuList = new string[] { "Objectives", "Request Funds", "My Jobs", "My Career", "Back" };
                advaceChildren.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = subMenuList[i];
                myOfficeBool = true;

                int copy = i;
                advaceChildren.GetComponent<Button>().onClick.AddListener(delegate { MenuSubButtonsCallBack(copy); });

            }
            localMenuList.SetActive(false);
            localCareerSubMenuList.SetActive(true);
        }

        //SEASON STATS
        else if (buttonPressed == 4)
        {
            localMenuList.SetActive(false);

            for (int i = 0; i < 3; i++)
            {
                GameObject advaceChildren = Instantiate(menuCard);
                advaceChildren.transform.SetParent(localCareerSubMenuList.transform, false);
                string[] subMenuList = new string[] { "Club Competitions", "Other Leagues", "Back" };
                advaceChildren.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = subMenuList[i];
                seasonStatsBool = true;

                int copy = i;
                advaceChildren.GetComponent<Button>().onClick.AddListener(delegate { MenuSubButtonsCallBack(copy); });

            }
            localMenuList.SetActive(false);
            localCareerSubMenuList.SetActive(true);
        }

        //CALENDER
        else if (buttonPressed == 5)
        {
            //Turn off all buttons in container
            //Spawn new buttons
        }

        //SETTINGS
        else if (buttonPressed == 6)
        {
            localMenuList.SetActive(false);

            for (int i = 0; i < 4; i++)
            {
                GameObject advaceChildren = Instantiate(menuCard);
                advaceChildren.transform.SetParent(localCareerSubMenuList.transform, false);
                string[] subMenuList = new string[] { "Save", "Game Settings", "Tracks", "Back" };
                advaceChildren.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = subMenuList[i];
                settingsBool = true;

                int copy = i;
                advaceChildren.GetComponent<Button>().onClick.AddListener(delegate { MenuSubButtonsCallBack(copy); });

            }
            localMenuList.SetActive(false);
            localCareerSubMenuList.SetActive(true);
        }

        //QUIT
        else if (buttonPressed == 7)
        {
            localMenuList.SetActive(false);

            for (int i = 0; i < 3; i++)
            {
                GameObject advaceChildren = Instantiate(menuCard);
                advaceChildren.transform.SetParent(localCareerSubMenuList.transform, false);
                string[] subMenuList = new string[] { "Yes", "No", "Save" };
                advaceChildren.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = subMenuList[i];
                quitBool = true;

                int copy = i;
                advaceChildren.GetComponent<Button>().onClick.AddListener(delegate { MenuSubButtonsCallBack(copy); });

            }

            localMenuList.SetActive(false);
            localCareerSubMenuList.SetActive(true);
        }
    }

    private void MenuSubButtonsCallBack(int buttonPressed)
    {
        //if advance is true and match day is true
        if (advanceBool && matchDay)
        {
            if (buttonPressed == 0)
            {
                //play match Page
            }

            else if (buttonPressed == 1)
            {
                //Sim Match
            }

            else if (buttonPressed == 2)
            {
                // back button
                //localCareerSubMenuList.SetActive(false);
                localMenuList.SetActive(true);
                for (int i = 0; i < localCareerSubMenuList.transform.childCount; i++)
                {
                    Transform child = localCareerSubMenuList.transform.GetChild(i);
                    Destroy(child.gameObject);
                }
                advanceBool = false;
                //advanceButtonPressedAlready = true;
            }
        }

        //if advance is true and matchday is false
        else if (advanceBool && !matchDay)
        {
            //stop simulation
            if (buttonPressed == 0)
            {
                localMenuList.SetActive(true);
                for (int i = 0; i < localCareerSubMenuList.transform.childCount; i++)
                {
                    Transform child = localCareerSubMenuList.transform.GetChild(i);
                    Destroy(child.gameObject);
                }
                advanceBool = false;
                //advanceButtonPressedAlready = true;
            }
        }

        //if squad is true
        if (squadBool)
        {
            if (buttonPressed == 0)
            {
                //Team Management
            }

            else if (buttonPressed == 1)
            {
                //Squad Report
            }
            else if (buttonPressed == 2)
            {
                //back
                localMenuList.SetActive(true);
                for (int i = 0; i < localCareerSubMenuList.transform.childCount; i++)
                {
                    Transform child = localCareerSubMenuList.transform.GetChild(i);
                    Destroy(child.gameObject);
                }
                squadBool = false;
            }
        }
        //if transfer is true
        if (transferBool)
        {
            if (buttonPressed == 0)
            {
                //Buy Piece
            }

            else if (buttonPressed == 1)
            {
                //Sell piece
            }
            else if (buttonPressed == 2)
            {
                //ShortList
            }
            else if (buttonPressed == 3)
            {
                //Negotiations
            }
            else if (buttonPressed == 4)
            {
                //Budget Allocation
            }
            else if (buttonPressed == 5)
            {
                //Back
                localMenuList.SetActive(true);
                for (int i = 0; i < localCareerSubMenuList.transform.childCount; i++)
                {
                    Transform child = localCareerSubMenuList.transform.GetChild(i);
                    Destroy(child.gameObject);
                }
                transferBool = false;
            }
        }
        //if My Office is active
        if (myOfficeBool)
        {
            if (buttonPressed == 0)
            {
                //Objectives
            }
            else if (buttonPressed == 1)
            {
                //Request Funds
            }
            else if (buttonPressed == 2)
            {
                //My Jobs
            }
            else if (buttonPressed == 3)
            {
                //My Career
            }
            else if (buttonPressed == 4)
            {
                //Back
                localMenuList.SetActive(true);
                for (int i = 0; i < localCareerSubMenuList.transform.childCount; i++)
                {
                    Transform child = localCareerSubMenuList.transform.GetChild(i);
                    Destroy(child.gameObject);
                }
                myOfficeBool = false;
            }
        }
        //if Season Stats is true
        if (seasonStatsBool)
        {
            if (buttonPressed == 0)
            {
                //Club Competitons
            }

            else if (buttonPressed == 1)
            {
                //Other Leagues
            }

            else if (buttonPressed == 2)
            {
                //Back
                localMenuList.SetActive(true);
                for (int i = 0; i < localCareerSubMenuList.transform.childCount; i++)
                {
                    Transform child = localCareerSubMenuList.transform.GetChild(i);
                    Destroy(child.gameObject);
                }
                seasonStatsBool = false;
            }
        }
        //if Settings is active
        if (settingsBool)
        {
            if (buttonPressed == 0)
            {
                //Save
            }
            else if (buttonPressed == 1)
            {
                //Game Settings
            }
            else if (buttonPressed == 2)
            {
                //Tracks
            }
            else if (buttonPressed == 3)
            {
                //back
                localMenuList.SetActive(true);
                for (int i = 0; i < localCareerSubMenuList.transform.childCount; i++)
                {
                    Transform child = localCareerSubMenuList.transform.GetChild(i);
                    Destroy(child.gameObject);
                }
                settingsBool = false;
            }
        }
        //if Quit is Active
        if (quitBool)
        {
            if (buttonPressed == 0)
            {
                //Quit career Mode
            }

            else if (buttonPressed == 1)
            {
                //Don't Quit
                localMenuList.SetActive(true);
                for (int i = 0; i < localCareerSubMenuList.transform.childCount; i++)
                {
                    Transform child = localCareerSubMenuList.transform.GetChild(i);
                    Destroy(child.gameObject);
                }
                quitBool = false;
            }
            else if (buttonPressed == 2)
            {
                //Save
            }
        }

    }
    //Local Career League Table
    private void LocalCareerLeagueTable()
    {

        for (int i = 0; i <= 19; i++)
        {
            GameObject cardCell = Instantiate(clubCard);
            cardCell.transform.SetParent(leagueTable.transform, false);
            cardCell.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = (i + 1).ToString();
            cardCell.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = MenuUI.Instance.englishClubs[i].clubShortName;
            cardCell.transform.GetChild(2).GetComponent<Image>().sprite = MenuUI.Instance.englishClubs[i].clubLogo;
        }
    }
}
