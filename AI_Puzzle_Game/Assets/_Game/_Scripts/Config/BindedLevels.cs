using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.Linq;

// [Serializable]
[CreateAssetMenu(fileName = "BindedLevels", menuName = "Configs/BindedLevels"), Serializable]
public class BindedLevels : ScriptableObject
{
    [SerializeField] private List<LevelIslands> _levelIslandsMap = new();
    public List<LevelIslands> GetAllLevelIslands => _levelIslandsMap;
    
    public bool IsSceneBinded(int scene) {
        for (int i = 0; i < _levelIslandsMap.Count; i++)
        {
            LevelIslands island = this._levelIslandsMap[i];
            if (island.sceneIndex == scene) return true;
        }
        return false;
    }

    public LevelIslands GetLevelIslands(int scene)
    {
        for (int i = 0; i < _levelIslandsMap.Count; i++)
        {
            LevelIslands island = _levelIslandsMap[i];
            if (island.sceneIndex == scene) return island;
        }

        return null;
    }

    public void BindLevel(int scene, LevelIslands levelIslands)
    {
        _levelIslandsMap.Add(levelIslands);
        Save();
    }

    public void UnbindLevel(int scene)
    {
        var levelIslands = GetLevelIslands(scene);
        _levelIslandsMap.Remove(levelIslands);
        _levelIslandsMap.RemoveAll(l => l == null );
        Save();
    }

    public void Save()
    {
        #if UNITY_EDITOR
        EditorUtility.SetDirty(this);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        #endif
    }

    public void TryChangeLevelNumber(int levelNumber, LevelIslands islands) {
        if (levelNumber <= -1) {
            islands.LevelNumber = -1;
            islands.next = null;
            UpdateNext();
            return;
        }
        var level = (from li in _levelIslandsMap 
                    where li.LevelNumber == levelNumber
                    select li).FirstOrDefault();

        if (level == null) {
            islands.LevelNumber = levelNumber;
            UpdateNext();
            return;
        }
        
        var holder = islands.LevelNumber;
        islands.LevelNumber = levelNumber;
        level.LevelNumber = holder;
        UpdateNext();
    }

    private void UpdateNext() {
        var levels =(from lvl in _levelIslandsMap
                     where lvl.LevelNumber >= 0 && lvl.Playable
                     select lvl).ToHashSet().OrderBy((x) => x.LevelNumber).ToList();

        if (levels.Count == 1) return;

        for (int i = 0; i < levels.Count - 1; i++)
        {
            var lvl = levels[i];
            lvl.next = levels[i + 1];
        }
    }
}
