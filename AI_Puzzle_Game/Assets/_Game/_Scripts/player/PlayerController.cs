using UnityEngine;

public class PlayerController : BaseBehaviour {
    private PlayerInputsReader inputs;
    private float moveSpeed {get; set;}
    private Vector3 moveDirection;
    PlayerState playerState = PlayerState.IDLE;
    PlayerState CurrentPlayerState {
        get => playerState; 
        set {
            playerState = value;
            GameManager.GetGameManager.CurrentPlayerState = playerState;
        }
    }
    protected override void OnStart()
    {
        var configs = Resources.Load<PlayerConfigs>("PlayerConfigs");
        inputs = Resources.Load<PlayerInputsReader>("PlayerInputsReader");
        moveSpeed = configs.MoveSpeed;

        inputs.OnMove += dir => {
            if (CurrentPlayerState == PlayerState.IDLE)
            {
                moveDirection = dir;
                CurrentPlayerState = PlayerState.MOVING;
            }
        };
    }
    protected override void OnGameplayStart()
    {
    }
}