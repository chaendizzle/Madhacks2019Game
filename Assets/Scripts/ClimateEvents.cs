using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClimateEvents : MonoBehaviour
{
    public bool shatterPlatforms = false;
    public bool darkenScreen = false;
    public bool wind = false;
    public bool clouds = false;
    public bool waterLevel = false;
    public bool shrinkingPlatforms = false;
    public bool shatterSky = false;
    public List<GameObject> shatters;

    [Serializable]
    public struct ClimateEvent
    {
        public string name;
        public Sprite sprite;
    }
    public List<ClimateEvent> events;
    IEnumerator current = null;

    public static ClimateEvents GetInstance()
    {
        return GameObject.FindGameObjectWithTag("ClimateEventsHandler").GetComponent<ClimateEvents>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    IEnumerator shatterSkyCoroutine;
    // Update is called once per frame
    void Update()
    {
        // if shattering the sky, periodically spawn shattering icebergs in the sky
        if (shatterSky && shatterSkyCoroutine == null)
        {
            shatterSkyCoroutine = ShatterSky();
            StartCoroutine(shatterSkyCoroutine);
        }
        if (!shatterSky && shatterSkyCoroutine != null)
        {
            StopCoroutine(shatterSkyCoroutine);
            shatterSkyCoroutine = null;
        }
    }

    IEnumerator ShatterSky()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        while (true)
        {
            Rect rect = CameraMovement.CameraRect();
            float xPos = UnityEngine.Random.Range((rect.xMin + rect.xMax) * 0.5f, rect.xMax);
            GameObject go = Instantiate(shatters[UnityEngine.Random.Range(0, shatters.Count)],
                new Vector3(xPos, rect.yMax + 7f, transform.position.z), transform.rotation);
            go.transform.eulerAngles = new Vector3(0f, 0f, 90f);
            Vector3 pos = go.transform.position;
            go.AddComponent<ShatterPlatform>();
            yield return new WaitForSeconds(UnityEngine.Random.Range(1.4f, 2.2f));
        }
    }

    public Sprite StartClimateEvent()
    {
        if (current != null)
        {
            StopCoroutine(current);
        }
        int index = UnityEngine.Random.Range(0, events.Count);
        current = RunClimateEvent(events[index].name, 30f);
        StartCoroutine(current);
        return events[index].sprite;
    }

    IEnumerator RunClimateEvent(string name, float time)
    {
        SetClimateEvent(name);
        foreach (GameObject go in Director.GetInstance().GetPlatforms())
        {
            if (go != null)
            {
                go.GetComponent<Platform>().UpdateClimateEvents();
            }
        }
        yield return new WaitForSeconds(time);
        SetClimateEvent("None");
        foreach (GameObject go in Director.GetInstance().GetPlatforms())
        {
            if (go != null)
            {
                go.GetComponent<Platform>().UpdateClimateEvents();
            }
        }
        ScoreBar.GetInstance().Reset();
        current = null;
    }

    private void SetClimateEvent(string name)
    {
        shatterPlatforms = false;
        darkenScreen = false;
        wind = false;
        clouds = false;
        waterLevel = false;
        shrinkingPlatforms = false;
        shatterSky = false;
        switch (name)
        {
            case "RisingSeaLevel":
                waterLevel = true;
                shatterPlatforms = true;
                break;
            case "ArcticErosion":
                shatterPlatforms = true;
                break;
            case "InclementWeather":
                clouds = true;
                wind = true;
                darkenScreen = true;
                break;
            case "WarmWaterFlow":
                shrinkingPlatforms = true;
                break;
            case "ArcticStorm":
                clouds = true;
                wind = true;
                darkenScreen = true;
                shatterPlatforms = true;
                shatterSky = true;
                break;
            default:
                break;
        }
    }
}
