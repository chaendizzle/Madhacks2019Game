using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShatterPlatform : MonoBehaviour
{
    public bool shattering = false;
    // when player comes within distance, this platform shatters
    public float distance = 3f;
    public float shatterTime = 1.5f;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Rect rect = CameraMovement.CameraRect();
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null && !shattering && PlayerGG.GetInstance().state == PlayerGG.State.ALIVE &&
            (Mathf.Abs(transform.position.x - player.transform.position.x) < distance || transform.position.y > rect.yMax))
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
        Vector3 pos = transform.position;
        p.Explode();
        if (ClimateEvents.GetInstance().shatterSky)
        {
            foreach (GameObject fragment in GameObject.FindGameObjectsWithTag("Fragment"))
            {
                if (Vector2.Distance(fragment.transform.position, pos) < 2f)
                {
                    float sign = Mathf.Sign(fragment.transform.position.x - pos.x);
                    fragment.GetComponent<Rigidbody2D>().velocity += sign * Vector2.right * UnityEngine.Random.Range(0.25f, 3f);
                }
            }
        }
    }
}
