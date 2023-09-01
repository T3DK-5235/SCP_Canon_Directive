using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class SaveHandler : MonoBehaviour
{
    [SerializeField] PublicGameVariables publicGameVariables;
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
        string totalProposalsString = System.IO.File.ReadAllText(Application.persistentDataPath + "/ProposalList.json");
        // Debug.Log(totalProposalsString);

        //Check this isnt inefficient
        GenericProposal[] proposalArray = JsonHelper.FromJson<GenericProposal>(totalProposalsString); 
        proposalsList._proposals.AddRange(proposalArray);
    }

    private void InitExtraInfo() {
        string totalExtraInfoString = System.IO.File.ReadAllText(Application.persistentDataPath + "/ExtraInfoList.json");

        GenericExtraInfo[] extraInfoArray = JsonHelper.FromJson<GenericExtraInfo>(totalExtraInfoString); 
        extraInfoList._extraInfo.AddRange(extraInfoArray);
    }

    public void GetSaves() {
        //There is always 3 saves. Sometimes they are just empty
        //Wrapper used to store saves?

        hiddenGameVariables._lastSavedProposal = 0;
    }
}
