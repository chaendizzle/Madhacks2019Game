using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShatterPlatform : MonoBehaviour
{
    bool shattering = false;
    // when player comes within distance, this platform shatters
    public float distance = 3f;
    public float shatterTime = 1.8f;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null && !shattering &&
            Mathf.Abs(transform.position.x - player.transform.position.x) < distance)
        {
            shattering = true;
            StartCoroutine(Shatter());
        }
    }
    
    IEnumerator Shatter()
    {
        Platform p = GetComponent<Platform>();
        Instantiate(p.warningPrefab, transform.position + Vector3.up * 1.2f, transform.rotation);
        yield return new WaitForSeconds(shatterTime);
        p.Explode();
    }
}
