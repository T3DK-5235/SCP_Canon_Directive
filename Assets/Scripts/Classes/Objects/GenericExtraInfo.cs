using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public class GenericExtraInfo
{
    //For employing people, this type is "PotentialEmployees"
    //For information on a group, this is "Background"
    //For information on an SCP/tale, this is "Information"
    [SerializeField] private string extraInfoType;

    [SerializeField] private int infoID;
    [SerializeField] private string infoTitle;
    [SerializeField] private List<string> infoDescription;

    //Ctrl Shift P then search generate set and get methods

    public GenericExtraInfo(string extraInfoType, int infoID, string infoTitle, List<string> infoDescription) {
        this.extraInfoType = extraInfoType;
        this.infoID = infoID;
        this.infoTitle = infoTitle;
        this.infoDescription = infoDescription;
    }

    // ==============================================================================================================
    // |                                    Code to get the extra proposal info                                     |
    // ==============================================================================================================
    
    public string getExtraInfoType()
    {
        return this.extraInfoType;
    }

    public int getInfoID()
    {
        return this.infoID;
    }

    public string getInfoTitle()
    {
        return this.infoTitle;
    }

    public List<string> getInfoDescription()
    {
        return this.infoDescription;
    }
}
