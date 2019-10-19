using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxBackground : MonoBehaviour
{
    public float speed = 3f;
    Renderer r;

    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector3(0, 0, transform.position.z);
        transform.localScale = CameraMovement.CameraRect().size * 1f;
        r = GetComponent<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 offset = (Time.time * speed) * Vector2.right;
        r.material.mainTextureOffset = offset;
    }
}
