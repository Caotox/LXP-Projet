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
    public PiggyController piggy;

    void Start()
    {
        if (piggy == null)
            piggy = FindFirstObjectByType<PiggyController>();
    }

    void Update()
    {
        string action = piggy.currentAction;

            HandleAlimentAction(
                isTomate, isSalade, isPain, isViande,
                isCut, isCooked, isAssiette,
                action
            );
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
            if (isPain && isCooked && !isAssiette)
            {
                ReplaceWithPrefab(painAssiettePrefab);
                return;
            }
        }
    }

    void ReplaceWithPrefab(GameObject prefab)
    {
        Instantiate(prefab, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
