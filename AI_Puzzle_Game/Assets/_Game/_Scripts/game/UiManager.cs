using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UiManager : MonoBehaviour
{
    [SerializeField] GameObject OnGameUi, OnVictoryUi, OnLooseUi, SettingsUi, OnPauseUi, DialogUi, Clippa;
    [SerializeField] Button nextBtn;
    [SerializeField] TMP_Text tutorialHintText;
    [SerializeField] Button tutorialButton;
    [SerializeField] Image[] coins;
    [SerializeField] Sprite coinSprite;

    public TMP_Text getTutorialText => tutorialHintText;
    public Button getTutorialButton => tutorialButton;

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
        DialogUi?.SetActive(s == GameState.DIALOG);
        Clippa?.SetActive(s == GameState.DIALOG);

        if (s == GameState.VICTORY)
        {
            nextBtn.gameObject.SetActive(LevelsManager.Instance.NextLevel != null);
            var collectables = LevelsManager.Instance.CurrentLevel.LevelData.collectableTaken;

            for (int i = 0; i < collectables.Length; i++)
            {
                if (collectables[i])
                {
                    coins[i].sprite = coinSprite;
                }
            }
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
        try
        {
            AudioManager.GetInstance().PlayUiAudio();
        } catch(NullReferenceException) {}
    }
}
