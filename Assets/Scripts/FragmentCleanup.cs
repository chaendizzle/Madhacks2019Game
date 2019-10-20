using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FragmentCleanup : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(CleanupFragments());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator CleanupFragments()
    {
        while (true)
        {
            yield return new WaitForSeconds(10f);
            Rect rect = CameraMovement.CameraRect();
            foreach (GameObject go in GameObject.FindGameObjectsWithTag("Fragment"))
            {
                // check for cleanup: if the fragment is 10 units away
                if (Mathf.Abs(go.transform.position.x - rect.xMax) > 10f
                    && Mathf.Abs(go.transform.position.x - rect.xMin) > 10f
                    || Mathf.Abs(go.transform.position.y - rect.yMax) > 10f
                    && Mathf.Abs(go.transform.position.y - rect.yMin) > 10f)
                {
                    Destroy(go);
                }
            }
        }
    }
}
