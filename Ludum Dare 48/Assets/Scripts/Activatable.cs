using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Activatable : MonoBehaviour
{

    public UnderWaterCreature controller;

    private void Start()
    {
        controller.enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Activate")
        {
            controller.enabled = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Activate")
        {
            controller.enabled = false;
        }
    }
}
