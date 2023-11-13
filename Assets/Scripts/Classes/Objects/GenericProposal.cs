using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public class GenericProposal
{
    [SerializeField] private string proposalDescription; 
    [SerializeField] private int proposalID;
    [SerializeField] private int requiredMonth;
    [SerializeField] private string proposalTitle;
                           
    [SerializeField] private List<int> proposalPrerequisites;
    [SerializeField] private List<ChoiceRequirementList> proposalChoiceRequirements;

    [SerializeField] private List<int> proposalPostUnlocksAccept; 
    [SerializeField] private List<int> proposalPostUnlocksDeny; 
    
    [SerializeField] private List<string> proposalStatChangesAccept;
    [SerializeField] private List<string> proposalStatChangesDeny;

    [SerializeField] private int extraInfo;
    [SerializeField] private int followUpInfo;
    [SerializeField] private int achievement;
    [SerializeField] private List<int> relatedArticles;

    //ProposalPrereqs are proposals that have to be done to contribute to unlocking this proposal
    //proposalChoiceRequirements are a list of proposals that only one has to be done to contribute to unlocking this proposal
    //
    // Description | ID | Month needed
    // Direct Proceeding Proposal
    // Unlocks after processing proposal and clicking accept | Unlocks after processing proposal and clicking Deny
    // Stats changed by proposal (eg MTF, 60, 0 for increase MTF by 60 permanently. OR BrokeMasq 60 0 which increases the progression for a canon by 60)
    // In addition, stats can store if a requirement is fulfilled, like if a d class choice is made eg: "Requirement name (same as enum), num reference to enum value (_DClassMethod, 1, 0);
    public GenericProposal(string proposalTitle, string proposalDescription, int proposalID, int requiredMonth,
                           List<int> proposalPrerequisites, List<ChoiceRequirementList> proposalChoiceRequirements,
                           List<int> proposalPostUnlocksAccept, List<int> proposalPostUnlocksDeny, 
                           List<string> proposalStatChangesAccept, List<string> proposalStatChangesDeny,
                           int extraInfo, int followUpInfo, int achievement, List<int> relatedArticles) {
        this.proposalTitle = proposalTitle;
        this.proposalDescription = proposalDescription;
        this.proposalID = proposalID;
        this.requiredMonth = requiredMonth;

        this.proposalPrerequisites = proposalPrerequisites;
        this.proposalChoiceRequirements = proposalChoiceRequirements;

        this.proposalPostUnlocksAccept = proposalPostUnlocksAccept;
        this.proposalPostUnlocksDeny = proposalPostUnlocksDeny;

        this.proposalStatChangesAccept = proposalStatChangesAccept;
        this.proposalStatChangesDeny = proposalStatChangesDeny;

        this.extraInfo = extraInfo;
        this.followUpInfo = followUpInfo;
        this.achievement = achievement;
        this.relatedArticles = relatedArticles;
    }

    // ==============================================================================================================
    // |                          Code to update whether a proposal can be made active                              |
    // ==============================================================================================================

    //Pass in the previous proposal uuid that was a prerequisite
    public void UpdatePrerequisites(int prerequisiteFilled) {
        //Loop through all prerequisites and when the filled prereq is found, remove it from the list
        //The prereq will be found, as the previous proposal will have stored this proposal as a PostUnlock thus we know when to check
        for (int i = 0; i < proposalPrerequisites.Count; i++) {
            if (proposalPrerequisites[i] == prerequisiteFilled) {
                //When prereq found, remove it from the List
                //This means when the list length is 0, it can be moved to the activeProposalEventBus
                proposalPrerequisites.RemoveAt(i);
            }
        }
    }
    
    //Checks which choice requirements the current proposal needs to have fulfilled. Eg: if a choice of dclass is made
    //Each Requirement is stored as a list of proposalIDs
    public void UpdateChoiceRequirements(int choiceRequirementFilled) {
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

    public bool IsProposalAvailable(int currentMonth) {
        //If there are no unfulfilled prereqs, reqs, and the current month is correct then the proposal is available
        // Debug.Log("Proposal to Check: " + proposalID + " ---- Prereqs: " + proposalPrerequisites.Count + " ---- Reqs: " + proposalChoiceRequirements.Count);
        if (proposalPrerequisites.Count == 0 && proposalChoiceRequirements.Count == 0 && currentMonth >= requiredMonth) {
            return true;
        } else {
            return false;
        }
    }    

    // ==============================================================================================================
    // |                                  Code to get the affects of the proposal                                   |
    // ==============================================================================================================

    public List<int> getPostUnlocksAccept() {
        return proposalPostUnlocksAccept;
    } 

    public List<int> getPostUnlocksDeny() {
        return proposalPostUnlocksDeny;
    } 

    public List<string> getStatChangesAccept() {
        return proposalStatChangesAccept;
    }

    public List<string> getStatChangesDeny() {
        return proposalStatChangesDeny;
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

    public int getFollowUpInfo() {
        return followUpInfo;
    }

    public int getAchievement() {
        return achievement;
    }

    public List<int> getRelatedArticles() {
        return relatedArticles;
    }
}
