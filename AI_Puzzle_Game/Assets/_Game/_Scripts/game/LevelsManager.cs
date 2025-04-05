using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelsManager : MonoBehaviour {
    [SerializeField] BindedLevels bindedLevels;
    private static LevelsManager instance;
    public static LevelsManager Instance => instance;
    private LevelIslands currentLevel;
    private LevelIslands nextLevel;
    public LevelIslands CurrentLevel => currentLevel;
    public LevelIslands NextLevel => nextLevel;
    [HideInInspector] public List<LevelIslands> orderedLevels = new();
    [HideInInspector] public LevelIslands tutorialLevel;

    void Awake()
    {
        instance ??= this;
        if (instance != this) Destroy(gameObject);

        DontDestroyOnLoad(this);

        SceneManager.sceneLoaded += OnSceneLoaded;

        GameManager.GetInstance().OnGameStateChanged += OnGameStateChange;
        var allLevels = bindedLevels.GetAllLevelIslands;
        orderedLevels =(from bl in allLevels
                        where bl.Playable && bl.LevelNumber > 0 
                        select bl
                    ).ToHashSet().OrderBy((x) => x.LevelNumber).ToList();

        var v = from bl in bindedLevels.GetAllLevelIslands
                        where bl.Playable && bl.LevelNumber == 0
                        select bl
                        ;
        
        tutorialLevel = v.Count() == 0 ? null : v.First();
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
        currentLevel = bindedLevels.GetLevelIslands(scene.buildIndex);
        currentLevel?.LoadGame();
    }

    public void UnlockNext() {
        nextLevel = currentLevel.next;
        if (nextLevel == null) return;

        nextLevel.LoadGame();
        if (!nextLevel.LevelData.unlocked) {
            nextLevel.Unlock();
        }
    }

    private void OnVictory() {
        UnlockNext();
        currentLevel.OnSucceed();
    }

    public void OnGameStateChange(GameState state) {
        if (state == GameState.VICTORY) {
            OnVictory();
        }
        else if (state == GameState.DEFEAT) {

        }
    }
}