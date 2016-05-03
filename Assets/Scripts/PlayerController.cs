using UnityEngine;
using System.Collections;
using System;

public class PlayerController : MonoBehaviour
{
    private float speed;
    private int currScale;
    private const int MIN_SCALE = 1, MAX_SCALE = 3;
    private Rigidbody2D myRigidbody;

    void Start()
    {
        speed = 8;
        currScale = 2;
        myRigidbody = this.GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        UpdatePlayer();        
    }

    /// <summary>
    /// Update Player.
    /// </summary>
    private void UpdatePlayer()
    {
        Move(Input.GetAxis("Horizontal"));
    }

    /// <summary>
    /// Player movement.
    /// </summary>
    /// <param name="axis"> axis value (-1 to 1) </param>
    private void Move(float axis)
    {
        myRigidbody.velocity = new Vector2(axis * speed, myRigidbody.velocity.y);
    }

    /// <summary>
    /// What happens to player when he get an item.
    /// </summary>
    /// <param name="tag"> item type </param>
    public void ItemEffect(string tag)
    {
        switch (tag)
        {
            case "PowerUp": 
                if(currScale < MAX_SCALE) 
                { 
                    currScale++; 
                    Resize(0.5f); 
                } break;
            case "PowerDown": 
                if(currScale > MIN_SCALE) 
                { 
                    currScale--; 
                    Resize(-0.5f); 
                } break;
        }
    }

    /// <summary>
    /// Player resize
    /// </summary>
    /// <param name="scaleFactor"> scale factor </param>
    private void Resize(float scaleFactor)
    {
        this.transform.localScale = new Vector3(transform.localScale.x + scaleFactor,
                                                transform.localScale.y,
                                                transform.localScale.z);
    }
}