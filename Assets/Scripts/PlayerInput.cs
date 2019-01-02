using UnityEngine;
using System.Collections.Generic;
using System;

public enum CustomTrigger { moving, airborne, attacking };

/// <summary>
/// High-level player input script.
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
public class PlayerInput : MonoBehaviour {
    [Tooltip("Movement speed of the player.")] [Range(1, 20)]
    public float speed = 5;

    [Tooltip("How high the player can jump.")] [Range(0, 20)]
    public float jumpPower = 5;

    [Tooltip("Gravity multiplier for when you jump higher.")] [Range(0, 10)]
    public float fallMultiplier = 2.5f;

    [Tooltip("Gravity multiplier for when you tap jump. Should not be higher than fallMultiplier.")] [Range(0, 8)]
    public float lowJumpMultiplier = 2f;

    // Contains all possible enums and whether they are triggered (true/false)
    public Dictionary<CustomTrigger, bool> states = new Dictionary<CustomTrigger, bool>();

    private Rigidbody2D rb2d;
    private bool facingRight = true;
    private float moveX;

    private void Awake() {
        rb2d = GetComponent<Rigidbody2D>();

        // Loops through enums in CustomTrigger and adds them to the dictionary
        foreach (CustomTrigger ct in Enum.GetValues(typeof(CustomTrigger)))
            states.Add(ct, false);
    }

    private void Update() {
        HandleInput();
        HandleStates();

        // Makes player face correct direction, assuming sprite is initially drawn with player facing right
        if ((moveX < 0 && facingRight) || (moveX > 0 && !facingRight))
            FlipPlayer();
    }

    private void FixedUpdate() {
        // Horizontal movement
        rb2d.velocity = new Vector2(Input.GetAxis("Horizontal") * speed, rb2d.velocity.y);

        // Enables slightly higher jump if jump button is held down, like Mario (https://youtu.be/7KiK0Aqtmzc)
        if (rb2d.velocity.y < 0)
            rb2d.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        else if (rb2d.velocity.y > 0 && !Input.GetButton("Jump"))
            rb2d.velocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
    }

    /// <summary>
    /// The thing Britain couldn't do in 1776.
    /// </summary>
    private void HandleStates() {
        states[CustomTrigger.moving] = moveX != 0 ? true : false;
        states[CustomTrigger.airborne] = rb2d.velocity.y != 0 ? true : false;
    }

    /// <summary>
    /// Generic input implementation.
    /// </summary>
    private void HandleInput() {
        moveX = Input.GetAxis("Horizontal");
        if (Input.GetButtonDown("Jump")) Jump();
    }

    /// <summary>
    /// Generic jump implementation.
    /// </summary>
    private void Jump() {
        rb2d.velocity = Vector2.up * jumpPower;
    }

    /// <summary>
    /// Flips player object.
    /// </summary>
    private void FlipPlayer() {
        facingRight = !facingRight;
        Vector2 localScale = transform.localScale;
        localScale.x *= -1;
        transform.localScale = localScale;
    }
}
