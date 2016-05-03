using UnityEngine;
using System.Collections;

public class BGMenuScene : StaticScreen 
{
    public GameObject pressStart;

    void Start()
    {
        ResizeObject(this.gameObject);
        ResizeObject(pressStart);
    }

	new void Update ()
    {
        base.Update();
	}
}