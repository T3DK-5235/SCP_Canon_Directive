using System.Collections;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Collections.Generic;

public class SaveExample : MonoBehaviour
{
    string proposalTitle; 
    string proposalDescription; 
    int proposalID;
    int requiredMonth;
                           
    List<int> proposalPrerequisites;

    List<int> proposalPostUnlocksAccept; 
    List<int> proposalPostUnlocksDeny; 
    
    List<string> proposalStatChangesAccept;
    List<string> proposalStatChangesDeny;

    int extraInfo;

    [SerializeField] private GenericProposal exampleProposal;

    // Start is called before the first frame update
    // Use this to change the new proposal being entered
    void Start()
    {
        proposalTitle = "This is proposal 2 title";
        proposalDescription = "This is proposal 2 description"; 
        requiredMonth = 0;

        proposalPrerequisites = new List<int>() 
        {
            1,
            2
        };

        proposalPostUnlocksAccept = new List<int>() 
        {
            4,
            5
        };

        proposalPostUnlocksDeny = new List<int>() 
        {
            6,
            7
        };

        proposalStatChangesAccept = new List<string>() 
        {
            "MTF",
            "5",
            "2",
        };

        proposalStatChangesDeny = new List<string>() 
        {
            "Researchers",
            "5",
            "-5",
        };

        extraInfo = 1;

    }

    public void addNewProposalToJSON() {

        //Extract string from json here
        string currentProposals = System.IO.File.ReadAllText(Application.persistentDataPath + "/ProposalList.json");

        //Put all info into an array of proposal s
        GenericProposal[] proposalArray = JsonHelper.FromJson<GenericProposal>(currentProposals);

        //Debug.Log(proposalArray[0].getPostUnlocksAccept()[0]);

        //Need to check that this doesnt take shit tons of memory
        Array.Resize(ref proposalArray, proposalArray.Length + 1);

        //Add new proposal to array
        exampleProposal = new GenericProposal(proposalTitle, proposalDescription, (proposalArray.Length - 1), requiredMonth,
                                              proposalPrerequisites,
                                              proposalPostUnlocksAccept, proposalPostUnlocksDeny, 
                                              proposalStatChangesAccept, proposalStatChangesDeny, extraInfo);

        proposalArray[proposalArray.Length - 1] = exampleProposal;

        Debug.Log("created basic proposal");

        // List<GenericProposal> proposalList = new List<GenericProposal>() 
        // {
        //     exampleProposal,
        //     exampleProposal2
        // };

        // GenericProposal[] proposalArray = proposalList.ToArray();

        //The "true" is used to give it formatting rather than 1 long line
        string jsonObject = JsonHelper.ToJson(proposalArray);
        //This code replaces current file contents
        System.IO.File.WriteAllText(Application.persistentDataPath + "/ProposalList.json", jsonObject);
    }
}
