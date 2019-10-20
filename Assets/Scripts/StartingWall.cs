using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartingWall : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        float xPos = CameraMovement.CameraRect().xMin + GetComponent<SpriteRenderer>().bounds.extents.y;
        transform.position = new Vector3(xPos, transform.position.y, transform.position.z);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
