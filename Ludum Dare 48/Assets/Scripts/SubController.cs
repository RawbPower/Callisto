using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SubController : UnderWaterCreature
{
    public float waterRatio;
    public float waterFillRate = 0.2f;
    public float normalPropulsion;
    public float dashPropulsion;
    public float rotationRate;

    public float sonarRadius = 49.0f;
    public float sonarMaxDist = 100.0f;

    public float minDensity = 0.8f;
    public float maxDensity = 1.2f;
    public float sinkDensity;

    public Trackable[] targets;
    public ParticleSystem bubblesLeft;
    public ParticleSystem bubblesRight;
    public ParticleSystem bubblesTop;
    public WaterBar waterbar;
    public GameObject sharkEntry;
    public GameObject crashDepth;
    public Text depthText;

    private float depth;
    private bool sinkSound;
    private float propAngle;
    private float propAngleRounded;
    private float desiredPropAngle;

    private float vertical;
    private float horizontal;

    private SpriteRenderer sr;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        sr = gameObject.GetComponent<SpriteRenderer>();
        waterRatio = 0.5f;
        waterbar.SetWaterRatio(waterRatio);
        depth = 0;
        sinkSound = false;
        propAngle = 0.0f;
        desiredPropAngle = propAngle;
    }

    void Update()
    {
        vertical = Input.GetAxisRaw("Vertical");
        horizontal = Input.GetAxisRaw("Horizontal");

	    desiredPropAngle = (Mathf.Abs(horizontal) > 0.0f) ? Ramp(vertical, -0.4f, 0.4f, -45.0f, 45.0f) : 0.0f;

        if (desiredPropAngle > propAngle)
        {
            propAngle += rotationRate * Time.deltaTime;
        }
        else
        {
            propAngle -= rotationRate * Time.deltaTime;
        }

        if (desiredPropAngle < 0.0f)
        {
            if (propAngle < desiredPropAngle)
            {
                propAngle = desiredPropAngle;
            }
        }
        else if (desiredPropAngle > 0.0f)
        {
            if (propAngle > desiredPropAngle)
            {
                propAngle = desiredPropAngle;
            }
        }

        propAngleRounded = 5.0f * Mathf.RoundToInt(propAngle / 5.0f);

        base.rotation = propAngleRounded;

        //transform.localRotation = Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y, propAngle);


        /*if (Input.GetKey("e"))
        {
            transform.localRotation = Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z + rotationRate*Time.deltaTime);
        }

        if (Input.GetKey("q"))
        {
            transform.localRotation = Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z - rotationRate * Time.deltaTime);
        }*/

        propDirection = new Vector2(horizontal * Mathf.Cos(propAngle*Mathf.Deg2Rad), Mathf.Abs(horizontal) * Mathf.Sin(propAngle * Mathf.Deg2Rad));

        //propDirection = new Vector2(horizontal, vertical);

        if (Input.mouseScrollDelta.y > 0.0f)
        {
            waterRatio -= Input.mouseScrollDelta.y * waterFillRate * Time.deltaTime;
            if (!bubblesTop.isEmitting)
            {
                bubblesTop.Play();
            }
        }
        else
        {
            if (bubblesTop.isPlaying)
            {
                bubblesTop.Stop(true, ParticleSystemStopBehavior.StopEmitting);
            }

            if (Input.mouseScrollDelta.y < 0.0f)
            {
                waterRatio -= Input.mouseScrollDelta.y * waterFillRate * Time.deltaTime;
            }
        }

        waterRatio = Mathf.Clamp(waterRatio, 0.0f, 1.0f);

        if (horizontal > 0.0f)
        {
            transform.localRotation = Quaternion.Euler(transform.eulerAngles.x, 0.0f, transform.eulerAngles.z);
            if (!bubblesLeft.isEmitting)
            {

                bubblesLeft.Play();
            }
        }
        else if (horizontal < 0.0f)
        {
            transform.localRotation = Quaternion.Euler(transform.eulerAngles.x, 180.0f, transform.eulerAngles.z);
            if (!bubblesRight.isEmitting)
            {
                bubblesRight.Play();
            }
        }
        else
        {
            if(bubblesLeft.isPlaying)
            {
                bubblesLeft.Stop(true, ParticleSystemStopBehavior.StopEmitting);
            }

            if (bubblesRight.isPlaying)
            {
                bubblesRight.Stop(true, ParticleSystemStopBehavior.StopEmitting);
            }
        }

        if (Input.GetKey("space"))
        {
            base.propulsion = dashPropulsion;
        }
        else
        {
            base.propulsion = normalPropulsion;
        }

        waterbar.SetWaterRatio(waterRatio);

        depth = (406.0f - transform.position.y) / 1.5f;

        depthText.text = "Depth: " + Mathf.Round(depth);

        if (transform.position.y < sharkEntry.transform.position.y)
        {
            DialogueTrigger dt = sharkEntry.GetComponent<DialogueTrigger>();
            if (dt != null)
            {
                dt.TriggerDialogue();
            }
        }

        if (transform.position.y < crashDepth.transform.position.y)
        {
            if (!sinkSound)
            {
                FindObjectOfType<AudioManager>().Play("Sinking");
                sinkSound = true;
            }
            waterRatio = 1.0f;
            base.density = sinkDensity;
            waterbar.BreakWaterBar();

            DialogueTrigger dt = crashDepth.GetComponent<DialogueTrigger>();
            if (dt != null)
            {
                dt.TriggerDialogue();
            }
        }
        else
        {
            base.density = Ramp(waterRatio, 0.0f, 1.0f, minDensity, maxDensity);
        }

        base.mass = base.density * volume;

        SonarTrack();
    }

    private void SonarTrack()
    {
        foreach (Trackable target in targets)
        {
            Vector3 dir = target.transform.position - transform.position;

            RectTransform marker = target.marker;
            float radius = Ramp(dir.magnitude, 0.0f, sonarMaxDist, 0.0f, sonarRadius);
            marker.localPosition = radius * dir.normalized;
        }
    }

    private float Ramp(float t, float aIn, float bIn, float aOut, float bOut)
    {
        float value = (t - aIn) / (bIn - aIn);
        value =  aOut + value*(bOut - aOut);
        return Mathf.Clamp(value, aOut, bOut);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "StartDialogue")
        {
            DialogueTrigger dt = GetComponent<DialogueTrigger>();
            if (dt != null)
            {
                dt.TriggerDialogue();
            }
        }
        else if (other.tag == "Shark")
        {
            Vector2 dir = transform.position - other.transform.position;
            if (dir.magnitude < 5.0f)
            {
                dir = dir.normalized;
                float angle = Random.Range(-45.0f, 45.0f);
                dir = Quaternion.Euler(0.0f, 0.0f, angle) * dir;
                base.hitForce = 1000 * dir;
            }
        }
        else if (other.tag == "Serpent")
        {
            other.GetComponentInParent<SerpentAI>().activate = true;
        }
    }
}
