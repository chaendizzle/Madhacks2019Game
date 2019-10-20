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
        transform.eulerAngles = new Vector3(0f, 0f, 0f);
    }

    // Update is called once per frame
    void Update()
    {
        Rect rect = CameraMovement.CameraRect();
        float yPos = transform.position.y;
        yPos = Mathf.Clamp(yPos, rect.yMin + 1.2f, rect.yMax - 1.2f);
        transform.position = new Vector3(transform.position.x, yPos, transform.position.z);
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
