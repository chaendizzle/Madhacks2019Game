using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AntiLighting : MonoBehaviour
{
    SpriteRenderer sr;

    public float targetAlpha = 0f;
    public float alpha = 0f;

    private float transitionRate = 0.5f;

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
        if (ClimateEvents.GetInstance().darkenScreen)
        {
            targetAlpha = 0.75f;
        }
        else
        {
            targetAlpha = 0;
        }

        alpha = Mathf.Lerp(alpha, targetAlpha, transitionRate * Time.deltaTime);
        if (Mathf.Abs(alpha - targetAlpha) < 0.01f)
        {
            alpha = targetAlpha;
        }
        sr.color = new Color(0f, 0f, 0f, alpha);
    }
}
