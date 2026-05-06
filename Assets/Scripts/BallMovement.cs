using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro; 
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
    [SerializeField] private TextMeshProUGUI winnerText; 

    private int hitCounter;
    private Rigidbody2D rb;
    private TrailRenderer trail; // NEW: To control the neon trail
    private float currentMaxSpeed; 

    private int pointCap;
    private int gameMode;
    private bool gameOver = false;
    
    private float serveDirection = -1f; 
    private float lastHitTime = 0f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        trail = GetComponent<TrailRenderer>(); // NEW: Link the trail component
        
        pointCap = PlayerPrefs.GetInt("PointCap", 21);
        gameMode = PlayerPrefs.GetInt("GameMode", 0);

        if (winnerScreen != null) winnerScreen.SetActive(false);

        // Randomize the first serve direction
        if (Random.value > 0.5f) serveDirection = 1f;
        else serveDirection = -1f;

        Invoke("StartBall", 2f);
    }

    private void FixedUpdate()
    {
        rb.linearVelocity = Vector2.ClampMagnitude(rb.linearVelocity, currentMaxSpeed);
    }

    private void StartBall()
    {
        // Turn the trail back ON when the ball starts moving
        if (trail != null) trail.emitting = true;

        currentMaxSpeed = initialSpeed + (speedIncrease * hitCounter);
        rb.linearVelocity = new Vector2(serveDirection, 0) * currentMaxSpeed;
    }

    private void ResetBall()
    {
        // NEW: Clear the trail and turn it OFF so it doesn't draw a line during teleport
        if (trail != null) 
        {
            trail.Clear(); 
            trail.emitting = false;
        }

        rb.linearVelocity = Vector2.zero;
        transform.position = Vector2.zero;
        hitCounter = 0;
        Invoke("StartBall", 2f);
    }

    private void PlayerBounce(Transform myObject)
    {
        hitCounter++;
        Vector2 ballPos = transform.position;
        Vector2 playerPos = myObject.position;

        float xDirection = (transform.position.x > 0) ? -1 : 1;
        float yDirection = (ballPos.y - playerPos.y) / myObject.GetComponent<Collider2D>().bounds.size.y;
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
            if (Time.time > lastHitTime + 0.1f)
            {
                lastHitTime = Time.time; 
                PlayerBounce(collision.transform);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (gameOver) return; 

        // If someone scores, turn off the trail immediately!
        if (trail != null) trail.emitting = false;

        if(transform.position.x > 0)
        {
            playerScore.text = (int.Parse(playerScore.text) + 1).ToString();
            serveDirection = -1f;
            CheckWinCondition();
        }
        else if(transform.position.x < 0)
        {
            AIScore.text = (int.Parse(AIScore.text) + 1).ToString();
            serveDirection = 1f; 
            CheckWinCondition();
        }
    }

    private void CheckWinCondition()
    {
        int p1Score = int.Parse(playerScore.text);
        int p2Score = int.Parse(AIScore.text);

        if (p1Score >= pointCap || p2Score >= pointCap)
        {
            if (Mathf.Abs(p1Score - p2Score) >= 2) EndGame(p1Score, p2Score);
            else ResetBall();
        }
        else ResetBall();
    }

    private void EndGame(int p1Score, int p2Score)
    {
        gameOver = true;
        if (trail != null) trail.emitting = false;
        rb.linearVelocity = Vector2.zero; 
        transform.position = Vector2.zero; 
        winnerScreen.SetActive(true);

        if (p1Score > p2Score)
        {
            winnerText.text = (gameMode == 1) ? "WINNER : PLAYER 1" : "WINNER : PLAYER"; 
        }
        else
        {
            winnerText.text = (gameMode == 1) ? "WINNER : PLAYER 2" : "WINNER : AI"; 
        }
    }
}