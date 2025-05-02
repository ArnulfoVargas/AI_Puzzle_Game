using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class Interactable : BaseBehaviour, IInteractable
{
    [SerializeField, Range(0,2)] private int collectableNumber;
    [SerializeField] private MeshRenderer meshRenderer;
    private bool collected;
    private BoxCollider col;
    LevelIslands levelIslands;
    private bool wasCollected = false;

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

        if (!wasCollected)
            levelIslands.OnTakeInteractable(collectableNumber);
    }
}