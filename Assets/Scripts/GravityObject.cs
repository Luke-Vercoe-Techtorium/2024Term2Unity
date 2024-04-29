using System.Collections.Generic;
using UnityEngine;

public class GravityObject : MonoBehaviour
{
    public readonly static float GravityStrength = 1.0f;

    public static List<GravityObject> Objects = new();
    [HideInInspector]
    public Rigidbody2D rb = null;
    [HideInInspector]
    public GravityObject StrongestObject = null;

    public void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        Objects.Add(this);
    }

    public void OnDestroy()
    {
        Objects.Remove(this);
        foreach (var obj in Objects)
            if (obj.StrongestObject == this)
                obj.StrongestObject = null;
    }

    public void FixedUpdate()
    {
        var strongestGravity = -Mathf.Infinity;
        if (StrongestObject != null)
        {
            Vector2 toStrongest = StrongestObject.transform.position - transform.position;
            strongestGravity = GravityStrength * StrongestObject.rb.mass * rb.mass / toStrongest.sqrMagnitude;
        }

        foreach (var obj in Objects)
        {
            if (obj != this)
            {
                Vector2 toObj = obj.transform.position - transform.position;
                var gravity = GravityStrength * obj.rb.mass * rb.mass / toObj.sqrMagnitude;

                if (gravity > strongestGravity)
                    StrongestObject = obj;

                rb.AddForce(gravity * toObj.normalized);
            }
        }
    }

    public void OnDrawGizmos()
    {
        Color oldColor = Gizmos.color;
        if (rb != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawRay(transform.position, rb.velocity);
        }
        Gizmos.color = Color.blue;
        Gizmos.DrawRay(transform.position, 0.5f * transform.localScale.y * transform.up);
        Gizmos.color = oldColor;
    }
}
