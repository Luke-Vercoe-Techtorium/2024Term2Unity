using UnityEngine;

public class Sword : MonoBehaviour
{
    [SerializeField]
    private GameObject Player = null;

    public void LateUpdate()
    {
        var mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        var swordDirection = (mouseWorldPosition - Player.transform.position).normalized;
        swordDirection.z = 0.0f;

        transform.SetPositionAndRotation(
            Player.transform.position,
            Quaternion.RotateTowards(
                transform.rotation,
                Quaternion.FromToRotation(Vector2.right, swordDirection),
                720.0f * Time.deltaTime
            )
        );
    }
}
