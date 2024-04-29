using System.Collections.Generic;
using UnityEngine;

public class GravityObject : MonoBehaviour
{
    public readonly static float GravityStrength = 1.0f;

    public static List<GravityObject> Objects = new();
    [HideInInspector]
    public Rigidbody2D rb = null;

    public void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        Objects.Add(this);
    }

    public void OnDestroy()
    {
        Objects.Remove(this);
    }

    public void FixedUpdate()
    {
        foreach (var obj in Objects)
        {
            if (obj != this)
            {
                Vector2 toObj = obj.transform.position - transform.position;
                var gravity = GravityStrength * obj.rb.mass * rb.mass / toObj.sqrMagnitude * toObj.normalized;
                rb.AddForce(gravity);
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
