using UnityEngine;

public class ThirdPersonCharacter : MonoBehaviour
{
    public float movementSpeed = 5f;
    public float m_GroundDistance = 2f;
    public float m_JumpPower = 1f;
    private Vector3 m_GroundNormalVector;


    private Rigidbody m_RigidBody;
    private Transform m_MainCamera;
    private bool m_IsGrounded;
    private float m_GroundAngle;
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

    private void Move(Vector3 movement, bool jump)
    {
        //Si el movimiento es mayor que 1 (por ejemplo, diagonales) lo normalizamos.
        if (movement.magnitude > 1f) movement.Normalize();
        //Transformamos el movimiento en el mundo a un movimiento local

        movement = transform.InverseTransformDirection(movement);
        movement = Vector3.ProjectOnPlane(movement, m_GroundNormalVector);

        CheckGround();
        CalculateGroundAngle();

        if (m_IsGrounded && jump)
        {
            HandleJump();
        }
        if (m_IsGrounded)
        {
            HandleGroundMovement(movement);
        }
        else
        {
            HandleAirMovement(movement);
        }
    }

    private void HandleGroundMovement(Vector3 movement)
    {
        m_RigidBody.MovePosition(transform.position + movement * Time.deltaTime * movementSpeed);
    }

    private void HandleAirMovement(Vector3 movement)
    {
        m_RigidBody.MovePosition(transform.position + movement * Time.deltaTime * movementSpeed);
    }

    private void HandleJump()
    {
        m_RigidBody.velocity = new Vector3(
            m_RigidBody.velocity.x,
            m_JumpPower,
            m_RigidBody.velocity.z);
        m_IsGrounded = false;
    }

    private void CheckGround()
    {
        RaycastHit hitInfo;
        bool collision = Physics.Raycast(
            transform.position + (Vector3.up * 0.1f),
            Vector3.down,
            out hitInfo,
            m_GroundDistance
        );

        if (collision)
        {
            m_IsGrounded = true;
            m_GroundNormalVector = hitInfo.normal;
        }
        else
        {
            m_IsGrounded = false;
            m_GroundNormalVector = Vector3.up;
        }
    }

    private void CalculateGroundAngle()
    {
        if (!m_IsGrounded)
        {
            m_GroundAngle = 90f;
            return;
        }
        else
        {
            m_GroundAngle = Vector3.Angle(m_GroundNormalVector, transform.forward);
        }
    }

    private void ApplyGravity()
    {
        if (!m_IsGrounded)
        {
            transform.position += Physics.gravity * Time.deltaTime;
        }
    }


    private void Update()
    {
        //Obtener inputs
        float xMovement = Input.GetAxis("Horizontal");
        float yMovement = Input.GetAxis("Vertical");

        //Calcular direccion movimiento respecto a la camara
        m_CamForward = Vector3.Scale(m_MainCamera.forward, new Vector3(1, 0, 1)).normalized;
        m_Move = yMovement * m_CamForward + xMovement * m_MainCamera.right;

        bool jump = Input.GetKeyDown(KeyCode.Space);

        //Aplicamos el movimiento
        Move(m_Move, jump);
    }
}
