using UnityEngine;
using System.Collections;

public class Util
{
    private static Util instance;
    private Vector3 screenResolution;
    private float worldScreenHeight;
    private float worldScreenWidth;

    public enum Axis
    {
        Width = 0, Height = 1, RotatedWidth = 2, RotatedHeight = 3, Both = 4
    }

    public Util()
    {
        screenResolution = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 10));
        worldScreenHeight = Camera.main.orthographicSize * 2.0f;
        worldScreenWidth = (worldScreenHeight / Screen.height) * Screen.width;
    }

    public static Util GetInstance
    {
        get
        {
            if (instance == null) instance = new Util();

            return instance;
        }
    }

    public Vector2 GetScreenResolution
    {
        get { return screenResolution; }
    }

    public Vector3 GetScreenPositionPercent(float percentX, float percentY)
    {
        return new Vector3((2 * screenResolution.x * percentX) / 100.0f,
                           (2 * screenResolution.y * percentY) / 100.0f,
                            10);
    }

    public Vector3 GetAdjustedPosition(Vector3 position)
    {
        return new Vector3(position.x - (2 * screenResolution.x) / 2.0f,
                            position.y - (2 * screenResolution.y) / 2.0f,
                            position.z);
    }

    public Vector3 ResizeSpriteToScreen(float oldWidth, float oldHeight, int axis)
    {
        switch (axis)
        {
            case (int)Util.Axis.Width:         return new Vector3(worldScreenWidth / oldWidth, oldHeight, 1);
            case (int)Util.Axis.Height:        return new Vector3(oldWidth, worldScreenHeight / oldHeight, 1);
            case (int)Util.Axis.RotatedWidth:  return new Vector3(oldWidth, worldScreenWidth / oldHeight, 1);
            case (int)Util.Axis.RotatedHeight: return new Vector3(worldScreenHeight / oldWidth, oldHeight, 1);
            default:                           return new Vector3(worldScreenWidth / oldWidth, worldScreenHeight / oldHeight, 1); 
        }
    }
}