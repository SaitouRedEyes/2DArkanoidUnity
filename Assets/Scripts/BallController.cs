using UnityEngine;
using System.Collections;

public class BallController : MonoBehaviour
{
    private float speedX, speedY, acceleration;
    private int speedIncrease, itemDropRate;
    private GameController gc;
    private Vector3 initialPos;
    private GameObject powerUp, powerDown;

    void Start()
    {
        initialPos = this.transform.localPosition;
        speedX = Random.Range(0, 2) == 0 ? Random.Range(-0.06f, -0.03f) : Random.Range(0.03f, 0.06f);
        speedY = Random.Range(0.05f, 0.1f);
        acceleration = 0.002f;
        speedIncrease = GameObject.FindGameObjectsWithTag("Block").Length * 10 / 100 == 0 ? 1 : GameObject.FindGameObjectsWithTag("Block").Length * 10 / 100;
        gc = GameObject.FindObjectOfType<GameController>();

        powerUp = Resources.Load("Prefabs/PowerUp") as GameObject;
        powerDown = Resources.Load("Prefabs/PowerDown") as GameObject;
        itemDropRate = 10;
    }

    void FixedUpdate()
    {
        Move();
    }
    
    /// <summary>
    /// The gateway of the objects that collide with the ball.
    /// </summary>
    /// <param name="other"> the collided object </param>
    void OnCollisionEnter2D(Collision2D other)
    {
        switch (other.transform.tag)
        {
            case "Player": CollisionWithPlayer(other); break;
            case "TopBound": speedY *= -1; break;
            case "VerticalBound": speedX *= -1; break;
            case "BottomBound": this.speedX = this.speedY = 0; gc.GameOver(); break;
            case "Block": CollisionWithBlock(other); break;
        }
    }

    /// <summary>
    /// Ball movement.
    /// </summary>
    private void Move()
    {
        transform.localPosition = new Vector3(transform.localPosition.x + speedX,
                                              transform.localPosition.y + speedY,
                                              transform.localPosition.z);
    }

    /// <summary>
    /// Ball collision with blocks.
    /// </summary>
    /// <param name="other"> blocks </param>
    private void CollisionWithBlock(Collision2D other)
    {
        int numBlocks = GameObject.FindGameObjectsWithTag("Block").Length - 1;

        Collision(other);        
        Destroy(other.gameObject);
        gc.ScoreValue = Random.Range(1, 4) * 10;

        //Drop Item
        if (Random.Range(1, 11) <= itemDropRate)
        {
            if (Random.Range(1, 3) == 1) Instantiate(powerUp, other.transform.localPosition, Quaternion.identity);
            else Instantiate(powerDown, other.transform.localPosition, Quaternion.identity);
        }

        //Ball Acceleration
        if (numBlocks % speedIncrease == 0) 
        {
            speedX = speedX > 0 ? speedX + acceleration : speedX - acceleration;
            speedY = speedY > 0 ? speedY + acceleration : speedY - acceleration;
        }

        //Win - Reset blocks
        if (numBlocks == 0)
        {
            gc.SetupBlocks();
            this.transform.position = initialPos;

            if (speedY < 0) speedY *= -1;
        }
    }

    /// <summary>
    /// Ball collision with player
    /// </summary>
    /// <param name="other"> Player </param>
    private void CollisionWithPlayer(Collision2D other)
    {
        Collision(other);
    }

    /// <summary>
    /// Method that detect collision with any object
    /// </summary>
    /// <param name="other"> collided object </param>
    private void Collision(Collision2D other)
    {
        Vector3 contactPoint = other.contacts[0].normal;

        if (contactPoint.x == -1 && contactPoint.y == 0 || contactPoint.x == 1 && contactPoint.y == 0) speedX *= -1;
        else if (contactPoint.x == 0 && contactPoint.y == -1 || contactPoint.x == 0 && contactPoint.y == 1) speedY *= -1;
        else if ((contactPoint.x < 0 && contactPoint.y < 0) || (contactPoint.x > 0 && contactPoint.y < 0)) 
        {
            //Debug.Log("Quina Inf Esq || Quina Inf Dir" + contactPoint);
            if (speedY < 0) speedX *= -1; 
            else InvertDirections(other);
        }
        else if ((contactPoint.x < 0 && contactPoint.y > 0) || (contactPoint.x > 0 && contactPoint.y > 0)) 
        { 
            //Debug.Log("Quina Sup Esq || Quina Sup Dir" + contactPoint);
            if (speedY > 0) speedX *= -1; 
            else InvertDirections(other);
        }
    }

    /// <summary>
    /// Invert the ball direction
    /// </summary>
    /// <param name="other"> collided object </param>
    private void InvertDirections(Collision2D other)
    {
        if (speedX > 0)
        {
            if (this.transform.localPosition.x < other.collider.bounds.center.x) { speedX *= -1; speedY *= -1; }
            else { speedY *= -1; }
        }
        else
        {
            if (this.transform.localPosition.x > other.collider.bounds.center.x) { speedX *= -1; speedY *= -1; }
            else { speedY *= -1; }
        }
    }
}