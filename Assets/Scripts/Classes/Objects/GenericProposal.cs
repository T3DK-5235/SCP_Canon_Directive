using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public class GenericProposal
{   
    [SerializeField] private string proposalTitle;
    [SerializeField] private string proposalDescription;
    [SerializeField] private int proposalID;
    [SerializeField] private int requiredMonth;
    [SerializeField] private int extraInfo;
    [SerializeField] private List<Prerequisites> proposalPrerequisites;
    [SerializeField] private List<PostUnlocks> proposalPostUnlocks;
    [SerializeField] private List<int> relatedArticles;

    //ProposalPrereqs are proposals that have to be done to contribute to unlocking this proposal
    //proposalChoiceRequirements are a list of proposals that only one has to be done to contribute to unlocking this proposal
    //
    // Description | ID | Month needed
    // Direct Proceeding Proposal
    // Unlocks after processing proposal and clicking accept | Unlocks after processing proposal and clicking Deny
    // Stats changed by proposal (eg MTF, 60, 0 for increase MTF by 60 permanently. OR BrokeMasq 60 0 which increases the progression for a canon by 60)
    // In addition, stats can store if a requirement is fulfilled, like if a d class choice is made eg: "Requirement name (same as enum), num reference to enum value (_DClassMethod, 1, 0);
    public GenericProposal(string proposalTitle, string proposalDescription, int proposalID, int requiredMonth, int extraInfo, 
                           List<Prerequisites> proposalPrerequisites, List<PostUnlocks> proposalPostUnlocks,
                           List<int> relatedArticles) {
        this.proposalTitle = proposalTitle;
        this.proposalDescription = proposalDescription;
        this.proposalID = proposalID;
        this.requiredMonth = requiredMonth;

        this.extraInfo = extraInfo;

        this.proposalPrerequisites = proposalPrerequisites;
        this.proposalPostUnlocks = proposalPostUnlocks;

        this.relatedArticles = relatedArticles;
    }

    // ==============================================================================================================
    // |                          Code to update whether a proposal can be made active                              |
    // ==============================================================================================================

    //Pass in the previous proposal uuid that was a prerequisite
    public void UpdateSingularRequirements(int prerequisiteFilled) {
        List<int> proposalSingularRequirements = proposalPrerequisites[0].getProposalSingularRequirements();
        //Loop through all prerequisites and when the filled prereq is found, remove it from the list
        //The prereq will be found, as the previous proposal will have stored this proposal as a PostUnlock thus we know when to check
        for (int i = 0; i < proposalSingularRequirements.Count; i++) {
            if (proposalSingularRequirements[i] == prerequisiteFilled) {
                //When prereq found, remove it from the List
                //This means when the list length is 0, it can be moved to the activeProposalEventBus
                proposalSingularRequirements.RemoveAt(i);
            }
        }
    }
    
    //Checks which choice requirements the current proposal needs to have fulfilled. Eg: if a choice of dclass is made
    //Each Requirement is stored as a list of proposalIDs
    public void UpdateChoiceRequirements(int choiceRequirementFilled) {
        List<ChoiceRequirementList> proposalChoiceRequirements = proposalPrerequisites[0].getProposalChoiceRequirements();
        //Loops through all requirement lists (groups of proposalIDs where ONE must be completed)
        for (int i = 0; i < proposalChoiceRequirements.Count; i++) {
            //For each requirement list loop through it
            for (int j = 0; j < proposalChoiceRequirements[i].getChoiceProposalIDs().Count; j++) {
                //If one of the values stored in proposalChoiceRequirements[i] matches the filled requirement
                if (proposalChoiceRequirements[i].getChoiceProposalIDs()[j] == choiceRequirementFilled) {
                    //Remove the whole list (As unlike Prereqs, which use AND logic, req lists use OR logic)
                    proposalChoiceRequirements.RemoveAt(i);
                    //If it is found, then this list will be deleted, so need to exit out from the current for loop
                    return;
                }
            }
        }
    }

    //Need to check when adding to active bus AND when about to be played
    public bool CheckStatRequirements(HiddenGameVariables hiddenGameVariables) {
        List<string> statRequirements = proposalPrerequisites[0].getStatRequirements();
        //Stat requirements are stored as i and i + 1 in a list. i = the stat itself and i + 1 is the value.
        //A negative on the value means the actual stat must be less than the presented number
        int statRequirementsFufilled = 0;
        for(int i = 0; i < statRequirements.Count; i += 2) {
            if (statRequirements[i] == "DClassMethod") {
                //Changes the string into the appropriate Enum for comparison to current Enum
                DClassMethodEnum requiredDClassMethod;
                Enum.TryParse(statRequirements[i+1], true, out requiredDClassMethod);
                if (requiredDClassMethod == hiddenGameVariables._chosenDClassMethod) {
                    statRequirementsFufilled++;
                }
                continue;
            } else if (statRequirements[i] == "Morale") {
                int requiredProposalStat = Int32.Parse(statRequirements[i + 1]);
                if(requiredProposalStat > 0 &&
                   hiddenGameVariables._currentMorale > requiredProposalStat) {
                   statRequirementsFufilled++;
                } else if (requiredProposalStat <= 0 &&
                   hiddenGameVariables._currentMorale <= requiredProposalStat) {
                }
                continue;
            }
        }

        if (statRequirementsFufilled == statRequirements.Count) {
            return true;
        } else {
            return false;
        }
    }

    public bool IsProposalAvailable(int currentMonth, HiddenGameVariables hiddenGameVariables) {
        //If there are no unfulfilled prereqs, reqs, and the current month is correct then the proposal is available
        // Debug.Log("Proposal to Check: " + proposalID + " ---- Prereqs: " + proposalPrerequisites.Count + " ---- Reqs: " + proposalChoiceRequirements.Count);
        if (proposalPrerequisites[0].getProposalSingularRequirements().Count == 0 && 
            proposalPrerequisites[0].getProposalChoiceRequirements().Count == 0 && 
            CheckStatRequirements(hiddenGameVariables) &&
            currentMonth >= requiredMonth) {
            return true;
        } else {
            return false;
        }
    }    

    // ==============================================================================================================
    // |                                  Code to get the affects of the proposal                                   |
    // ==============================================================================================================

    public List<PostUnlocks> getPostUnlocks() {
        return proposalPostUnlocks;
    }

    public string getProposalTitle() {
        return proposalTitle;
    }

    public string getProposalDescription() {
        return proposalDescription;
    }

    public int getProposalID() {
        return proposalID;
    }

    public int getExtraInfo() {
        return extraInfo;
    }

    public List<int> getRelatedArticles() {
        return relatedArticles;
    }
}
