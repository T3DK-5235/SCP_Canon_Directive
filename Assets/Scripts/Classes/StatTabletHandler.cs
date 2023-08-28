using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatTabletHandler : MonoBehaviour
{
    [SerializeField] HiddenGameVariables hiddenGameVariables;

    //creates a clone of the current stats
    public HiddenGameVariables storeCurrentStats() {
        HiddenGameVariables statClone = Instantiate(hiddenGameVariables);
        return statClone;
    }

    //Functions here are called after a stat change

    //Takes in the stat clone which has the original values
    //TODO call this after proposal accepted, but also when month is over to return all the MTF and Researchers that can be
    public void getChanges(HiddenGameVariables statClone) {
        //2 bars exist, one of usable MTF, one hatched shows all possible mtf, and one solid shows available MTF

        //It might be better to minus one from the other and check if that is 0 in the if statement for optimisation?
        if(hiddenGameVariables._totalMTF != statClone._totalMTF) {
            int permMTFChange = statClone._totalMTF - hiddenGameVariables._totalMTF;
            //Update slider here by MTFChange value (have to decrease size as total went up)
        }
        if(hiddenGameVariables._availableMTF != statClone._availableMTF) {
            int tempMTFChange = statClone._availableMTF - hiddenGameVariables._availableMTF;
            //Update slider here by MTFChange value (decrease when MTF used, increase when they return)
        }

        //Removes the current statClone to prevent it taking up memory
        Destroy(statClone);
    }
}
