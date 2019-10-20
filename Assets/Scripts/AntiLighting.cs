using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AntiLighting : MonoBehaviour
{
    SpriteRenderer sr;

    public float alpha = 0f;

    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector3(0, 0, transform.position.z);
        transform.localScale = CameraMovement.CameraRect().size * 1.05f;
        sr = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        sr.color = new Color(0f, 0f, 0f, alpha);
    }
}
