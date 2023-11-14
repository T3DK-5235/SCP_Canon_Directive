using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.SocialPlatforms.Impl;

public class SaveHandler : MonoBehaviour
{
    [SerializeField] HiddenGameVariables hiddenGameVariables;

    [SerializeField] ProposalsList proposalsList;

    [SerializeField] ExtraInfoList extraInfoList;
    [SerializeField] AchievementsList achievementsList;
    [SerializeField] DetailsList detailsList;

    void Awake() {
        //Empties lists on startup
        proposalsList._proposals.Clear();
        extraInfoList._extraInfo.Clear();
        achievementsList._achievements.Clear();

        InitProposals();
        InitExtraInfo();
        InitAchievements();
        InitDetails();
        GetSaves();
    }

    //TODO Make an initial save file that can be used as a template to create new save files from
    //TODO This will include stuff like the basic proposal list

    private void InitProposals() {
        TextAsset proposalListAsset = Resources.Load("ProposalList") as TextAsset;
        string proposalsString = proposalListAsset.ToString();

        Debug.Log(proposalsString);

        //Check this isnt inefficient
        GenericProposal[] proposalArray = JsonHelper.FromJson<GenericProposal>(proposalsString); 
        proposalsList._proposals.AddRange(proposalArray);
    }

    private void InitAchievements() {
        TextAsset achievementListAsset = Resources.Load("AchievementList") as TextAsset;
        string achievementsString = achievementListAsset.ToString();

        Debug.Log(achievementsString);

        GenericAchievement[] achievementArray = JsonHelper.FromJson<GenericAchievement>(achievementsString); 
        achievementsList._achievements.AddRange(achievementArray);
    }

    private void InitExtraInfo() {
        TextAsset extraInfoAsset = Resources.Load("ExtraInfoList") as TextAsset;
        string extraInfoString = extraInfoAsset.ToString();

        Debug.Log(extraInfoString);

        GenericExtraInfo[] extraInfoArray = JsonHelper.FromJson<GenericExtraInfo>(extraInfoString); 
        extraInfoList._extraInfo.AddRange(extraInfoArray);
    }

    private void InitDetails() {
        TextAsset detailListAsset = Resources.Load("DetailList") as TextAsset;
        string detailsString = detailListAsset.ToString();

        Debug.Log(detailsString);

        GenericDetails[] detailArray = JsonHelper.FromJson<GenericDetails>(detailsString); 
        detailsList._details.AddRange(detailArray);
    }

    public void GetSaves() {
        //There is always 3 saves. Sometimes they are just empty
        //Wrapper used to store saves?

        hiddenGameVariables._lastSavedProposal = 0;
    }
}
