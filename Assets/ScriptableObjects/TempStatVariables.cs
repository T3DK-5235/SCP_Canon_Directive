using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TempStatVariables", menuName = "Runtime Variables/TempStatVariables")]
public class TempStatVariables : ScriptableObject 
{
    public List<int> _statsChanged = new List<int>();
    public List<ActiveStatChange> _tempStatsChanged = new List<ActiveStatChange>();

    [Header("Hidden Stats")]

    public DClassMethodEnum _chosenDClassMethod = DClassMethodEnum.NONE;
    public MajorCanonEnum _currentMajorCanon = MajorCanonEnum.VANILLA;

    [Header("UI Stats")]

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