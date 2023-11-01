using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AchievementsList", menuName = "Runtime Variables/AchievementsList")]
public class AchievementsList : ScriptableObject 
{
    public List<GenericAchievement> _achievements = new List<GenericAchievement>();

    //Use this to store order of achievements. Can change list by removing element then doing .Insert(Position, Item);
    //public List<int> _achievementsOrder = new List<int>();

    public List<GameObject> _displayedAchievements = new List<GameObject>();
}