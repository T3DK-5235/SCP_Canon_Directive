using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIHandler : MonoBehaviour
{
    [SerializeField] HiddenGameVariables hiddenGameVariables;

    [SerializeField] GameObject projectClipboardOverlay;

    //TODO utilize events to fire the below actions instead of update?
    //TODO update background of window 
    //TODO add screen overlays

    void Update() {
        int currentID = hiddenGameVariables._currentProposal.getProposalID();

        //hardcoded at only proposal 0 for initial experimentation
        if (currentID == 0){
            ProjectClipboardOverlay();
        } else {
            projectClipboardOverlay.SetActive(false);
        }
    }

    private void ProjectClipboardOverlay() {
        projectClipboardOverlay.SetActive(true);
    }
}
