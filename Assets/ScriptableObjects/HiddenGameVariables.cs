using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "HiddenGameVariables", menuName = "Runtime Variables/HiddenGameVariables")]
public class HiddenGameVariables : ScriptableObject 
{

    [Header("Game State Handling")]
    public GameStateEnum _currentGameState;
    public int _currentMonth = 0;
    public int _currentMonthProposals = 0;
    public int _numMonthlyProposals;

    //====================================================================
    //                       PROPOSAL INFO SECTION                       |
    //====================================================================
    [Header("Overall Proposal Handling")]

    public GenericProposal _prevProposal;
    public int _lastSavedProposal;
    public bool _hasActiveProposal = false;

    //====================================================================
    //                   CURRENT PROPOSAL INFO SECTION                   |
    //====================================================================
    [Header("Current Proposal Handling")]

    public GenericProposal _currentProposal;
    public GenericExtraInfo _currentExtraInfo;
    //Will be true for accepted and false for denied
    public ProposalChoiceEnum _proposalDecision = ProposalChoiceEnum.NONE;

    //====================================================================
    //                         STAT CHANGE SECTION                       |
    //====================================================================
    [Header("Stat Change Handling")]

    public List<ActiveStatChange> _statChangeEventBus = new List<ActiveStatChange>();

    [Space(20)]

    public DClassMethodEnum _chosenDClassMethod = DClassMethodEnum.NONE;
    public MajorCanonEnum _currentMajorCanon = MajorCanonEnum.VANILLA;

    [Space(20)]

    public int _totalMTF = 50;
    public int _availableMTF = 50;

    [Space(10)]

    public int _totalResearchers = 50;
    public int _availableResearchers = 50;

    [Space(10)]

    public int _totalDClass = 50;
    public int _availableDClass = 50;

    [Space(10)]

    public int _totalMorale = 50;
    public int _currentMorale = 50;

    [Space(20)]

    public int _favourGOC = 50;
    public int _favourNalka = 50;
    public int _favourMekanite = 50;
    public int _favourSerpentsHand = 50;
    public int _favourFactory = 50;
    public int _favourAnderson = 50;

    // Create a nested class.
    [System.Serializable] // This attribute is necessary for the class to show up in the Unity Inspector.
    public class StatCopy
    {
        public List<int> __statsChanged = new List<int>();
        public List<ActiveStatChange> __tempStatsChanged = new List<ActiveStatChange>();

        [Header("Hidden Stats")]

        public DClassMethodEnum __chosenDClassMethod = DClassMethodEnum.NONE;
        public MajorCanonEnum __currentMajorCanon = MajorCanonEnum.VANILLA;

        [Header("Foundation UI Stats")]

        public int __totalMTF = 50;
        public int __availableMTF = 50;

        [Space(10)]

        public int __totalResearchers = 50;
        public int __availableResearchers = 50;

        [Space(10)]

        public int __totalDClass = 50;
        public int __availableDClass = 50;

        [Space(10)]

        public int __totalMorale = 50;
        public int __currentMorale = 50;

        [Header("GoI UI Stats")]
        public int __favourGOC = 50;
        public int __favourNalka = 50;
        public int __favourMekanite = 50;
        public int __favourSerpentsHand = 50;
        public int __favourFactory = 50;
        public int __favourAnderson = 50;
    }

    // You can create instances of the nested class within the Scriptable Object.
    public StatCopy _myStatCopy;

    public static object DecisionChoiceEnum { get; private set; }

    public void ResetToBase(GenericProposal initialProposal) {
        _currentMonth = 0;
        _currentMonthProposals = 0;

        _lastSavedProposal = 0;

        _currentGameState = GameStateEnum.PROPOSAL_ONGOING;

        _totalMTF = 50;
        _availableMTF = 50;

        _totalResearchers = 50;
        _availableResearchers = 50;

        _totalDClass = 50;
        _availableDClass = 50;

        _totalMorale = 50;
        _currentMorale = 50;

        _favourGOC = 50;
        _favourNalka = 50;
        _favourMekanite = 50;
        _favourSerpentsHand = 50;
        _favourFactory = 50;
        _favourAnderson = 50;

        _currentProposal = initialProposal;

        _statChangeEventBus = new List<ActiveStatChange>();
    }
}