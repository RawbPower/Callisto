using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SharkAI : UnderWaterCreature
{

    public Transform target;
    private bool foundTarget;

    public Transform sharkZoneTop;
    public Transform sharkZoneBottom;
    public ParticleSystem bubblesLeft;
    public ParticleSystem bubblesRight;

    public float propCooldown = 5.0f;
    public float propTime = 2.0f;
    //public ParticleSystem bubbles;

    private float propTimer;
    private bool sharkSound;

    protected override void Start()
    {
        base.Start();
        base.propDirection = Vector2.zero;
        propTimer = 0.0f;
        sharkSound = false;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 dir = target.transform.position - transform.position;
        bool InSharkZone = (target.transform.position.y < sharkZoneTop.position.y) && (target.transform.position.y > sharkZoneBottom.position.y);

        if (InSharkZone)
        {
            if (!sharkSound)
            {
                FindObjectOfType<AudioManager>().Stop("Music");
                FindObjectOfType<AudioManager>().Play("Shark");
                sharkSound = true;
            }

            if (propTimer <= 0.0f)
            {
                base.propDirection = dir / dir.magnitude;
                propTimer = propCooldown;

                if (Vector2.Dot(base.propDirection, new Vector2(1.0f, 0.0f)) > 0.0f)
                {
                    if (!bubblesLeft.isEmitting)
                    {

                        bubblesLeft.Play();
                    }
                }
                else
                {
                    if (!bubblesRight.isEmitting)
                    {

                        bubblesRight.Play();
                    }
                }
            }
            else if (propTimer > (propCooldown - propTime))
            {
                propTimer -= Time.deltaTime;
                base.propDirection = dir / dir.magnitude;
            }
            else
            {
                propTimer -= Time.deltaTime;
                if (propTimer < 0.0f)
                {
                    propTimer = 0.0f;
                }

                if (bubblesLeft.isPlaying)
                {
                    bubblesLeft.Stop(true, ParticleSystemStopBehavior.StopEmitting);
                }

                if (bubblesRight.isPlaying)
                {
                    bubblesRight.Stop(true, ParticleSystemStopBehavior.StopEmitting);
                }
            }
        }
        else
        {
            if (sharkSound)
            {
                FindObjectOfType<AudioManager>().Stop("Shark");
                FindObjectOfType<AudioManager>().Play("Music");
                sharkSound = false;
            }
        }

        if (Vector2.Dot(dir, new Vector2(1.0f, 0.0f)) > 0.0f)
        {
            transform.localRotation = Quaternion.Euler(transform.eulerAngles.x, 0.0f, transform.eulerAngles.z);
        }
        else
        {
            transform.localRotation = Quaternion.Euler(transform.eulerAngles.x, 180.0f, transform.eulerAngles.z);
        }
    }
}
