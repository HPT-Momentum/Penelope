using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Waypoint : MonoBehaviour
{
    public Color color;
    public RawImage image;

    public Vector2 position
    {
        get { return new Vector2(transform.position.x, transform.position.z); }
    }
}
