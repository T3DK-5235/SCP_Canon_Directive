using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class SaveHandler : MonoBehaviour
{
    [SerializeField] PublicGameVariables publicGameVariables;
    [SerializeField] HiddenGameVariables hiddenGameVariables;

    [SerializeField] ProposalsList proposalsList;

    private GenericProposal exampleProposal;

    // string proposalDescription; 
    // int proposalID;
    // int requiredMonth;
                           
    // List<int> proposalPrerequisites;

    // List<int> proposalPostUnlocksAccept; 
    // List<int> proposalPostUnlocksDeny; 
    
    // List<string> proposalStatChangesAccept;
    // List<string> proposalStatChangesDeny;

    void Awake() {
        InitProposals();
    }

    public void InitProposals() {

        string totalProposalsString = System.IO.File.ReadAllText(Application.persistentDataPath + "/ProposalList.json");

        // Debug.Log(totalProposalsString);

        //Check this isnt inefficient
        GenericProposal[] proposalArray = JsonHelper.FromJson<GenericProposal>(totalProposalsString); 
        proposalsList._proposals.AddRange(proposalArray);   
    }
}
