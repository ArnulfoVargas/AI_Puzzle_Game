using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UiManager : MonoBehaviour
{
    [SerializeField] GameObject OnGameUi, OnVictoryUi, OnLooseUi, SettingsUi, OnPauseUi;
    [SerializeField] Button nextBtn;
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

        if (s == GameState.VICTORY) {
            nextBtn.interactable = LevelsManager.Instance.NextLevel != null;

        }
    }

    public void Retry() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void OpenLevel(int index) {
        SceneManager.LoadScene(index);
    }

    public void NextLevel() {
        var next = LevelsManager.Instance.NextLevel;
        if (next)
            OpenLevel(next.sceneIndex);
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
        SceneManager.LoadScene(1);
    }

    public void PlaySound() {
        AudioManager.GetInstance().PlayUiAudio();
    }
}
