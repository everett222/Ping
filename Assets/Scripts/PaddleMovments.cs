using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float movementSpeed;
    [SerializeField] private bool isAI;
    [SerializeField] private GameObject ball;

    [Header("Paddle Settings")]
    [SerializeField] private bool isRightPaddle; // Check this box ONLY on the right paddle
    [SerializeField] private KeyCode upKey = KeyCode.W;
    [SerializeField] private KeyCode downKey = KeyCode.S;

    private Rigidbody2D rb;
    private Vector2 playerMove;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        // When the game starts, check what mode the Main Menu set
        if (isRightPaddle)
        {
            // Check if we selected 2-Player mode (1 = True, 0 = False)
            if (PlayerPrefs.GetInt("TwoPlayerMode", 0) == 1)
            {
                isAI = false;
                upKey = KeyCode.UpArrow;    // Assign Up arrow for Player 2
                downKey = KeyCode.DownArrow; // Assign Down arrow for Player 2
            }
            else
            {
                isAI = true;
                // Grab the AI speed set by the Main Menu (defaults to 5 if not found)
                movementSpeed = PlayerPrefs.GetFloat("AIDifficulty", 5f);
            }
        }
    }

    void Update()
    {
        if (isAI)
        {
            AIControl();
        }
        else
        {
            PlayerControl();
        }
    }

    private void PlayerControl()
    {
        // Use specific keys so two players can share a keyboard
        if (Input.GetKey(upKey))
        {
            playerMove = new Vector2(0, 1);
        }
        else if (Input.GetKey(downKey))
        {
            playerMove = new Vector2(0, -1);
        }
        else
        {
            playerMove = new Vector2(0, 0);
        }
    }

    private void AIControl()
    {
        if (ball.transform.position.y > transform.position.y + 0.5f)
        {
            playerMove = new Vector2(0, 1);
        }
        else if (ball.transform.position.y < transform.position.y - 0.5f)
        {
            playerMove = new Vector2(0, -1);
        }
        else
        {
            playerMove = new Vector2(0, 0);
        }
    }

    private void FixedUpdate()
    {
        rb.linearVelocity = playerMove * movementSpeed;
    }
}