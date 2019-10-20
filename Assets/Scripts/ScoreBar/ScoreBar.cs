using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreBar : MonoBehaviour
{
    public float eventPeriod;

    private SimpleHealthBar healthBar;
    private GameObject warningImages;
    private GameObject backgroundImage; 

    private float ppm = 0;
    private Sprite eventSprite;

    private static ScoreBar instance;
    
    [System.Serializable]
    public enum  State
    {
        FILLING,
        SICKO_MODE
    }

    public State currentState;
    public static ScoreBar GetInstance()
    {
        return instance;
    }

    void Start()
    {
        healthBar = transform.Find("Simple Bar").Find("Status Fill 01")
            .gameObject.GetComponent<SimpleHealthBar>();
        warningImages = transform.Find("Warning").gameObject;
        backgroundImage = transform.Find("Background").gameObject;

        instance = this;
        healthBar.UpdateColor(Color.red);
        healthBar.UpdateBar(ppm, eventPeriod);
        warningImages.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        //TODO: only update if player is alive
        ppm += Time.deltaTime;

        if (ppm > eventPeriod && currentState == State.FILLING)
        {
            currentState = State.SICKO_MODE;
            eventSprite = ClimateEvents.GetInstance().StartClimateEvent();
        }
        
        if (currentState == State.SICKO_MODE)
        {
            ppm = eventPeriod;
            SickoMode();
            warningImages.SetActive(true);
        }

        healthBar.UpdateBar(ppm, eventPeriod);
    }


    private enum SickoState
    {
        BLACK,
        YELLOW
    }
    private SickoState sickoState = SickoState.BLACK;
    private float lastUpdateTime;
    public float blinkPeriod;

    void SickoMode()
    {
        if (Time.time - lastUpdateTime < blinkPeriod)
            return;
        lastUpdateTime = Time.time;
        switch (sickoState) 
        {
            case SickoState.BLACK:
                backgroundImage.GetComponent<Image>().color = Color.yellow;
                sickoState = SickoState.YELLOW;
                break;
            case SickoState.YELLOW:
                backgroundImage.GetComponent<Image>().color = Color.black;
                sickoState = SickoState.BLACK;
                break;
        }
    }

    //reset the bar after event
    public void Reset()
    {
        backgroundImage.GetComponent<Image>().color = Color.black;
        sickoState = SickoState.BLACK;
        currentState = State.FILLING;
        warningImages.SetActive(false);
        ppm = 0; 
    }
}
