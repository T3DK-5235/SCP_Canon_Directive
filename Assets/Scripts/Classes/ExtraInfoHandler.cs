using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExtraInfoHandler : MonoBehaviour
{
    GenericExtraInfo relevantInfoObject;
    [SerializeField] HiddenGameVariables hiddenGameVariables;

    [SerializeField] ExtraInfoList extraInfoList;

    [SerializeField] GameObject extraInfoClipboard;

    [Header("Events")]
    public GameEvent onExtraInfoFound;

    public void updateExtraInfo(Component sender, object data) {        
        if(hiddenGameVariables._currentProposal.getExtraInfo() == null) {
            hiddenGameVariables._currentExtraInfo = null;
            //TODO animation of removing clipboard here
            extraInfoClipboard.SetActive(false);
            return;
            //Exit from the function if there is no extra info to get
        }
            
        relevantInfoObject = extraInfoList._extraInfo[hiddenGameVariables._currentProposal.getExtraInfo()];
        hiddenGameVariables._currentExtraInfo = relevantInfoObject;

        onExtraInfoFound.Raise();
    }
}
