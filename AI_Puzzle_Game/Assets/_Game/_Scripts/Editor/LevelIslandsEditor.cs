using System;
using System.IO;
using UnityEngine;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using UnityEditor.SceneManagement;

[CustomEditor(typeof(LevelIslands))]
public class LevelIslandsEditor : Editor {
    [SerializeField] private VisualTreeAsset visualTree;
    [SerializeField] private BindedLevels bindedLevels;
    VisualElement root;
    private ObjectField from, to;
    private Button bindCurrentButton, unbindBtn, addBtn, openSceneBtn;
    private VisualElement sceneSelectedContainer, sceneUnselectedContainer, islandList;
    private LevelIslands parent;
    private Label title;

    private void OnEnable()
    {
        parent = (LevelIslands)target;

        EditorSceneManager.activeSceneChanged += (e, s) => {
            UpdateOpenSceneBtn();
        };
    }

    public override VisualElement CreateInspectorGUI() {
        root = new VisualElement();
        visualTree.CloneTree(root);
        SetUpElements();
        SetUpHints();
        UpdateSelected();
        SetUpListeners();
        UpdateOpenSceneBtn();
        return root;
    }

    private void SetUpElements()
    {
        sceneSelectedContainer = root.Q<VisualElement>("SceneSelectedContainer");
        sceneUnselectedContainer = root.Q<VisualElement>("SceneUnselectedContainer");
        bindCurrentButton = root.Q<Button>("Active-btn");
        title = root.Q<Label>("Title");
        unbindBtn = root.Q<Button>("Unbind-btn");
        addBtn = root.Q<Button>("add-btn");
        from = root.Q<ObjectField>("from-t");
        to = root.Q<ObjectField>("to-t");
        islandList = root.Q<VisualElement>("Island-list");
        openSceneBtn = root.Q<Button>("open-scene-btn");
    }

    private void SetUpHints() {
        to.tooltip = "Takes the transform position and creates a point in the world";
        from.tooltip = "Takes the transform position and creates a point in the world";
    }
    
    private void SetUpListeners() {
        bindCurrentButton.RegisterCallback<ClickEvent>((evt) =>
        {
            int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
            BindScene(currentSceneIndex);
        });
        
        unbindBtn.RegisterCallback<ClickEvent>((evt) =>
        {
            bindedLevels.UnbindLevel(parent.sceneIndex);
            parent.Unbind();
            UpdateSelected();
        });
        
        addBtn.RegisterCallback<ClickEvent>((e) => { AddPoint();});

        openSceneBtn.RegisterCallback<ClickEvent>((e) => {
            var scene = EditorBuildSettings.scenes[parent.sceneIndex];
            EditorSceneManager.OpenScene(scene.path);
        });
    }

    private void UpdateOpenSceneBtn() {
        if (EditorSceneManager.GetActiveScene().buildIndex != parent.sceneIndex) {
            openSceneBtn.RemoveFromClassList("h-0");
            openSceneBtn.RemoveFromClassList("hidden");
        } else {

            openSceneBtn.AddToClassList("h-0");
            openSceneBtn.AddToClassList("hidden");
        }
    }

    private void SetUpSceneList() {
        var sceneList = root.Q<VisualElement>("SceneList");
        sceneList.Clear();
        
        var sceneCount = SceneManager.sceneCountInBuildSettings;
        
        for (int i = 0; i < sceneCount; i++)
        {
            if (bindedLevels.IsSceneBinded(i)) continue;
            var scene = EditorBuildSettings.scenes[i];
            var sceneName = Path.GetFileNameWithoutExtension(scene.path);
            
            Label index = new Label(i.ToString());
            index.AddToClassList("scene-index");
            
            Label name = new Label(sceneName);
            
            Button button = new Button(() =>
            {
              BindScene(i);  
            });
            button.text = "Bind scene";

            VisualElement e = new VisualElement();
            e.AddToClassList("scene-list-item");
            e.Add(index);
            e.Add(name);
            e.Add(button);
            
            sceneList.Add(e);
        }

        SetCurrentSceneButtonVisibility();
    }

    private void SetCurrentSceneButtonVisibility()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        if (!bindedLevels.IsSceneBinded(currentSceneIndex))
        {
            bindCurrentButton.style.visibility = sceneUnselectedContainer.style.visibility;
        }
        else
        {
            bindCurrentButton.style.visibility = Visibility.Hidden;
        }
    }

    private void BindScene(int scene)
    {
        bindedLevels.BindLevel(scene, parent);
        parent.sceneIndex = scene;
        UpdateOpenSceneBtn();
        UpdateSelected();
        SaveAssets();
    }

    private void UpdateSelected()
    {
        if (parent.sceneIndex == -1)
        {
            ShowUnselectedContainer();
            SetTitleText("Select scene");
            SetCurrentSceneButtonVisibility();
            SetUpSceneList();
            
            return;
        }
        
        ShowSelectedContainer();
        SetTitleText("Customize scene");
        RenderPoints();
        SetCurrentSceneButtonVisibility();
    }

    private void SetTitleText(string title)
    {
        this.title.text = title;
    }

    private void ShowSelectedContainer()
    {
        sceneSelectedContainer.style.visibility = Visibility.Visible;
        sceneUnselectedContainer.style.visibility = Visibility.Hidden;
        ToggleHeight(sceneSelectedContainer, true);
    }

    private void ShowUnselectedContainer()
    {
        sceneSelectedContainer.style.visibility = Visibility.Hidden;
        sceneUnselectedContainer.style.visibility = Visibility.Visible;
        ToggleHeight(sceneSelectedContainer, false);
    }

    void ToggleHeight(VisualElement el, bool auto)
    {
        el.RemoveFromClassList("h-auto");
        el.RemoveFromClassList("h-0");
        if (auto)
        {
            el.AddToClassList("h-auto");
        }
        else
        {
            el.AddToClassList("h-0");
        }
    }

    private void SaveAssets()
    {
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh(); 
    }

    VisualElement CreatePointItem(int pointId)
    {
        var point = parent.Paths[pointId];
        
        VisualElement itemBase = new VisualElement();
        itemBase.AddToClassList("island-item");
        
        VisualElement infoCont = new VisualElement();
        infoCont.AddToClassList("island-cont-l");
        
        Label label = new Label(pointId.ToString());
        label.AddToClassList("island-cont-label");
        
        VisualElement transformCont = new VisualElement();
        transformCont.AddToClassList("island-transform");

        Vector3Field fromField = new Vector3Field();
        fromField.SetEnabled(false);
        fromField.label = "From";
        fromField.value = point.GetFrom;
        
        Vector3Field toField = new Vector3Field();
        toField.SetEnabled(false);
        toField.label = "To";
        toField.value = point.GetTo;

        ObjectField fromChangeField = new ObjectField("Update From Position");
        fromChangeField.objectType = typeof(Transform);
        fromChangeField.tooltip = "Once you assing a Transform object, automatically will update the point value and set the input into null";

        fromChangeField.RegisterValueChangedCallback((e) => {
            if (e.newValue) {
                point.SetFrom((Transform)e.newValue);
                fromField.value = point.GetFrom;
                point.RecalculateCenter();
            }
            fromChangeField.value = null;
        });

        ObjectField toChangeField = new ObjectField("Update To Position");
        toChangeField.objectType = typeof(Transform);
        toChangeField.tooltip = "Once you assing a Transform object, automatically will update the point value and set the input into null";

        toChangeField.RegisterValueChangedCallback((e) => {
            if (e.newValue) {
                point.SetTo((Transform)e.newValue);
                toField.value = point.GetTo;
                point.RecalculateCenter();
            }
            toChangeField.value = null;
        });       

        transformCont.Add(fromField);
        transformCont.Add(fromChangeField);
        transformCont.Add(toField);
        transformCont.Add(toChangeField);
        
        infoCont.Add(label);
        infoCont.Add(transformCont);
        
        VisualElement btnCont = new VisualElement();
        btnCont.AddToClassList("island-cont-r");
        
        Button recalculateCenter  = new Button();
        recalculateCenter.text = "Recalculate center";
        recalculateCenter.AddToClassList("btn-warning");
        recalculateCenter.RegisterCallback<ClickEvent>((evt) =>
        {
            point.RecalculateCenter();
        });
        
        Button removeBtn = new Button();
        removeBtn.text = "Remove point";
        removeBtn.AddToClassList("btn-error");
        removeBtn.RegisterCallback<ClickEvent>((evt) =>
        {
            AssetDatabase.DeleteAsset(AssetDatabase.GetAssetPath(point.GetInstanceID()));
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            parent.RemovePointAtIndex(pointId);
            RenderPoints();
        });
        
        btnCont.Add(recalculateCenter);
        btnCont.Add(removeBtn);
        
        itemBase.Add(infoCont);
        itemBase.Add(btnCont);
        return itemBase;
    }

    private void AddPoint()
    {
        Transform from = this.from.value as Transform;
        Transform to = this.to.value as Transform;

        if (from == null || to == null)
        {
            Debug.LogError("Properties from or to are null");
            return;
        }

        IslandPoint islandPoint = ScriptableObject.CreateInstance<IslandPoint>();
        islandPoint.SetFrom(from);
        islandPoint.SetTo(to);
        islandPoint.RecalculateCenter();

        this.from.value = null;
        this.to.value = null;

        string path = Path.GetFullPath(parent.getScenePath).Replace(Path.GetFileName(parent.getScenePath), "");
        string fullPath = Path.Join(path,"points");

        if (!Directory.Exists(fullPath))
        {
            Directory.CreateDirectory(fullPath);
        } 
        AssetDatabase.Refresh();
        string assetPath = parent.getScenePath.Replace(parent.GetSceneName() + ".unity", "");
        AssetDatabase.CreateAsset(islandPoint, $"{assetPath}/points/{parent.GetSceneName()}_{System.Guid.NewGuid()}.asset");
        AssetDatabase.SaveAssets();
        
        parent.AddPoint(islandPoint);
        RenderPoints();
    }

    private void RenderPoints()
    {
        islandList.Clear();

        for (int i = 0; i < parent.Paths.Count; i++)
        {
            islandList.Add(CreatePointItem(i));
        }
    }
}