using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ProceduresSO", menuName = "Scriptable Objects/ProceduresSO")]
public class ProceduresSO : ScriptableObject
{
    public Dictionary<string, Procedure> proceduresDictionary = new Dictionary<string, Procedure>();
}
