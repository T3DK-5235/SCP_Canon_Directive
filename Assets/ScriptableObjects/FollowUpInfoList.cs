using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "FollowUpInfoList", menuName = "Runtime Variables/FollowUpInfoList")]
public class FollowUpInfoList : ScriptableObject 
{
    public List<GenericFollowUpInfo> _followUpInfo = new List<GenericFollowUpInfo>();
    public List<int> _currentFollowUpInfo = new List<int>();
}