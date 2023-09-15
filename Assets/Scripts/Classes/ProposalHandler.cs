using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProposalHandler : MonoBehaviour
{
    public List<int> activeProposalEventBus;
    public List<int> standbyProposalEventBus;

    [SerializeField] PublicGameVariables publicGameVariables;
    [SerializeField] HiddenGameVariables hiddenGameVariables;

    //private TempStatVariables tempStatVariables;

    [SerializeField] ProposalsList proposalsList;

    [Header("Events")]
    public GameEvent updateFlashingStats;
    public GameEvent ifMonthEnd;

    void Awake() {
        //Stores proposal objects
        activeProposalEventBus = new List<int>();
        standbyProposalEventBus = new List<int>();

        //Get the last saved proposal (0 when starting) and set it to the current proposal
        hiddenGameVariables._currentProposal = proposalsList._proposals[hiddenGameVariables._lastSavedProposal];
        //TODO write a save manager that'd pop in this "lastSavedProposal" plus the rest of the stats
    }

    //Listens to the PlayerProposalDecision event system
    public void proposalDecision(Component sender, object data) {

        //TODO send tempStatVariables to UI to update bars with
        //TODO also update hiddenGameVariables 

        List<string> proposalStatChanges = null;
        List<int> proposalPostUnlocks = null;

        //Changes what is unlocked and changed based on player decision
        if (data == "accept") {
            proposalPostUnlocks = hiddenGameVariables._currentProposal.getPostUnlocksAccept();
        } else if (data == "deny") {
            proposalPostUnlocks = hiddenGameVariables._currentProposal.getPostUnlocksDeny();
        }
        
        updateNewStats();

        checkInactiveProposals(proposalPostUnlocks);
        checkStandbyProposals();

        //Increase the number of proposals recorded as being done that month
        ifMonthEnd.Raise();
        // getNextProposal();
    }

    //Might need to be a Coroutine to prevent game from continuing before all stats are changed
    //Listens to the PlayerProposalTempDecision event system
    public void handleStatChanges(Component sender, object data) {
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


        hiddenGameVariables._myStatCopy = statCopy;

        if (data == "accept") {
            proposalStatChanges = hiddenGameVariables._currentProposal.getStatChangesAccept();
        } else if (data == "deny") {
            proposalStatChanges = hiddenGameVariables._currentProposal.getStatChangesDeny();
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
                changeAvailableMorale(statAmount, statDuration);
                continue;
            }

            //TODO add rest of stats
        }
        updateFlashingStats.Raise();
    }

    public void updateNewStats() {
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

        //loop through all active stat changes
        for (int i = 0; i < hiddenGameVariables._myStatCopy.__tempStatsChanged.Count; i++) {
            //Add all temp stat changes to the stat change bus
            hiddenGameVariables._statChangeEventBus.Add(hiddenGameVariables._myStatCopy.__tempStatsChanged[i]);
        }
    }

    public void checkInactiveProposals(List<int> proposalPostUnlocks) {
        //If the proposal has any PostUnlocks (proposals that can be added to the standby list)
        if (proposalPostUnlocks.Count > 0) {
            //Loops through the PostUnlocks
            for (int i = 0; i < proposalPostUnlocks.Count; i++) {
                //Get the proposal uuid from proposalPostUnlocks
                //Find the Proposal Object at the location of the uuid in the proposal list from the scriptable object
                GenericProposal proposalToUpdate = proposalsList._proposals[proposalPostUnlocks[i]];
                //Update the prereq of the newly unlocked proposal
                proposalToUpdate.updatePrerequisites(hiddenGameVariables._currentProposal.getProposalID());
                
                //Add that Proposal ID to the standby bus
                standbyProposalEventBus.Add(proposalPostUnlocks[i]);
            }
        }
    }

    public void checkStandbyProposals() {
        for(int i = 0; i < standbyProposalEventBus.Count; i++) {
            //Check if the proposal is available to be moved (or at least, its ID) to the active bus
            if (proposalsList._proposals[standbyProposalEventBus[i]].isProposalAvailable(publicGameVariables._currentMonth)) {
                //add the proposal to the active event bus, then remove it from the standby bus
                activeProposalEventBus.Add(standbyProposalEventBus[i]);
                standbyProposalEventBus.RemoveAt(i);
            }
        }
    }

    // public void handleRequirementChanges() {   
    //     string requirementChange = hiddenGameVariables._currentProposal.getRequirementChange();

    //     //If the string is empty, then just return from the function
    //     //Fix this later as this'd add extra frame to stack which isnt efficient
    //     if(requirementChange == "") { return; }

    //     //requirementChange is stored as Requirement#Choice
    //     //For Example DClassMethod#1 which would be the PRISON choice for DClass
    //     string[] requirements = requirementChange.Split("#");
    //     int chosenPath = Int32.Parse(requirements[1]);

    //     //If the requirement is to change the DClassMethod and it hasnt already been set
    //     if (requirements[0] == "DClassMethod" && hiddenGameVariables._chosenDClassMethod != DClassMethodEnum.NONE) {
    //         switch(chosenPath) 
    //         {
    //             case 1:
    //                 hiddenGameVariables._chosenDClassMethod = DClassMethodEnum.PRISON;
    //                 break;
    //             case 2:
    //                 hiddenGameVariables._chosenDClassMethod = DClassMethodEnum.PARTIALVOLUNTEER;
    //                 break;
    //             case 3:
    //                 hiddenGameVariables._chosenDClassMethod = DClassMethodEnum.CLONING;
    //                 break;
    //             default:
    //                 break;
    //         } 
    //     } else if (requirements[0] == "MajorCanon" && hiddenGameVariables._currentMajorCanon != MajorCanonEnum.VANILLA) {
    //         switch(chosenPath) 
    //         {
    //             case 1:
    //                 hiddenGameVariables._currentMajorCanon = MajorCanonEnum.BROKENMASQ;
    //                 break;
    //             case 2:
    //                 hiddenGameVariables._currentMajorCanon = MajorCanonEnum.BELLEVERSE;
    //                 break;
    //             case 3:
    //                 hiddenGameVariables._currentMajorCanon = MajorCanonEnum.RATSNEST;
    //                 break;
    //             default:
    //                 break;
    //         }
    //     }
    // }

    public void getNextProposal(Component sender, object data) {

        //If there is only 1 proposal possible, choose that, if not then randomly (Make this slightly more deterministic at a later date maybe) choose
        // int nextProposalPos = activeProposalEventBus[0];
        // if(activeProposalEventBus.Count != 1) {
        int nextProposalPos = UnityEngine.Random.Range(0, activeProposalEventBus.Count - 1);
        // }

        //Add Proposal to currentProposal variable
        hiddenGameVariables._prevProposal = hiddenGameVariables._currentProposal;
        hiddenGameVariables._currentProposal = proposalsList._proposals[activeProposalEventBus[nextProposalPos]];

        //Remove Proposal from active event bus
        activeProposalEventBus.RemoveAt(nextProposalPos);

        //TODO Actually check if this proposal can be accepted with the users current stats (Maybe not here, but somewhere)
    }

    //TODO actually check if all of these need to be public
    

    // ==============================================================================================================
    // |                                               MTF STAT CODE                                                |
    // ==============================================================================================================

    public void changeAvailableMTF(int availableMTF, int duration) {
        //Change scriptable object
        hiddenGameVariables._myStatCopy.__availableMTF = hiddenGameVariables._availableMTF + availableMTF;
        //AvailableMTF is stat ID 0. This will be used when updating the UI to figure out what stats actually changed
        hiddenGameVariables._myStatCopy.__statsChanged.Add(0);

        //Create new instance of scriptable object storing the changed stat, amount, and duration
        ActiveStatChange statChange = new ActiveStatChange("MTF", availableMTF, duration);
        //Add the stat change to a list in the statCopy. This will be moved to the statChangeEventBus when confirmed
        hiddenGameVariables._myStatCopy.__tempStatsChanged.Add(statChange);
    }

    public void changeTotalMTF(int totalMTF) {
        hiddenGameVariables._myStatCopy.__totalMTF = hiddenGameVariables._totalMTF + totalMTF;
        hiddenGameVariables._myStatCopy.__statsChanged.Add(1);
    }

    // ==============================================================================================================
    // |                                            RESEARCHER STAT CODE                                            |
    // ==============================================================================================================

    public void changeAvailableResearchers(int availableResearchers, int duration) {
        hiddenGameVariables._myStatCopy.__availableResearchers = hiddenGameVariables._availableResearchers + availableResearchers;
        hiddenGameVariables._myStatCopy.__statsChanged.Add(2);

        ActiveStatChange statChange = new ActiveStatChange("Researchers", availableResearchers, duration);
        hiddenGameVariables._myStatCopy.__tempStatsChanged.Add(statChange);
    }

    public void changeTotalResearchers(int totalResearchers) {
        hiddenGameVariables._myStatCopy.__totalResearchers = hiddenGameVariables._totalResearchers + totalResearchers;
        hiddenGameVariables._myStatCopy.__statsChanged.Add(3);
    }

    // ==============================================================================================================
    // |                                               D-CLASS STAT CODE                                            |
    // ==============================================================================================================

    public void changeAvailableDClass(int availableDClass, int duration) {
        hiddenGameVariables._myStatCopy.__availableDClass = hiddenGameVariables._availableDClass + availableDClass;
        hiddenGameVariables._myStatCopy.__statsChanged.Add(4);

        ActiveStatChange statChange = new ActiveStatChange("DClass", availableDClass, duration);
        hiddenGameVariables._myStatCopy.__tempStatsChanged.Add(statChange);
    }

    public void changeTotalDClass(int totalDClass) {
        hiddenGameVariables._myStatCopy.__totalDClass = hiddenGameVariables._totalDClass + totalDClass;
        hiddenGameVariables._myStatCopy.__statsChanged.Add(5);
    }

    // ==============================================================================================================
    // |                                                MORALE STAT CODE                                            |
    // ==============================================================================================================

    public void changeAvailableMorale(int currentMorale, int duration) {
        hiddenGameVariables._myStatCopy.__currentMorale = hiddenGameVariables._currentMorale + currentMorale;
        hiddenGameVariables._myStatCopy.__statsChanged.Add(6);

        ActiveStatChange statChange = new ActiveStatChange("Morale", currentMorale, duration);
        hiddenGameVariables._myStatCopy.__tempStatsChanged.Add(statChange);
    }

    public void changeTotalMorale(int totalMorale) {
        hiddenGameVariables._myStatCopy.__totalMorale = hiddenGameVariables._totalMorale + totalMorale;
        hiddenGameVariables._myStatCopy.__statsChanged.Add(7);;
    }

    //TODO add rest of stats
}
