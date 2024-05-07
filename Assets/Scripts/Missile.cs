using UnityEngine;

public class Missile : MonoBehaviour
{
    public GravityObject Player = null;
    public float Speed = 10.0f;

    public void FixedUpdate()
    {
        var aimPoint = Player.transform.position;
        for (var i = 0; i < 20; i++)
        {
            var distance = (aimPoint - transform.position).magnitude;
            var time = distance / Speed;
            aimPoint = (Vector2)Player.transform.position + Player.rb.velocity * time;
        }

        var aimDirection = aimPoint - transform.position;
        transform.SetPositionAndRotation(
            transform.position + Speed * Time.fixedDeltaTime * transform.up,
            Quaternion.RotateTowards(
                transform.rotation,
                Quaternion.FromToRotation(
                    Vector2.up,
                    aimDirection.normalized
                ),
                180.0f * Time.fixedDeltaTime
            )
        );
    }
}
