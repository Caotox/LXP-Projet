using UnityEngine;

public class PiggyController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float dashSpeed = 12f;
    public float dashDuration = 0.2f;
    public float dashCooldown = 1f;
    public KeyCode interactKey = KeyCode.E;
    public float interactRange = 1f; 
    public LayerMask interactLayer; 
    private Vector2 moveDir;
    private bool isDashing = false;
    private float dashTime = 0f;
    private float dashCooldownTimer = 0f;
    public Sprite saladeCoupeeSprite;

    void Update()
    {
        HandleMovement();
        HandleDash();
        HandleInteraction();
    }

    void HandleMovement()
    {
        if (isDashing) return;

        float moveX = 0f;
        float moveY = 0f;

        if (Input.GetKey(KeyCode.Q) || Input.GetKey(KeyCode.LeftArrow)) moveX = -1f;
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) moveX = 1f;
        if (Input.GetKey(KeyCode.Z) || Input.GetKey(KeyCode.UpArrow)) moveY = 1f;
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
        if (Input.GetKeyDown(interactKey))
        {
            Debug.Log("Interaction key pressed");
            RaycastHit2D hit = Physics2D.Raycast(transform.position, moveDir, interactRange, interactLayer);

            if (hit.collider != null)
            {
                Debug.Log("Objet interactif détecté : ");
                GameObject target = hit.collider.gameObject;

                if (target.CompareTag("Salad"))
                {
                    Debug.Log("Salade détectée !");
                    Debug.Log("Changement de sprite pour la salade");
                    Sprite newSprite = saladeCoupeeSprite;
                    target.GetComponent<SpriteRenderer>().sprite = newSprite;
                }
            }
        }
    }
}
