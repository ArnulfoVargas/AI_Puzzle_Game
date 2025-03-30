using System;
using System.Collections.Generic;
using UnityEditor;

#if UNITY_EDITOR
using UnityEditor.SceneManagement;
#endif

using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(fileName = "LevelIslands", menuName = "Configs/LevelIslands"), Serializable]
public class LevelIslands : ScriptableObject
{
    [SerializeField] private List<IslandPoint> paths = new();
    public int sceneIndex = -1;
    public string scenePath = "";
    public List<IslandPoint> Paths => paths;
    [SerializeField] private LevelData levelData;
    public LevelData LevelData => levelData;
    [SerializeField] private int levelNumber = -1;
    public LevelIslands next;
    [SerializeField] private bool startsUnlocked = false;
    public bool StartsUnlocked {
        get => startsUnlocked;
        set {
            startsUnlocked = value;
        }
    }
    public int LevelNumber {
        get => levelNumber;
        set {
            levelNumber = value;
        }
    }
    [SerializeField] private bool playable = true;
    public bool Playable {
        get => playable;
        set {
            playable = value;
        }
    }

    public Vector3? TravelTo(int pathIndex)
    {
        if (paths.Count == 0 ) return Vector3.zero; 
        if (pathIndex >= paths.Count) return null;
        return paths[pathIndex].GetCenter;
    }

    private void Awake()
    {
        #if UNITY_EDITOR
        Selection.activeObject = null;
        #endif
    }

    public void Unlock() {
        levelData.unlocked = true;
        SaveGame();
    }

    public void Unbind()
    {
        sceneIndex = -1;
        scenePath = "";
    }

    public void OnTakeInteractable(int index) {
        levelData.collectableTaken[index] = true;
    }

    public void OnSucceed() {
        levelData.levelSucceed = true;
        SaveGame();
    }

    public Scene getScene => SceneManager.GetSceneByBuildIndex(sceneIndex);
    public string getScenePath => getScene.path;
    public string GetSceneName() {
        var scenePathArr = getScenePath.Split('/'); 
        return scenePathArr[scenePathArr.Length - 1].Replace(".unity", "");
    }

    public void AddPoint(IslandPoint islandPoint)
    {
        paths.Add(islandPoint);
        Save();
    }

    public void RemovePointAtIndex(int index)
    {
        #if UNITY_EDITOR
        var p = paths[index];
        if (p) {
            AssetDatabase.DeleteAsset(AssetDatabase.GetAssetPath(p));
            AssetDatabase.Refresh();
        }
        #endif
        paths.RemoveAt(index);

        Save();
    }

    public void Validate()
    {
        #if UNITY_EDITOR
        var sceneByIndex = EditorSceneManager.GetSceneByBuildIndex(sceneIndex);
        var sceneByPath = EditorSceneManager.GetSceneByPath(scenePath);

        if (sceneByIndex.buildIndex == sceneByPath.buildIndex) return;

        if (sceneByIndex.buildIndex == -1 && sceneByPath.buildIndex >= 0) {
            sceneIndex = sceneByPath.buildIndex;
        } else if (sceneByPath.buildIndex == -1 && sceneByIndex.buildIndex >= 0) {
            scenePath = sceneByIndex.path;
        }else if (sceneByIndex.buildIndex != sceneByPath.buildIndex) {
            sceneIndex = sceneByPath.buildIndex;
        } else {
            Unbind();
        }
        #endif
    }
    public void ValidatePoints() {
        #if UNITY_EDITOR
        for (int i = paths.Count - 1; i >= 0 ; i--)
        {
            if (paths[i] == null) RemovePointAtIndex(i);
        }
        Save();
        #endif
    }

    public void Save()
    {
        #if UNITY_EDITOR
        EditorUtility.SetDirty(this);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        #endif
    }

    private void SaveGame() {
        ES3.Save($"{levelNumber:00}-{scenePath}-data", levelData);
    }

    public LevelData LoadGame() {
        levelData = ES3.Load<LevelData>($"{levelNumber:00}-{scenePath}-data", new LevelData{
            collectableTaken = new[]{false, false, false},
            recordMoves = 0,
            levelSucceed = false,
            unlocked = startsUnlocked
        });

        return levelData;
    }
}