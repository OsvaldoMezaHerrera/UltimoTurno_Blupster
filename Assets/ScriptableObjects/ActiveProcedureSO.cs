using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ActiveProcedureSO", menuName = "Scriptable Objects/ActiveProcedureSO")]
public class ActiveProcedureSO : ScriptableObject
{
    public string idProcedure;
    public List<ProcedureStep> procedureSteps = new List<ProcedureStep>();
}
