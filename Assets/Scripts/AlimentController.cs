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
    public GameObject tomateCutPrefab;
    public GameObject saladeCutPrefab;
    public GameObject viandeCookedPrefab;
    public GameObject painAssiettePrefab;
    public GameObject assiettePainViandePrefab;
    public GameObject assiettePainViandeTomatePrefab;
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

        if (!string.IsNullOrEmpty(action))
        {
            HandleAlimentAction(
                isTomate, isSalade, isPain, isViande,
                isCut, isCooked, isAssiette,
                action
            );
        }
    }

    public void HandleAlimentAction(
        bool isTomate, bool isSalade, bool isPain, bool isViande,
        bool isCut, bool isCooked, bool isAssiette,
        string action
    )
    {
        Debug.Log("Handling action: " + action);
    }

    void ReplaceWithPrefab(GameObject prefab)
    {
        Instantiate(prefab, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
