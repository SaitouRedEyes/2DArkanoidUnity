using UnityEngine;
using System.Collections;

public class Item : MonoBehaviour
{
    private float speed;

    // Use this for initialization
    void Start()
    {
        speed = -0.03f;
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.position = new Vector3(this.transform.position.x,
                                                this.transform.position.y + speed,
                                                this.transform.position.z);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        switch (other.transform.tag)
        {
            case "Player": other.GetComponent<PlayerController>().ItemEffect(this.gameObject.tag); 
                           Destroy(this.gameObject); break;
            case "BottomBound": Destroy(this.gameObject); break;
        }
    }
}