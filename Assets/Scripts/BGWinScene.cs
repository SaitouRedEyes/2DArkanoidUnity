using UnityEngine;
using System.Collections;

public class BGWinScene : StaticScreen
{
    void Start()
    {
        ResizeObject(this.gameObject);
    }

    new void Update()
    {
        base.Update();
    }
}