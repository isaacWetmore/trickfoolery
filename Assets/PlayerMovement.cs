using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Logic for basic game movement and dash mechanic.
/// </summary>
public class PlayerMovement : MonoBehaviour

{
    // Start is called before the first frame update
    
    //Player inputs
    private float _playerInputVertical;
    private float _playerInputHorizontal;
    private Vector3 _movementDirection;
    
    
    private float speedChangeFactor = 50f;
    public float dashCdTimer = 0;
    private float lastDesiredMoveSpeed;
    private float desiredMoveSpeed;
    private float moveSpeed;

    private Rigidbody _rb;
    private Transform _t;
    
    
    //Speed of different movement abilities
    private float WALKSPEED = 10f;
    private float DASHSPEED = 50f;
    private float DASHTIME = 0.15f;
    private float DASHCD = 0.5f;

    public MovementState state;

    public enum MovementState
    {
        walking,
        dashing
    }
        
    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _t = GetComponent<Transform>();
        moveSpeed = WALKSPEED;
    }

    // Update is called once per frame
    void Update()
    {
        //Calculate Inputs for player movement
        _playerInputVertical = Input.GetAxisRaw("Vertical");
        _playerInputHorizontal = Input.GetAxisRaw("Horizontal");
        _movementDirection = new Vector3(_playerInputHorizontal, 0, _playerInputVertical);
        _movementDirection.Normalize();

        if (_movementDirection != Vector3.zero && state != MovementState.dashing)
        {
            transform.forward = _movementDirection;
        } ;

        if (Input.GetButton("Jump") && dashCdTimer <= 0)
        {
            StartCoroutine(Dash());
        }

        //Process the cooldown timer for dashing
        if (dashCdTimer > 0)
            dashCdTimer -= Time.deltaTime;
    }

    private void FixedUpdate()
    {
        if (state == MovementState.walking) 
            _rb.MovePosition(transform.position + _movementDirection * WALKSPEED * Time.deltaTime);
        if (state == MovementState.dashing)
            _rb.MovePosition(transform.position + transform.forward * DASHSPEED * Time.deltaTime);
    }

    IEnumerator Dash()
    {
        dashCdTimer = DASHCD;
    
        float startTime = Time.time;

        while (Time.time < startTime + DASHTIME)
        {
            state = MovementState.dashing;
            //TODO: Add momentum to make dashing a little more fluid. 
            yield return null;
        }
        
        state = MovementState.walking;
    }
    

    /// <summary>
    /// Method <c>Dash</c> applies dash when spacebar is pressed.
    /// </summary>
    // IEnumerator Dash()
    //
    // {
    //     float time;
    //     float boostFactor = speedChangeFactor;
    //     float startTime = Time.time;
    //     
    //     
    //     time = 0;
    //     float difference = Mathf.Abs(dashSpeed - walkSpeed);
    //     while (time < difference)
    //     {
    //         moveSpeed = Mathf.Lerp(dashSpeed, walkSpeed, time / difference);
    //         time += Time.deltaTime * boostFactor;
    //         yield return null;
    //     }
    //
    //     moveSpeed = walkSpeed;
    //     speedChangeFactor = 1f;
    // }
}
