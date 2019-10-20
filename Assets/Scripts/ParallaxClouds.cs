using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxClouds : MonoBehaviour
{
    public Vector2 speed = new Vector2(-23.5f, 0.3f);
    private readonly float SPEEDFACTOR = 0.05f;

    // Start is called before the first frame update
    void Start()
    {
        transform.position += (Vector3) CameraMovement.CameraRect().size;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
