using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ReplayButton : MonoBehaviour
{
    public void ReplayGame()
    {
        FindObjectOfType<AudioManager>().Stop("Sinking");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }
}
