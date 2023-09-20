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

        //Proposals with an extra info of -1 have no related extra info
        if(hiddenGameVariables._currentProposal.getExtraInfo() < 0) {
            hiddenGameVariables._currentExtraInfo = null;
            //TODO animation of removing clipboard (Not here but somewhere in UI class after finished proposal)
            extraInfoClipboard.SetActive(false);

            return;
            //Exit from the function if there is no extra info to get
        }

        relevantInfoObject = extraInfoList._extraInfo[hiddenGameVariables._currentProposal.getExtraInfo()];
        hiddenGameVariables._currentExtraInfo = relevantInfoObject;

        onExtraInfoFound.Raise();
    }
}
