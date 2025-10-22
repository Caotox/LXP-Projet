using UnityEngine;

public class PiggyController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float dashSpeed = 12f;
    public float dashDuration = 0.2f;
    public float dashCooldown = 1f;
    public string currentAction = "";

    public KeyCode interactKey = KeyCode.E;
    public KeyCode grabKey = KeyCode.F; 
    public float interactRange = 1f;
    public LayerMask interactLayer;
    public LayerMask grabLayer;

    public Sprite saladeCoupeeSprite;
    public Sprite tomateCoupeeSprite;

    private Vector2 moveDir;
    private bool isDashing = false;
    private float dashTime = 0f;
    private float dashCooldownTimer = 0f;

    private GameObject heldObject = null; 
    private int originalSortingOrder; 
    private SpriteRenderer heldObjectRenderer;

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
    Collider2D[] objetsSurZone = Physics2D.OverlapCircleAll(areaHit.transform.position, 1f);
    
    foreach (Collider2D obj in objetsSurZone)
    {
        AlimentController alimentTemp = obj.GetComponent<AlimentController>();
        if (alimentTemp != null)
        {
            aliment = alimentTemp;
            Debug.Log("Aliment trouvé sur la zone: " + aliment.name);
            break;
        }
    }

    if (aliment == null)
    {
        Debug.Log("Aucun aliment trouvé sur la zone " + areaTag);
        return;
    }

    Debug.Log("Aliment: " + aliment.name + " - isTomate: " + aliment.isTomate + " - isOnPlanche: " + aliment.isOnPlanche + " - isCut: " + aliment.isCut);

    if (areaTag == "Planche")
    {
        Debug.Log("Sur la planche à découper");
        if (aliment.isOnPlanche && (aliment.isTomate || aliment.isSalade) && !aliment.isCut)
        {
            currentAction = "isCutting";
            Debug.Log("Action définie: " + currentAction);
        }
        else
        {
            Debug.Log(" - isOnPlanche: " + aliment.isOnPlanche);
            Debug.Log(" - !isCut: " + !aliment.isCut);
        }
    }
    else if (areaTag == "PlaqueDeCuisson")
    {
        if (aliment.isViande && !aliment.isCooked)
        {
            currentAction = "isCooking";
        }
    }
    else if (areaTag == "Assiette")
    {
        if (!aliment.isAssiette)
        {
            currentAction = "isPlatting";
        }
    }

    if (!string.IsNullOrEmpty(currentAction))
    {
        Debug.Log("Exécution de l'action: " + currentAction);
        aliment.HandleAlimentAction(aliment.isTomate, 
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


    void HandleGrab()
    {
        if (Input.GetKeyDown(grabKey) && heldObject == null)
        {
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
            }
            Debug.Log("Objet saisi: " + (heldObject != null ? heldObject.name : "Aucun"));
        }

        if (Input.GetKey(grabKey) && heldObject != null)
        {
            Vector3 holdOffset = new Vector3(moveDir.x, moveDir.y, 0) * 0.5f; 
            heldObject.transform.position = transform.position + holdOffset;
        }

        if (Input.GetKeyUp(grabKey) && heldObject != null)
        {

            Collider2D[] overlaps = Physics2D.OverlapCircleAll(heldObject.transform.position, 1f);

            foreach (Collider2D overlap in overlaps)
            {
                if (overlap.CompareTag("Planche"))
                {
                    AlimentController aliment = heldObject.GetComponent<AlimentController>();
                    if (aliment != null)
                    {
                        if (aliment.isTomate)
                        {
                            Debug.Log("Tomate posée sur la planche à découper !");
                            aliment.isOnPlanche = true;
                        }
                        else if (aliment.isSalade)
                        {
                            Debug.Log("Salade posée sur la planche à découper !");
                            aliment.isOnPlanche = true;
                        }
                    }
                }


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
