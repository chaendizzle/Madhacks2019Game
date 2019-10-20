using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Warning : MonoBehaviour
{
    public float alphaMin = 0.25f;
    public float alphaMax = 0.9f;

    public float time = 1.8f;
    public int flashes = 3;

    SpriteRenderer sr;

    // Start is called before the first frame update
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        StartCoroutine(Flash());
    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator Flash()
    {
        // flash 3 times
        for (int i = 0; i < flashes; i++)
        {
            sr.color = new Color(1f, 1f, 1f, alphaMax);
            yield return new WaitForSeconds(time / (flashes * 2f));
            sr.color = new Color(1f, 1f, 1f, alphaMin);
            yield return new WaitForSeconds(time / (flashes * 2f));
        }
        Destroy(gameObject);
    }
}
