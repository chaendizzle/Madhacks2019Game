using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterSprite : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector3(0, 0, transform.position.z);
        transform.localScale = CameraMovement.CameraRect().size * 1f;
        SetHeight(-4.5f);
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void SetHeight(float y)
    {
        Rect camera = CameraMovement.CameraRect();
        float yPos = (y + camera.yMin) * 0.5f;
        transform.position = new Vector3(transform.position.x, yPos, transform.position.z);
        transform.localScale = new Vector3(transform.localScale.x, camera.yMin - y, transform.localScale.z);
    }
}
