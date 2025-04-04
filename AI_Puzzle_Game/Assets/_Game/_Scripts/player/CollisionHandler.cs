using _Game._Scripts.interfaces;
using DG.Tweening;
using UnityEngine;

public class CollisionHandler : BaseBehaviour, IDamageable, ITeleporteable, IWinner
{
    [SerializeField] private PlayerController player;
    Tweener tweener;

    protected override void OnStart()
    {
        player = GetComponent<PlayerController>();
    }

    public void Damage()
    {
        GameManager.GetInstance().OnLoose();
    }

    public void Teleport(ITeleporter teleporter)
    {
        tweener = CameraManager.Instace.TravelToIsland(teleporter.GetIslandIndex(), out Vector3 endPosition);

        tweener.OnComplete(() =>
        {
            GameManager.GetInstance().OnGameplay();
            var p = teleporter.GetTeleportLocation();
            transform.position = new Vector3(Mathf.RoundToInt(p.x), 0, Mathf.RoundToInt(p.z));
            player.SetIdleState();
        });

        player.SetTravelingState();
    }

    void OnDestroy()
    {
        tweener.Kill();
    }
    public void Win()
    {
        GameManager.GetInstance().OnWin();
    }
}