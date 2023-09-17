using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "HiddenGameVariables", menuName = "Runtime Variables/HiddenGameVariables")]
public class HiddenGameVariables : ScriptableObject 
{
    public int _currentMonth = 0;
    public bool _hasActiveProposal = false;

    public GenericProposal _prevProposal;
    public GenericProposal _currentProposal;

    public GenericExtraInfo _currentExtraInfo;

    public int _lastSavedProposal;

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


    // Create a nested class.
    [System.Serializable] // This attribute is necessary for the class to show up in the Unity Inspector.
    public class StatCopy
    {
        public List<int> __statsChanged = new List<int>();
        public List<ActiveStatChange> __tempStatsChanged = new List<ActiveStatChange>();

        [Header("Hidden Stats")]

        public DClassMethodEnum __chosenDClassMethod = DClassMethodEnum.NONE;
        public MajorCanonEnum __currentMajorCanon = MajorCanonEnum.VANILLA;

        [Header("UI Stats")]

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
    }

    // You can create instances of the nested class within the Scriptable Object.
    public StatCopy _myStatCopy;
}