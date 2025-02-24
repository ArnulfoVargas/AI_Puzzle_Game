using UnityEngine;
using UnityEngine.SceneManagement;
using Utils;

public class GameManager : MonoBehaviour {
    private static GameManager Instance;
    public float cameraRotation = 0;
    public On<GameState> OnGameStateChanged;
    private GameState currentGameState;
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
}