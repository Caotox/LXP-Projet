using UnityEngine;
using UnityEngine.UI;

public class PiggyController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float dashSpeed = 12f;
    public float dashDuration = 0.2f;
    public float dashCooldown = 1f;
    public string currentAction = "";
    public GameObject alertPanel;
    public Text messageText;

    public KeyCode interactKey = KeyCode.E;
    public KeyCode grabKey = KeyCode.F; 
    public float interactRange = 1f;
    public LayerMask interactLayer;
    public LayerMask grabLayer;
    public GameObject steakCookedPrefab;
    public GameObject tomatePrefab;
    public GameObject SaladePrefab;
    public GameObject SteakPrefab;
    public GameObject PainPrefab;
    public GameObject PainTomatePrefab;
    public GameObject PainSaladePrefab;
    public GameObject PainSteakPrefab;
    public GameObject PainSaladeTomatePrefab;
    public GameObject PainTomateSteakPrefab;
    public GameObject PainSaladeSteakPrefab;
    public GameObject PainSaladeTomateSteakPrefab;

    private Vector2 moveDir;
    private bool isDashing = false;
    private float dashTime = 0f;
    private float dashCooldownTimer = 0f;

    private GameObject heldObject = null; 
    private int originalSortingOrder;
    private SpriteRenderer heldObjectRenderer;

    void Start()
    {
        alertPanel.SetActive(false);
    }

    void Update()
    {
        HandleMovement();
        HandleDash();
        HandleInteraction();
        HandleGrab();
    }

    void HandleMovement()
    {
        if (isDashing) return;

        float moveX = 0f;
        float moveY = 0f;

        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) moveX = -1f;
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) moveX = 1f;
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) moveY = 1f;
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)) moveY = -1f;

        moveDir = new Vector2(moveX, moveY).normalized;
        transform.position += (Vector3)(moveDir * moveSpeed * Time.deltaTime);
    }

    void HandleDash()
    {
        if (dashCooldownTimer > 0f)
            dashCooldownTimer -= Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.Space) && dashCooldownTimer <= 0f && moveDir != Vector2.zero)
        {
            isDashing = true;
            dashTime = dashDuration;
            dashCooldownTimer = dashCooldown;
        }

        if (isDashing)
        {
            transform.position += (Vector3)(moveDir * dashSpeed * Time.deltaTime);
            dashTime -= Time.deltaTime;

            if (dashTime <= 0f)
                isDashing = false;
        }
    }

    void HandleInteraction()
{
    if (!Input.GetKeyDown(interactKey))
        return;

    Collider2D areaHit = Physics2D.OverlapCircle(transform.position, interactRange, interactLayer);
    if (areaHit == null)
    {
        Debug.Log("Aucune zone détectée");
        return;
    }

    string areaTag = areaHit.tag;
    Debug.Log("Zone détectée: " + areaTag);

    AlimentController aliment = null;
    AlimentController heldAliment = heldObject?.GetComponent<AlimentController>();

    // LOGIQUE DIFFÉRENTE POUR CHAQUE ZONE
    if (areaTag == "Assiette")
    {
        // POUR L'ASSIETTE : utiliser directement l'assiette
        aliment = areaHit.GetComponent<AlimentController>();
        
        if (aliment == null || !aliment.isAssiette)
        {
            Debug.Log("Aucune assiette valide trouvée");
            return;
        }

        if (heldAliment == null)
        {
            Debug.Log("Tu ne tiens rien à ajouter !");
            return; 
        }

        Debug.Log("Interaction Assiette. Tenu: " + heldAliment.name + ". Assiette: " + aliment.name);
        
        if (CanAssembleOnPlate(aliment, heldAliment))
        {
            AssembleBurger(aliment, heldAliment);
        }
        return;
    }
    else // POUR PLANCHE ET POÊLE
    {
        // Chercher les aliments SUR la zone (pas la zone elle-même)
        Collider2D[] objetsSurZone = Physics2D.OverlapCircleAll(areaHit.transform.position, 1f);
        
        foreach (Collider2D obj in objetsSurZone)
        {
            AlimentController alimentTemp = obj.GetComponent<AlimentController>();
            // Prendre le premier aliment valide qui n'est pas tenu
            if (alimentTemp != null && alimentTemp != heldAliment) 
            {
                aliment = alimentTemp;
                Debug.Log("Aliment trouvé sur " + areaTag + ": " + aliment.name);
                break;
            }
        }

        if (aliment == null)
        {
            Debug.Log("Aucun aliment trouvé sur la " + areaTag);
            return;
        }

        if (areaTag == "Planche")
        {
            Debug.Log("Tentative de découpe sur: " + aliment.name);
            Debug.Log("isOnPlanche: " + aliment.isOnPlanche + ", isCut: " + aliment.isCut);
            
            if (aliment.isOnPlanche && (aliment.isTomate || aliment.isSalade || aliment.isPain) && !aliment.isCut)
            {
                currentAction = "isCutting";
                Debug.Log("Action de découpe définie !");
            }
            else
            {
                Debug.Log("Conditions non remplies pour découper");
                return;
            }
        }
        else if (areaTag == "Poele")
        {
            Debug.Log("Tentative de cuisson sur: " + aliment.name);
            if (aliment.isOnPoele && aliment.isViande && !aliment.isCooked)
            {
                currentAction = "isCooking";
                Debug.Log("Action de cuisson définie !");
            }
            else if (aliment.CompareTag("PoeleViande")){
                Instantiate(steakCookedPrefab, aliment.transform.position, Quaternion.identity);
                Destroy(aliment.gameObject);
            }
            else
            {
                Debug.Log("Conditions non remplies pour cuire");
                return;
            }
        }

        // EXÉCUTER L'ACTION
        if (!string.IsNullOrEmpty(currentAction))
        {
            Debug.Log("Exécution de l'action: " + currentAction);
            aliment.HandleAlimentAction(
                aliment.isTomate, 
                aliment.isSalade, 
                aliment.isPain, 
                aliment.isViande,
                aliment.isCut, 
                aliment.isCooked, 
                aliment.isAssiette,
                currentAction
            );
            currentAction = "";
        }
    }
}

    bool CanAssembleOnPlate(AlimentController assiette, AlimentController ingredientTenu)
    {
        if (ingredientTenu.isPain && ingredientTenu.isCut)
        {
            if (!assiette.hasPain)
            {
                return true;
            }
            else
            {
                Debug.Log("Il y a déjà du pain sur l'assiette !");
                return false;
            }
        }

        if (ingredientTenu.isTomate || ingredientTenu.isSalade || ingredientTenu.isViande)
        {
            if (!assiette.hasPain)
            {
                Debug.Log("Mets le pain d'abord !");
                return false;
            }

            if ((ingredientTenu.isTomate || ingredientTenu.isSalade) && !ingredientTenu.isCut)
            {
                Debug.Log("L'ingrédient doit être coupé !");
                return false;
            }

            if (ingredientTenu.isViande && !ingredientTenu.isCooked)
            {
                Debug.Log("La viande doit être cuite !");
                return false;
            }

            return true;
        }

        Debug.Log("Ingrédient non valide pour l'assemblage !");
        return false;
    }

void AssembleBurger(AlimentController assiette, AlimentController ingredientTenu)
{
    Vector3 position = assiette.transform.position;
    GameObject newBurger = null;

    Debug.Log("=== DÉBUT ASSEMBLAGE ===");
    Debug.Log("Assiette: " + assiette.name);
    Debug.Log("Ingrédient tenu: " + ingredientTenu.name);

    if (ingredientTenu.isPain && ingredientTenu.isCut)
    {
        assiette.hasPain = true;
        Debug.Log(" Pain ajouté");
    }
    else if (ingredientTenu.isTomate)
    {
        assiette.hasTomate = true;
        Debug.Log(" Tomate ajoutée");
    }
    else if (ingredientTenu.isSalade)
    {
        assiette.hasSalade = true;
        Debug.Log(" Salade ajoutée");
    }
    else if (ingredientTenu.isViande)
    {
        assiette.hasSteak = true;
        Debug.Log(" Steak ajouté");
    }

    GameObject prefabToUse = GetBurgerPrefab(assiette);
    
    if (prefabToUse != null)
    {
        newBurger = Instantiate(prefabToUse, position, Quaternion.identity);
        Debug.Log(" Nouveau burger créé: " + prefabToUse.name);

        AlimentController newBurgerController = newBurger.GetComponent<AlimentController>();
        if (newBurgerController != null)
        {
            newBurgerController.isAssiette = true;
            newBurgerController.hasPain = assiette.hasPain;
            newBurgerController.hasTomate = assiette.hasTomate;
            newBurgerController.hasSalade = assiette.hasSalade;
            newBurgerController.hasSteak = assiette.hasSteak;
            
            if (assiette.hasPain) 
            {
                newBurgerController.isPain = true;
                newBurgerController.isCut = true;
            }
            
        }

        Debug.Log("Destruction de l'ancienne assiette: " + assiette.gameObject.name);
        Destroy(assiette.gameObject);
    }
    else
    {
        Debug.LogError("Aucun prefab trouvé pour cette combinaison !");
        return;
    }

    if (heldObject != null)
    {
        Destroy(heldObject);
        heldObject = null;
        heldObjectRenderer = null;
        Debug.Log("Ingrédient tenu détruit");
    }
}
    GameObject GetBurgerPrefab(AlimentController assiette)
{
    Debug.Log($"Composition - Pain:{assiette.hasPain} Tomate:{assiette.hasTomate} Salade:{assiette.hasSalade} Steak:{assiette.hasSteak}");

    if (assiette.hasTomate && assiette.hasSalade && assiette.hasSteak)
    {
        Debug.Log(" Choix: PainSaladeTomateSteakPrefab (Burger complet)");
        return PainSaladeTomateSteakPrefab;
    }
    else if (assiette.hasTomate && assiette.hasSalade)
    {
        Debug.Log(" Choix: PainSaladeTomatePrefab");
        return PainSaladeTomatePrefab;
    }
    else if (assiette.hasTomate && assiette.hasSteak)
    {
        Debug.Log(" Choix: PainTomateSteakPrefab");
        return PainTomateSteakPrefab;
    }
    else if (assiette.hasSalade && assiette.hasSteak)
    {
        Debug.Log(" Choix: PainSaladeSteakPrefab");
        return PainSaladeSteakPrefab;
    }
    else if (assiette.hasTomate)
    {
        Debug.Log(" Choix: PainTomatePrefab");
        return PainTomatePrefab;
    }
    else if (assiette.hasSalade)
    {
        Debug.Log(" Choix: PainSaladePrefab");
        return PainSaladePrefab;
    }
    else if (assiette.hasSteak)
    {
        Debug.Log(" Choix: PainSteakPrefab");
        return PainSteakPrefab;
    }
    else if (assiette.hasPain)
    {
        Debug.Log(" Choix: PainPrefab (juste le pain)");
        return PainPrefab;
    }
    
    Debug.LogError(" Aucune combinaison valide !");
    return null;
}

    GameObject FindBurgerOnPlate(Vector3 platePosition)
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(platePosition, 0.5f);
        foreach (Collider2D collider in colliders)
        {
            if (collider.gameObject.name.Contains("Pain") || collider.gameObject.name.Contains("Burger"))
            {
                AlimentController aliment = collider.GetComponent<AlimentController>();
                if (aliment != null && (aliment.isPain || aliment.isAssiette))
                {
                    Debug.Log("Burger trouvé sur l'assiette: " + collider.gameObject.name);
                    return collider.gameObject;
                }
            }
        }
        return null;
    }

    void HandleGrab()
    {
        if (Input.GetKeyDown(grabKey) && heldObject == null)
        {
            Debug.Log("Tentative de saisie d'un objet");
            RaycastHit2D hit = Physics2D.Raycast(transform.position, moveDir, interactRange, grabLayer);
            if (hit.collider != null)
            {
                heldObject = hit.collider.gameObject;
                heldObjectRenderer = heldObject.GetComponent<SpriteRenderer>();
                if (heldObjectRenderer != null)
                {
                    originalSortingOrder = heldObjectRenderer.sortingOrder;
                    heldObjectRenderer.sortingOrder = 2;
                }
                Rigidbody2D rb = heldObject.GetComponent<Rigidbody2D>();
                if (rb != null)
                {
                    rb.bodyType = RigidbodyType2D.Kinematic;
                }
                
                Debug.Log("Objet saisi: " + heldObject.name);
                
                if (heldObject.tag == "Tomate")
                {
                    Instantiate(tomatePrefab, heldObject.transform.position, Quaternion.identity);
                }
                else if (heldObject.tag == "Salad")
                {
                    Instantiate(SaladePrefab, heldObject.transform.position, Quaternion.identity);
                }
                else if (heldObject.tag == "Steak")
                {
                    Instantiate(SteakPrefab, heldObject.transform.position, Quaternion.identity);
                }
            }
        }

        if (Input.GetKey(grabKey) && heldObject != null)
        {
            Vector3 holdOffset = new Vector3(moveDir.x, moveDir.y, 0) * 0.5f; 
            heldObject.transform.position = transform.position + holdOffset;
        }

        if (Input.GetKeyUp(grabKey) && heldObject != null)
        {
            AlimentController aliment = heldObject.GetComponent<AlimentController>();
            if (aliment != null)
            {
                aliment.OnDropped(); 
            }

            Rigidbody2D rb = heldObject.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.bodyType = RigidbodyType2D.Dynamic;
            }
            
            heldObject = null;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, transform.position + (Vector3)(moveDir * interactRange));
    }
}
