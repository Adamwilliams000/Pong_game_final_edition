using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallBounce : MonoBehaviour
{
    public GameObject hitSFX;
    public float bounceForce = 200f;

    public BallMovement ballMovement;
    public ScoreManager scoreManager;

    private void Bounce(Collision2D collision)
    {
        Vector3 ballPosition = transform.position;
        Vector3 racketPosition = collision.transform.position;
        float racketHeight = collision.collider.bounds.size.y;

        float positionX;
        if (collision.gameObject.name == "Player1")
        {
            positionX = 1;
        }
        else
        {
            positionX = -1;
        }

        float positionY = (ballPosition.y - racketPosition.y) / racketHeight;

        ballMovement.IncreaseHitCounter();

        // Apply a random direction to the ball's movement
        float randomAngle = Random.Range(-45f, 45f);
        Vector2 randomDirection = Quaternion.Euler(0, 0, randomAngle) * new Vector2(positionX, positionY);
        ballMovement.MoveBall(randomDirection);

        // Add bounce force to the ball
        GetComponent<Rigidbody2D>().AddForce(randomDirection * bounceForce, ForceMode2D.Impulse);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name == "Player1" || collision.gameObject.name == "Player2")
        {
            Bounce(collision);
        }

        else if (collision.gameObject.name == "Right Border")
        {
            scoreManager.Player1Goal();
            ballMovement.player1Start = false;
            StartCoroutine(ballMovement.Launch());
        }

        else if (collision.gameObject.name == "Left Border")
        {
            scoreManager.Player2Goal();
            ballMovement.player1Start = true;
            StartCoroutine(ballMovement.Launch());
        }

        Instantiate(hitSFX, transform.position, transform.rotation);
    }
}
