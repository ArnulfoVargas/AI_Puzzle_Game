using UnityEngine;

public class Target : MonoBehaviour, IPlayerMovementSoundEmmiter
{
    [SerializeField] private bool isActive = true;
    [SerializeField] private Transform visual;

    void Start()
    {
        SetLayer();
    }

    private void SetLayer()
    {
        if (isActive)
        {
            gameObject.layer = LayerMask.NameToLayer("Default");
        }
        else
        {
            gameObject.layer = LayerMask.NameToLayer("Wall");
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out IWinner winner)) {
            if (isActive) {
                winner.Win();
                winner.SetPosition(visual.position);
                return;
            }
        }
    }

    public void OnOpenGate() {
        isActive = true;
        SetLayer();
    }

    public void PlaySound()
    {
        if (isActive) return;
        
        AudioManager.GetInstance()?.SetAudioWithZeroPosition(Audio_Type.LOCKED);
    }
}
