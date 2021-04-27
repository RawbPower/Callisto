using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmControl : MonoBehaviour
{

    public float minDistance;

    public Transform pivot;
    public Transform parent;
    public SpriteRenderer flash;
    public PolygonCollider2D flashTrigger;

    public float flashTime = 1.0f;
    private float flashTimer;

    private void Awake()
    {
        flash.enabled = false;
        flashTrigger.enabled = false;
    }
    // Start is called before the first frame update
    void Start()
    {
        flashTimer = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 pos = Camera.main.WorldToScreenPoint(transform.position);
        Vector3 dir = Input.mousePosition - pos;
        if (dir.sqrMagnitude > minDistance * minDistance)
        {
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

            Vector3 armDir = transform.position - pivot.position;
            transform.position = parent.position + armDir;
        }

        if (Input.GetMouseButtonDown(0))
        {
            if (flashTimer <= 0.0f)
            {
                flashTimer = flashTime;
            }
        }

        if (flashTimer > 0.0f)
        {
            flash.enabled = true;
            flashTrigger.enabled = true;
            flashTimer -= Time.deltaTime;
        }
        else
        {
            flashTimer = 0.0f;
            flash.enabled = false;
            flashTrigger.enabled = false;
        }
    }
}
