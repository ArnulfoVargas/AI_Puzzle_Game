using _Game._Scripts.interfaces;
using UnityEngine;

public class Teleporter : MonoBehaviour, ITeleporter
{
    [SerializeField] private int teleportToIslandIndex;
    [SerializeField] private Transform teleportLocation;
    public int GetIslandIndex()
    {
        return teleportToIslandIndex;
    }

    public Vector3 GetTeleportLocation()
    {
        return teleportLocation.position;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out ITeleporteable teleporteable)) {
            teleporteable.Teleport(this);
        }
    }
}
