using UnityEngine;
using System.IO;
using System.Collections.Generic;
using System.Text;

public class DataManager : MonoBehaviour
{
    // --- Singleton Pattern ---
    public static DataManager Instance { get; private set; }

    // --- Données à collecter ---
    public int clickCount = 0;
    public float sessionTime = 0f;
    public List<Vector3> playerPositions = new List<Vector3>();
    public List<string> interactedObjects = new List<string>();

    // --- Référence au Joueur ---
    public GameObject player; // Tu devras glisser ton objet "Player" ici dans l'Inspector

    // --- Configuration ---
    public float logInterval = 2f; // Enregistre la position toutes les 2 secondes
    private bool isSaving = false;

    void Awake()
    {
        // Gestion du Singleton : s'il y en a déjà un, on détruit le nouveau
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        // On garde ce GameObject même si on change de scène (optionnel mais utile)
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        // Lance l'enregistrement de la position toutes les X secondes
        InvokeRepeating(nameof(RecordPosition), logInterval, logInterval);
        Debug.Log("✅ DataManager initialisé. Prêt à collecter des données.");
    }

    void Update()
    {
        // Met à jour le temps total écoulé
        sessionTime = Time.time;

        // Sauvegarde manuelle avec la touche 'S'
        if (Input.GetKeyDown(KeyCode.S) && !isSaving)
        {
            SaveDataToCSV();
        }
    }

    // --- Méthodes publiques appelées par d'autres scripts ---

    public void AddClick()
    {
        clickCount++;
        Debug.Log($"[Log] Clics totaux : {clickCount}");
    }

    public void RegisterInteraction(string objName)
    {
        // Évite les doublons dans la liste
        if (!interactedObjects.Contains(objName))
        {
            interactedObjects.Add(objName);
            Debug.Log($"[Log] Nouvel objet interacté : {objName}");
        }
    }

    // --- Méthodes internes ---

    void RecordPosition()
    {
        if (player != null)
        {
            playerPositions.Add(player.transform.position);
        }
        else
        {
            Debug.LogWarning("[DataManager] Le joueur n'est pas assigné !");
        }
    }

    void SaveDataToCSV()
    {
        isSaving = true;

        // Chemin du fichier : Dossier persistant de l'utilisateur
        string path = Path.Combine(Application.dataPath, "../session_data.csv");

        StringBuilder sb = new StringBuilder();

        // 1. Ligne d'en-tête
        sb.AppendLine("ClickCount,SessionTime,InteractedObjects,PlayerPositions");

        // 2. Données
        // Note : Pour les listes, on joint les éléments avec un séparateur "|"
        string objectsStr = string.Join(" | ", interactedObjects);
        string positionsStr = string.Join(" | ", playerPositions.ConvertAll(p => p.ToString()));

        sb.AppendLine($"{clickCount},{sessionTime:F2},\"{objectsStr}\",\"{positionsStr}\"");

        // 3. Écriture dans le fichier
        try
        {
            File.WriteAllText(path, sb.ToString());
            Debug.Log($"✅ ✅ ✅ DONNÉES SAUVEGARDÉES AVEC SUCCÈS !\nChemin : {path}");
        }
        catch (System.Exception e)
        {
            Debug.LogError($"❌ Erreur lors de la sauvegarde : {e.Message}");
        }

        isSaving = false;
    }
}