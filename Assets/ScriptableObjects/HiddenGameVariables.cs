using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "HiddenGameVariables", menuName = "Runtime Variables/HiddenGameVariables")]
public class HiddenGameVariables : ScriptableObject 
{
    public bool _hasActiveProposal = false;

    public GenericProposal _prevProposal;
    public GenericProposal _currentProposal;

    public GenericExtraInfo _currentExtraInfo;

    public int _lastSavedProposal;

    [Space(20)]

    public DClassMethodEnum _chosenDClassMethod = DClassMethodEnum.NONE;
    public MajorCanonEnum _currentMajorCanon = MajorCanonEnum.VANILLA;

    [Space(20)]

    public int _totalMTF = 100;
    public int _availableMTF = 100;

    [Space(10)]

    public int _totalResearchers = 100;
    public int _availableResearchers = 100;

    [Space(10)]

    public int _totalDClass = 100;
    public int _availableDClass = 100;

    [Space(10)]

    public int _totalMorale = 100;
    public int _currentMorale = 100;


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

        public int __totalMTF = 100;
        public int __availableMTF = 100;

        [Space(10)]

        public int __totalResearchers = 100;
        public int __availableResearchers = 100;

        [Space(10)]

        public int __totalDClass = 100;
        public int __availableDClass = 100;

        [Space(10)]

        public int __totalMorale = 100;
        public int __currentMorale = 100;
    }

    // You can create instances of the nested class within the Scriptable Object.
    public StatCopy _myStatCopy;

}