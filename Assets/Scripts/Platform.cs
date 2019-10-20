using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    GameObject bottom = null;
    public GameObject warningPrefab;

    // Start is called before the first frame update
    void Start()
    {
        // scale PlatformBottom downwards
        Rect rect = CameraMovement.CameraRect();
        Transform bot = transform.Find("PlatformBottom");
        if (bot != null)
        {
            bottom = bot.gameObject;
            bottom.transform.position = new Vector3(bottom.transform.position.x,
                (rect.yMin + transform.position.y) * 0.5f,
                bottom.transform.position.z);
            SpriteRenderer sr = bottom.GetComponent<SpriteRenderer>();
            float spriteScale = transform.localScale.y * sr.sprite.rect.height / sr.sprite.pixelsPerUnit;
            bottom.transform.localScale = new Vector3(bottom.transform.localScale.x,
                Mathf.Abs(rect.yMin - transform.position.y) / spriteScale,
                bottom.transform.localScale.z);
        }

        UpdateClimateEvents();
    }
    
    public void UpdateClimateEvents()
    {
        // handle climate events
        if (ClimateEvents.GetInstance().shatterPlatforms)
        {
            gameObject.AddComponent<ShatterPlatform>();
        }
        else
        {
            ShatterPlatform shatform = gameObject.GetComponent<ShatterPlatform>();
            if (shatform != null && !shatform.shattering)
            {
                Destroy(shatform);
            }
        }
        if (ClimateEvents.GetInstance().shrinkingPlatforms)
        {
            gameObject.AddComponent<ShrinkPlatform>();
        }
        else
        {
            ShrinkPlatform shrinkform = gameObject.GetComponent<ShrinkPlatform>();
            if (shrinkform != null && !shrinkform.shrinking)
            {
                Destroy(shrinkform);
            }
        }
    }

    bool exploded = false;
    public void Explode()
    {
        if (!exploded)
        {
            explodeGameObject(bottom);
            explodeGameObject(gameObject);
            exploded = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time > 5)
        {
            //Explode();
        }
    }

    private void explodeGameObject(GameObject g)
    {
        if (g == null)
        {
            return;
        }
        Explodable ex = g.GetComponent<Explodable>();
        if (ex == null)
        {
            return;
        }
        ex.allowRuntimeFragmentation = true;
        ex.extraPoints = 5;
        ex.shatterType = Explodable.ShatterType.Voronoi;
        ex.explode();
    }
}
