using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "HiddenGameVariables", menuName = "Runtime Variables/HiddenGameVariables")]
public class HiddenGameVariables : ScriptableObject 
{
    public bool _hasActiveProposal = false;

    public GenericProposal _prevProposal;
    public GenericProposal _currentProposal;

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
}