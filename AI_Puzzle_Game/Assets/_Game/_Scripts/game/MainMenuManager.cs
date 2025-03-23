using System;
using UnityEngine;

[Serializable]
public enum MainMenuState {
    HOME = 0,
    LEVEL_SELECT,
    SETTINGS
}

public class MainMenuManager : MonoBehaviour {
    [SerializeField] GameObject HomeUi, LevelSelectUi, SettingsUi;
    private MainMenuState state;

    void Start()
    {
        SetState(0);
    }

    public void SetState(int state) {
        this.state = (MainMenuState) state;

        HomeUi.SetActive(this.state == MainMenuState.HOME);
        LevelSelectUi.SetActive(this.state == MainMenuState.LEVEL_SELECT);
        SettingsUi.SetActive(this.state == MainMenuState.SETTINGS);
    }

    public void OpenTutorial() {

    }

    public void QuitGame() {
        Application.Quit();
    }
}