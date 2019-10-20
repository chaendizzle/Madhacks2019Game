using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public float xmovement;
    public bool up;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    // Update is called once per frame
    public void UpdateInput()
    {
        // keyboard input
        xmovement = 0;
        if (Input.GetKey("left") && !Input.GetKey("right"))
        {
            xmovement = -1;
        }
        if (!Input.GetKey("left") && Input.GetKey("right"))
        {
            xmovement = 1;
        }
        up = false;
        if (Input.GetKeyDown("up"))
        {
            up = true;
        }
        // touch input for jump
        foreach (Touch t in Input.touches)
        {
            if (t.phase == TouchPhase.Began)
            {
                up = true;
            }
        }
        if (Mathf.Abs(Input.acceleration.x) > 0.05f)
        {
            xmovement = Mathf.Clamp(Input.acceleration.x * 4f, -1f, 1f);
        }
    }
}
