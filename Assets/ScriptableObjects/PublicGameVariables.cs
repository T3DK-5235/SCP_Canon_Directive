using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PublicGameVariables", menuName = "Runtime Variables/PublicGameVariables")]
public class PublicGameVariables : ScriptableObject 
{
    //TODO move this to hiddengamevariables and delete this SO, it isnt needed
    public int _currentMonth;
}