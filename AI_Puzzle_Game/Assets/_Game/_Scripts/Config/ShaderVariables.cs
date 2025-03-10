using UnityEngine;

[CreateAssetMenu(fileName = "ShaderVariables", menuName = "Configs/ShaderVariables", order = 0)]
public class ShaderVariables : ScriptableObject {
    [SerializeField] float radius;   
    [SerializeField] Color baseColor;
    [SerializeField] Color color;
    [SerializeField] Color fluorColor;

    void OnValidate()
    {
        Shader.SetGlobalFloat("_Global_Radius", radius);
        Shader.SetGlobalColor("_Global_BaseColor", baseColor);
        Shader.SetGlobalColor("_Global_Color", color);
        Shader.SetGlobalColor("_Global_FluorColor", fluorColor);
    }
}