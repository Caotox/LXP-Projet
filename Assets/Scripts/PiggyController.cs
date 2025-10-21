using UnityEngine;
using UnityEngine.InputSystem;

public class PiggyController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public InputActionReference horizontalMove;
    public InputActionReference verticalMove;
    public Vector3 moveDir;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        //Move
    }
    void Move()
    {
        moveDir = new Vector3(horizontalMove.action.ReadValue<float>(), 0, verticalMove.action.ReadValue<float>());
        transform.position += moveDir.normalized * moveSpeed * Time.deltaTime;

    }
}
