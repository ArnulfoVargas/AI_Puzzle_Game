using UnityEngine;

public class Target : MonoBehaviour, IPlayerMovementSoundEmmiter
{
    [SerializeField] private bool isActive = true;

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
