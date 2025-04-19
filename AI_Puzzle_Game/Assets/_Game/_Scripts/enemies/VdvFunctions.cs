using UnityEngine;

public class VdvFunctions : BaseBehaviour {
    [SerializeField] int colorCount;
    [SerializeField] MeshRenderer meshRenderer;
    private int index;
    private float offset;

    protected override void OnStart()
    {
        offset = 1f / colorCount;
    }

    public void OnCollide()
    {
        index ++;
        index %= colorCount;

        meshRenderer.material.SetFloat("_Movement", index * offset);
    } 
}