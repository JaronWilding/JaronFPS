using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


public class PlayerMovement : MonoBehaviour
{
    [Header("Input Fields")]
    [SerializeField] private Camera mainCam = null;
    [SerializeField] private Transform camOffset = null;

    [Header("Basic Movements")]
    [SerializeField] public float mouseSensitivity = 1.0f;
    [SerializeField] private float moveSpeed = 10.0f;
    [SerializeField] private float jumpForce = 6.0f;
    [SerializeField] private float flyForce = 8.0f;
    [SerializeField] private bool invertCamera = false;
    [SerializeField] private Vector2 cameraLimits = new Vector2(-90f, 90f);

    private Rigidbody _rbd = null;
    private Transform _mainCamTransform = null;
    private float mouseX = 0f, mouseY = 0f, x = 0f, z = 0f;
    private bool jump = false;
    private int jumpAmount = 2;
    private bool isGrounded = true;
    
    void Start()
    {
        _rbd = GetComponent<Rigidbody>();
        _mainCamTransform = mainCam.transform;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        GetInputs();
        MoveCamera();
    }
    private void GetInputs()
    {
        mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        mouseY += Input.GetAxis("Mouse Y") * mouseSensitivity;
        x = Input.GetAxis("Horizontal");
        z = Input.GetAxis("Vertical");
        jump = Input.GetKeyDown(KeyCode.Space);
    }

    private void MoveCamera()
    {
        //Clamps the camera rotation to the set limits (Defaults is -90 by 90).
        mouseY = Mathf.Clamp(mouseY, cameraLimits.x, cameraLimits.y);

        //One-line if statement, just allows player to invert camera movement.
        _mainCamTransform.localRotation = (invertCamera) ? Quaternion.Euler(new Vector3(mouseY, 0f, 0f)) : Quaternion.Euler(new Vector3(-mouseY, 0f, 0f));
    }

    void FixedUpdate()
    {
        MoveAndRotate();
        Grounded();
        if (jump && (isGrounded || jumpAmount > 0))
            PlayerJump();
    }

    private void MoveAndRotate()
    {
        //https://forum.unity.com/threads/limit-diagonal-speed-without-normalizing.468959/
        //Code was used here, as normalizing the proper way (which would just result in direction),
        //gave input lag. Clamping the magnitude to 0-1 makes it a lot easier.
        Vector3 offset = new Vector3(x, 0, z);
        Vector3 direction = offset.normalized;
        Vector3 final = direction * (Mathf.Clamp01(offset.magnitude / direction.magnitude) * moveSpeed) * Time.fixedDeltaTime;

        _rbd.MoveRotation(_rbd.rotation * Quaternion.Euler(new Vector3(0f, mouseX, 0f)));
        if (final.magnitude > 0.001f)
            _rbd.MovePosition(transform.position + (transform.forward * final.z) + (transform.right * final.x));
    }



    private void OnDrawGizmos()
    {
        float radius = GetComponent<CapsuleCollider>().radius;
        Vector3 pos1 = transform.position + (Vector3.up * radius);
        Vector3 pos2 = transform.position + new Vector3(0f, 2f, 0f) + (-Vector3.up * radius);

        Gizmos.DrawSphere(pos1, radius);
        Gizmos.DrawSphere(pos2, radius);
    }

    private void PlayerJump()
    {
        if(jumpAmount < 2)
            _rbd.AddRelativeForce(Vector3.up * flyForce, ForceMode.Impulse);
        else
            _rbd.AddRelativeForce(Vector3.up * jumpForce, ForceMode.Impulse);
        
        jumpAmount--;
        jump = false;
    }


    private void OnCollisionEnter(Collision col)
    {
        if (col.collider.tag != "Player")
        {
            isGrounded = true;
            jumpAmount = 2;
        }
        else
            isGrounded = false;
    }
    private void OnCollisionExit(Collision collision)
    {
        isGrounded = false;
    }

    //Still trying to get this to work...
    private void Grounded()
    {
        //https://answers.unity.com/questions/196381/how-do-i-check-if-my-rigidbody-player-is-grounded.html
        //Code referenced from here.
        //isGrounded = Physics.Raycast(transform.position, -Vector3.up, 1f);\
        float radius = GetComponent<CapsuleCollider>().radius;
        Vector3 pos1 = transform.position + (Vector3.up * radius);
        Vector3 pos2 = transform.position + new Vector3(0f, 2f, 0f) + (-Vector3.up * radius);

        //isGrounded = Physics.CheckCapsule(pos1, pos2, radius/2, LayerMask.NameToLayer("Player"));
        //Debug.Log(isGrounded);

        RaycastHit hit;

        //isGrounded = Physics.CheckCapsule(pos1, pos2, radius, layerMask);
        //isGrounded = Physics.SphereCast(pos1, radius, Vector3.up, out hit, Mathf.Infinity, LayerMask.NameToLayer("Player"));

        //Debug.Log(isGrounded);
        //if (hit.collider != null)
        //    Debug.Log(hit.collider.name);

    }

}
