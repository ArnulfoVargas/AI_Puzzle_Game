using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TutorialManager : BaseBehaviour {
    [SerializeField, Range(0.01f, .5f)] float timeBetweenLetters = 0.1f;
    private static TutorialManager instance;
    public static TutorialManager GetInstance => instance;
    public UiManager uiManager;
    private TMP_Text UiText;
    private Button button;
    private bool dialogEnded;
    private int index, maxIndex, dialogsIndex, maxDialogsIndex;
    private float currentTime;
    private TutorialHint hint;

    void Awake()
    {
        instance = this;
    }

    public void SetDialogState(TutorialHint _hint) {
        GameManager.GetInstance().OnDialog();
        UiText ??= uiManager.getTutorialText;
        if (!button) {
            button = uiManager.getTutorialButton;
            button.onClick.AddListener(OnNextText);
        }
        hint = _hint;
        dialogsIndex = 0;
        maxDialogsIndex = hint.Texts.Length;
        ResetText();
    }

    private void ResetText() {
        UiText.text = "";

        maxIndex = hint.Texts[dialogsIndex].Length;
        try {
            index = 1;
            UiText.text += this.hint.Texts[dialogsIndex][0];
        } catch {
            index = 0;
        }
        currentTime = 0;
    }

    public void OnNextText() {
        if (!dialogEnded) {
            dialogEnded = true;
            UiText.text = hint.Texts[dialogsIndex];
            return;
        }

        dialogEnded = false;
        dialogsIndex++;

        if (dialogsIndex >= maxDialogsIndex) {
            GameManager.GetInstance().OnGameplay();
            return;
        }
  
        ResetText();
    }

    protected override void OnDialogUpdate()
    {
        if (dialogEnded) return;

        currentTime += Time.fixedDeltaTime;

        if (currentTime >= timeBetweenLetters) {
            currentTime = 0;

            UiText.text += hint.Texts[dialogsIndex][index];

            index++;
            if (index >= maxIndex) {
                dialogEnded = true;
            }
        }
    }

    protected override void OnVictoryStart()
    {
        ES3.Save<bool>("first_play", false);
    }
}