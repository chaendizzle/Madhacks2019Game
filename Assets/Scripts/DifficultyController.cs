using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DifficultyController : MonoBehaviour
{

    public float minDifficulty; //not quite, but 1 above
    public float secondsUntilMinDifficulty;

    // Update is called once per frame
    void Update()
    {
        Director.GetInstance().platformBudget = (secondsUntilMinDifficulty / Time.time) + (minDifficulty - 1);
    }
}
