using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "BaseLightningVariables", menuName = "BaseLightningVariables", order = 0)]
public class BaseLightningVariables : ScriptableObject {
    [SerializeField] Color MainColor, AmbientColor, LightColor;
    [SerializeField, Range(0,1)] float SpecularIntensity, SpecularPower;
    [SerializeField] Color RimLightShadow, RimLightLights;
    [SerializeField] float FresnelPowerLights, FresnelPower;

    void OnValidate()
    {
        Valid();
    }

    void Valid() {
        Shader.SetGlobalColor("_Global_Light_MainColor", MainColor);
        Shader.SetGlobalColor("_Global_Light_AmbientColor", AmbientColor);
        Shader.SetGlobalColor("_Global_Light_LightColor", LightColor);
        Shader.SetGlobalColor("_Global_Light_RimLightShadowColor", RimLightShadow);
        Shader.SetGlobalColor("_Global_Light_RimLightLightsColor", RimLightLights);
        
        Shader.SetGlobalFloat("_Global_Light_SpecularIntensity", SpecularIntensity);
        Shader.SetGlobalFloat("_Global_Light_FresnelPowerLights", FresnelPowerLights);
        Shader.SetGlobalFloat("_Global_Light_FresnelPower", FresnelPower);
        Shader.SetGlobalFloat("_Global_Light_SpecularPower", SpecularPower);
        Save();
    }
    
    public void Save()
    {
        #if UNITY_EDITOR
        EditorUtility.SetDirty(this);
        AssetDatabase.Refresh();
        #endif
    }
}