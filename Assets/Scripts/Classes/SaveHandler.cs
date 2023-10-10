using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class SaveHandler : MonoBehaviour
{
    [SerializeField] HiddenGameVariables hiddenGameVariables;

    [SerializeField] ProposalsList proposalsList;

    [SerializeField] ExtraInfoList extraInfoList;

    void Awake() {
        InitProposals();
        InitExtraInfo();
        GetSaves();
    }

    //TODO Make an initial save file that can be used as a template to create new save files from
    //TODO This will include stuff like the basic proposal list

    private void InitProposals() {
        //Empties list on startup
        proposalsList._proposals.Clear();
        extraInfoList._extraInfo.Clear();

        TextAsset proposalListAsset = Resources.Load("ProposalList") as TextAsset;
        string proposalsString = proposalListAsset.ToString();

        Debug.Log(proposalsString);

        //Check this isnt inefficient
        GenericProposal[] proposalArray = JsonHelper.FromJson<GenericProposal>(proposalsString); 
        proposalsList._proposals.AddRange(proposalArray);
    }

    private void InitExtraInfo() {
        TextAsset extraInfoAsset = Resources.Load("ExtraInfoList") as TextAsset;
        string extraInfoString = extraInfoAsset.ToString();

        Debug.Log(extraInfoString);

        GenericExtraInfo[] extraInfoArray = JsonHelper.FromJson<GenericExtraInfo>(extraInfoString); 
        extraInfoList._extraInfo.AddRange(extraInfoArray);
    }

    public void GetSaves() {
        //There is always 3 saves. Sometimes they are just empty
        //Wrapper used to store saves?

        hiddenGameVariables._lastSavedProposal = 0;
    }
}
