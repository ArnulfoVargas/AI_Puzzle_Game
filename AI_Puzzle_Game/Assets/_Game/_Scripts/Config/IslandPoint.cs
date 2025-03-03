using UnityEngine;

public class IslandPoint : ScriptableObject
{
    private string id;
    private Transform from;
    private Transform to;
    
    private Vector3 center;

    public void SetFrom(Transform t)
    {
        from = t;
    }

    public void SetTo(Transform t)
    {
        to = t;
    }
    
    public Transform GetFrom => from;
    public Transform GetTo => to;
    public Vector3 GetCenter => center;
    
    public void RecalculateCenter()
    {
        var maxX = Mathf.Max(from.position.x, to.position.x);
        var maxZ = Mathf.Max(from.position.z, to.position.z);
        var minX = Mathf.Min(from.position.x, to.position.x);
        var minZ = Mathf.Min(from.position.z, to.position.z);
        
        var disX = (maxX - minX) * .5f;
        var disZ = (maxZ - minZ) * .5f;
        
        center = new Vector3(minX + disX, 0, minZ + disZ);
    }
}
