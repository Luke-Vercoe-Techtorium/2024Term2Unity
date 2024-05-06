using UnityEngine;

public class MissileSpawner : MonoBehaviour
{
    [SerializeField]
    private Missile MissilePrefab = null;

    public float SpawnRadius = 50.0f;

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            var spawnOffset = Random.insideUnitCircle.normalized * SpawnRadius;
            var rotation = Quaternion.FromToRotation(Vector2.up, Random.insideUnitCircle.normalized);
            var missile = Instantiate(MissilePrefab, spawnOffset, rotation);
            missile.Player = gameObject;
        }
    }
}
