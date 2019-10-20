using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreBar : MonoBehaviour
{
    public float eventPeriod;

    private SimpleHealthBar healthBar;
    private GameObject backgroundImage;
    private GameObject warningBar;
    private GameObject eventImage;
    private GameObject livesText;

    private float ppm = 0;

    [HideInInspector]
    public float score = 0;

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

    public void SetLives(int lives)
    {
        livesText.GetComponent<Text>().text = "Lives: " + lives;
    }

    void Start()
    {
        Transform co2Bar = transform.Find("CO2Bar");
        healthBar = co2Bar.Find("Simple Bar").Find("Status Fill 01")
            .gameObject.GetComponent<SimpleHealthBar>();
        backgroundImage = co2Bar.Find("Background").gameObject;

        Transform warningBarT = transform.Find("WarningBar");
        warningBar = warningBarT.gameObject;
        eventImage = warningBarT.Find("EventName").gameObject;

        Transform livesT = transform.Find("Lives");
        livesText = livesT.Find("LivesText").gameObject;

        instance = this;
        healthBar.UpdateColor(Color.red);
        healthBar.UpdateBar(ppm, eventPeriod);
        warningBar.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        //TODO: only update if player is alive
        ppm += Time.deltaTime;
        score += Time.deltaTime * 6;

        if (ppm > eventPeriod && currentState == State.FILLING)
        {
            currentState = State.SICKO_MODE;
            eventImage.GetComponent<Image>().sprite = ClimateEvents.GetInstance().StartClimateEvent();
            warningBar.SetActive(true);
        }
        
        if (currentState == State.SICKO_MODE)
        {
            ppm = eventPeriod;
            SickoMode();
            warningBar.SetActive(true);
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
        warningBar.SetActive(false);
        ppm = 0; 
    }
}
