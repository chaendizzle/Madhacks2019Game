using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShrinkPlatform : MonoBehaviour
{
    bool shrinking = false;
    // when player comes within distance, this platform shatters
    public float distance = 5f;
    public float shrinkTime = 1.8f;

    public float shrinkMin = 0.5f;

    float initialScale = 1f;
    // Start is called before the first frame update
    void Start()
    {
        initialScale = transform.localScale.x;
    }

    // Update is called once per frame
    void Update()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null && !shrinking &&
            Mathf.Abs(transform.position.x - player.transform.position.x) < distance)
        {
            shrinking = true;
            StartCoroutine(Shrink());
        }
    }

    float time = 0f;
    IEnumerator Shrink()
    {
        Platform p = GetComponent<Platform>();
        Instantiate(p.warningPrefab, transform.position + Vector3.up * 1.2f, transform.rotation);
        while (time < shrinkTime)
        {
            time += Time.deltaTime;
            float xscale = Mathf.Lerp(initialScale, shrinkMin, time / shrinkTime);
            transform.localScale = new Vector3(xscale, transform.localScale.y, transform.localScale.z);
            yield return null;
        }
        Destroy(this);
    }
}
