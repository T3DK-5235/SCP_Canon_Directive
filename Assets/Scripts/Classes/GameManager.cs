using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
//using UnityEngine.UIElements;

using TMPro;

public class GameManager : MonoBehaviour
{
    [SerializeField] HiddenGameVariables hiddenGameVariables;

    [SerializeField] ProposalsList proposalsList;
    [SerializeField] AchievementsList achievementsList;

    [Header("Months")]
    [SerializeField] int monthLength;

    //How many proposals have been played this month
    [SerializeField] int numMonthlyProposal;

    [Header("Events")]
    
    public GameEvent onInitUI;
    public GameEvent onInitProposals;

    public GameEvent onGetNextProposal;
    public GameEvent onUpdateExtraInfo;
    public GameEvent onUpdateProposalUI;
    public GameEvent onHandleStatChanges;
    public GameEvent onUpdateFlashingStatUI;
    public GameEvent onProposalFullDecision;
    public GameEvent onUpdateStatUI;
    public GameEvent onCheckInvalidStats;

    public GameEvent onSwitchTabletState;

    void Awake()
    {
        //Initialize basic values
        monthLength = 4;
        numMonthlyProposal = 0;

        hiddenGameVariables.ResetToBase();
        
        onInitProposals.Raise();
        onInitUI.Raise();


        //Set the current proposal to a base proposal. This can be altered if save data is found
        // try {
            hiddenGameVariables._currentProposal = proposalsList._proposals[0];
        // }
        // catch (Exception e) {
        //     print("error");
        // }

        //TODO gets save data from json save file (may change this to a save scene menu)

        getNewMonthLength();
        
        DecideNextAction(null, null);
    }

    public void DecideNextAction(Component sender, object data) {
        if (hiddenGameVariables._currentGameState == GameStateEnum.PROPOSAL_LOADING) {
            //The above GameState will be caused by UIHandler "LoadNextProposal"
            
            onGetNextProposal.Raise();
            
        } else if (hiddenGameVariables._currentGameState == GameStateEnum.PROPOSAL_ONGOING) {
            //The above GameState will be caused by ProposalHandler "GetNextProposal"

            onUpdateProposalUI.Raise();

            //If extra info actually exists

            // Debug.Log(hiddenGameVariables._currentProposal.getExtraInfo());
            if(hiddenGameVariables._currentProposal.getExtraInfo() != -1) {
                onUpdateExtraInfo.Raise();
            }
            //TODO call to slide clipboard and extra info board onscreen

            int currentProposalID = hiddenGameVariables._currentProposal.getProposalID();
            if (currentProposalID <= 6) {
                TutorialCheck(currentProposalID);
            }
            
        } else if (hiddenGameVariables._currentGameState == GameStateEnum.PROPOSAL_TEMP_DECISION) {
            //The above gamestate will be caused by DecisionButton

            //Raises an event to make a stat copy - ProposalHandler "HandleStatChanges"
            onHandleStatChanges.Raise();
            
            while(hiddenGameVariables._myStatCopy == null) {
                Debug.Log("Creating stat copy");
            }
            onCheckInvalidStats.Raise();

            //Raises an event to flash the UI stat bars to show changes - 
            onUpdateFlashingStatUI.Raise();

        } else if (hiddenGameVariables._currentGameState == GameStateEnum.PROPOSAL_FULL_DECISION) {
            //The above gamestate will be caused by DecisionButton

            hiddenGameVariables._numMonthlyProposals++;

            string newAnimType = "";
            //If there has been the max number of proposals this month and it isn't the tutorial month
            if (hiddenGameVariables._numMonthlyProposals >= monthLength && hiddenGameVariables._currentProposal.getProposalID() >= 6) {
                newMonth();
                newAnimType = "newMonth";
            } else {
                newAnimType = "newProposal";
            }

            //Raises an event to finalize the current stats and run animations if needed - ProposalHandler "ProposalDecision" + UIHandler "CheckNextAnim"
            onProposalFullDecision.Raise(newAnimType);

        } else if (hiddenGameVariables._currentGameState == GameStateEnum.PROPOSAL_STATS_UPDATED) {
            //The above gamestate will be caused by ProposalHandler "UpdateNewStats" - Is used to check stats are applied before changing UI
        
            onUpdateStatUI.Raise();
        }
    }

    //====================================================================
    //                         ACHIEVEMENT SECTION                       |
    //====================================================================

    //Called by proposal Handler
    //Listens to event onAchievementDone
    // public void updateAchievementList(Component sender, object data) {
    //     int completedAchievement = (int)data;
    //     //Add int pointer of achievement that got completed to completed achievement list
    //     achievementsList._completedAchievements.Add(completedAchievement);
    // }

    //====================================================================
    //                          NEW MONTH SECTION                        |
    //====================================================================

    private void newMonth() {
        //Increase the current month number
        hiddenGameVariables._currentMonth++;
        getNewMonthLength();
        //Reset number of proposals done that month to 0
        hiddenGameVariables._numMonthlyProposals = 0;
        CheckStatBus();
    }

    private void getNewMonthLength() {
        monthLength = UnityEngine.Random.Range(5, 8);
    }

    //====================================================================
    //                     STAT BUS CHECKING SECTION                     |
    //====================================================================

    private void CheckStatBus() {

        for (int i = 0; i < hiddenGameVariables._statChangeEventBus.Count; i++) {
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
    }

    //====================================================================
    //                       UNIQUE EVENTS SECTION                       |
    //====================================================================
 
    private void TutorialCheck(int currentID) {
        if (currentID == 1){
            onSwitchTabletState.Raise();   
        }
    }
}
