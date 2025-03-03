using _Game._Scripts.interfaces;
using DG.Tweening;
using UnityEngine;

public class CollisionHandler : BaseBehaviour
{
    [SerializeField] private PlayerController player;
    const string TELEPORT_TAG = "Teleporter";
    Tweener tweener;

    protected override void OnStart()
    {
        player = GetComponent<PlayerController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out ITeleporter teleporter))
        {
            tweener = CameraManager.Instace.TravelToIsland(teleporter.GetIslandIndex(), out Vector3 endPosition);
            tweener.OnComplete(() =>
            {
                var p = teleporter.GetTeleportLocation();
                transform.position = new Vector3(Mathf.RoundToInt(p.x), 0, Mathf.RoundToInt(p.z));
                player.SetIdleState();
            });
            player.SetTravelingState();
        }
    }
}
