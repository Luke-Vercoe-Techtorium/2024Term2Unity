using UnityEngine;

public class Missile : MonoBehaviour
{
    public GameObject Player = null;
    public float Speed = 10.0f;

    public void FixedUpdate()
    {
        var toPlayer = Player.transform.position - transform.position;
        toPlayer.z = 0.0f;

        transform.SetPositionAndRotation(
            transform.position + Speed * Time.fixedDeltaTime * transform.up,
            Quaternion.RotateTowards(
                transform.rotation,
                Quaternion.FromToRotation(
                    transform.up,
                    toPlayer.normalized
                ),
                90.0f * Time.fixedDeltaTime
            )
        );
    }
}
