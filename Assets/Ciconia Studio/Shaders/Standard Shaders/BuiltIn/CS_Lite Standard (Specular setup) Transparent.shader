Shader "Custom/URPStandard"
{
    Properties
    {
        // Main Properties
        [MainTexture] _BaseMap("Base Map (RGB)", 2D) = "white" {}
        [MainColor] _BaseColor("Base Color", Color) = (1, 1, 1, 1)
        
        // Surface Inputs
        _Smoothness("Smoothness", Range(0, 1)) = 0.5
        _MetallicGlossMap("Metallic Map", 2D) = "white" {}
        _Metallic("Metallic", Range(0, 1)) = 0
        
        // Normal Map
        [Normal] _BumpMap("Normal Map", 2D) = "bump" {}
        _BumpScale("Normal Scale", Float) = 1.0
        
        // Occlusion
        _OcclusionMap("Occlusion Map", 2D) = "white" {}
        _OcclusionStrength("Occlusion Strength", Range(0, 1)) = 1
        
        // Alpha
        _Transparency("Transparency", Range(0, 1)) = 1
        
        // Texture Tiling and Offset
        [HideInInspector] _MetallicGlossMap_ST("Metallic Map Tiling", Vector) = (1,1,0,0)
        [HideInInspector] _BumpMap_ST("Normal Map Tiling", Vector) = (1,1,0,0)
        
        // Render Settings
        [Enum(UnityEngine.Rendering.CullMode)] _Cull("Cull", Float) = 2
        [Enum(UnityEngine.Rendering.CompareFunction)] _ZTest("ZTest", Float) = 4
        [Enum(Off, 0, On, 1)] _ZWrite("ZWrite", Float) = 1
        
        // Stencil
        [IntRange] _Stencil("Stencil ID", Range(0, 255)) = 0
    }

    SubShader
    {
        Tags 
        { 
            "RenderType" = "Transparent"
            "RenderPipeline" = "UniversalPipeline"
            "Queue" = "Transparent"
            "UniversalMaterialType" = "Lit"
        }

        Pass
        {
            Name "ForwardLit"
            Tags { "LightMode" = "UniversalForward" }

            Blend SrcAlpha OneMinusSrcAlpha
            ZWrite [_ZWrite]
            ZTest [_ZTest]
            Cull [_Cull]

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            
            // Material Keywords
            #pragma shader_feature_local _NORMALMAP
            #pragma shader_feature_local _METALLICSPECGLOSSMAP
            #pragma shader_feature_local _OCCLUSIONMAP
            
            // Universal Pipeline Keywords
            #pragma multi_compile _ _MAIN_LIGHT_SHADOWS _MAIN_LIGHT_SHADOWS_CASCADE
            #pragma multi_compile _ _ADDITIONAL_LIGHTS_VERTEX _ADDITIONAL_LIGHTS
            #pragma multi_compile_fragment _ _ADDITIONAL_LIGHT_SHADOWS
            #pragma multi_compile_fragment _ _SHADOWS_SOFT
            
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"

            struct Attributes
            {
                float4 positionOS : POSITION;
                float3 normalOS : NORMAL;
                float4 tangentOS : TANGENT;
                float2 uv : TEXCOORD0;
            };

            struct Varyings
            {
                float4 positionCS : SV_POSITION;
                float2 uv : TEXCOORD0;
                float2 uvMetallic : TEXCOORD1;
                float2 uvNormal : TEXCOORD2;
                float3 positionWS : TEXCOORD3;
                float3 normalWS : TEXCOORD4;
                float4 tangentWS : TEXCOORD5;
            };

            TEXTURE2D(_BaseMap);
            TEXTURE2D(_BumpMap);
            TEXTURE2D(_MetallicGlossMap);
            TEXTURE2D(_OcclusionMap);
            SAMPLER(sampler_BaseMap);

            CBUFFER_START(UnityPerMaterial)
                float4 _BaseMap_ST;
                float4 _MetallicGlossMap_ST;
                float4 _BumpMap_ST;
                float4 _BaseColor;
                float _Metallic;
                float _Smoothness;
                float _BumpScale;
                float _OcclusionStrength;
                float _Transparency;
            CBUFFER_END

            Varyings vert(Attributes input)
            {
                Varyings output = (Varyings)0;

                VertexPositionInputs vertexInput = GetVertexPositionInputs(input.positionOS.xyz);
                VertexNormalInputs normalInput = GetVertexNormalInputs(input.normalOS, input.tangentOS);

                output.positionCS = vertexInput.positionCS;
                output.uv = TRANSFORM_TEX(input.uv, _BaseMap);
                output.uvMetallic = TRANSFORM_TEX(input.uv, _MetallicGlossMap);
                output.uvNormal = TRANSFORM_TEX(input.uv, _BumpMap);
                output.positionWS = vertexInput.positionWS;
                output.normalWS = normalInput.normalWS;
                output.tangentWS = float4(normalInput.tangentWS, input.tangentOS.w);

                return output;
            }

            float4 frag(Varyings input) : SV_Target
            {
                // Sample textures with their respective UVs
                float4 baseMap = SAMPLE_TEXTURE2D(_BaseMap, sampler_BaseMap, input.uv);
                float4 finalColor = baseMap * _BaseColor;
                
                // Normal mapping with dedicated UV coordinates
                float3 normalTS = UnpackNormalScale(SAMPLE_TEXTURE2D(_BumpMap, sampler_BaseMap, input.uvNormal), _BumpScale);
                float3 normalWS = normalize(TransformTangentToWorld(normalTS,
                    float3x3(input.tangentWS.xyz, cross(input.normalWS, input.tangentWS.xyz) * input.tangentWS.w, input.normalWS)));
                
                // Metallic/Smoothness with dedicated UV coordinates
                float4 metallicGloss = SAMPLE_TEXTURE2D(_MetallicGlossMap, sampler_BaseMap, input.uvMetallic);
                float metallic = metallicGloss.r * _Metallic;
                float smoothness = metallicGloss.a * _Smoothness;
                
                // Occlusion
                float occlusion = SAMPLE_TEXTURE2D(_OcclusionMap, sampler_BaseMap, input.uv).r;
                occlusion = lerp(1, occlusion, _OcclusionStrength);
                
                // Lighting calculation
                float3 positionWS = input.positionWS;
                float4 shadowCoord = TransformWorldToShadowCoord(positionWS);
                Light mainLight = GetMainLight(shadowCoord);
                
                float3 lightDir = mainLight.direction;
                float3 lightColor = mainLight.color;
                float3 viewDir = normalize(GetWorldSpaceViewDir(positionWS));
                float3 halfDir = normalize(lightDir + viewDir);
                
                float NdotL = saturate(dot(normalWS, lightDir));
                float NdotH = saturate(dot(normalWS, halfDir));
                float NdotV = saturate(dot(normalWS, viewDir));
                
                // PBR lighting
                float3 ambient = SampleSH(normalWS) * occlusion;
                float3 diffuse = lightColor * NdotL;
                
                // Specular calculation considering metallic workflow
                float3 specular = pow(NdotH, smoothness * 100) * _Smoothness;
                float3 metallicColor = lerp(float3(0.04, 0.04, 0.04), finalColor.rgb, metallic);
                specular *= metallicColor;
                
                finalColor.rgb = lerp(finalColor.rgb, metallicColor, metallic);
                finalColor.rgb *= (ambient + diffuse) + specular;
                finalColor.a *= _Transparency;
                
                return finalColor;
            }
            ENDHLSL
        }     
    }
}