using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndScreen : MonoBehaviour
{

    public GameObject Replay;

    private void Start()
    {
        Replay.SetActive(false);
    }

    public void EndGame()
    {
        Replay.SetActive(true);
    }
}
