using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro; // NEW: This lets Unity talk to your TextMeshPro text!
using UnityEngine.SceneManagement;

public class BallMovement : MonoBehaviour
{
    [SerializeField] private float initialSpeed = 10;
    [SerializeField] private float speedIncrease = 0.25f;
    [SerializeField] private float edgeSpeedBonus = 10f; 
    [SerializeField] private Text playerScore;
    [SerializeField] private Text AIScore;

    [Header("Winner Screen UI")]
    [SerializeField] private GameObject winnerScreen; 
    [SerializeField] private TextMeshProUGUI winnerText; // UPDATED: Now looks for TextMeshPro!

    private int hitCounter;
    private Rigidbody2D rb;
    private float currentMaxSpeed; 

    private int pointCap;
    private int gameMode;
    private bool gameOver = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        
        pointCap = PlayerPrefs.GetInt("PointCap", 21);
        gameMode = PlayerPrefs.GetInt("GameMode", 0);

        // Make sure the winner screen is hidden when the game starts
        if (winnerScreen != null) winnerScreen.SetActive(false);

        Invoke("StartBall", 2f);
    }

    private void FixedUpdate()
    {
        rb.linearVelocity = Vector2.ClampMagnitude(rb.linearVelocity, currentMaxSpeed);
    }

    private void StartBall()
    {
        currentMaxSpeed = initialSpeed + (speedIncrease * hitCounter);
        rb.linearVelocity = new Vector2(-1, 0) * currentMaxSpeed;
    }

    private void ResetBall()
    {
        rb.linearVelocity = new Vector2(0, 0);
        transform.position = new Vector2(0, 0);
        hitCounter = 0;
        Invoke("StartBall", 2f);
    }

    private void PlayerBounce(Transform myObject)
    {
        hitCounter++;

        Vector2 ballPos = transform.position;
        Vector2 playerPos = myObject.position;

        float xDirection, yDirection;
        
        if(transform.position.x > 0) xDirection = -1;
        else xDirection = 1;

        yDirection = (ballPos.y - playerPos.y) / myObject.GetComponent<Collider2D>().bounds.size.y;
        
        if(yDirection == 0) yDirection = 0.25f;

        float distanceFromCenter = Mathf.Abs(yDirection);
        currentMaxSpeed = initialSpeed + (speedIncrease * hitCounter) + (distanceFromCenter * edgeSpeedBonus);

        Vector2 bounceDirection = new Vector2(xDirection, yDirection).normalized;
        rb.linearVelocity = bounceDirection * currentMaxSpeed;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Paddle"))
        {
            PlayerBounce(collision.transform);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (gameOver) return; 

        if(transform.position.x > 0)
        {
            playerScore.text = (int.Parse(playerScore.text) + 1).ToString();
            CheckWinCondition();
        }
        else if(transform.position.x < 0)
        {
            AIScore.text = (int.Parse(AIScore.text) + 1).ToString();
            CheckWinCondition();
        }
    }

    private void CheckWinCondition()
    {
        int p1Score = int.Parse(playerScore.text);
        int p2Score = int.Parse(AIScore.text);

        if (p1Score >= pointCap || p2Score >= pointCap)
        {
            if (gameMode != 1) 
            {
                if (Mathf.Abs(p1Score - p2Score) >= 2) EndGame(p1Score, p2Score);
                else ResetBall();
            }
            else 
            {
                EndGame(p1Score, p2Score);
            }
        }
        else
        {
            ResetBall();
        }
    }

    private void EndGame(int p1Score, int p2Score)
    {
        gameOver = true;
        rb.linearVelocity = Vector2.zero; 
        transform.position = Vector2.zero; 

        // Turn on the Winner Screen UI
        winnerScreen.SetActive(true);

        // Decide who won based on the scores
        if (p1Score > p2Score)
        {
            winnerText.text = "Winner: Chilli Playa!";
        }
        else
        {
            if (gameMode == 1) winnerText.text = "Winner: Playa 2!";
            else winnerText.text = "Winner: AI!";
        }
    }

    // Function for your "Main Menu" button to use
    public void LoadMainMenu()
    {
        SceneManager.LoadScene("MainMenu"); 
    }
}