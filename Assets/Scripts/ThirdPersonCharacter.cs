using UnityEngine;

public class ThirdPersonCharacter : MonoBehaviour
{
    public float movementSpeed = 5f;

    private Rigidbody m_RigidBody;
    private Transform m_MainCamera;
    private Vector3 m_CamForward;             //Direccion de movimiento
    private Vector3 m_Move;

    private void Start()
    {
        m_RigidBody = GetComponent<Rigidbody>();
        m_MainCamera = Camera.main.transform;

        if (m_RigidBody == null)
        {
            Debug.LogWarning("[Warning]: no rigidbody found.");
        }
        if (Camera.main == null)
        {
            Debug.LogWarning("[Warning]: no main camera found.");
        }
    }

    private void Move(Vector3 movement)
    {
        //Si el movimiento es mayor que 1 (por ejemplo, diagonales) lo normalizamos.
        if (movement.magnitude > 1f) movement.Normalize();
        //Transformamos el movimiento en el mundo a un movimiento local
        movement = transform.InverseTransformDirection(movement);

        //Aplicar movimiento al rigidBody
        m_RigidBody.MovePosition(this.transform.position + movement * Time.deltaTime * movementSpeed);
    }


    private void FixedUpdate()
    {
        //Obtener inputs
        float xMovement = Input.GetAxis("Horizontal");
        float yMovement = Input.GetAxis("Vertical");

        //Calcular direccion movimiento respecto a la camara
        m_CamForward = Vector3.Scale(m_MainCamera.forward, new Vector3(1, 0, 1)).normalized;
        m_Move = yMovement * m_CamForward + xMovement * m_MainCamera.right;

        //Aplicamos el movimiento
        Move(m_Move);
    }
}
