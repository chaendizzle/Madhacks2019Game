using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        // scale PlatformBottom downwards
        Rect rect = CameraMovement.CameraRect();
        Transform bot = transform.Find("PlatformBottom");
        if (bot == null)
        {
            return;
        }
        GameObject bottom = bot.gameObject;
        bottom.transform.position = new Vector3(bottom.transform.position.x,
            (rect.yMin + transform.position.y) * 0.5f,
            bottom.transform.position.z);
        SpriteRenderer sr = bottom.GetComponent<SpriteRenderer>();
        float spriteScale = transform.localScale.y * sr.sprite.rect.height / sr.sprite.pixelsPerUnit;
        bottom.transform.localScale = new Vector3(bottom.transform.localScale.x,
            Mathf.Abs(rect.yMin - transform.position.y) / spriteScale,
            bottom.transform.localScale.z);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    
}
