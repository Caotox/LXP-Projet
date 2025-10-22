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

    public Sprite saladeCoupeeSprite;
    public Sprite tomateCoupeeSprite;

    private Vector2 moveDir;
    private bool isDashing = false;
    private float dashTime = 0f;
    private float dashCooldownTimer = 0f;

    private GameObject heldObject = null; 

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

    RaycastHit2D hit = Physics2D.Raycast(transform.position, moveDir, interactRange, interactLayer);

    if (hit.collider == null)
        return;

        GameObject target = hit.collider.gameObject;
        AlimentController aliment = target.GetComponent<AlimentController>();


        if (aliment == null)
            return;
    
    if (aliment.isTomate)
    {
        // Action spécifique à la tomate
    }
    else if (aliment.isSalade)
    {
        // Action spécifique à la salade
    }
    else if (aliment.isViande)
    {
        // Action spécifique à la viande
    }
    else if (aliment.isPain)
    {
        // Action spécifique au pain
    }
        
    Collider2D areaHit = Physics2D.OverlapCircle(transform.position, interactRange, interactLayer);
    if (areaHit == null)
        return;

    string areaTag = areaHit.tag;

    if (areaTag == "PlancheADecouper")
    {
        if ((aliment.isTomate || aliment.isSalade) && !aliment.isCut)
        {
            currentAction = "isCutting";
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
            RaycastHit2D hit = Physics2D.Raycast(transform.position, moveDir, interactRange, interactLayer);

            if (hit.collider != null)
            {
                heldObject = hit.collider.gameObject;
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
