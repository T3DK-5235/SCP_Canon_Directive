using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;

public class ProposalHandler : MonoBehaviour
{
    public List<GenericProposal> activeProposalEventBus;
    public List<GenericProposal> standbyProposalEventBus;

    public List<ScriptableObject> statChangeEventBus;

    private static StatTabletHandler statTabletHandler;
    //private SaveHandler saveHandler;

    [SerializeField] PublicGameVariables publicGameVariables;
    [SerializeField] HiddenGameVariables hiddenGameVariables;

    [SerializeField] ProposalsList proposalsList;

    //Maybe instead try make a UI scriptable object
    [SerializeField] GameObject proposalDescription_Text;

    void Awake() {
        //Stores proposal objects
        activeProposalEventBus = new List<GenericProposal>();
        standbyProposalEventBus = new List<GenericProposal>();

        statTabletHandler = new StatTabletHandler();
        // saveHandler = new SaveHandler();

        //Stores stat scriptable objects
        //Check every month
        statChangeEventBus = new List<ScriptableObject>();
        //Construct all of the proposals and add them to the ProposalList scriptable object


        //Get the last saved proposal (0 when starting) and set it to the current proposal
        hiddenGameVariables._currentProposal = proposalsList._proposals[hiddenGameVariables._lastSavedProposal];
    }

    void Update() {
        //Maybe put in try catch
        //get the text from the proposal UI object and set it to the current proposal's description
        proposalDescription_Text.transform.GetComponent<TextMeshProUGUI>().text = hiddenGameVariables._currentProposal.getProposalDescription();
    }

    //Might need to be a Coroutine to prevent game from continuing before all stats are changed
    // TODO make this into a listener function that checks whether accept or deny is ticked
    //Listens to the PlayerProposalTempDecision event system
    public void handleStatChanges(Component sender, object data) {
        List<string> proposalStatChanges = null;

        //Save an instance of the stats prior to changes for comparison purposes
        //This only really needs to be used when resetting requirements such as the D-Class 
        HiddenGameVariables statClone = statTabletHandler.storeCurrentStats();

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
                    changePermanentMTF(statAmount);
                    //Continue resets the loop to prevent extreme nesting when checking stat changes
                    continue;
                }
                //If the duration isn't permanent then this code is run instead
                changeTempMTF(statAmount, statDuration);
                continue;
            }

            if(proposalStatChanges[i] == "Researchers") {
                if(statDuration == 0) {
                    changePermanentResearchers(statAmount);
                    continue;
                }
                changeTempResearchers(statAmount, statDuration);
                continue;
            }

            if(proposalStatChanges[i] == "DClass") {
                if(statDuration == 0) {
                    changePermanentDClass(statAmount);
                    continue;
                }
                changeTempDClass(statAmount, statDuration);
                continue;
            }

            if(proposalStatChanges[i] == "Morale") {
                if(statDuration == 0) {
                    changePermanentMorale(statAmount);
                    continue;
                }
                changeTempMorale(statAmount, statDuration);
                continue;
            }

            //Display stat changes from stat handler, need to pass in statClone from above

            //TODO add rest of stats
        }
    }

    //Listens to the PlayerProposalDecision event system
    public void proposalDecision(Component sender, object data) {
        // Debug.Log("Accept proposal has recieved: " + data);
        // if (data is string) {
        //     Will send the choice of either deny or accept
        // }

        List<int> proposalPostUnlocks = null;
        List<string> proposalStatChanges = null;

        //Changes what is unlocked and changed based on player decision
        if (data == "accept") {
            proposalPostUnlocks = hiddenGameVariables._currentProposal.getPostUnlocksAccept();
        } else if (data == "deny") {
            proposalPostUnlocks = hiddenGameVariables._currentProposal.getPostUnlocksDeny();
        }
        
        //If the proposal has any PostUnlocks
        if (proposalPostUnlocks.Count > 0) {
            //Loops through the PostUnlocks
            for (int i = 0; i < proposalPostUnlocks.Count; i++) {
                //Get the proposal uuid from proposalPostUnlocks
                //Find the Proposal Object at the location of the uuid in the proposal list from the scriptable object
                //Add that Proposal Object to the standby bus
                standbyProposalEventBus.Add(proposalsList._proposals[proposalPostUnlocks[i]]);
            }
        }

        //TODO raise a event that the proposal is finished for the game manager to access?
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

    public void getNextProposal() {
        //Check Standby bus for any proposals to make active
        checkStandbyProposals();

        //Make this slightly more deterministic at a later date maybe
        int nextProposal = UnityEngine.Random.Range(0, activeProposalEventBus.Count - 1);

        //Add Proposal to currentProposal variable
        hiddenGameVariables._prevProposal = hiddenGameVariables._currentProposal;
        hiddenGameVariables._currentProposal = activeProposalEventBus[nextProposal];

        //Remove Proposal from active event bus
        activeProposalEventBus.RemoveAt(nextProposal);

        //TODO Actually check if this proposal can be accepted with the users current stats (Maybe not here, but somewhere)
    }

    public void checkStandbyProposals() {
        for(int i = 0; i < standbyProposalEventBus.Count; i++) {
            //Check if the proposal is available to be moved to the active bus
            if (standbyProposalEventBus[i].isProposalAvailable()) {
                //add the proposal to the active event bus, then remove it from the standby bus
                activeProposalEventBus.Add(standbyProposalEventBus[i]);
                standbyProposalEventBus.RemoveAt(i);
            }
        }
    }

    // ==============================================================================================================
    // |                                               MTF STAT CODE                                                |
    // ==============================================================================================================

    public void changeTempMTF(int tempMTF, int duration) {
        //Change scriptable object
        hiddenGameVariables._availableMTF += tempMTF;

        //Create new instance of scriptable object storing the changed stat, amount, and duration
        ActiveStatChange statChange = ActiveStatChange.CreateInstance("MTF", tempMTF, duration);
        //Add that instance to the statChangeEventBus
        statChangeEventBus.Add(statChange);
    }

    public void changePermanentMTF(int permMTF) {
        //Change scriptable object
        hiddenGameVariables._totalMTF += permMTF;
    }

    // ==============================================================================================================
    // |                                            RESEARCHER STAT CODE                                            |
    // ==============================================================================================================

    public void changeTempResearchers(int tempResearchers, int duration) {
        //Change scriptable object
        hiddenGameVariables._availableResearchers += tempResearchers;

        //Create new instance of scriptable object storing the changed stat, amount, and duration
        ActiveStatChange statChange = ActiveStatChange.CreateInstance("Researchers", tempResearchers, duration);
        //Add that instance to the statChangeEventBus
        statChangeEventBus.Add(statChange);
    }

    public void changePermanentResearchers(int permResearchers) {
        //Change scriptable object
        hiddenGameVariables._totalResearchers += permResearchers;
    }

    // ==============================================================================================================
    // |                                               D-CLASS STAT CODE                                            |
    // ==============================================================================================================

    public void changeTempDClass(int tempDClass, int duration) {
        //Change scriptable object
        hiddenGameVariables._availableDClass += tempDClass;

        //Create new instance of scriptable object storing the changed stat, amount, and duration
        ActiveStatChange statChange = ActiveStatChange.CreateInstance("DClass", tempDClass, duration);
        //Add that instance to the statChangeEventBus
        statChangeEventBus.Add(statChange);
    }

    public void changePermanentDClass(int permDClass) {
        //Change scriptable object
        hiddenGameVariables._totalDClass += permDClass;
    }

    // ==============================================================================================================
    // |                                                MORALE STAT CODE                                            |
    // ==============================================================================================================

    public void changeTempMorale(int tempMorale, int duration) {
        //Change scriptable object
        hiddenGameVariables._currentMorale += tempMorale;

        //Create new instance of scriptable object storing the changed stat, amount, and duration
        ActiveStatChange statChange = ActiveStatChange.CreateInstance("Morale", tempMorale, duration);
        //Add that instance to the statChangeEventBus
        statChangeEventBus.Add(statChange);
    }

    public void changePermanentMorale(int permMorale) {
        //Change scriptable object
        hiddenGameVariables._totalMorale += permMorale;
    }
}
