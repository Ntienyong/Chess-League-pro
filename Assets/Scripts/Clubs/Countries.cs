using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Countries
{
    public int idNumber;
    public string countryName;
    public string leagues;
    public Sprite countryFlag;

    public Countries(int idNumber, string countryName, string leagues, Sprite countryFlag)
    {
        this.idNumber = idNumber;
        this.countryName = countryName;
        this.leagues = leagues;
        this.countryFlag = countryFlag;
    }
}
