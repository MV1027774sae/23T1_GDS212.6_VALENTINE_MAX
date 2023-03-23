using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class PlayerMovementTutorial : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed;

    public float groundDrag;

    public float jumpForce;
    public float jumpCooldown;
    public float airMultiplier;

    [HideInInspector] public float walkSpeed;
    [HideInInspector] public float sprintSpeed;

    [Header("Keybinds")]
    public KeyCode jumpKey = KeyCode.Space;
    public KeyCode throwKey = KeyCode.E;

    [Header("Ground Check")]
    public float playerHeight;
    public LayerMask whatIsGround;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private bool grounded;

    public Transform orientation;
    public Transform follow;
    public Transform throwPos;

    float horizontalInput;
    float verticalInput;

    Vector3 moveDirection;

    Rigidbody rb;

    public GameObject pillmin;

    [Header("Layermask")]
    [SerializeField] private LayerMask pillminLayer;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
    }

    private void Update()
    {
        grounded = Physics.Raycast(groundCheck.position, Vector3.down, 0.3f, whatIsGround);

        MyInput();
        SpeedControl();
        PillminThrow();

        if (grounded)
            rb.drag = groundDrag;
        else
            rb.drag = 0;

        if (Input.GetKeyDown(throwKey))
        {
            PillminThrow();
        }
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    private void MyInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");
    }

    private void MovePlayer()
    {
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        if (grounded)
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);

        else if (!grounded)
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f * airMultiplier, ForceMode.Force);
    }

    private void SpeedControl()
    {
        Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        if (flatVel.magnitude > moveSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * moveSpeed;
            rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
        }
    }

    private void PillminThrow()
    {
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        if (Input.GetKeyDown(throwKey))
        {
            Collider[] pillminZone = Physics.OverlapSphere(follow.position, 1.5f, pillminLayer);

            foreach (Collider pillmin in pillminZone)
            {
                pillmin.GetComponent<Transform>().position = new Vector3 (throwPos.position.x, throwPos.position.y, throwPos.position.z);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(follow.position, 1.5f);
    }

    IEnumerator Velocity()
    {
        yield return new WaitForSeconds(2);

        pillmin.GetComponent<PillminScript>().rb.velocity = new Vector3(0, 0, 0);
    }
        
        
}