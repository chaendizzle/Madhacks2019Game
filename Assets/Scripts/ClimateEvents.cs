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

    // Update is called once per frame
    void Update()
    {
        
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
        yield return new WaitForSeconds(time);
        SetClimateEvent("None");
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
        switch (name)
        {
            case "RisingSeaLevel":
                waterLevel = true;
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
                break;
            default:
                break;
        }
    }
}
