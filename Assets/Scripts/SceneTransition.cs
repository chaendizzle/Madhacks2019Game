using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour
{
    public void TransitionToGame()
    {
        SceneManager.LoadScene("SampleScene");
    }

    public void TransitionToMenu()
    {
        SceneManager.LoadScene("Title");
    }
}
