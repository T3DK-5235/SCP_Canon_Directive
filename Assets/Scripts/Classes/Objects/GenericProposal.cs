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

    [SerializeField] private List<int> proposalPostUnlocksAccept; 
    [SerializeField] private List<int> proposalPostUnlocksDeny; 
    
    [SerializeField] private List<string> proposalStatChangesAccept;
    [SerializeField] private List<string> proposalStatChangesDeny;

    //ProposalPrereqs are proposals that have to be done to unlock this proposal
    //ProposalRequirements are a set of states that have to be set to unlock this proposal. For example, if a D-Class obtainment scenario is decided
    //
    // Description | ID | Month needed
    // Direct Proceeding Proposal
    // Unlocks after processing proposal and clicking accept | Unlocks after processing proposal and clicking Deny
    // Stats changed by proposal (eg MTF, 60, 0 for increase MTF by 60 permanently. OR BrokeMasq 60 0 which increases the progression for a canon by 60)
    // In addition, stats can store if a requirement is fulfilled, like if a d class choice is made eg: "Requirement name (same as enum), num reference to enum value (_DClassMethod, 1, 0);
    public GenericProposal(string proposalTitle, string proposalDescription, int proposalID, int requiredMonth,
                           List<int> proposalPrerequisites,
                           List<int> proposalPostUnlocksAccept, List<int> proposalPostUnlocksDeny, 
                           List<string> proposalStatChangesAccept, List<string> proposalStatChangesDeny) {
        this.proposalTitle = proposalTitle;
        this.proposalDescription = proposalDescription;
        this.proposalID = proposalID;
        this.requiredMonth = requiredMonth;

        this.proposalPrerequisites = proposalPrerequisites;

        this.proposalPostUnlocksAccept = proposalPostUnlocksAccept;
        this.proposalPostUnlocksDeny = proposalPostUnlocksDeny;

        this.proposalStatChangesAccept = proposalStatChangesAccept;
        this.proposalStatChangesDeny = proposalStatChangesDeny;
    }

    // ==============================================================================================================
    // |                          Code to update whether a proposal can be made active                              |
    // ==============================================================================================================

    //Pass in the previous proposal uuid that was a prerequisite
    public void updatePrerequisites(int prerequisiteFilled) {
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

    //Checks which requirements the current proposal needs and if they're fulfilled
    // public void updateRequirements() {
    //     //Loops through all requirements to check what they are
    //     for (int i = 0; i < proposalRequirements.Count; i++) {
    //         //Enums are compared first as they are faster than strings
    //         //If one of the requirements is for the DClassDecision and the DClassDecision has been made...
    //         if (hiddenGameVariables._chosenDClassMethod != DClassMethodEnum.NONE && proposalRequirements[i] == "_chosenDClassMethod") {
    //             //...Remove the req from the List
    //             proposalRequirements.RemoveAt(i);
    //         } else if (hiddenGameVariables._currentMajorCanon != MajorCanonEnum.VANILLA && proposalRequirements[i] == "_currentMajorCanon") {
    //             proposalRequirements.RemoveAt(i);
    //         }
    //     }
    // }
    
    public bool isProposalAvailable(int currentMonth) {
        //If there are no unfulfilled prereqs, reqs, and the current month is correct then the proposal is available
        if (proposalPrerequisites.Count == 0 && currentMonth <= requiredMonth) {
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
}
