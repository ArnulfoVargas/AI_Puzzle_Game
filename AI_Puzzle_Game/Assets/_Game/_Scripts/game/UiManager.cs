using UnityEngine;
using UnityEngine.SceneManagement;

public class UiManager : MonoBehaviour
{
    [SerializeField] GameObject OnGameUi, OnVictoryUi, OnLooseUi, SettingsUi, OnPauseUi;
    // private GameObject[] UIs;
    GameManager gameManager;
    void OnEnable()
    {
        gameManager = GameManager.GetInstance();

        gameManager.OnGameStateChanged += SetUisVisibility;
        SetUisVisibility(gameManager.CurrentGameState);
    }

    void OnDisable()
    {
        gameManager.OnGameStateChanged -= SetUisVisibility;
    }

    private void SetUisVisibility(GameState s) {
        OnGameUi?.SetActive(s == GameState.GAMEPLAY);
        OnVictoryUi?.SetActive(s == GameState.VICTORY);
        OnLooseUi?.SetActive(s == GameState.DEFEAT);
        SettingsUi?.SetActive(s == GameState.SETTINGS);
        OnPauseUi?.SetActive(s == GameState.PAUSE);
    }

    public void Retry() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void OpenLevel(int index) {
        SceneManager.LoadScene(index);
    }

    public void Resume() {
        gameManager.OnGameplay();
    }

    public void Pause()
    {
        gameManager.OnPause();
    }

    public void Settings() {
        gameManager.OnSettings();
    }

    public void Home() {
        SceneManager.LoadScene(0);
    }
}
