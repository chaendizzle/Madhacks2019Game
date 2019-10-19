using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public Vector2 speed = new Vector2(5f, 0f);

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = transform.position + (Vector3)speed * Time.deltaTime;
    }

    public static Rect CameraRect()
    {
        Vector2 min = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, 0));
        Vector2 max = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));
        return new Rect(min, max - min);
    }
}
