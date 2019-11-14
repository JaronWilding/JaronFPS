using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CC_PlayerMovement : MonoBehaviour
{
    [SerializeField, Range(0,30)] private float _speed = 12f;
    [SerializeField, Range(-15, 0)] private float _gravity = -9.81f;
    [SerializeField, Range(1, 10)] private float _jumpHeight = 2.5f;

    [SerializeField] private Transform _groundCheck = null;
    [SerializeField, Range(0, 2)] private float _groundDist = 0.4f;
    [SerializeField] private LayerMask _groundMask;
    [SerializeField] private bool _showGizmo = false;

    private CharacterController _charCon = null;
    private Vector3 _velocity;
    private bool _grounded;
    

    void Start()
    {
        _charCon = GetComponent<CharacterController>();
    }

    private void OnDrawGizmos()
    {
        if(_showGizmo)
        {
            Gizmos.DrawWireSphere(_groundCheck.position, _groundDist);
        }
    }

    void Update()
    {
        _grounded = Physics.CheckSphere(_groundCheck.position, _groundDist, _groundMask);

        if(_grounded && _velocity.y < 0)
        {
            _velocity.y = -2f;
        }


        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 fixedMovement = new Vector3(x, 0, z);
        FixNormalize(ref fixedMovement);
        
        Vector3 movement = transform.right * fixedMovement.x + transform.forward * fixedMovement.z;

        _charCon.Move(movement * _speed * Time.deltaTime);

        if(Input.GetButtonDown("Jump") && _grounded)
        {
            _velocity.y += Mathf.Sqrt(_jumpHeight * -2f * _gravity);
        }

        _velocity.y += _gravity * Time.deltaTime;

        _charCon.Move(_velocity * Time.deltaTime);
    }

    private void FixNormalize(ref Vector3 movement)
    {
        Vector3 direction = movement.normalized;
        Vector3 mainDirection = direction * Mathf.Clamp01(movement.magnitude / direction.magnitude);
        movement = mainDirection;
    }
}
