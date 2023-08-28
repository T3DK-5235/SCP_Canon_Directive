using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] HiddenGameVariables hiddenGameVariables;

    private ProposalHandler proposalHandler;

    // Start is called before the first frame update
    void Awake()
    {
        //TODO gets save data from json save file (may change this to a save scene menu)
        //TODO create object pool for main proposals, extra info(, and news?)
    }

    // Update is called once per frame
    void Update()
    {
        //If there is not currently an active proposal
        if(hiddenGameVariables._hasActiveProposal != true) {
            proposalHandler.getNextProposal();
        }

        //Get next proposal
        //Check standby proposal    
    }
}
