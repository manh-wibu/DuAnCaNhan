using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CollisionSenses : CoreComponent
{
    protected Movement Movement
    {
        get => movement ?? core.GetCoreComponent(ref movement);
    }
    private Movement movement;

    public Transform GroundCheck 
    {  
        get => GenericNotImplementedError<Transform>.TryGet(groundCheck, core.transform.parent.name);
        private set => groundCheck = value;

    }
    public Transform WallCheck 
    {
        get => GenericNotImplementedError<Transform>.TryGet(wallCheck, core.transform.parent.name);
        private set => wallCheck = value; 
    }
    public Transform LedgeCheck 
    {
        get => GenericNotImplementedError<Transform>.TryGet(ledgeCheck, core.transform.parent.name);
        private set => ledgeCheck = value; 
    }
    public float GroundCheckRadius { get => groundCheckRadius; set => groundCheckRadius = value; }
    public float WallCheckDistance { get => wallCheckDistance; set => wallCheckDistance = value; }
    public LayerMask WhatIsGround { get => whatIsGround; set => whatIsGround = value; }

    [SerializeField] private Transform groundCheck;
    [SerializeField] private Transform wallCheck;
    [SerializeField] private Transform ledgeCheck;

    [SerializeField] private float groundCheckRadius;
    [SerializeField] private float wallCheckDistance;

    public Vector2 wallCheckSize = new Vector2(0.1f, 1.5f);

    [SerializeField] private LayerMask whatIsGround;


    public bool Ground
    {
        get => Physics2D.OverlapCircle(GroundCheck.position, groundCheckRadius, whatIsGround);
    }

    public bool WallFront
    {
        get => Physics2D.BoxCast(wallCheck.position, wallCheckSize, 0f, Vector2.right * Movement.FacingDirection,wallCheckDistance, whatIsGround);
    }

    public bool LedgeFront
    {
        get => Physics2D.Raycast(LedgeCheck.position, Vector2.down, wallCheckDistance, whatIsGround);
    }

    public bool WallBack
    {
        get => Physics2D.Raycast(wallCheck.position, Vector2.right * -Movement.FacingDirection, wallCheckDistance, whatIsGround);
    }
}
