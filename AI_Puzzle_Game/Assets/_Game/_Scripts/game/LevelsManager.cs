using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelsManager : MonoBehaviour {
    [SerializeField] BindedLevels bindedLevels;
    private static LevelsManager instance;
    public static LevelsManager Instance => instance;
    private int lastUnlockedLevel = 0;
    private LevelIslands currentLevel;
    public LevelIslands CurrentLevel => currentLevel;
    public List<LevelIslands> orderedLevels;

    void Awake()
    {
        instance ??= this;
        if (instance != this) Destroy(gameObject);

        DontDestroyOnLoad(this);

        lastUnlockedLevel = ES3.Load<int>("last_unlocked_lvl", 0);

        SceneManager.sceneLoaded += OnSceneLoaded;

        GameManager.GetInstance().OnGameStateChanged += OnGameStateChange;

        orderedLevels =(from bl in bindedLevels.GetAllLevelIslands 
                        where bl.Playable && bl.LevelNumber >= 0 
                        orderby bl.LevelNumber ascending 
                        select bl
                    ).ToList();
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
        currentLevel = bindedLevels.GetLevelIslands(scene.buildIndex);
    }

    public void UnlockNext() {

    }

    public void OnGameStateChange(GameState state) {
        if (state == GameState.VICTORY) {

        }
        else if (state == GameState.DEFEAT) {

        }
    }
}