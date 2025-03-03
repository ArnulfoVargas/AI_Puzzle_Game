using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;


public class BindedLevels : ScriptableObject
{
    private List<LevelIslands> _levelIslandsMap = new();
    
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
        levelIslands.sceneIndex = scene;
        _levelIslandsMap.Add(levelIslands);
        Save();
    }

    public void UnbindLevel(int scene)
    {
        var levelIslands = GetLevelIslands(scene);
        _levelIslandsMap.Remove(levelIslands);
        Save();
    }

    public void Save()
    {
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }
}
