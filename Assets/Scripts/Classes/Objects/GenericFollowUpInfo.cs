using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public class GenericFollowUpInfo
{
    [SerializeField] private int infoID;
    [SerializeField] private string followUpInfo;

    //Ctrl Shift P then search generate set and get methods

    public GenericFollowUpInfo(int infoID, string followUpInfo) {
        this.infoID = infoID;
        this.followUpInfo = followUpInfo;
    }

    // ==============================================================================================================
    // |                                    Code to get the extra proposal info                                     |
    // ==============================================================================================================

    public int getInfoID()
    {
        return this.infoID;
    }

    public string getInfo()
    {
        return this.followUpInfo;
    }
}
