using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
//using UnityEngine.UIElements;

using TMPro;

public class EditorSOResetter : MonoBehaviour
{
    [SerializeField] HiddenGameVariables hiddenGameVariables;
    [SerializeField] DetailsList detailsList;

    void Awake()
    {
        //TODO REMOVE THIS AFTER TESTING
        hiddenGameVariables._totalMTF = 50;
        hiddenGameVariables._availableMTF = 50;

        hiddenGameVariables._totalResearchers = 50;
        hiddenGameVariables._availableResearchers = 50;

        hiddenGameVariables._totalDClass = 50;
        hiddenGameVariables._availableDClass = 50;

        hiddenGameVariables._totalMorale = 50;
        hiddenGameVariables._currentMorale = 50;

        hiddenGameVariables._favourGOC = 50;
        hiddenGameVariables._favourNalka = 50;
        hiddenGameVariables._favourMekanite = 50;
        hiddenGameVariables._favourSerpentsHand = 50;
        hiddenGameVariables._favourFactory = 50;
        hiddenGameVariables._favourAnderson = 50;

        detailsList._discoveredSCPs = new List<int>();
        detailsList._discoveredTales = new List<int>();
        detailsList._discoveredCanons = new List<int>();
        detailsList._discoveredSeries = new List<int>();
        detailsList._discoveredGroups = new List<int>();
    }
}
