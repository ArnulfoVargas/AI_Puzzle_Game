#ifndef GLOBAL_VARIABLES_DEFINED
#define GLOBAL_VARIABLES_DEFINED

half _Global_Radius;
float4 _Global_BaseColor;
float4 _Global_Color;
float4 _Global_FluorColor;

#endif

void GetRadius_half(out float radius) {
    radius = _Global_Radius;
}

void GetBaseColor_half(out float4 baseColor) {
    baseColor = _Global_BaseColor;
}

void GetColor_half(out float4 color) {
    color = _Global_Color;
}

void GetFluorColor_half(out float4 fluor) {
    fluor = _Global_FluorColor;
}