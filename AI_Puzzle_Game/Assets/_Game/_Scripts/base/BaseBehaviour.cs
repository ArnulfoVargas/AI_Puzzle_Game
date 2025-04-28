using UnityEngine;

public class BaseBehaviour : MonoBehaviour
{
    protected GameManager manager;
    protected GameState currentState;
    // protected delegate void UpdateFunc();
    // protected UpdateFunc update;

    protected void SetUp()
    {
        manager = GameManager.GetInstance();
        manager.OnGameStateChanged += UpdateState;
        UpdateState(manager.CurrentGameState);

        OnStart();
    }

    void OnEnable()
    {
        manager.OnGameStateChanged += UpdateState;
        UpdateState(manager.CurrentGameState);
    }

    void OnDisable()
    {
        manager.OnGameStateChanged -= UpdateState;
    }

    void Start()  {
        SetUp();
    }
    void FixedUpdate()
    {
        switch (currentState)
        {
            default: break;
            case GameState.GAMEPLAY:
                OnGameplayUpdate();
                break;
            case GameState.PAUSE:
                OnPauseUpdate();
                break;
            case GameState.ISLAND_CHANGE:
                OnIslandChangeUpdate();
                break;
            case GameState.VICTORY:
                OnVictoryUpdate();
                break;
            case GameState.DEFEAT:
                OnDefeatUpdate();
                break;
            case GameState.DIALOG:
                OnDialogUpdate();
                break;
        }

        // update.Invoke();
    }

    protected void UpdateState(GameState state)
    {
        currentState = state;
        BeforeUpdate(currentState);
        switch (state)
        {
            default: break;
            case GameState.GAMEPLAY:
                OnGameplayStart();
                // update = OnGameplayUpdate;
                break;
            case GameState.PAUSE:
                OnPauseStart();
                // update = OnPauseUpdate;
                break;
            case GameState.ISLAND_CHANGE:
                OnIslandChangeStart();
                // update = OnIslandChangeUpdate;
                break;
            case GameState.VICTORY:
                OnVictoryStart();
                // update = OnVictoryUpdate;
                break;
            case GameState.DEFEAT:
                OnDefeatStart();
                // update = OnDefeatUpdate;
                break;
            case GameState.DIALOG:
                OnDialogStart();
                // update = OnDialogUpdate;
                break;
            case GameState.SETTINGS:
                OnSettingsStart();
                // update = OnSettingsUpdate;
                break;
        }

        OnUpdateState(state);
    }

/// <summary>
/// Functions made for update properties once the state change
/// </summary>
#region OnStartFunctions
    virtual protected void OnStart() {}
    virtual protected void OnGameplayStart() { }
    virtual protected void OnPauseStart() { }
    virtual protected void OnIslandChangeStart() { }
    virtual protected void OnVictoryStart() { }
    virtual protected void OnDefeatStart() { }
    virtual protected void OnDialogStart() { }
    virtual protected void OnSettingsStart() { }
#endregion

/// <summary>
/// Functions made for update the behaviour in order by the current game state
/// </summary>
#region OnUpdateFunctions
    virtual protected void BeforeUpdate(GameState state) { }
    virtual protected void OnGameplayUpdate() { }
    virtual protected void OnPauseUpdate() { }
    virtual protected void OnIslandChangeUpdate() { }
    virtual protected void OnVictoryUpdate() { }
    virtual protected void OnDefeatUpdate() { }
    virtual protected void OnDialogUpdate() { }
    virtual protected void OnSettingsUpdate() { }

    virtual protected void OnUpdateState(GameState state) {}
#endregion

}
