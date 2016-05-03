using UnityEngine;
using System.Collections;

public class BGGameOverScene : StaticScreen
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