using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

[CustomEditor(typeof(BindedLevels))]
public class BindedIslandsViewer : Editor
{
    [SerializeField] private VisualTreeAsset visualTree;
    private VisualElement root;
    private BindedLevels parent;
    private ScrollView unbinded, binded;
    private VisualElement uparent, bparent;

    private void OnEnable()
    {
        parent = (BindedLevels)target;
    }

    public override VisualElement CreateInspectorGUI() {
        root = new VisualElement();
        visualTree.CloneTree(root);
        SetUpElements();
        SetUp();
        return root;
    }

    private void SetUpElements()
    {
        unbinded = root.Q<ScrollView>("unbinded");
        binded = root.Q<ScrollView>("binded");
        uparent = root.Q<VisualElement>("u-parent");
        bparent = root.Q<VisualElement>("b-parent");
    }

    private void SetUp()
    {
        unbinded.Clear();
        binded.Clear();
        uparent.style.visibility = Visibility.Visible;
        bparent.style.visibility = Visibility.Visible;
        
        var sceneCount = SceneManager.sceneCountInBuildSettings;

        for (int i = 0; i < sceneCount; i++)
        {
            var scene = EditorBuildSettings.scenes[i];
            var sceneName = Path.GetFileNameWithoutExtension(scene.path);
            
            if (parent.IsSceneBinded(i))
            {
                var lvlIslands = parent.GetLevelIslands(i);
                if (lvlIslands != null)
                {
                    lvlIslands.Validate();
                    binded.Add(CreateBindedScene(i, sceneName));
                    continue;  
                }
                else
                {
                    parent.UnbindLevel(i);
                }
            }
            
            unbinded.Add(CreateUnbindedScene(i, sceneName));
        }
        
        if (binded.childCount == 0) bparent.style.visibility = Visibility.Hidden;
        if (unbinded.childCount == 0) uparent.style.visibility = Visibility.Hidden;
    }

    VisualElement CreateBindedScene(int i, string sceneName)
    {
        VisualElement e = new VisualElement();
        e.AddToClassList("scene-list-item");
        ObjectField objectField = new ObjectField();
        var levelIsland = parent.GetLevelIslands(i);
        objectField.objectType = typeof(LevelIslands);
        objectField.value = levelIsland;
        objectField.label = sceneName;
        objectField.RegisterCallback<ChangeEvent<LevelIslands>>((v) => {
            objectField.value = v.previousValue;
        });

        var playableField = new Toggle();
        playableField.label = "Is playable";
        playableField.value = levelIsland.Playable;
        playableField.RegisterCallback<ChangeEvent<bool>>((v) => {
            parent.GetLevelIslands(i).Playable = v.newValue;
        });

        var levelNumberField = new IntegerField();
        levelNumberField.label = "Level number";
        levelNumberField.value = levelIsland.LevelNumber;
        levelNumberField.RegisterCallback<ChangeEvent<int>>(e => {
            parent.TryChangeLevelNumber(e.newValue, levelIsland);
        });

        e.Add(objectField);
        e.Add(playableField);
        e.Add(levelNumberField);
        return e;
    }

    VisualElement CreateUnbindedScene(int i, string sceneName)
    {
        Label index = new Label(i.ToString());
        index.AddToClassList("scene-index");
        
        Label name = new Label(sceneName);
        
        VisualElement e = new VisualElement();
        e.AddToClassList("scene-list-item");
        e.Add(index);
        e.Add(name);
        return e;
    } 
}
