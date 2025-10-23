using UnityEngine;

public class AlimentController : MonoBehaviour
{
    public bool isTomate;
    public bool isSalade;
    public bool isPain;
    public bool isViande;
    public bool isCut;
    public bool isCooked;
    public bool isAssiette;
    public bool isOnPlanche = false;
    public bool isOnPoele = false;
    public GameObject tomateCutPrefab;
    public GameObject saladeCutPrefab;
    public GameObject painCutPrefab;
    public GameObject viandeCookedPrefab;
    public GameObject viandeCookedPlatePrefab;
    public GameObject painAssiettePrefab;
    public GameObject assiettePainViandePrefab;
    public GameObject assiettePainSaladePrefab;
    public GameObject assiettePainTomatePrefab;
    public GameObject assiettePainViandeTomatePrefab;
    public GameObject assiettePainSaladeTomatePrefab;
    public GameObject assiettePainViandeSaladePrefab;
    public GameObject assiettePainViandeTomateSaladePrefab;
    // autres Prefabs selon les besoins

    public PiggyController piggyController; 

    void Start()
    {
        if (piggyController == null)
            piggyController = FindFirstObjectByType<PiggyController>();
    }

    void Update()
    {
        if (piggyController == null) return;

        string action = piggyController.currentAction;
    }

    public void HandleAlimentAction(
        bool isTomate, bool isSalade, bool isPain, bool isViande,
        bool isCut, bool isCooked, bool isAssiette,
        string action
    )
    {
        Debug.Log("Handling action: " + action);
        if (action == "isCutting")
        {
            if (isOnPlanche)
            {
                if (isTomate && !isCut)
                {
                    Debug.Log("Découpe la tomate !");
                    ReplaceWithPrefab(tomateCutPrefab);
                }
                else if (isSalade && !isCut)
                {
                    Debug.Log("Découpe la salade !");
                    ReplaceWithPrefab(saladeCutPrefab);
                }
                 else if (isPain && !isCut)
                {
                    Debug.Log("Découpe le pain !");
                    ReplaceWithPrefab(painCutPrefab);
                }
            }
            else
            {
                Debug.Log("L'aliment n'est pas sur la planche !");
            }
        }
    }

    void ReplaceWithPrefab(GameObject prefab)
    {
        Debug.Log("Prefab reçu: " + (prefab != null ? prefab.name : "NULL"));
    
    if (prefab == null)
    {
        
        return;
    }
        Instantiate(prefab, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
