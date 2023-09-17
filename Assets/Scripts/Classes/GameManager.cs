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

            //Increase the current month number
            hiddenGameVariables._currentMonth++;

            checkStatBus();

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

    private void checkStatBus() {

        //TODO figure out why hiddenGameVariables._statChangeEventBus[i].updateStatDuration(); doesnt work

        for (int i = 0; i < hiddenGameVariables._statChangeEventBus.Count; i++) {


            if (hiddenGameVariables._statChangeEventBus[i] == null) {
                Debug.Log("I mean it exists");
            }



            //Updates the number of months left for the stat
            hiddenGameVariables._statChangeEventBus[i].updateStatDuration();

            string changedStat = hiddenGameVariables._statChangeEventBus[i].getStatChanged();
            int statEffect = hiddenGameVariables._statChangeEventBus[i].getStatChangedEffect();

            if(hiddenGameVariables._statChangeEventBus[i].getStatChangedDuration() == 0) {

                if(changedStat == "MTF") {
                    //Set the stat back to its normal value (if it went down by 10, this will do +10 (or rather, --10).)
                    hiddenGameVariables._availableMTF -= statEffect;
                    //Remove the stat change from the bus as it is finished with
                    hiddenGameVariables._statChangeEventBus.RemoveAt(i);
                    //As the size of the bus has decreased, decrement i, this is because another stat change will be in the position of the old one.
                    //TODO figure out if this i--; should be at end  as right now it wont work
                    i--;
                    continue;
                }
                
                if(changedStat == "Researchers") {
                    hiddenGameVariables._availableResearchers -= statEffect;
                    hiddenGameVariables._statChangeEventBus.RemoveAt(i);
                    i--;
                    continue;
                }
                
                //TODO add rest of stats. MAYBE TRY DICTIONARY to fix issues


            }

            //Check if stat is finished and if so, remove and delete it
        }
    }
}
