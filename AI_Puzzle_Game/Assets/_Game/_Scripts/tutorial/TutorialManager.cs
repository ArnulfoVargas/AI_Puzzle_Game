using UnityEngine;

public class TutorialManager : BaseBehaviour {
    private static TutorialManager instance;
    public static TutorialManager GetInstance => instance;

    void Awake()
    {
        instance = this;
    }

    public void SetDialogState(TutorialHint hint) {
        GameManager.GetInstance().OnDialog();
        print("dialog");
    }

    protected override void OnVictoryStart()
    {
        ES3.Save<bool>("first_play", false);
    }
}