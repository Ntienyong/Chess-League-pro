using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[System.Serializable]
public class Clubs
{
    public int idNumber;
    public string clubName;
    public string clubShortName;
    public string league;
    public string country;
    public Sprite clubLogo;
    public Color[] piecesColor;
    public string domesticExpectations, continentalExpectations, financialExpectations;

    public Clubs(int idNumber, string clubName, string clubShortName, string league, string country, Sprite clubLogo, Color[] piecesColor, string domesticExpectations, string continentalExpectations, string financialExpectations)
    {
        this.idNumber = idNumber;
        this.clubName = clubName;
        this.clubShortName = clubShortName;
        this.league = league;
        this.country = country;
        this.clubLogo = clubLogo;
        this.piecesColor = piecesColor;
        this.domesticExpectations = domesticExpectations;
        this.continentalExpectations = continentalExpectations;
        this.financialExpectations = financialExpectations;
    }
}
