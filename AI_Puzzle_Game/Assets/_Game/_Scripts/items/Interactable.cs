using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(BoxCollider))]
public class Interactable : BaseBehaviour, IInteractable
{
    [SerializeField, Range(0,2)] private int collectableNumber;
    [SerializeField] private MeshRenderer meshRenderer;
    [SerializeField] Transform particleSpawnPosition;
    private BoxCollider col;
    LevelIslands levelIslands;
    private bool wasCollected = false;
    [SerializeField] UnityEvent OnInteractEvent;

    protected override void OnStart()
    {
        col = GetComponent<BoxCollider>();
        var c = LevelsManager.Instance.CurrentLevel;
        if (c != null) {
            levelIslands = c;

            if (levelIslands.LevelData.collectableTaken[collectableNumber]){
                wasCollected = true;
                meshRenderer.material.SetFloat("_Alpha", 0.75f);
            }
        } else {
            gameObject.SetActive(false);
            Debug.LogWarning("Current scene is not binded");
        }
    }

    protected virtual void OnTriggerEnter(Collider other){
        if (other.gameObject.TryGetComponent(out CollisionHandler ch)) {
            OnInteract();
        }
    }

    public void OnInteract()
    {
        col.enabled = false;
        gameObject.SetActive(false);
        AudioManager.GetInstance().SetAudioWithZeroPosition(Audio_Type.COIN);
        ParticlesManager.Instance.SpawnParticle(ParticleType.COIN_COLLECTED, particleSpawnPosition.position, Quaternion.LookRotation(Vector3.up, Vector3.right));

        if (!wasCollected)
            levelIslands.OnTakeInteractable(collectableNumber);
    }
}