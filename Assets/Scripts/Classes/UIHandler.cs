using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;

public class UIHandler : MonoBehaviour
{
    [SerializeField] HiddenGameVariables hiddenGameVariables;

    [SerializeField] GameObject projectClipboardOverlay;

    //Maybe instead try make a UI scriptable object
    [SerializeField] GameObject proposalDescription_Text;

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

    void Start() {
        //Maybe put in try catch
        //get the text from the proposal UI object and set it to the current proposal's description
        proposalDescription_Text.transform.GetComponent<TextMeshProUGUI>().text = hiddenGameVariables._currentProposal.getProposalDescription();
    }

    //TODO have this be an event that can be called
    public void updateProposal(Component sender, object data) {
        proposalDescription_Text.transform.GetComponent<TextMeshProUGUI>().text = hiddenGameVariables._currentProposal.getProposalDescription();
    }

    private void ProjectClipboardOverlay() {
        projectClipboardOverlay.SetActive(true);
    }
}
