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
    public bool hasPain = false;
    public bool hasTomate = false;
    public bool hasSalade = false;
    public bool hasSteak = false;
    
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
        if (action == "isCooking")
        {
            if (isOnPoele)
            {
                if (isViande && !isCooked)
                {
                    Debug.Log("Cuit la viande !");
                    ReplaceWithPrefab(viandeCookedPlatePrefab);
                }
            }
            else
            {
                Debug.Log("L'aliment n'est pas dans la poêle !");
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
    
    public void OnDropped()
    {
        isOnPlanche = false;
        isOnPoele = false;
        
        Collider2D[] overlaps = Physics2D.OverlapCircleAll(transform.position, 1f);

        foreach (Collider2D overlap in overlaps)
        {
            if (overlap.CompareTag("Planche"))
            {
                if (isTomate || isSalade || isPain)
                {
                    Debug.Log(gameObject.name + " posé sur la planche à découper !");
                    isOnPlanche = true;
                }
                else
                {
                    Debug.LogWarning("Cet ingrédient ne peut pas être coupé !");
                }
            }

            if (overlap.CompareTag("Poele"))
            {
                if (isViande && !isCooked)
                {
                    Debug.Log("Viande posée sur la plaque de cuisson !");
                    isOnPoele = true;
                }
                else if (isViande && isCooked)
                {
                    Debug.LogWarning("Cette viande est déjà cuite !");
                }
                else
                {
                    Debug.LogWarning("Seule la viande peut être cuite !");
                }
            }

            if (overlap.CompareTag("Assiette"))
            {
                AlimentController assiette = overlap.GetComponent<AlimentController>();
                
                if (isPain && isCut && assiette != null && assiette.isAssiette && !assiette.hasPain)
                {
                    Debug.Log("Pain coupé posé sur l'assiette ! Création du burger...");
                    
                    if (painAssiettePrefab != null)
                    {
                        GameObject newBurger = Instantiate(painAssiettePrefab, overlap.transform.position, Quaternion.identity);
                        
                        AlimentController newBurgerController = newBurger.GetComponent<AlimentController>();
                        if (newBurgerController != null)
                        {
                            newBurgerController.isAssiette = true;
                            newBurgerController.hasPain = true;
                            newBurgerController.isPain = true;
                            newBurgerController.isCut = true;
                            Debug.Log(" Burger avec pain créé depuis painAssiettePrefab !");
                        }
                        
                        
                        Destroy(assiette.gameObject);
                        Destroy(gameObject);
                    }
                    else
                    {
                        Debug.LogError(" painAssiettePrefab n'est pas assigné dans AlimentController !");
                    }
                }
                else if (isPain && !isCut)
                {
                    Debug.LogWarning("Le pain doit être coupé d'abord !");
                }
                else if (isPain && isCut && assiette != null && assiette.hasPain)
                {
                    Debug.LogWarning("Il y a déjà du pain sur cette assiette !");
                }
            }
        }
    }
    
    public void HandlePlatting(AlimentController ingredientTenu)
    {
        Debug.Log("Assemblage avec: " + ingredientTenu.name);
    }
}