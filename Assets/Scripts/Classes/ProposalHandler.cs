using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProposalHandler : MonoBehaviour
{
    [SerializeField] HiddenGameVariables hiddenGameVariables;

    public List<int> activeProposalEventBus;
    public List<int> standbyProposalEventBus;
    [SerializeField] ProposalsList proposalsList;
    [SerializeField] DetailsList detailsList;

    [Header("Related Extra Info")]
    GenericExtraInfo relevantInfoObject;
    [SerializeField] ExtraInfoList extraInfoList;
    [SerializeField] GameObject extraInfoClipboard;

    [Header("Related Achievement")]
    [SerializeField] AchievementsList achievementsList;

    [Header("Related Follow Up Info")]

     [SerializeField] FollowUpInfoList followUpInfoList;

    [Header("Events")]
    //Used to update details UI 
    public GameEvent onSwitchDetailsMenu;
    public GameEvent onAchievementCompleted;

    public void InitProposals(Component sender, object data) {
        //Stores proposal objects
        activeProposalEventBus = new List<int>();
        standbyProposalEventBus = new List<int>();

        //Get the last saved proposal (0 when starting) and set it to the current proposal
        hiddenGameVariables._currentProposal = proposalsList._proposals[hiddenGameVariables._lastSavedProposal];
        //TODO write a save manager that'd pop in this "lastSavedProposal" plus the rest of the stats

        //Clears on startup. Might only be needed for in engine testing?
        hiddenGameVariables._statChangeEventBus.Clear();
    }

    //====================================================================
    //                         NEXT PROPOSAL SECTION                     |
    //====================================================================

    public void GetNextProposal(Component sender, object data) {
        //Add Proposal to currentProposal variable
        hiddenGameVariables._prevProposal = hiddenGameVariables._currentProposal;
        int nextProposalPos = PickNextProposalInt();
        hiddenGameVariables._currentProposal = proposalsList._proposals[activeProposalEventBus[nextProposalPos]];

        //Remove Proposal from active event bus
        activeProposalEventBus.RemoveAt(nextProposalPos);

        HandleExtraInfo();

        // hiddenGameVariables._currentGameState = GameStateEnum.PROPOSAL_ONGOING;
        // DecideNextAction.Raise();

        //Tells the gameflow to move on, doesn't have to know what to do next
        //Removes PROPOSAL_INITIALIZATION from queue
        hiddenGameVariables._gameFlowEventBus.Dequeue();
    }

    private int PickNextProposalInt() {
        int nextProposalPos = 0;
        bool foundNextProposal = false;
        //Loop is used to check that stat requirements are still correct after waiting in activeProposalEventBus
        do {
            //Gets a random value less than the size of the activeProposalEventBus
            nextProposalPos = UnityEngine.Random.Range(0, activeProposalEventBus.Count - 1);

            //If the proposal, since being added to the active stat bus, no longer has its stat requirements filled then it is removed
            if (proposalsList._proposals[activeProposalEventBus[nextProposalPos]].CheckStatRequirements(hiddenGameVariables) == false) {
                standbyProposalEventBus.Add(activeProposalEventBus[nextProposalPos]);
                activeProposalEventBus.Remove(nextProposalPos);
                foundNextProposal = false;
            } else {
                //If the proposal can still proceed, it will break out of the while loop
                foundNextProposal = true;
            }
        } while (foundNextProposal == false);

        //TODO add a system where each proposal has a set chance/importance. All these chances are added up and forced into a 0-100 range and then picked
        return nextProposalPos;
    }

    public void HandleExtraInfo() {  

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
    }


    //====================================================================
    //                      TEMP STAT CHANGE SECTION                     |
    //====================================================================

    //Listens to the PlayerProposalTempDecision event system
    public void HandleStatChanges(Component sender, object data) {

        List<string> proposalStatChanges = null;

        HiddenGameVariables.StatCopy statCopy = new HiddenGameVariables.StatCopy();

        //Get current values and assign them as base values to be changed in the stat copy
        statCopy.__chosenDClassMethod = hiddenGameVariables._chosenDClassMethod;
        statCopy.__currentMajorCanon = hiddenGameVariables._currentMajorCanon;

        statCopy.__totalMTF = hiddenGameVariables._totalMTF;
        statCopy.__availableMTF = hiddenGameVariables._availableMTF;

        statCopy.__totalResearchers = hiddenGameVariables._totalResearchers;
        statCopy.__availableResearchers = hiddenGameVariables._availableResearchers;

        statCopy.__totalDClass = hiddenGameVariables._totalDClass;
        statCopy.__availableDClass = hiddenGameVariables._availableDClass;

        statCopy.__totalMorale = hiddenGameVariables._totalMorale;
        statCopy.__currentMorale = hiddenGameVariables._currentMorale; 

        statCopy.__favourGOC = hiddenGameVariables._favourGOC;
        statCopy.__favourNalka = hiddenGameVariables._favourNalka;
        statCopy.__favourMekanite = hiddenGameVariables._favourMekanite;
        statCopy.__favourSerpentsHand = hiddenGameVariables._favourSerpentsHand;
        statCopy.__favourFactory = hiddenGameVariables._favourFactory;
        statCopy.__favourAnderson = hiddenGameVariables._favourAnderson; 

        hiddenGameVariables._myStatCopy = statCopy;

        List<PostUnlocks> currentPostUnlocks = hiddenGameVariables._currentProposal.getPostUnlocks();

        if (hiddenGameVariables._proposalDecision == ProposalChoiceEnum.ACCEPT) {
            PostUnlocks postUnlocksAccept = currentPostUnlocks[0];
            proposalStatChanges = postUnlocksAccept.getProposalStatChanges();
        } else if (hiddenGameVariables._proposalDecision == ProposalChoiceEnum.DENY) {
            PostUnlocks postUnlocksAccept = currentPostUnlocks[1];
            proposalStatChanges = postUnlocksAccept.getProposalStatChanges();
        }

        // Generic Stat changes are stored in blocks of 3 in the order "stat, amount, duration"
        // Certain stat changes change enums instead, such as when changing indirect proposal requirements
        // In this case it is instead "Requirement name (same as enum), num reference to enum value (_DClassMethod, 1, 0);

        // Stores all possible stats
        for(int i = 0; i < proposalStatChanges.Count; i += 3) {
            //Convert string from json to int for numerical comparisons
            int statAmount = Int32.Parse(proposalStatChanges[i+1]);
            int statDuration = Int32.Parse(proposalStatChanges[i+2]);

            //A duration of 0 here means permanence 
            if(proposalStatChanges[i] == "MTF") {
                //Only checks duration of stat after stat is found
                if(statDuration == 0) {
                    //Increase the total MTF number
                    changeTotalMTF(statAmount);
                    //Continue resets the loop to prevent extreme nesting when checking stat changes
                    continue;
                }
                //If the duration isn't permanent then this code is run instead
                changeAvailableMTF(statAmount, statDuration);
                continue;
            }

            if(proposalStatChanges[i] == "Researchers") {
                if(statDuration == 0) {
                    changeTotalResearchers(statAmount);
                    continue;
                }
                changeAvailableResearchers(statAmount, statDuration);
                continue;
            }

            if(proposalStatChanges[i] == "DClass") {
                if(statDuration == 0) {
                    changeTotalDClass(statAmount);
                    continue;
                }
                changeAvailableDClass(statAmount, statDuration);
                continue;
            }

            if(proposalStatChanges[i] == "Morale") {
                if(statDuration == 0) {
                    changeTotalMorale(statAmount);
                    continue;
                }
                changeCurrentMorale(statAmount, statDuration);
                continue;
            }

            if(proposalStatChanges[i] == "GOC") {
                changeGOCFavor(statAmount);
                continue;
            }

            if(proposalStatChanges[i] == "Nalka") {
                changeNalkaFavor(statAmount);
                continue;
            }

            if(proposalStatChanges[i] == "Mekanite") {
                changeMekaniteFavor(statAmount);
                continue;
            }

            if(proposalStatChanges[i] == "SerpentsHand") {
                changeSerpentsHandFavor(statAmount);
                continue;
            }

            if(proposalStatChanges[i] == "Factory") {
                changeFactoryFavor(statAmount);
                continue;
            }

            if(proposalStatChanges[i] == "Anderson") {
                changeAndersonFavor(statAmount);
                continue;
            }

            if(proposalStatChanges[i] == "DClassMethod") {
                changeDClassMethod(statAmount);
                continue;
            }
        }
    }


    //====================================================================
    //                      PERM STAT CHANGE SECTION                     |
    //====================================================================

    public void ProposalDecision(Component sender, object data) {
        List<int> proposalPostUnlocks = null;
        int proposalFollowUpUnlocks = -1;
        int proposalAchievement = -1;

        List<PostUnlocks> currentPostUnlocks = hiddenGameVariables._currentProposal.getPostUnlocks();

        //TODO remove after testing
        LinkedList<ProposalChoiceEnum> choiceList = new LinkedList<ProposalChoiceEnum>();
        choiceList.AddFirst(hiddenGameVariables._proposalDecision);
        Debug.Log(choiceList.First.Value);

        //Changes what is unlocked and changed based on player decision
        if (hiddenGameVariables._proposalDecision == ProposalChoiceEnum.ACCEPT) {
            //TODO figure out if its worth caching this bit as its also used above
            PostUnlocks postUnlocksAccept = currentPostUnlocks[0];
            proposalPostUnlocks = postUnlocksAccept.getProposalIDs();
            proposalFollowUpUnlocks = postUnlocksAccept.getFollowUpInfo();
            proposalAchievement = postUnlocksAccept.getAchievement();
        } else if (hiddenGameVariables._proposalDecision == ProposalChoiceEnum.DENY) {
            PostUnlocks postUnlocksDeny = currentPostUnlocks[1];
            proposalPostUnlocks = postUnlocksDeny.getProposalIDs();
            proposalFollowUpUnlocks = postUnlocksDeny.getFollowUpInfo();
            proposalAchievement = postUnlocksDeny.getAchievement();
        }
        
        // UpdateNewStats();

        // //TODO maybe pass hiddenGameVariables._currentProposal in, to avoid extra calls?

        // CheckInactiveProposals(proposalPostUnlocks);

        // CheckAchievements(proposalAchievement);

        // CheckFollowUp(proposalFollowUpUnlocks);

        // CheckDetails();

        // CheckStandbyProposals();

        //reset choice to a blank string for next use
        hiddenGameVariables._proposalDecision = ProposalChoiceEnum.NONE;

        bool updatedStats = UpdateNewStats();
        bool checkedInactiveProposals = CheckInactiveProposals(proposalPostUnlocks);
        bool checkedAchievements = CheckAchievements(proposalAchievement);
        bool checkedFollowUp = CheckFollowUp(proposalFollowUpUnlocks);
        bool checkedDetails = CheckDetails();
        bool checkedStandbyProposals = CheckStandbyProposals();


        while (updatedStats && checkedInactiveProposals &&
               checkedAchievements && checkedFollowUp &&
               checkedDetails && checkedStandbyProposals) {

            //Removes PROPOSAL_FULL_DECISION
            hiddenGameVariables._gameFlowEventBus.Dequeue();
            break;
        }
        
    }

    private bool UpdateNewStats() {
        //set current variables to the variables changed by the current proposal

        hiddenGameVariables._chosenDClassMethod = hiddenGameVariables._myStatCopy.__chosenDClassMethod;
        hiddenGameVariables._currentMajorCanon = hiddenGameVariables._myStatCopy.__currentMajorCanon;

        hiddenGameVariables._totalMTF = hiddenGameVariables._myStatCopy.__totalMTF;
        hiddenGameVariables._availableMTF = hiddenGameVariables._myStatCopy.__availableMTF;

        hiddenGameVariables._totalResearchers = hiddenGameVariables._myStatCopy.__totalResearchers;
        hiddenGameVariables._availableResearchers = hiddenGameVariables._myStatCopy.__availableResearchers;

        hiddenGameVariables._totalDClass = hiddenGameVariables._myStatCopy.__totalDClass;
        hiddenGameVariables._availableDClass = hiddenGameVariables._myStatCopy.__availableDClass;

        hiddenGameVariables._totalMorale = hiddenGameVariables._myStatCopy.__totalMorale;
        hiddenGameVariables._currentMorale = hiddenGameVariables._myStatCopy.__currentMorale;

        hiddenGameVariables._favourGOC = hiddenGameVariables._myStatCopy.__favourGOC;
        hiddenGameVariables._favourNalka = hiddenGameVariables._myStatCopy.__favourNalka;
        hiddenGameVariables._favourMekanite = hiddenGameVariables._myStatCopy.__favourMekanite;
        hiddenGameVariables._favourSerpentsHand = hiddenGameVariables._myStatCopy.__favourSerpentsHand;
        hiddenGameVariables._favourFactory = hiddenGameVariables._myStatCopy.__favourFactory;
        hiddenGameVariables._favourAnderson = hiddenGameVariables._myStatCopy.__favourAnderson;

        //loop through all active stat changes
        for (int i = 0; i < hiddenGameVariables._myStatCopy.__tempStatsChanged.Count; i++) {
            //Add all temp stat changes to the stat change bus
            hiddenGameVariables._statChangeEventBus.Add(hiddenGameVariables._myStatCopy.__tempStatsChanged[i]);
        }

        // hiddenGameVariables._currentGameState = GameStateEnum.PROPOSAL_STATS_UPDATED;
        // DecideNextAction.Raise();
        return true;
    }

    // Updates proposal's prereqs and adds them to the standby bus if they aren't already
    private bool CheckInactiveProposals(List<int> proposalPostUnlocks) {
        //Loops through the PostUnlocks and updates each mentioned proposal's prereq list and adds it to standby bus if needed
        for (int i = 0; i < proposalPostUnlocks.Count; i++) {
            
            //If the unlock is a prereq it is 0 or greater
            //If the value is a negative, then the unlock is a requirement
            //E.g: 1 means update a prereq of proposal 1 WHEREAS -1 means update a requirement of proposal 1
            if (proposalPostUnlocks[i] >= 0) {
                //[1] Get the proposal uuid from proposalPostUnlocks
                //[2] Find the Proposal Object at the location of the uuid in the proposal list from the scriptable object
                //[3] Update the prereq of the proposal by removing the ID of the current proposal from the prereq list
                //[         2          ][          1           ][                                    3                                    ]
                proposalsList._proposals[proposalPostUnlocks[i]].UpdateSingularRequirements(hiddenGameVariables._currentProposal.getProposalID());
            } else {
                //Math.Abs makes i positive as the negative section is no longer needed
                proposalsList._proposals[Math.Abs(proposalPostUnlocks[i])].UpdateChoiceRequirements(hiddenGameVariables._currentProposal.getProposalID());
            }
            
            //Loop through standby event bus to check that proposal isnt already in bus
            bool addToBus = true;
            for (int j = 0; j < standbyProposalEventBus.Count; j++) {
                /*
                If a j proposalID in standbyBus equals i proposalID going to be added then don't add, 
                as it'll be duplicated, instead update existing proposal
                */
                if(standbyProposalEventBus[j] == proposalPostUnlocks[i]) {
                    addToBus = false;
                }
            }
            if (addToBus == true) {
                standbyProposalEventBus.Add(Math.Abs(proposalPostUnlocks[i]));
            }
        }
        return true;
    } 

    private bool CheckAchievements(int proposalAchievement) {
        //TODO remove all the hiddenGameVariables._currentProposal calls and pass it in to avoid unneeded calls
        //If there is actually an achievement
        if (proposalAchievement != -1) {
            //Set the related achievement to being true
            achievementsList._achievements[proposalAchievement].setAchievementCompletion(true);
            //TODO figure out how to decouple this better, maybe?
            onAchievementCompleted.Raise(proposalAchievement);
        }
        return true;
    }

    private bool CheckDetails() {
        //If there are any details
        List<int> proposalDetails = hiddenGameVariables._currentProposal.getRelatedArticles();
        if(proposalDetails.Count > 0) {
            for(int i = 0; i < proposalDetails.Count; i++) {
                //Gets the category of detail at position [relatedArticle] 
                string currentDetailCategory = detailsList._details[proposalDetails[i]].getDetailCategory();
                //If the related article is an SCP
                if (currentDetailCategory == "SCP") {
                    detailsList._discoveredSCPs.Add(proposalDetails[i]);
                    detailsList._newlyDiscoveredDetails++;
                } else if (currentDetailCategory == "Tale") {
                    detailsList._discoveredTales.Add(proposalDetails[i]);
                    detailsList._newlyDiscoveredDetails++;
                } else if (currentDetailCategory == "Canon") {
                    detailsList._discoveredCanons.Add(proposalDetails[i]);
                    detailsList._newlyDiscoveredDetails++;
                } else if (currentDetailCategory == "Series") {
                    detailsList._discoveredSeries.Add(proposalDetails[i]);
                    detailsList._newlyDiscoveredDetails++;
                } else if (currentDetailCategory == "Group") {
                    detailsList._discoveredGroups.Add(proposalDetails[i]);
                    detailsList._newlyDiscoveredDetails++;
                }
            }

            // Debug.Log("Raising even to update the details menu");
            //TODO figure out how to decouple this better, maybe?
            onSwitchDetailsMenu.Raise();
        }
        return true;
    }

    private bool CheckFollowUp(int proposalFollowUpUnlocks) {
        if(proposalFollowUpUnlocks != -1) {
            followUpInfoList._currentFollowUpInfo.Add(proposalFollowUpUnlocks);
        }
        return true;
    }

    // Checks for Standby -> Active proposal possibilities
    // DOES NOT UPDATE EXISTING STANDBY PROPOSALS
    private bool CheckStandbyProposals() {
        for(int i = 0; i < standbyProposalEventBus.Count; i++) {
            //Check if the proposal is available to be moved (or at least, its ID) to the active bus
            if (proposalsList._proposals[standbyProposalEventBus[i]].IsProposalAvailable(hiddenGameVariables._currentMonth, hiddenGameVariables)) {
                //add the proposal to the active event bus, then remove it from the standby bus
                activeProposalEventBus.Add(standbyProposalEventBus[i]);
                standbyProposalEventBus.RemoveAt(i);
            }
        }
        return true;
    }

    // ==============================================================================================================
    // |                                               MTF STAT CODE                                                |
    // ==============================================================================================================

    private void changeAvailableMTF(int availableMTF, int duration) {
        //Change scriptable object
        hiddenGameVariables._myStatCopy.__availableMTF = hiddenGameVariables._availableMTF + availableMTF;
        //AvailableMTF is stat ID 0. This will be used when updating the UI to figure out what stats actually changed
        hiddenGameVariables._myStatCopy.__statsChanged.Add(0);

        //Create new instance of scriptable object storing the changed stat, amount, and duration
        ActiveStatChange statChange = new ActiveStatChange("MTF", availableMTF, duration);
        //Add the stat change to a list in the statCopy. This will be moved to the statChangeEventBus when confirmed
        hiddenGameVariables._myStatCopy.__tempStatsChanged.Add(statChange);

        //Debug.Log("Active Stat change check | MTF-affect: " + hiddenGameVariables._myStatCopy.__tempStatsChanged[0].getStatChangedEffect() + " | MTF-duration: " + hiddenGameVariables._myStatCopy.__tempStatsChanged[0].getStatChangedDuration());
    }

    private void changeTotalMTF(int totalMTF) {
        hiddenGameVariables._myStatCopy.__totalMTF = hiddenGameVariables._totalMTF + totalMTF;
        //Also have to increase available MTF parallel to increasing total MTF
        hiddenGameVariables._myStatCopy.__availableMTF = hiddenGameVariables._availableMTF + totalMTF;

        //Add the two stat ids to the stat changed list
        hiddenGameVariables._myStatCopy.__statsChanged.Add(0);
        hiddenGameVariables._myStatCopy.__statsChanged.Add(1);
    }

    // ==============================================================================================================
    // |                                            RESEARCHER STAT CODE                                            |
    // ==============================================================================================================

    private void changeAvailableResearchers(int availableResearchers, int duration) {
        hiddenGameVariables._myStatCopy.__availableResearchers = hiddenGameVariables._availableResearchers + availableResearchers;
        hiddenGameVariables._myStatCopy.__statsChanged.Add(2);

        ActiveStatChange statChange = new ActiveStatChange("Researchers", availableResearchers, duration);
        hiddenGameVariables._myStatCopy.__tempStatsChanged.Add(statChange);
    }

    public void changeTotalResearchers(int totalResearchers) {
        hiddenGameVariables._myStatCopy.__totalResearchers = hiddenGameVariables._totalResearchers + totalResearchers;
        hiddenGameVariables._myStatCopy.__availableResearchers = hiddenGameVariables._availableResearchers + totalResearchers;

        hiddenGameVariables._myStatCopy.__statsChanged.Add(2);
        hiddenGameVariables._myStatCopy.__statsChanged.Add(3);
    }

    // ==============================================================================================================
    // |                                               D-CLASS STAT CODE                                            |
    // ==============================================================================================================

    private void changeAvailableDClass(int availableDClass, int duration) {
        hiddenGameVariables._myStatCopy.__availableDClass = hiddenGameVariables._availableDClass + availableDClass;
        hiddenGameVariables._myStatCopy.__statsChanged.Add(4);

        ActiveStatChange statChange = new ActiveStatChange("DClass", availableDClass, duration);
        hiddenGameVariables._myStatCopy.__tempStatsChanged.Add(statChange);
    }

    private void changeTotalDClass(int totalDClass) {
        hiddenGameVariables._myStatCopy.__totalDClass = hiddenGameVariables._totalDClass + totalDClass;
        hiddenGameVariables._myStatCopy.__availableDClass = hiddenGameVariables._availableDClass + totalDClass;

        hiddenGameVariables._myStatCopy.__statsChanged.Add(4);
        hiddenGameVariables._myStatCopy.__statsChanged.Add(5);
    }

    // ==============================================================================================================
    // |                                                MORALE STAT CODE                                            |
    // ==============================================================================================================

    private void changeCurrentMorale(int currentMorale, int duration) {
        hiddenGameVariables._myStatCopy.__currentMorale = hiddenGameVariables._currentMorale + currentMorale;
        hiddenGameVariables._myStatCopy.__statsChanged.Add(6);

        ActiveStatChange statChange = new ActiveStatChange("Morale", currentMorale, duration);
        hiddenGameVariables._myStatCopy.__tempStatsChanged.Add(statChange);
    }

    private void changeTotalMorale(int totalMorale) {
        hiddenGameVariables._myStatCopy.__totalMorale = hiddenGameVariables._totalMorale + totalMorale;
        hiddenGameVariables._myStatCopy.__currentMorale = hiddenGameVariables._currentMorale + totalMorale;

        hiddenGameVariables._myStatCopy.__statsChanged.Add(6);
        hiddenGameVariables._myStatCopy.__statsChanged.Add(7);
    }

    //TODO add rest of stats

    //GOI Stats

    private void changeGOCFavor(int additionalGOCFavour) {
        hiddenGameVariables._myStatCopy.__favourGOC = hiddenGameVariables._favourGOC + additionalGOCFavour;
        hiddenGameVariables._myStatCopy.__statsChanged.Add(8);
    }

    private void changeNalkaFavor(int additionalNalkaFavour) {
        hiddenGameVariables._myStatCopy.__favourNalka = hiddenGameVariables._favourNalka + additionalNalkaFavour;
        hiddenGameVariables._myStatCopy.__statsChanged.Add(9);
    }

    private void changeMekaniteFavor(int additionalMekaniteFavour) {
        hiddenGameVariables._myStatCopy.__favourMekanite = hiddenGameVariables._favourMekanite + additionalMekaniteFavour;
        hiddenGameVariables._myStatCopy.__statsChanged.Add(10);
    }

    private void changeSerpentsHandFavor(int additionalSerpentsHandFavour) {
        hiddenGameVariables._myStatCopy.__favourSerpentsHand = hiddenGameVariables._favourSerpentsHand + additionalSerpentsHandFavour;
        hiddenGameVariables._myStatCopy.__statsChanged.Add(11);
    }

    private void changeFactoryFavor(int additionalFactoryFavour) {
        hiddenGameVariables._myStatCopy.__favourFactory = hiddenGameVariables._favourFactory + additionalFactoryFavour;
        hiddenGameVariables._myStatCopy.__statsChanged.Add(12);
    }

    private void changeAndersonFavor(int additionalAndersonFavour) {
        hiddenGameVariables._myStatCopy.__favourAnderson = hiddenGameVariables._favourAnderson + additionalAndersonFavour;
        hiddenGameVariables._myStatCopy.__statsChanged.Add(13);
    }

    private void changeDClassMethod(int dClassEnumValue) {
        hiddenGameVariables._myStatCopy.__chosenDClassMethod = (DClassMethodEnum)dClassEnumValue;
    }
}
