using System;
using _Game._Scripts.interfaces;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

public class CollisionHandler : BaseBehaviour, IDamageable, ITeleporteable, IWinner
{
    [SerializeField] private PlayerController player;
    [SerializeField] private Transform visual;
    [SerializeField] private Transform particlePosition;
    [SerializeField] private UnityEvent OnTeleportEnd;
    [SerializeField] private PlayerAnimator animator;
    Tweener tweener;

    public PlayerState GetPlayerState() {
        return player.CurrentPlayerState;
    }
    protected override void OnStart()
    {
        player = GetComponent<PlayerController>();
    }

    public void Damage()
    {
        try
        {
            AudioManager.GetInstance().SetAudioWithZeroPosition(Audio_Type.DISSOLVE);
            ParticlesManager.Instance.SpawnParticle(ParticleType.DEATH, particlePosition.position);
        } catch (NullReferenceException) {}
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
        player.CurrentPlayerState = PlayerState.ANIMATION;
        visual.localScale = new Vector3(1, 1, -1);
        GameManager.GetInstance().OnPlayerVictoryAnimation();
    }

    public void SetPosition(Vector3 newPos)
    {
        visual.position = newPos;
    }
}