using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

// [Serializable]
[CreateAssetMenu(fileName = "BindedLevels", menuName = "Configs/BindedLevels"), Serializable]
public class BindedLevels : ScriptableObject
{
    [SerializeField] private List<LevelIslands> _levelIslandsMap = new();
    
    public bool IsSceneBinded(int scene) {
        foreach (var island in this._levelIslandsMap)
        {
            if (island.sceneIndex == scene) return true;
        }
        return false;
    }

    public LevelIslands GetLevelIslands(int scene)
    {
        foreach (var island in _levelIslandsMap)
        {
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
        EditorUtility.SetDirty(this);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }
}
