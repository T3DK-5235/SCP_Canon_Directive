using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Wrapper of lists for saving to json
[Serializable]
public class Prerequisites
{
    public List<int> proposalSingularRequirements;
    public List<ChoiceRequirementList> proposalChoiceRequirements;
    public List<string> statRequirements;

    public Prerequisites(List<int> proposalSingularRequirements, List<ChoiceRequirementList> proposalChoiceRequirements, List<string> statRequirements) {
        this.proposalSingularRequirements = proposalSingularRequirements;
        this.proposalChoiceRequirements = proposalChoiceRequirements;
        this.statRequirements = statRequirements;
    }

    public List<int> getProposalSingularRequirements()
    {
        return this.proposalSingularRequirements;
    }

    public List<ChoiceRequirementList> getProposalChoiceRequirements()
    {
        return this.proposalChoiceRequirements;
    }

    public List<string> getStatRequirements()
    {
        return this.statRequirements;
    }
}