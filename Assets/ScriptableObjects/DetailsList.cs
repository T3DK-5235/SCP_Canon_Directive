using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DetailsList", menuName = "Runtime Variables/DetailsList")]
public class DetailsList : ScriptableObject 
{
    public List<GenericDetails> _details = new List<GenericDetails>();
    public List<int> _discoveredSCPs = new List<int>();
    public List<int> _discoveredTales = new List<int>();
    public List<int> _discoveredCanons = new List<int>();
    public List<int> _discoveredSeries = new List<int>();
    public List<int> _discoveredGroups = new List<int>();
}