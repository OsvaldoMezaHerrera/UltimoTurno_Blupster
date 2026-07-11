using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Procedure
{
    public string idProcedure;
    public string name;
    public List<ProcedureStep> procedureSteps = new List<ProcedureStep>();
}
