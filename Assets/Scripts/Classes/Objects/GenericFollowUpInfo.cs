using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public class GenericFollowUpInfo
{
    [SerializeField] private int followUpID;
    [SerializeField] private string followUpInfo;

    //Ctrl Shift P then search generate set and get methods

    public GenericFollowUpInfo(int followUpID, string followUpInfo) {
        this.followUpID = followUpID;
        this.followUpInfo = followUpInfo;
    }

    // ==============================================================================================================
    // |                                    Code to get the extra proposal info                                     |
    // ==============================================================================================================

    public int getInfoID()
    {
        return this.followUpID;
    }

    public string getInfo()
    {
        return this.followUpInfo;
    }
}
