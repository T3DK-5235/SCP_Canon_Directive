using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Wrapper of lists for saving to json
[Serializable]
public class PostUnlocks
{
    public List<int> proposalIDs;
    public List<int> proposalStatChanges;
    public int followUpInfo;
    public int achievement;

    public PostUnlocks(List<int> proposalIDs, List<int> proposalStatChanges, int followUpInfo, int achievement) {
        this.proposalIDs = proposalIDs;
        this.proposalStatChanges = proposalStatChanges;
        this.followUpInfo = followUpInfo;
        this.achievement = achievement;
    }
    
    public List<int> getProposalIDs()
    {
        return this.proposalIDs;
    }

    public List<int> getProposalStatChanges()
    {
        return this.proposalStatChanges;
    }

    public int getFollowUpInfo()
    {
        return this.followUpInfo;
    }

    public int getAchievement()
    {
        return this.achievement;
    }
}