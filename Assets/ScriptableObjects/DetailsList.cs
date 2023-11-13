using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DetailsList", menuName = "Runtime Variables/DetailsList")]
public class DetailsList : ScriptableObject 
{
    public List<GenericDetails> _details = new List<GenericDetails>();
    public List<int> _discoveredDetails = new List<int>();
}