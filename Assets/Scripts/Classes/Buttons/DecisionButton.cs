using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DecisionButton : MonoBehaviour, IPointerClickHandler {

    [SerializeField] GameObject decisionButton;
    [SerializeField] GameObject signatureButton;

    [SerializeField] GameObject acceptStamp;
    [SerializeField] GameObject denyStamp;
    [SerializeField] GameObject signature; 

    private static string choice = "";

    [Header("Events")]
    public GameEvent onDecisionMade;
    public GameEvent onDecisionChecked;

    public void OnPointerClick(PointerEventData eventData)
    {
        //TODO Check that the proposal can be accepted due to the stats
        //This deals with the button used for accepting or denying the proposal
        if(eventData.pointerPress == decisionButton) {
            if (eventData.button == PointerEventData.InputButton.Left) {
                choice = "accept";
                //onDecisionChecked.Raise(choice); //Used to display stat changes
                if(denyStamp.activeSelf) {
                    denyStamp.SetActive(false);
                }
                acceptStamp.SetActive(true);
            } else if (eventData.button == PointerEventData.InputButton.Right) {
                choice = "deny";
                //onDecisionChecked.Raise(choice);
                if(acceptStamp.activeSelf) {
                    acceptStamp.SetActive(false);
                }
                denyStamp.SetActive(true);
            }
        }

        //This deals with if the signature button has been clicked and a choice has been made
        if(eventData.pointerPress == signatureButton && choice != "") {
            signature.SetActive(true);

            //Disallow users interacting with the buttons at this point
            //TODO check if this is efficient
            CanvasGroup canvasGroup = GetComponentInParent<CanvasGroup>(true);
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;

            //Send out event
            onDecisionMade.Raise(choice);

            //reset choice to a blank string for next use
            choice = "";

            StartCoroutine(AnimationCoroutine(canvasGroup));
        }
    }

    IEnumerator AnimationCoroutine(CanvasGroup canvasGroup)
    {
        //TODO Do animation of sliding clipboard on screen in co-routine (or at least, raise an event for the UIHandler to do)

        //yield on a new YieldInstruction that waits for 5 seconds.
        yield return new WaitForSeconds(3);

        signature.SetActive(false);
        denyStamp.SetActive(false);
        acceptStamp.SetActive(false);

        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
    }
}
