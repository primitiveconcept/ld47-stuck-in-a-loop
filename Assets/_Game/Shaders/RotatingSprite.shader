// Code originaly from Unity's built-in Sprites-Default.shader. Modified by ComputerKim, modifications are commented.

Shader "Sprites/Rotating" // Name changed
{
    Properties
    {
        [PerRendererData] _MainTex("Sprite Texture", 2D) = "white" {}
        _Color("Tint", Color) = (1,1,1,1)
        [MaterialToggle] PixelSnap("Pixel snap", Float) = 0
        [HideInInspector] _RendererColor("RendererColor", Color) = (1,1,1,1)
        [HideInInspector] _Flip("Flip", Vector) = (1,1,1,1)
        [PerRendererData] _AlphaTex("External Alpha", 2D) = "white" {}
        [PerRendererData] _EnableExternalAlpha("Enable External Alpha", Float) = 0

        // Custom properties
        _RotationSpeed("Rotation Speed", Range(0, 10)) = 1
        _MotionBlurAngle("Motion Blur Angle?", Range(0, 0.1)) = 0.01
        _MotionBlurSamples("Motion Blur Samples", Range(0, 100)) = 10
    }

    SubShader
    {
        Tags
        {
            "Queue" = "Transparent"
            "IgnoreProjector" = "True"
            "RenderType" = "Transparent"
            "PreviewType" = "Plane"
            "CanUseSpriteAtlas" = "True"
        }

        Cull Off
        Lighting Off
        ZWrite Off
        Blend One OneMinusSrcAlpha

        Pass
        {
        CGPROGRAM
            #pragma vertex SpriteVert
            #pragma fragment CustomSpriteFrag // Originally SpriteFrag
            #pragma target 2.0
            #pragma multi_compile_instancing
            #pragma multi_compile_local _ PIXELSNAP_ON
            #pragma multi_compile _ ETC1_EXTERNAL_ALPHA
            #include "UnitySprites.cginc"

            // Custom properties
            float _RotationSpeed;
            float _MotionBlurAngle;
            int _MotionBlurSamples;

            float2 RotateZ(float2 xy, float offset) {
                float angle = _RotationSpeed * _Time.y * UNITY_PI - offset * _RotationSpeed, cosa = cos(angle), sina = sin(angle);
                float2x2 m = float2x2(cosa, -sina, sina, cosa);
                return mul(m, xy - float2(0.5, 0.5)) + float2(0.5, 0.5);
            }

            fixed4 CustomSpriteFrag(v2f IN) : SV_Target // Copy of SpriteFrag from UnitySprites.cginc
            {
                // Rotation
                fixed4 c = SampleSpriteTexture(RotateZ(IN.texcoord, 0)) * IN.color;

                // Motion blur
                float weight = 0.999, totalWeight = 1;
                for (int sample = 1; sample <= _MotionBlurSamples; sample++) {
                    totalWeight += weight *= 1;
                    c += SampleSpriteTexture(RotateZ(IN.texcoord, sample * _MotionBlurAngle)) * weight;
                }
                c /= totalWeight;

                c.rgb *= c.a;
                return c;
            }
        ENDCG
        }
    }
}