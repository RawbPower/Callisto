using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SerpentAI : UnderWaterCreature
{

    public bool activate;
    public GameObject serpentEnd;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        base.propDirection = Vector2.zero;
        activate = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (activate)
        {
            base.propDirection = new Vector2(-1.0f, 0.0f);

            if (transform.position.x < serpentEnd.transform.position.x)
            {
                DialogueTrigger dt = serpentEnd.GetComponent<DialogueTrigger>();
                if (dt != null)
                {
                    dt.TriggerDialogue();
                }
            }
        }
    }
}
