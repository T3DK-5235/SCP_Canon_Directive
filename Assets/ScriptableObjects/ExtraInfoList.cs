using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ExtraInfoList", menuName = "Runtime Variables/ExtraInfoList")]
public class ExtraInfoList : ScriptableObject 
{
    public List<GenericExtraInfo> _extraInfo = new List<GenericExtraInfo>();
}