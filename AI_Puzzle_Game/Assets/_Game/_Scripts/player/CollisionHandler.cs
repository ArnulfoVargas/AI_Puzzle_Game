using _Game._Scripts.interfaces;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

public class CollisionHandler : BaseBehaviour, IDamageable, ITeleporteable, IWinner
{
    [SerializeField] private PlayerController player;
    [SerializeField] private UnityEvent OnTeleportEnd;
    [SerializeField] private PlayerAnimator animator;
    Tweener tweener;

    protected override void OnStart()
    {
        player = GetComponent<PlayerController>();
    }

    public void Damage()
    {
        AudioManager.GetInstance().SetAudioWithZeroPosition(Audio_Type.DISSOLVE);
        animator.TriggerLoose();
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
            OnTeleportEnd?.Invoke();
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