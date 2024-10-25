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

    private static bool validProposal = false;

    [Header("Events")]
    public GameEvent HandleTempDecision;

    PointerEventData eventData;

    [SerializeField] Texture2D objectCursor;

    public void OnMouseEnter() {
        Cursor.SetCursor(objectCursor, new Vector2(12, 10), CursorMode.Auto);
    }

    public void OnMouseExit() {
        Cursor.SetCursor(null, new Vector2(12, 10), CursorMode.Auto);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        this.eventData = eventData;
        //This deals with the button used for accepting or denying the proposal
        if(eventData.pointerPress == decisionButton) {
            if (eventData.button == PointerEventData.InputButton.Left && acceptStamp.activeSelf != true) {
                hiddenGameVariables._proposalDecision = ProposalChoiceEnum.ACCEPT;
                if(denyStamp.activeSelf) {
                    denyStamp.SetActive(false);
                }
                acceptStamp.SetActive(true);
            } else if (eventData.button == PointerEventData.InputButton.Right && denyStamp.activeSelf != true) {
                hiddenGameVariables._proposalDecision = ProposalChoiceEnum.DENY;
                if(acceptStamp.activeSelf) {
                    acceptStamp.SetActive(false);
                }
                denyStamp.SetActive(true);
            } else {
                //If the user tries to accept twice. 
                return;
            }

            // hiddenGameVariables._currentGameState = GameStateEnum.PROPOSAL_TEMP_DECISION;
            // DecideNextAction.Raise();
            HandleTempDecision.Raise();
            
        }

        //This deals with if the signature button has been clicked and a choice has been made PLUS if the choice can actually be chosen
        if(validProposal == true && eventData.pointerPress == signatureButton && hiddenGameVariables._proposalDecision != ProposalChoiceEnum.NONE) {
            //Debug.Log("Proposal can be accepted");
            finishProposal();
        }
    }

    public void CheckInvalidStats(Component sender, object data) 
    {
        //Tracks if at least one stat is less than 0
        validProposal = true;

        //If the values in the stat copy are less than 0, the proposal is invalid
        //TODO fix this obamanation
        if (hiddenGameVariables._myStatCopy.__availableMTF < 0) {
            validProposal = false;
            //TODO raise an event to make the UI bar flash red?
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

        //Is true if there were no faults
        // if (validProposal == true) {
        //     Debug.Log("No faults found");
        // }
    }

    private void finishProposal() {
        signature.SetActive(true);
        //Disallow users interacting with the buttons at this point
        CanvasGroup canvasGroup = GetComponentInParent<CanvasGroup>(true);
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;

        // hiddenGameVariables._currentGameState = GameStateEnum.PROPOSAL_FULL_DECISION;
        // DecideNextAction.Raise();
        //Dequeue PROPOSAL_TEMP_DECISION
        hiddenGameVariables._gameFlowEventBus.Dequeue();

        StartCoroutine(IButtonReset(canvasGroup));
    }

    IEnumerator IButtonReset(CanvasGroup canvasGroup)
    {
        yield return new WaitForSeconds(0.2f);

        signature.SetActive(false);
        denyStamp.SetActive(false);
        acceptStamp.SetActive(false);

        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
    }
}
