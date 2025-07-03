Shader "Iridescence Thin Film/URP/Bubble" {
	Properties {
		[Header(Surface)]
		_Metallic                             ("Metallic", Range(0, 1)) = 1.0
		_Smoothness                           ("Smoothness", Range(0, 1)) = 0.5
		[Header(Bubble)]
		_NoiseMap      ("Noise", 2D) = "white" {}
		_ColorRampMap  ("ColorRamp",2D) = "white"{}
		_Distortion    ("Distortion", Float) = 6
		_RimPower      ("RimPower", Range(0, 5)) = 1
		_Blend         ("Blend",Range(0, 1)) = 0.5
		_SpeedColor    ("Speed Color", Float) = 0.3
		_SpeedVertex   ("Speed Vertex", Float) = 1
		_Direction     ("Direction", Vector) = (0, 1, 0, 0)
	}
	SubShader {
		Tags { "RenderPipeline" = "UniversalRenderPipeline" "IgnoreProjector" = "True" }

		HLSLINCLUDE
		#include "\Library\PackageCache\com.unity.render-pipelines.universal@7.4.1\ShaderLibrary\Core.hlsl"
		ENDHLSL

		Pass {
			Tags { "LightMode" = "UniversalForward" }
			ZWrite Off
			Blend SrcAlpha OneMinusSrcAlpha

			HLSLPROGRAM
			#pragma vertex SurfaceVertex
			#pragma fragment SurfaceFragment
			#define BUBBLE
			#include "Common.hlsl"

			// -------------------------------------
			// Universal Render Pipeline keywords
			#pragma multi_compile _ _MAIN_LIGHT_SHADOWS
			#pragma multi_compile _ _MAIN_LIGHT_SHADOWS_CASCADE
			#pragma multi_compile _ _SHADOWS_SOFT
			#pragma multi_compile _ DIRLIGHTMAP_COMBINED
			#pragma multi_compile _ LIGHTMAP_ON

			TEXTURE2D(_ColorRampMap); SAMPLER(sampler_ColorRampMap);

			void SurfaceFunction (Varyings IN, out SurfaceData surfaceData)
			{
				surfaceData = (SurfaceData)0;
				float2 uv = TRANSFORM_TEX(IN.uv, _NoiseMap);

				half nis = SAMPLE_TEXTURE2D(_NoiseMap, sampler_NoiseMap, uv).r;

				half3 viewWS = normalize(GetWorldSpaceViewDir(IN.positionWS));
				float rim = dot(viewWS, IN.normalWS);
				float rim2 = saturate(1.0 - pow(abs(rim), _RimPower));

				float2 uv2 = TRANSFORM_TEX(rim.xx, _ColorRampMap) * nis * _Distortion + _Time.xx * _SpeedColor;
				half4 c = SAMPLE_TEXTURE2D(_ColorRampMap, sampler_ColorRampMap, uv2);

				surfaceData.diffuse = 0.0;
				surfaceData.reflectance = ComputeFresnel0(0.0, _Metallic, 0.5 * 0.5 * 0.16);
				surfaceData.ao = 1.0;
				surfaceData.perceptualRoughness = 1.0 - _Smoothness;
				surfaceData.normalWS = normalize(IN.normalWS);
				surfaceData.emission = c.rgb;
				surfaceData.alpha = lerp(0.0, rim2, _Blend) * rim2;
				surfaceData.positionWS = IN.positionWS;
			}
			////////////////////////////////////////////////////////////////////////////////////////////////////
			half4 LightingFunction (SurfaceData surfaceData, LightingData lightingData)
			{
				return half4(surfaceData.diffuse + surfaceData.emission, surfaceData.alpha);
			}
			ENDHLSL
		}
		UsePass "Universal Render Pipeline/Lit/ShadowCaster"
		UsePass "Universal Render Pipeline/Lit/DepthOnly"
		UsePass "Universal Render Pipeline/Lit/Meta"
	}
	FallBack Off
}