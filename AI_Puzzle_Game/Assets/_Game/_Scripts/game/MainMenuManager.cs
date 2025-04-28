using System;
using System.Linq;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[Serializable]
public enum MainMenuState {
    HOME = 0,
    LEVEL_SELECT,
    SETTINGS,
    VOLUME,
    CONFIRMATION,
    CREDITS
}

public class MainMenuManager : MonoBehaviour {
    [SerializeField] GameObject HomeUi, LevelSelectUi, SettingsUi, VolumeSettingsUi, ConfirmationUi, CreditsUi, levelSelectorButtonsParent;
    [SerializeField] Transform buttonPrefab;
    private MainMenuState state;

    void Start()
    {
        SetState(0);
        BuildLevelSelector();
    }

    public void SetState(int state) {
        this.state = (MainMenuState) state;

        HomeUi.SetActive(this.state == MainMenuState.HOME);
        LevelSelectUi.SetActive(this.state == MainMenuState.LEVEL_SELECT);
        SettingsUi.SetActive(this.state == MainMenuState.SETTINGS);
        ConfirmationUi.SetActive(this.state == MainMenuState.CONFIRMATION);
        CreditsUi.SetActive(this.state == MainMenuState.CREDITS);
    }

    public void OpenTutorial() {
        var tutorial = LevelsManager.Instance.tutorialLevel;
        if (tutorial != null)
            SceneManager.LoadScene(tutorial.sceneIndex);
    }

    public void QuitGame() {
        Application.Quit();
    }

    public void ResetData() {
        foreach(var f in ES3.GetFiles()) {
            ES3.DeleteFile(f);
        }
        SceneManager.LoadScene(0);
    }

    public void BuildLevelSelector() {
        var levels = LevelsManager.Instance.orderedLevels;
        for (int i = 0; i < levels.Count; i ++) {
            var go = Instantiate(buttonPrefab);
            go.parent = levelSelectorButtonsParent.transform;

            var btn = go.GetComponent<Button>();
            var lvl = levels[i];
            btn.interactable = lvl.LoadGame().unlocked;
            btn.onClick.AddListener(() => {
                SceneManager.LoadScene(lvl.sceneIndex);
            });

            btn.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = lvl.LevelNumber.ToString();
        }
    }

    public void PlaySound() {
        AudioManager.GetInstance().PlayUiAudio();
    }
}