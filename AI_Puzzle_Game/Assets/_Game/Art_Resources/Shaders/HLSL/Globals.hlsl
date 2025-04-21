#ifndef GLOBAL_VARIABLES_DEFINED
#define GLOBAL_VARIABLES_DEFINED

half _Global_Radius;
float4 _Global_BaseColor;
float4 _Global_Color;
float4 _Global_FluorColor;

float4 _Global_Light_MainColor;
float4 _Global_Light_AmbientColor;
float4 _Global_Light_LightColor;
half _Global_Light_SpecularIntensity;
float4 _Global_Light_RimLightShadowColor;
float4 _Global_Light_RimLightLightsColor;
half _Global_Light_FresnelPowerLights;
half _Global_Light_FresnelPower;
half _Global_Light_SpecularPower;

#endif

void GetRadius_half(out float radius) {
    radius = _Global_Radius;
}

void GetBaseColor_float(out float4 baseColor) {
    baseColor = _Global_BaseColor;
}

void GetColor_float(out float4 color) {
    color = _Global_Color;
}

void GetFluorColor_float(out float4 fluor) {
    fluor = _Global_FluorColor;
}

void GetLightVariables_float(
    out float4 MainColor,
    out float4 AmbientColor,
    out float4 LightColor,
    out half SpecularIntensity,
    out float4 RimLightShadow,
    out float4 RimLightLights,
    out half FresnelPowerLights,
    out half FresnelPower,
    out half SpecularPower
    ) {
    MainColor = _Global_Light_MainColor;
    AmbientColor = _Global_Light_AmbientColor;
    LightColor = _Global_Light_LightColor;
    SpecularIntensity = _Global_Light_SpecularIntensity;
    RimLightShadow = _Global_Light_RimLightShadowColor;
    RimLightLights = _Global_Light_RimLightLightsColor;
    FresnelPowerLights = _Global_Light_FresnelPowerLights;
    FresnelPower = _Global_Light_FresnelPower;
    SpecularPower = _Global_Light_SpecularPower;
}