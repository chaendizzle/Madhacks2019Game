using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreBar : MonoBehaviour
{
    // Start is called before the first frame update
    public SimpleHealthBar healthBar;
    public GameObject warningImages;
    public GameObject image; 
    public float eventPeriod;

    private float ppm = 0;

    private static ScoreBar instance;
    public static ScoreBar GetInstance()
    {
        return instance;
    }

    void Start()
    {
        instance = this;
        healthBar.UpdateColor(Color.red);
        healthBar.UpdateBar(ppm, eventPeriod);
        warningImages.active = false;
    }

    // Update is called once per frame
    void Update()
    {
        //TODO: only update if player is alive
        ppm += Time.deltaTime;
        
        if (ppm > eventPeriod)
        {
            SickoMode();
            //go sicko mode
            //TODO: trigger event
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
        warningImages.active = true;
        if (Time.time - lastUpdateTime < blinkPeriod)
            return;
        lastUpdateTime = Time.time;
        switch (sickoState) 
        {
            case SickoState.BLACK:
                image.GetComponent<Image>().color = Color.yellow;
                sickoState = SickoState.YELLOW;
                break;
            case SickoState.YELLOW:
                image.GetComponent<Image>().color = Color.black;
                sickoState = SickoState.BLACK;
                break;
        }
    }

    //reset the bar after event
    public void Reset()
    {
        image.GetComponent<Image>().color = Color.black;
        sickoState = SickoState.BLACK;
        warningImages.active = false;
        ppm = 0; 
    }
}
