using UnityEngine;

[CreateAssetMenu(fileName = "PlayerConfigs", menuName = "Configs/PlayerConfigs")]
public class PlayerConfigs : ScriptableObject {
    [SerializeField] private float moveSpeed;   
    public float MoveSpeed => moveSpeed;
}