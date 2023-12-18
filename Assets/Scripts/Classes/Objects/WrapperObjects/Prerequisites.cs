using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Wrapper of lists for saving to json
[Serializable]
public class Prerequisites
{
    public List<int> proposalSingularRequirements;
    public List<int> proposalChoiceRequirements;
    public List<int> statRequirements;

    public ChoiceRequirementList(List<int> proposalSingularRequirements, List<int> proposalChoiceRequirements, List<int> statRequirements) {
        this.proposalSingularRequirements = proposalSingularRequirements;
        this.proposalChoiceRequirements = proposalChoiceRequirements;
        this.statRequirements = statRequirements;
    }

    public List<int> getProposalSingularRequirements()
    {
        return this.proposalSingularRequirements;
    }

    public List<int> getProposalChoiceRequirements()
    {
        return this.proposalChoiceRequirements;
    }

    public List<int> getStatRequirements()
    {
        return this.statRequirements;
    }
}