Shader "Custom/TMP_SDF-HDRP UNLIT SWITCH"
{
    Properties
    {
        [HDR]_FaceColor("Face Color", Color) = (1, 1, 1, 1)
        _IsoPerimeter("Outline Width", Vector, 4) = (0, 0, 0, 0)
        [HDR]_OutlineColor1("Outline Color 1", Color) = (0, 1, 1, 1)
        [HDR]_OutlineColor2("Outline Color 2", Color) = (0.009433985, 0.02534519, 1, 1)
        [HDR]_OutlineColor3("Outline Color 3", Color) = (0, 0, 0, 1)
        _OutlineOffset1("Outline Offset 1", Vector, 2) = (0, 0, 0, 0)
        _OutlineOffset2("Outline Offset 2", Vector, 2) = (0, 0, 0, 0)
        _OutlineOffset3("Outline Offset 3", Vector, 2) = (0, 0, 0, 0)
        [ToggleUI]_OutlineMode("OutlineMode", Float) = 0
        _Softness("Softness", Vector, 4) = (0, 0, 0, 0)
        [NoScaleOffset]_FaceTex("Face Texture", 2D) = "white" {}
        _FaceUVSpeed("_FaceUVSpeed", Vector, 2) = (0, 0, 0, 0)
        _FaceTex_ST("_FaceTex_ST", Vector, 4) = (1, 1, 0, 0)
        [NoScaleOffset]_OutlineTex("Outline Texture", 2D) = "white" {}
        _OutlineTex_ST("_OutlineTex_ST", Vector, 4) = (1, 1, 0, 0)
        _OutlineUVSpeed("_OutlineUVSpeed", Vector, 2) = (0, 0, 0, 0)
        _UnderlayColor("_UnderlayColor", Color) = (0, 0, 0, 1)
        _UnderlayOffset("Underlay Offset", Vector, 2) = (0, 0, 0, 0)
        _UnderlayDilate("Underlay Dilate", Float) = 0
        _UnderlaySoftness("_UnderlaySoftness", Float) = 0
        [ToggleUI]_BevelType("Bevel Type", Float) = 0
        _BevelAmount("Bevel Amount", Range(0, 1)) = 0.25
        _BevelOffset("Bevel Offset", Range(-0.5, 0.5)) = 0
        _BevelWidth("Bevel Width", Range(0, 0.5)) = 0.5
        _BevelRoundness("Bevel Roundness", Range(0, 1)) = 0
        _BevelClamp("Bevel Clamp", Range(0, 1)) = 0
        [HDR]_SpecularColor("Light Color", Color) = (1, 1, 1, 1)
        _LightAngle("Light Angle", Range(0, 6.28)) = 0
        _SpecularPower("Specular Power", Range(0, 4)) = 1
        _Reflectivity("Reflectivity Power", Range(5, 15)) = 5
        _Diffuse("Diffuse Shadow", Range(0, 1)) = 0.3
        _Ambient("Ambient Shadow", Range(0, 1)) = 0.3
        [NoScaleOffset]_MainTex("_MainTex", 2D) = "white" {}
        _GradientScale("_GradientScale", Float) = 10
        _ScaleRatioA("_ScaleRatioA", Float) = 0
        _LightColor("LightColor", Color) = (1, 1, 1, 1)
        _DarkColor("DarkColor", Color) = (0, 0, 0, 1)
    }
    CustomEditor "UnityEditor.ShaderGraph.GenericShaderGraphMaterialGUI"
    FallBack "Hidden/Shader Graph/FallbackError"
}