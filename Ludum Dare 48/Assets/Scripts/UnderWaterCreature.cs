using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnderWaterCreature : MonoBehaviour
{

    public float gravity = 10.0f;
    public float volume = 0.2f;
    public float propulsion;
    public float dragCoefficient = 0.47f;
    public float areaFront = 0.5f;
    public float areaTop = 0.7f;

    public float minSpeed = 0.01f;

    protected Vector2 propDirection;
    public float density;
    protected float mass;
    private Vector2 velocity;
    protected Vector2 hitForce;
    protected float rotation;

    private Rigidbody2D rb2d;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        rb2d = gameObject.GetComponent<Rigidbody2D>();
        mass = density * volume;
        hitForce = Vector2.zero;
        rotation = 0.0f;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        rb2d.AddForce(propulsion * propDirection);

        propDirection = Vector2.zero;

        rb2d.AddForce(hitForce);

        hitForce = Vector2.zero;

        velocity = rb2d.velocity;
        if (Mathf.Abs(velocity.x) < minSpeed)
        {
            velocity.x = 0.0f;
            //rb2d.velocity = new Vector2(0.0f, rb2d.velocity.y);
        }

        rb2d.AddForce(new Vector2(-0.5f * velocity.x * dragCoefficient * areaFront, 0.0f));

        rb2d.AddForce(new Vector2(0.0f, -mass * gravity));

        rb2d.AddForce(new Vector2(0.0f, volume * gravity));

        velocity = rb2d.velocity;
        if (Mathf.Abs(velocity.y) < minSpeed)
        {
            velocity.y = 0.0f;
            //rb2d.velocity = new Vector2(rb2d.velocity.x, 0.0f);
        }

        rb2d.AddForce(new Vector2(0.0f, -0.5f * velocity.y * dragCoefficient * areaTop));

        transform.localRotation = Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y, rotation);

        /*if (Input.GetKey(KeyCode.E))
        {
            Debug.Log("yo");
            rb2d.AddTorque(-torque);
        }

        if(Input.GetKey(KeyCode.Q))
        {
            rb2d.AddTorque(torque);
        }*/
    }
}
