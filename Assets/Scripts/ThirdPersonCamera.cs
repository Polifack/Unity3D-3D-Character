using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
    public bool lockCursor;
    public float mouseSensitivity = 10;
    public Transform target;
    public float distanceFromTarget = 2;
    public Vector2 pitchMinMax = new Vector2(-40, 85);
    public float rotationSmoothTime = .12f;

    private Vector3 m_smoothVel;
    private Vector3 m_currentRot;
    private float m_xRotation;
    private float m_yRotation;

    private void Start()
    {
        if (target == null)
        {
            Debug.LogWarning("[Warning]: no target found.");
        }

        if (lockCursor)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    private void LateUpdate()
    {
        m_xRotation += Input.GetAxis("Mouse X") * mouseSensitivity; //Se suma porque se considera que el movimiento que hagamos es positivo
        m_yRotation -= Input.GetAxis("Mouse Y") * mouseSensitivity; //Se resta porque se considera que el movimiento que hagamos tiene que ser al reves (salvo invertir el eje)
        m_yRotation = Mathf.Clamp(m_yRotation, pitchMinMax.x, pitchMinMax.y); //Limitamos la rotación en el eje Y

        //Suavizado
        m_currentRot = Vector3.SmoothDamp(m_currentRot, new Vector3(m_yRotation, m_xRotation), ref m_smoothVel, rotationSmoothTime);
        transform.eulerAngles = m_currentRot;

        //La posición de la camara depende de la distanceFromTarget
        transform.position = target.position - transform.forward * distanceFromTarget;
    }

}
