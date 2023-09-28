using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
//using UnityEngine.UIElements;

using TMPro;

public class GameManager : MonoBehaviour
{

    [SerializeField] HiddenGameVariables hiddenGameVariables;

    //Default length of a month
    [SerializeField] int monthLength = 4;

    //How many proposals have been played this month
    [SerializeField] int numMonthlyProposal = 1;

    [SerializeField] GameObject newMonthBlackout;

    [SerializeField] GameObject projectClipboardOverlay;

    [SerializeField] GameObject initialTabletScreen;
    [SerializeField] GameObject scorpLogo;

    [SerializeField] GameObject foundationStatScreen;

    [SerializeField] GameObject GoIStatScreen;

    private bool tabletOn = false;

    [Header("Events")]
    public GameEvent onProposalChanged;
    public GameEvent onNewMonth;

    void Awake()
    {
        //TODO gets save data from json save file (may change this to a save scene menu)
        //create object pool for main proposals, extra info(, and news?)?

        //Initial num of proposals in the first month
        monthLength = UnityEngine.Random.Range(4, 7);
    }


    //TODO utilize events to fire the below actions instead of update?
    //TODO update background of window 
    //TODO add screen overlays

    void Update() {
        int currentID = hiddenGameVariables._currentProposal.getProposalID();

        //hardcoded at only proposal 0 for initial experimentation
        if (currentID == 0){
            ProjectClipboardOverlay();
            //Stops month from progressing during tutorial
            numMonthlyProposal = 0;
        } else if (currentID == 1){
            projectClipboardOverlay.SetActive(false);
            TurnOnTablet();
            numMonthlyProposal = 0;
        }
    }

    public void checkMonth(Component sender, object data) {
        //Increment the number of proposals done this month
        string animType;
        numMonthlyProposal++;
        //If the month should end and the gamestate has moved to checking if it is the month end
        if(numMonthlyProposal == monthLength) {
            //Change to loading the month
            Debug.Log("Loading next month");
            animType = "NewMonth";

            //Temp call to do the new month blackout "animation"
            StartCoroutine(newMonthAnim());

            //Increase the current month number
            hiddenGameVariables._currentMonth++;

            checkStatBus();

            //Get new month length
            monthLength = UnityEngine.Random.Range(5, 8);
            numMonthlyProposal = 1;

        } else {
            Debug.Log("Load next proposal");

            animType = "NewProposal";
        }

        //Call animation for when new month/new proposal happens
        onProposalChanged.Raise(animType);
    }

    IEnumerator newMonthAnim() {
        newMonthBlackout.SetActive(true);

        yield return new WaitForSeconds(1);

        newMonthBlackout.SetActive(false);
    }

    private void checkStatBus() {

        for (int i = 0; i < hiddenGameVariables._statChangeEventBus.Count; i++) {
            // if (hiddenGameVariables._statChangeEventBus[i] == null) {
            //     Debug.Log("I mean it doesnt exist");
            // }
            //TODO check that this whole thing finishes before the next proposal

            //Updates the number of months left for the stat
            hiddenGameVariables._statChangeEventBus[i].updateStatDuration();

            string changedStat = hiddenGameVariables._statChangeEventBus[i].getStatChanged();
            int statEffect = hiddenGameVariables._statChangeEventBus[i].getStatChangedEffect();

            //If the stat effect runs out
            if(hiddenGameVariables._statChangeEventBus[i].getStatChangedDuration() == 0) {

                //TODO figure out if this can be sped up with dictionary
                //TODO if it can, look into using alternative json library that can serialize dictionaries

                if(changedStat == "MTF") {
                    //Set the stat back to its normal value (if it went down by 10, this will do +10 (or rather, --10).)
                    Debug.Log("Old Stat Value: " + hiddenGameVariables._availableMTF + "New Stat Value: " + (hiddenGameVariables._availableMTF - statEffect));
                    hiddenGameVariables._availableMTF -= statEffect;
                    //Remove the stat change from the bus as it is finished with
                    hiddenGameVariables._statChangeEventBus.RemoveAt(i);
                    //As the size of the bus has decreased, decrement i, this is because another stat change will be in the position of the old one.
                    i--;
                    //continue will start again from the top of the loop
                    continue;
                }
                
                if(changedStat == "Researchers") {
                    hiddenGameVariables._availableResearchers -= statEffect;
                    //TODO maybe move the following two lines to a small function to prevent duplicated code
                    hiddenGameVariables._statChangeEventBus.RemoveAt(i);
                    i--;
                    continue;
                }

                if(changedStat == "DClass") {
                    hiddenGameVariables._availableDClass -= statEffect;
                    hiddenGameVariables._statChangeEventBus.RemoveAt(i);
                    i--;
                    continue;
                }

                if(changedStat == "Morale") {
                    hiddenGameVariables._currentMorale -= statEffect;
                    hiddenGameVariables._statChangeEventBus.RemoveAt(i);
                    i--;
                    continue;
                }
                
                //GOI Stats

                if(changedStat == "GOC") {
                    hiddenGameVariables._favourGOC -= statEffect;
                    hiddenGameVariables._statChangeEventBus.RemoveAt(i);
                    i--;
                    continue;
                }

                if(changedStat == "Nalka") {
                    hiddenGameVariables._favourNalka -= statEffect;
                    hiddenGameVariables._statChangeEventBus.RemoveAt(i);
                    i--;
                    continue;
                }

                if(changedStat == "Mekanite") {
                    hiddenGameVariables._favourMekanite -= statEffect;
                    hiddenGameVariables._statChangeEventBus.RemoveAt(i);
                    i--;
                    continue;
                }

                if(changedStat == "SerpentsHand") {
                    hiddenGameVariables._favourSerpentsHand -= statEffect;
                    hiddenGameVariables._statChangeEventBus.RemoveAt(i);
                    i--;
                    continue;
                }

                if(changedStat == "Factory") {
                    hiddenGameVariables._favourFactory -= statEffect;
                    hiddenGameVariables._statChangeEventBus.RemoveAt(i);
                    i--;
                    continue;
                }

                if(changedStat == "Anderson") {
                    hiddenGameVariables._favourAnderson -= statEffect;
                    hiddenGameVariables._statChangeEventBus.RemoveAt(i);
                    i--;
                    continue;
                }
            }
        }
        onNewMonth.Raise();
    }

    private void TurnOnTablet() {
        if(tabletOn == false) {
            StartCoroutine(ITabletOn());
            StartCoroutine(IDisplayLogo());
            tabletOn = true;
        }
    }

    private void ProjectClipboardOverlay() {
        projectClipboardOverlay.SetActive(true);
    }


    IEnumerator ITabletOn()
    {
        initialTabletScreen.SetActive(true);

        Image initialTabletImage = initialTabletScreen.GetComponent<Image>();
        Color temp = initialTabletImage.color;

        float elapsedTime = 0;
        float duration = 0.1f;
        while (initialTabletImage.color.a < 0.95f){//elapsedTime < duration) {
            elapsedTime += Time.deltaTime;
            yield return new WaitForSeconds(0.1f);

            temp = initialTabletImage.color;
            temp.a = Mathf.Lerp(temp.a, 1, elapsedTime / duration); //Time.deltaTime
            initialTabletImage.color = temp;
        }

        //Syncs this co-routine with the other co-routine below
        yield return new WaitForSeconds(1f);

        temp = initialTabletImage.color;
        temp.a = 0f; //Time.deltaTime
        initialTabletImage.color = temp;
        initialTabletScreen.SetActive(false);
    }

    IEnumerator IDisplayLogo()
    {
        yield return new WaitForSeconds(0.5f);
        scorpLogo.SetActive(true);

        Image scorpImage = scorpLogo.GetComponent<Image>();
        Color temp = scorpImage.color;

        float elapsedTime = 0;
        float duration = 0.1f;

        while (scorpImage.color.a < 0.95f){//elapsedTime < duration) {
            elapsedTime += Time.deltaTime;
            yield return new WaitForSeconds(0.1f);

            temp = scorpImage.color;
            temp.a = Mathf.Lerp(temp.a, 1, elapsedTime / duration); //Time.deltaTime
            scorpImage.color = temp;
        }
        
        yield return new WaitForSeconds(0.4f);

        foundationStatScreen.SetActive(true);
        GoIStatScreen.SetActive(false);

        yield return new WaitForSeconds(0.1f);

        //Resets the image back to being invisible
        temp = scorpImage.color;
        temp.a = 0f; //Time.deltaTime
        scorpImage.color = temp;
        scorpLogo.SetActive(false);
    }
}
