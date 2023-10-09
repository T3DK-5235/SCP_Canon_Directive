using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DecisionButton : MonoBehaviour, IPointerClickHandler {

    [SerializeField] HiddenGameVariables hiddenGameVariables;
    [SerializeField] GameObject decisionButton;
    [SerializeField] GameObject signatureButton;

    [SerializeField] GameObject acceptStamp;
    [SerializeField] GameObject denyStamp;
    [SerializeField] GameObject signature; 

    private static string choice = "";

    private static bool validProposal = false;

    [Header("Events")]
    public GameEvent DecideNextAction;

    PointerEventData eventData;

    public void OnPointerClick(PointerEventData eventData)
    {
        this.eventData = eventData;
        //TODO Check that the proposal can be accepted due to the stats (raise event to check this, then set a variable in here if true?)
        //This deals with the button used for accepting or denying the proposal
        if(eventData.pointerPress == decisionButton) {
            if (eventData.button == PointerEventData.InputButton.Left) {
                choice = "accept";
                if(denyStamp.activeSelf) {
                    denyStamp.SetActive(false);
                }
                acceptStamp.SetActive(true);
            } else if (eventData.button == PointerEventData.InputButton.Right) {
                choice = "deny";
                if(acceptStamp.activeSelf) {
                    acceptStamp.SetActive(false);
                }
                denyStamp.SetActive(true);
            }
            hiddenGameVariables._currentGameState = GameStateEnum.PROPOSAL_TEMP_DECISION;
            DecideNextAction.Raise();
        }

        //TODO remove testing code
        //Debug.Log(" pointer pressed : " + eventData.pointerPress + " : : : " + validProposal.ToString() + " : : : " + choice);

        //This deals with if the signature button has been clicked and a choice has been made PLUS if the choice can actually be chosen
        if(validProposal == true && eventData.pointerPress == signatureButton && choice != "") {
            // Debug.Log("Proposal can be accepted");
            finishProposal();
        }
    }

    public void checkInvalidStats(Component sender, object data) 
    {

        //Tracks if at least one stat is less than 0
        validProposal = true;

        //If the values in the stat copy are less than 0, the proposal is invalid
        if (hiddenGameVariables._myStatCopy.__availableMTF < 0) {
            validProposal = false;
            //TODO raise an event to make the UI bar flash red?
                        
            // flashInvalidStat.Raise("MTF");
        }
        if (hiddenGameVariables._myStatCopy.__availableResearchers < 0) { 
            validProposal = false;
        }
        if (hiddenGameVariables._myStatCopy.__availableDClass < 0) { 
            validProposal = false;
        }
        if (hiddenGameVariables._myStatCopy.__currentMorale < 0) { 
            validProposal = false;
        }

        if (hiddenGameVariables._myStatCopy.__favourGOC < 0) { 
            validProposal = false;
        }
        if (hiddenGameVariables._myStatCopy.__favourNalka < 0) { 
            validProposal = false;
        }
        if (hiddenGameVariables._myStatCopy.__favourMekanite < 0) { 
            validProposal = false;
        }
        if (hiddenGameVariables._myStatCopy.__favourSerpentsHand < 0) { 
            validProposal = false;
        }
        if (hiddenGameVariables._myStatCopy.__favourFactory < 0) { 
            validProposal = false;
        }
        if (hiddenGameVariables._myStatCopy.__favourAnderson < 0) { 
            validProposal = false;
        }

        //Return true if there were no faults
        if (validProposal == true) {
            Debug.Log("No faults found");
        }
    }

    private void finishProposal() {
    
        signature.SetActive(true);

        //Disallow users interacting with the buttons at this point
        //TODO check if this is efficient
        CanvasGroup canvasGroup = GetComponentInParent<CanvasGroup>(true);
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;

        //Send out event
        DecideNextAction.Raise();

        //reset choice to a blank string for next use
        choice = "";

        StartCoroutine(AnimationCoroutine(canvasGroup));
    }

    IEnumerator AnimationCoroutine(CanvasGroup canvasGroup)
    {
        //TODO Do animation of sliding clipboard on screen in co-routine (or at least, raise an event for the UIHandler to do)

        yield return new WaitForSeconds(0.2f);

        signature.SetActive(false);
        denyStamp.SetActive(false);
        acceptStamp.SetActive(false);

        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
    }
}
