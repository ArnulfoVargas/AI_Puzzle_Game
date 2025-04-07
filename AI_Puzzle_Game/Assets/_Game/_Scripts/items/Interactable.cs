using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class Interactable : BaseBehaviour, IInteractable
{
    [SerializeField, Range(0,2)] private int collectableNumber;
    [SerializeField] private MeshRenderer meshRenderer;
    private bool collected;
    private BoxCollider col;
    LevelIslands levelIslands;

    protected override void OnStart()
    {
        col = GetComponent<BoxCollider>();
        var c = LevelsManager.Instance.CurrentLevel;
        if (c != null) {
            levelIslands = c;

            if (levelIslands.LevelData.collectableTaken[collectableNumber]){
                this.gameObject.SetActive(false);
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
        levelIslands.OnTakeInteractable(collectableNumber);

        gameObject.SetActive(false);
    }
}