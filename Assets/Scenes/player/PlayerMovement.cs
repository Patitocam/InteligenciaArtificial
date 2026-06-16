using UnityEngine;


public class PlayerMovement : MonoBehaviour
{
    public float speed = 10.0f;
    public Rigidbody rb;
    public Vector3 movement;

    // el jugador se mueve 
    void Start()
    {
        rb = this.GetComponent<Rigidbody>();
    }

    void Update()
    {
        movement = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        moveCharacter(movement);
    }

    void moveCharacter(Vector3 direction)
    {
        rb.velocity = -direction.normalized * speed ;
    }
}
