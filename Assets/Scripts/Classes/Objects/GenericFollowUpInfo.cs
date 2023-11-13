using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public class GenericFollowUpInfo
{
    [SerializeField] private int infoID;
    [SerializeField] private string infoTitle;
    [SerializeField] private List<string> infoDescription;

    //Ctrl Shift P then search generate set and get methods

    public GenericFollowUpInfo(int infoID, string infoTitle, List<string> infoDescription) {
        this.infoID = infoID;
        this.infoTitle = infoTitle;
        this.infoDescription = infoDescription;
    }

    // ==============================================================================================================
    // |                                    Code to get the extra proposal info                                     |
    // ==============================================================================================================

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
