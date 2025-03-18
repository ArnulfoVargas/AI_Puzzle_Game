using UnityEngine;

[CreateAssetMenu(fileName = "PlayerConfigs", menuName = "Configs/PlayerConfigs")]
public class PlayerConfigs : ScriptableObject {
    [SerializeField] private float moveSpeed;
    [SerializeField] private float acceleration;
    public float MoveSpeed => moveSpeed;
    public float Acceleration => acceleration;
}