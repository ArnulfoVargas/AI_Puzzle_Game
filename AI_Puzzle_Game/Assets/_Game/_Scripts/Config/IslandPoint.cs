using System;
using UnityEditor;
using UnityEngine;

[Serializable]
public class IslandPoint : ScriptableObject
{
    private string id;
    [SerializeField] private Vector3 from;
    [SerializeField] private Vector3 to;
    [SerializeField] private Vector3 center;

    public void SetFrom(Transform t)
    {
        from = t.position;
        Save();
    }

    public void SetTo(Transform t)
    {
        to = t.position;
        Save();
    }
    
    public Vector3 GetFrom => from;
    public Vector3 GetTo => to;
    public Vector3 GetCenter => center;
    
    public void RecalculateCenter()
    {
        var maxX = Mathf.Max(from.x, to.x);
        var maxZ = Mathf.Max(from.z, to.z);
        var minX = Mathf.Min(from.x, to.x);
        var minZ = Mathf.Min(from.z, to.z);
        
        var disX = (maxX - minX) * .5f;
        var disZ = (maxZ - minZ) * .5f;
        
        center = new Vector3(minX + disX, 0, minZ + disZ);
        Save();
    }    
    public void Save()
    {
        EditorUtility.SetDirty(this);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }
}
