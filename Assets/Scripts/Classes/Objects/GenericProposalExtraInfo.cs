using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public class GenericProposalExtraInfo
{
    //For employing people, this type is "PotentialEmployees"
    //For information on a group, this is "Information"
    //For information on an SCP/tale, this is "Background"
    [SerializeField] private string extraInfoType;

    //For "PotentialEmployees", this will be multiple, for anything else, probably 1
    [SerializeField] private int infoAmount;

    [SerializeField] private int infoID;
    [SerializeField] private string infoTitle;
    [SerializeField] private string infoDescription;

    //Ctrl Shift P then search generate set and get methods

public GenericProposalExtraInfo(string extraInfoType, int infoID, int infoAmount, string infoTitle, string infoDescription) {
        this.extraInfoType = extraInfoType;
        this.infoAmount = infoAmount;
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

    public int getInfoAmount()
    {
        return this.infoAmount;
    }

    public int getInfoID()
    {
        return this.infoID;
    }

    public string getInfoTitle()
    {
        return this.infoTitle;
    }

    public string getInfoDescription()
    {
        return this.infoDescription;
    }
}
