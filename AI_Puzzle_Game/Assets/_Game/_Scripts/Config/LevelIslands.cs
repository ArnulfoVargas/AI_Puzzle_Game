using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(fileName = "LevelIslands", menuName = "Configs/LevelIslands"), Serializable]
public class LevelIslands : ScriptableObject
{
    [SerializeField] private List<IslandPoint> paths = new();
    public int sceneIndex = -1;
    public List<IslandPoint> Paths => paths;

    public Vector3 TravelTo(int pathIndex)
    {
        return paths[pathIndex].GetCenter;
    }

    private void Awake()
    {
        Selection.activeObject = null;
    }

    public void Unbind()
    {
        sceneIndex = -1;
        foreach (var points in paths)
        {
            AssetDatabase.DeleteAsset(AssetDatabase.GetAssetPath(points.GetInstanceID()));   
        }
        
        Save();
        paths.Clear();
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
        paths.RemoveAt(index);
        Save();
    }

    public void Save()
    {
        EditorUtility.SetDirty(this);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }
}