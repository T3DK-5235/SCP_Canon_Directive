using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
//using UnityEngine.UIElements;

using TMPro;

using System.Linq;
using UnityEditor;

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

    // public GameEvent onGetNextProposal;
    // public GameEvent onUpdateExtraInfo;
    // public GameEvent onUpdateProposalUI;
    // public GameEvent onHandleStatChanges;
    // public GameEvent onUpdateFlashingStatUI;
    // public GameEvent onProposalFullDecision;
    // public GameEvent onUpdateStatUI;
    // public GameEvent onCheckInvalidStats;

    //Game flow event plus the Enum int value
    public GameEvent GFE_0;
    public GameEvent GFE_1;
    public GameEvent GFE_2;
    public GameEvent GFE_3;
    public GameEvent GFE_4;
    public GameEvent GFE_5;

    public GameEvent onSwitchTabletState;

    [Header("GameFlowBus Handling")]
    Coroutine gameFlowManagerCoroutine;
    private int currentBusSize;
    private GameFlowEventBus gameFlowEventBus;

    private static GameStateEnum[] genericProposalSet = new GameStateEnum[] {
        GameStateEnum.PROPOSAL_INITIALIZATION,
        GameStateEnum.ANIMATION_PROPOSAL_INITIALIZATION, 
        GameStateEnum.PROPOSAL_ONGOING, 
        GameStateEnum.PROPOSAL_FULL_DECISION,
        GameStateEnum.ANIMATION_PROPOSAL_FINALIZATION,
    };

    private static GameStateEnum[] newMonthAnimSet = new GameStateEnum[] {
        GameStateEnum.ANIMATION_NEWMONTH_INITIALIZATION,
        GameStateEnum.PROPOSAL_CHECKURGENT, 
        GameStateEnum.ANIMATION_NEWMONTH_FINALIZATION
    };
    

    void Awake()
    {
        //Initialize basic values
        monthLength = 4;
        numMonthlyProposal = 0;

        //TODO gets save data from json save file (may change this to a save scene menu)
        //TODO saved proposal is put here instead of default one
        hiddenGameVariables.ResetToBase(proposalsList._proposals[0]);
        gameFlowEventBus = hiddenGameVariables._gameFlowEventBus;

        onInitProposals.Raise();
        onInitUI.Raise();

        getNewMonthLength();
        


        // DecideNextAction(null, null);
        gameFlowEventBus.Enqueue(genericProposalSet);
        //First proposal is initialised at 0 or by the save manager
        gameFlowEventBus.Dequeue();
        hiddenGameVariables._currentGameState = gameFlowEventBus.Head(); 
        gameFlowManagerCoroutine = StartCoroutine(GameFlowManager());
    }

    IEnumerator GameFlowManager() {
        //Store bus size upon start of Coroutine
        currentBusSize = gameFlowEventBus.GetBusSize();

        //TODO remove this after testing [
        string currentFlowEventBus = "";
        for(int i = 0; i < gameFlowEventBus.GetBusSize(); i++) {
            GameStateEnum gameFlowEventBuss = gameFlowEventBus.GetFlowEventBus().ElementAt(i);
            currentFlowEventBus += "i:" + ((int)gameFlowEventBuss).ToString() + " -- ";
        }
        Debug.Log(currentFlowEventBus + "   The head of the queue is: " + gameFlowEventBus.Head());
        //TODO ]

        switch((int)hiddenGameVariables._gameFlowEventBus.Head())
        {
            //PROPOSAL_INITIALIZATION,              //0
            //PROPOSAL_ONGOING,               //1
            //PROPOSAL_FULL_DECISION,               //2
            //ANIMATION_PROPOSAL_INITIALIZATION,    //3
            //ANIMATION_PROPOSAL_FINALIZATION       //4
            case 0:
                //Init next proposal
                GFE_0.Raise();
                break;
                
            case 1:
                //HandleStatChanges - ProposalHandler
                //CheckInvalidStats - CURRENTLY decision button + O5 signature button
                //UpdateFlashingStatUI - UIHandler
                GFE_1.Raise();
                break;
            case 2:
                //Raises an event to finalize the current stats and run animations if needed - ProposalHandler "ProposalDecision" + UIHandler "CheckNextAnim"
                //TODO check that the program pauses on this line and adds the anim set before the next proposal set
                GFE_2.Raise(CheckNewMonth());
                //Load instructions for next proposal
                gameFlowEventBus.Enqueue(genericProposalSet);
                break;
            case 3:
                //TODO implement clipboards coming onscreen anim
                hiddenGameVariables._gameFlowEventBus.Dequeue();
                //GFE_3.Raise();
                break;
            case 4:
                //TODO implement clipboards going offscreen anim
                hiddenGameVariables._gameFlowEventBus.Dequeue();
                //GFE_4.Raise();
                break;
            case 5:
                //GFE_5.Raise();
                break;
            default:
                Debug.Log("Error in GameManager Switch");
                break;
        }

        //When an event is removed from the bus or added to it, then continue the coroutine
        yield return new WaitUntil(() => currentBusSize != gameFlowEventBus.GetBusSize());
        GameFlowLoop();
        //Ends current coroutine and frees resources
    }

    //This isn't done in the Coroutine above to prevent recursive frames
    private void GameFlowLoop() {
        StopCoroutine(gameFlowManagerCoroutine);
        gameFlowManagerCoroutine = StartCoroutine(GameFlowManager());
    }

    // public void DecideNextAction(Component sender, object data) {
    //     // hiddenGameVariables._currentProposal = proposalsList._proposals[0];

    //     if (hiddenGameVariables._currentGameState == GameStateEnum.PROPOSAL_LOADING) {
    //         //The above GameState will be caused by UIHandler "LoadNextProposal"
            
    //         onGetNextProposal.Raise();
            
    //     } else if (hiddenGameVariables._currentGameState == GameStateEnum.PROPOSAL_ONGOING) {
    //         //The above GameState will be caused by ProposalHandler "GetNextProposal"

    //         onUpdateProposalUI.Raise();

    //         //If extra info actually exists

    //         // Debug.Log(hiddenGameVariables._currentProposal.getExtraInfo());
    //         if(hiddenGameVariables._currentProposal.getExtraInfo() != -1) {
    //             onUpdateExtraInfo.Raise();
    //         }
    //         //TODO call to slide clipboard and extra info board onscreen

    //         int currentProposalID = hiddenGameVariables._currentProposal.getProposalID();
    //         if (currentProposalID <= 7) {
    //             TutorialCheck(currentProposalID);
    //         }
            
    //     } else if (hiddenGameVariables._currentGameState == GameStateEnum.PROPOSAL_TEMP_DECISION) {
    //         //The above gamestate will be caused by DecisionButton

    //         //Raises an event to make a stat copy - ProposalHandler "HandleStatChanges"
    //         onHandleStatChanges.Raise();
            
    //         while(hiddenGameVariables._myStatCopy == null) {
    //             Debug.Log("Creating stat copy");
    //         }
    //         onCheckInvalidStats.Raise();

    //         //Raises an event to flash the UI stat bars to show changes - 
    //         onUpdateFlashingStatUI.Raise();

    //     } else if (hiddenGameVariables._currentGameState == GameStateEnum.PROPOSAL_FULL_DECISION) {
    //         //The above gamestate will be caused by DecisionButton

    //         hiddenGameVariables._numMonthlyProposals++;

    //         string newAnimType = "";
    //         //If there has been the max number of proposals this month and it isn't the tutorial month
    //         if (hiddenGameVariables._numMonthlyProposals >= monthLength && hiddenGameVariables._currentProposal.getProposalID() >= 6) {
    //             newMonth();
    //             newAnimType = "newMonth";
    //         } else {
    //             newAnimType = "newProposal";
    //         }

    //         //Raises an event to finalize the current stats and run animations if needed - ProposalHandler "ProposalDecision" + UIHandler "CheckNextAnim"
    //         onProposalFullDecision.Raise(newAnimType);

    //     } else if (hiddenGameVariables._currentGameState == GameStateEnum.PROPOSAL_STATS_UPDATED) {
    //         //The above gamestate will be caused by ProposalHandler "UpdateNewStats" - Is used to check stats are applied before changing UI
        
    //         onUpdateStatUI.Raise();
    //     }
    // }

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

    private bool CheckNewMonth() {
        hiddenGameVariables._numMonthlyProposals++;
        //If there has been the max number of proposals this month and it isn't the tutorial month
        if (hiddenGameVariables._numMonthlyProposals >= monthLength && hiddenGameVariables._currentProposal.getProposalID() >= 6) {
            //Increase the current month number
            hiddenGameVariables._currentMonth++;
            getNewMonthLength();
            //Reset number of proposals done that month to 0
            hiddenGameVariables._numMonthlyProposals = 0;
            gameFlowEventBus.Enqueue(newMonthAnimSet);
            StartCoroutine(IWaitForAnim());
            return true;
        } else {
            return false;
        }
    }

    private void getNewMonthLength() {
        monthLength = UnityEngine.Random.Range(5, 8);
    }

    //====================================================================
    //                     STAT BUS CHECKING SECTION                     |
    //====================================================================

    IEnumerator IWaitForAnim() {
        yield return new WaitForSeconds(1f);
        CheckStatBus();
    }

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
                    // Debug.Log("Old Stat Value: " + hiddenGameVariables._availableMTF + "New Stat Value: " + (hiddenGameVariables._availableMTF - statEffect));
                    hiddenGameVariables._availableMTF -= statEffect;
                    //Remove the stat change from the bus as it is finished with
                    hiddenGameVariables._statChangeEventBus.RemoveAt(i);
                    //Use this to tell the UI which stat has been changed and needs to be updated visually
                    hiddenGameVariables._myStatCopy.__statsChanged.Add(0);
                    //As the size of the bus has decreased, decrement i, this is because another stat change will be in the position of the old one.
                    i--;
                    //continue will start again from the top of the loop
                    continue;
                }
                
                if(changedStat == "Researchers") {
                    hiddenGameVariables._availableResearchers -= statEffect;
                    //TODO maybe move the following two lines to a small function to prevent duplicated code
                    hiddenGameVariables._statChangeEventBus.RemoveAt(i);
                    hiddenGameVariables._myStatCopy.__statsChanged.Add(2);
                    i--;
                    continue;
                }

                if(changedStat == "DClass") {
                    hiddenGameVariables._availableDClass -= statEffect;
                    hiddenGameVariables._statChangeEventBus.RemoveAt(i);
                    hiddenGameVariables._myStatCopy.__statsChanged.Add(4);
                    i--;
                    continue;
                }

                if(changedStat == "Morale") {
                    hiddenGameVariables._currentMorale -= statEffect;
                    hiddenGameVariables._statChangeEventBus.RemoveAt(i);
                    hiddenGameVariables._myStatCopy.__statsChanged.Add(6);
                    i--;
                    continue;
                }
                
                //GOI Stats

                if(changedStat == "GOC") {
                    hiddenGameVariables._favourGOC -= statEffect;
                    hiddenGameVariables._statChangeEventBus.RemoveAt(i);
                    hiddenGameVariables._myStatCopy.__statsChanged.Add(8);
                    i--;
                    continue;
                }

                if(changedStat == "Nalka") {
                    hiddenGameVariables._favourNalka -= statEffect;
                    hiddenGameVariables._statChangeEventBus.RemoveAt(i);
                    hiddenGameVariables._myStatCopy.__statsChanged.Add(9);
                    i--;
                    continue;
                }

                if(changedStat == "Mekanite") {
                    hiddenGameVariables._favourMekanite -= statEffect;
                    hiddenGameVariables._statChangeEventBus.RemoveAt(i);
                    hiddenGameVariables._myStatCopy.__statsChanged.Add(10);
                    i--;
                    continue;
                }

                if(changedStat == "SerpentsHand") {
                    hiddenGameVariables._favourSerpentsHand -= statEffect;
                    hiddenGameVariables._statChangeEventBus.RemoveAt(i);
                    hiddenGameVariables._myStatCopy.__statsChanged.Add(11);
                    i--;
                    continue;
                }

                if(changedStat == "Factory") {
                    hiddenGameVariables._favourFactory -= statEffect;
                    hiddenGameVariables._statChangeEventBus.RemoveAt(i);
                    hiddenGameVariables._myStatCopy.__statsChanged.Add(12);
                    i--;
                    continue;
                }

                if(changedStat == "Anderson") {
                    hiddenGameVariables._favourAnderson -= statEffect;
                    hiddenGameVariables._statChangeEventBus.RemoveAt(i);
                    hiddenGameVariables._myStatCopy.__statsChanged.Add(13);
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
        //Makes sure to go to the next month
        if (currentID == 6) {
            hiddenGameVariables._numMonthlyProposals = 10;
        }
        if (currentID == 1){
            onSwitchTabletState.Raise("Initial Activation");   
        }
    }
}
