using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Wrapper of lists for saving to json
[Serializable]
public class ChoiceRequirementList
{
    public List<int> ChoiceProposalIDs;
    
    public ChoiceRequirementList(List<int> ChoiceProposalIDs) {
        this.ChoiceProposalIDs = ChoiceProposalIDs;
    }

    public List<int> getChoiceProposalIDs() {
        return ChoiceProposalIDs;
    } 
}