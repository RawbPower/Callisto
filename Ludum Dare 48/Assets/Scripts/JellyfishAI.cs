using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JellyfishAI : UnderWaterCreature
{

    public float rotationStepRange = 30.0f;
    public float propCooldown = 5.0f;
    public ParticleSystem bubbles;

    private float rotation;
    private float propTimer;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        rotation = 0.0f;
        propTimer = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (propTimer <= 0.0f)
        {
            rotation += Random.Range(-rotationStepRange, rotationStepRange);
            transform.eulerAngles = new Vector3(0.0f, 0.0f, rotation);
            base.propDirection = new Vector2(-Mathf.Sin(rotation * Mathf.Deg2Rad), Mathf.Cos(rotation * Mathf.Deg2Rad));
            propTimer = propCooldown;

            bubbles.Play();
        }
        else
        {
            propTimer -= Time.deltaTime;
            if (propTimer < 0.0f)
            {
                propTimer = 0.0f;
            }
        }
    }
}
