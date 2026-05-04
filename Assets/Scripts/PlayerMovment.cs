using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float movementSpeed = 8f;
    [SerializeField] private bool isAI; 
    [SerializeField] private GameObject ball;
    
    [Header("Multiplayer Settings")]
    [SerializeField] private bool isRightPaddle; // ONLY check this box on the right paddle

    private Rigidbody2D rb;
    private Vector2 playerMove;

   void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        // If this is the right paddle, check what the main menu told us to do
        if (isRightPaddle)
        {
            // GetInt reads our saved choice. Defaults to 0 (Easy AI) if nothing is saved.
            int gameMode = PlayerPrefs.GetInt("GameMode", 0); 
            
            if (gameMode == 1) 
            {
                isAI = false; // Turn AI off for 2-Player
                movementSpeed = 12f; // Player 2 speed
            }
            else if (gameMode == 0)
            {
                isAI = true; 
                movementSpeed = 5f;  // EASY AI: very slow
            }
            else if (gameMode == 2)
            {
                isAI = true; 
                movementSpeed = 8f;  // MEDIUM AI: normal speed
            }
            else if (gameMode == 3)
            {
                isAI = true; 
                movementSpeed = 12f; // HARD AI: super fast (matches player 1)
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
        // Player 1 (Left Paddle) uses W and S keys
        if (!isRightPaddle)
        {
            if (Input.GetKey(KeyCode.W)) playerMove = new Vector2(0, 1);
            else if (Input.GetKey(KeyCode.S)) playerMove = new Vector2(0, -1);
            else playerMove = new Vector2(0, 0);
        }
        // Player 2 (Right Paddle) uses Up and Down Arrow keys
        else
        {
            if (Input.GetKey(KeyCode.UpArrow)) playerMove = new Vector2(0, 1);
            else if (Input.GetKey(KeyCode.DownArrow)) playerMove = new Vector2(0, -1);
            else playerMove = new Vector2(0, 0);
        }
    }

    private void AIControl()
    {
        if (ball.transform.position.y > transform.position.y + 0.5f)
            playerMove = new Vector2(0, 1);
        else if (ball.transform.position.y < transform.position.y - 0.5f)
            playerMove = new Vector2(0, -1);
        else
            playerMove = new Vector2(0, 0);
    }

    private void FixedUpdate()
    {
        rb.linearVelocity = playerMove * movementSpeed;
    }
}