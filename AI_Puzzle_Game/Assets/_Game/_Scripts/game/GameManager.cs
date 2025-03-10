using UnityEditorInternal;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utils;

public class GameManager : MonoBehaviour {
    private static GameManager Instance;
    public float cameraRotation = 0;
    public On<GameState> OnGameStateChanged;
    [SerializeField] private GameState currentGameState;
    public PlayerState CurrentPlayerState = PlayerState.IDLE;
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
            Instance = null;
            Destroy(this);
        };
    }

    public void OnTravel() {
        CurrentGameState = GameState.ISLAND_CHANGE;
    }

    public void OnTravelEnd() {
        CurrentGameState = GameState.GAMEPLAY;
    }

    public void OnLoose()
    {
        CurrentGameState = GameState.DEFEAT;
    }

    public void OnWin() {
        CurrentGameState = GameState.VICTORY;
    }

    public static GameManager GetInstance() {
        return Instance;
    } 
}