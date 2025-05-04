using UnityEngine;
using UnityEngine.SceneManagement;
using Utils;

public class GameManager : MonoBehaviour {
    private static GameManager Instance;
    public float cameraRotation = 0;
    public On<GameState> OnGameStateChanged;
    public On<PlayerState, PlayerState> OnPlayerStateChanged;
    [SerializeField] private GameState currentGameState;
    private PlayerState playerState = PlayerState.IDLE;
    public PlayerState CurrentPlayerState {
        get => playerState;
        set {
            OnPlayerStateChanged?.Invoke(playerState, value);
            playerState = value;
        }
    }
    public GameState CurrentGameState {
        get => currentGameState;
        private set {
            currentGameState = value;
            OnGameStateChanged?.Invoke(currentGameState);
        }
    }

    void Awake()
    {
        Instance ??= this;

        if (Instance != this) {
            Destroy(this);
            return;
        }

        SceneManager.sceneUnloaded += (scene) => {
            CurrentPlayerState = PlayerState.IDLE;
            currentGameState = GameState.GAMEPLAY;
            cameraRotation = 0;
        };
    }

    public void OnTravel() {
        CurrentGameState = GameState.ISLAND_CHANGE;
    }

    public void OnGameplay() {
        CurrentGameState = GameState.GAMEPLAY;
    }

    public void OnPlayerVictoryAnimation() {
        CurrentGameState = GameState.VICTORY_ANIMATION;
    }

    public void OnPause() {
        CurrentGameState = GameState.PAUSE;
    }

    public void OnSettings() {
        CurrentGameState = GameState.SETTINGS;
    }

    public void OnLoose()
    {
        CurrentGameState = GameState.DEFEAT;
    }

    public void OnWin() {
        CurrentGameState = GameState.VICTORY;
    }

    public void OnDialog() {
        CurrentGameState = GameState.DIALOG; 
    }

    public static GameManager GetInstance() {
        return Instance;
    } 
}