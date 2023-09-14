using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    [SerializeField] HiddenGameVariables hiddenGameVariables;

    //Default length of a month
    [SerializeField] int monthLength = 4;

    //How many proposals have been played this month
    [SerializeField] int numMonthlyProposal = 1;

    [Header("Events")]
    public GameEvent onProposalChanged;

    void Awake()
    {
        //TODO gets save data from json save file (may change this to a save scene menu)
        //TODO create object pool for main proposals, extra info(, and news?)

        //Initial num of proposals in the first month
        monthLength = UnityEngine.Random.Range(4, 7);
    }

    // Update is called once per frame
    void Update()
    {
        //TODO move tutorialHandler to here instead
    }

    public void checkMonth(Component sender, object data) {
        //Increment the number of proposals done this month
        string animType;
        numMonthlyProposal++;
        //If the month should end and the gamestate has moved to checking if it is the month end
        if(numMonthlyProposal == monthLength) {
            //Change to loading the month
            Debug.Log("Loading next month");
            animType = "NewMonth";

            //Get new month length
            monthLength = UnityEngine.Random.Range(5, 8);
            numMonthlyProposal = 1;

        } else {
            Debug.Log("Load next proposal");

            animType = "NewProposal";
        }

        //Call animation for when new month/new proposal happens
        onProposalChanged.Raise(animType);
    }
}
