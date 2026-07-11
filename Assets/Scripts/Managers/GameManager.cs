using System.IO;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public ProceduresSO procedures;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            CargarProcedimientos();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void CargarProcedimientos()
    {
        // Obtiene la ruta del archivo JSON
        string pathFile = Path.Combine(Application.streamingAssetsPath, "procedures.json");

        // Valida que el archivo realmente exista
        if (File.Exists(pathFile))
        {
            string json = File.ReadAllText(pathFile);
            // Extrae los datos
            ProcedureList proceduresList = JsonUtility.FromJson<ProcedureList>(json);

            // Almacena todos los procedimientos encontrados en un ScriptableObject
            for (int i = 0; i < proceduresList.list.Count; i++)
            {
                Procedure currentProc = proceduresList.list[i];
                procedures.proceduresDictionary.Add(currentProc.idProcedure, currentProc);
            }

            Debug.Log("Procedimientos cargados con éxito.");
        }
        else
        {
            Debug.Log("No se encontró el archivo JSON de procedimientos.");
        }
    }

}
