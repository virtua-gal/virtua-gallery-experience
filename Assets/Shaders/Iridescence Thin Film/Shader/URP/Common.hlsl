#ifndef COMMON
#define COMMON

#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/CommonMaterial.hlsl"
#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/BSDF.hlsl"

struct Attributes
{
	float4 positionOS   : POSITION;
	float3 normalOS     : NORMAL;
	float4 tangentOS    : TANGENT;
	float2 uv           : TEXCOORD0;
#if LIGHTMAP_ON
	float2 uvLightmap   : TEXCOORD1;
#endif
	UNITY_VERTEX_INPUT_INSTANCE_ID
};

struct Varyings
{
	float2 uv          : TEXCOORD0;
#if LIGHTMAP_ON
	float2 uvLightmap  : TEXCOORD1;
#endif
	float3 positionWS  : TEXCOORD2;
	half3  normalWS    : TEXCOORD3;
#ifdef _NORMALMAP
	half4 tangentWS    : TEXCOORD4;
#endif
	float4 positionCS  : SV_POSITION;
};

struct SurfaceData
{
	half3 diffuse;              // diffuse color. should be black for metals.
	half3 reflectance;          // reflectance color at normal indicence. It's monochromatic for dieletrics.
	half3 normalWS;             // normal in world space
	half  ao;                   // ambient occlusion
	half  perceptualRoughness;  // perceptual roughness. roughness = perceptualRoughness * perceptualRoughness;
	half3 emission;             // emissive color
	half  alpha;                // 0 for transparent materials, 1.0 for opaque.
	float3 positionWS;          // world position
};

struct LightingData 
{
	Light light;
	half3 environmentLighting;
	half3 environmentReflections;
	half3 halfDirectionWS;
	half3 viewDirectionWS;
	half3 reflectionDirectionWS;
	half3 normalWS;
	half NdotL;
	half NdotV;
	half NdotH;
	half LdotH;
};

void SurfaceFunction (Varyings IN, out SurfaceData surfaceData);
half4 LightingFunction (SurfaceData surfaceData, LightingData lightingData);

half3 GetPerPixelNormalScaled (TEXTURE2D_PARAM(normalMap, sampler_NormalMap), float2 uv, half3 normal, half4 tangent, half scale)
{
	half3 bitangent = cross(normal, tangent.xyz) * tangent.w;
	half3 normalTS = UnpackNormalScale(SAMPLE_TEXTURE2D(normalMap, sampler_NormalMap, uv), scale);
	return normalize(mul(normalTS, half3x3(tangent.xyz, bitangent, normal)));
}

// defined in latest URP
#if SHADER_LIBRARY_VERSION_MAJOR < 9
// Computes the world space view direction (pointing towards the viewer).
float3 GetWorldSpaceViewDir (float3 positionWS)
{
	if (unity_OrthoParams.w == 0)
	{
		// Perspective
		return _WorldSpaceCameraPos - positionWS;
	}
	else
	{
		// Orthographic
		float4x4 viewMat = GetWorldToViewMatrix();
		return viewMat[2].xyz;
	}
}
#endif

half3 EnvironmentBRDF (half3 f0, half roughness, half NdotV)
{
	half fresnelTerm = Pow4(1.0 - NdotV);
	half3 grazingTerm = saturate((1.0 - roughness) + f0);

	half surfaceReduction = 1.0 / (roughness * roughness + 1.0);
	return lerp(f0, grazingTerm, fresnelTerm) * surfaceReduction;
}

#ifdef BUBBLE
	CBUFFER_START(UnityPerMaterial)
		float4 _NoiseMap_ST, _ColorRampMap_ST, _Direction;
		half _Metallic, _Smoothness, _RimPower, _Distortion, _Blend, _SpeedColor, _SpeedVertex;
	CBUFFER_END
	TEXTURE2D(_NoiseMap); SAMPLER(sampler_NoiseMap);
#endif

Varyings SurfaceVertex (Attributes IN)
{
#ifdef BUBBLE
	float2 uv = TRANSFORM_TEX(IN.uv, _NoiseMap);
	float4 t = SAMPLE_TEXTURE2D_LOD(_NoiseMap, sampler_NoiseMap, uv.xy * 2.0 + (_Time.x * _SpeedVertex), 0.0);
	IN.positionOS.xyz += t.y * _Direction.xyz;
#endif

	VertexPositionInputs vertexInput = GetVertexPositionInputs(IN.positionOS.xyz);
	VertexNormalInputs vertexNormalInput = GetVertexNormalInputs(IN.normalOS, IN.tangentOS);

	Varyings OUT;
	OUT.uv = IN.uv;
#if LIGHTMAP_ON
	OUT.uvLightmap = IN.uvLightmap.xy * unity_LightmapST.xy + unity_LightmapST.zw;
#endif
	OUT.positionWS = vertexInput.positionWS;
	OUT.normalWS = vertexNormalInput.normalWS;
#ifdef _NORMALMAP
	OUT.tangentWS = float4(vertexNormalInput.tangentWS, IN.tangentOS.w * GetOddNegativeScale());
#endif
	OUT.positionCS = vertexInput.positionCS;
	return OUT;
}

half4 SurfaceFragment (Varyings IN) : SV_Target
{
	SurfaceData surfaceData;
	SurfaceFunction(IN, surfaceData);

	half3 viewDirectionWS = normalize(GetWorldSpaceViewDir(IN.positionWS));
	half3 reflectionDirectionWS = reflect(-viewDirectionWS, surfaceData.normalWS);

	// shadowCoord is position in shadow light space
	float4 shadowCoord = TransformWorldToShadowCoord(IN.positionWS);
	Light light = GetMainLight(shadowCoord);

	LightingData lightingData;
	lightingData.light = light;
	lightingData.environmentLighting = SAMPLE_GI(IN.uvLightmap, SampleSH(surfaceData.normalWS), surfaceData.normalWS) * surfaceData.ao;
	lightingData.environmentReflections = GlossyEnvironmentReflection(reflectionDirectionWS, surfaceData.perceptualRoughness, surfaceData.ao);
	lightingData.halfDirectionWS = normalize(light.direction + viewDirectionWS);
	lightingData.viewDirectionWS = viewDirectionWS;
	lightingData.reflectionDirectionWS = reflectionDirectionWS;
	lightingData.normalWS = surfaceData.normalWS;
	lightingData.NdotL = saturate(dot(surfaceData.normalWS, lightingData.light.direction));
	lightingData.NdotV = saturate(dot(surfaceData.normalWS, lightingData.viewDirectionWS)) + HALF_MIN;
	lightingData.NdotH = saturate(dot(surfaceData.normalWS, lightingData.halfDirectionWS));
	lightingData.LdotH = saturate(dot(lightingData.light.direction, lightingData.halfDirectionWS));
	return LightingFunction(surfaceData, lightingData);
}

#endif