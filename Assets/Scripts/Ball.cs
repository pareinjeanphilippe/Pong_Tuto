using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Ball : MonoBehaviour
{
    public float speed = 10f;
    private Rigidbody2D rb;
    public ScoreManager scoreManager;
    private bool Launched = false;
    private AudioSource audioSource;
    public AudioClip SfxRacket, SfxWalls, SfxLoose;
    private float curSpeed;
    public RacketComputer racketComputer;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();   
        audioSource = GetComponent<AudioSource>();  
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space) && Launched==false) 
        {
            LaunchBall();
            Launched = true;    
        }
    }

    void LaunchBall()
    {
        if (scoreManager.EndGame == true) SceneManager.LoadScene("Pong");

        curSpeed = speed;
        float x = Random.Range(0 , 2) == 0 ? -1 : 1;
        float y = Random.Range(0 , 2) ==0 ? -1 : 1; 
        
        rb.velocity = new Vector2 (x, y) * speed;
    }

    private void FixedUpdate()
    {
        rb.velocity = rb.velocity.normalized * curSpeed;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        switch (collision.collider.tag)
        {
            case "P1Loose":
                HandleScoreAndReinit(2);
                break;

            case "P2Loose":
                HandleScoreAndReinit(1);
                break;

            case "Walls":
                audioSource.PlayOneShot(SfxWalls);
                curSpeed += 0.5f;
                break;

            case "Racket":
                audioSource.PlayOneShot(SfxRacket);
                break;
        }
    }

    void HandleScoreAndReinit(int player)
    {
        Launched=false;
        rb.velocity = Vector2.zero;
        transform.position = Vector2.zero;
        scoreManager.AddScore(player);
        audioSource.PlayOneShot(SfxLoose);

        racketComputer.speed = Random.Range(10f, 15f);
        racketComputer.marginOfError = Random.Range(0.05f, 0.15f);
        racketComputer.followDistance = Random.Range(3f, 6f);
    }
}
