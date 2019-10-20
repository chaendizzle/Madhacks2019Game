using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterSprite : MonoBehaviour
{
    public float targetHeight = 0f;
    public float height = 0f;
    public float speed = 0.25f;
    public float transitionRate = 0.5f;
    float darkness;
    SpriteRenderer sr;

    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector3(0, 0, transform.position.z);
        transform.localScale = CameraMovement.CameraRect().size * 5f;
        SetHeight(-3.9f);
        targetHeight = -3.9f;
        sr = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (height != targetHeight)
        {
            height = Mathf.MoveTowards(height, targetHeight, speed * Time.deltaTime);
            SetHeight(height);
        }
        if (ClimateEvents.GetInstance().waterLevel)
        {
            targetHeight = 0;
        }
        else
        {
            targetHeight = -3.9f;
        }

        float targetDarkness;
        if (ClimateEvents.GetInstance().darkenScreen)
        {
            targetDarkness = 0.8f;
        }
        else
        {
            targetDarkness = 0f;
        }

        darkness = Mathf.Lerp(darkness, targetDarkness, transitionRate * Time.deltaTime);
        if (Mathf.Abs(darkness - targetDarkness) < 0.01f)
        {
            darkness = targetDarkness;
        }
        sr.color = new Color(1f - darkness, 1f - darkness, 1f - darkness);
    }

    public void SetHeight(float y)
    {
        height = y;
        Rect camera = CameraMovement.CameraRect();
        float yPos = (y + camera.yMin) * 0.5f;
        transform.position = new Vector3(transform.position.x, yPos, transform.position.z);
        transform.localScale = new Vector3(transform.localScale.x, Mathf.Abs(camera.yMin - y - 1f), transform.localScale.z);
        Director.GetInstance().waterHeight = y + 0.25f;
    }
}
