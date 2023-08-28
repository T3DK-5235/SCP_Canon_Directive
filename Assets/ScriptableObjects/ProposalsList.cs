using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ProposalsList", menuName = "Runtime Variables/ProposalsList")]
public class ProposalsList : ScriptableObject 
{
    public List<GenericProposal> _proposals = new List<GenericProposal>();
}