using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Flashable : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Camera")
        {
            DialogueTrigger dt = GetComponent<DialogueTrigger>();
            if (dt != null)
            {
                dt.TriggerDialogue();
            }

            Trackable tracking = GetComponent<Trackable>();
            if (tracking)
            {
                Image marker = tracking.marker.GetComponent<Image>();
                if (marker)
                {
                    marker.enabled = false;
                }
            }
        }
    }
}
