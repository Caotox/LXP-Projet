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
        if (action == "isCutting")
        {
            if (isTomate && !isCut)
            {
                ReplaceWithPrefab(tomateCutPrefab);
                return;
            }
            if (isSalade && !isCut)
            {
                ReplaceWithPrefab(saladeCutPrefab);
                return;
            }
        }

        if (action == "isCooking")
        {
            if (isViande && !isCooked)
            {
                ReplaceWithPrefab(viandeCookedPrefab);
                return;
            }
        }

        if (action == "isPlatting")
        {

            if (isPain && !isAssiette)
            {
                ReplaceWithPrefab(painAssiettePrefab);
                return;
            }

            if (isAssiette && isPain && NearbyHas("ViandeCooked"))
            {
                ReplaceWithPrefab(assiettePainViandePrefab);
                return;
            }

            if (ComparePrefabName("AssiettePainViande") && NearbyHas("TomateCut"))
            {
                ReplaceWithPrefab(assiettePainViandeTomatePrefab);
                return;
            }

            if (ComparePrefabName("AssiettePainViandeTomate") && NearbyHas("SaladeCut"))
            {
                ReplaceWithPrefab(assiettePainViandeTomateSaladePrefab);
                return;
            }
        }
    }

    bool NearbyHas(string namePart)
    {
        Collider2D[] nearby = Physics2D.OverlapCircleAll(transform.position, 1.2f);
        foreach (var obj in nearby)
        {
            if (obj.name.Contains(namePart))
                return true;
        }
        return false;
    }

    bool ComparePrefabName(string namePart)
    {
        return gameObject.name.Contains(namePart);
    }

    void ReplaceWithPrefab(GameObject prefab)
    {
        Instantiate(prefab, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
