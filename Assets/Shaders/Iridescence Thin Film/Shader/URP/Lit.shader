Shader "Iridescence Thin Film/URP/Lit" {
	Properties {
		[Header(Surface)]
		_BaseColor                            ("Base Color", Color) = (1, 1, 1, 1)
		_BaseMap                              ("Base Map", 2D) = "white" {}
		_Metallic                             ("Metallic", Range(0, 1)) = 1.0
		_Smoothness                           ("Smoothness", Range(0, 1)) = 0.5
		_AmbientOcclusion                     ("AmbientOcclusion", Range(0, 1)) = 1.0
		[NoScaleOffset]_AmbientOcclusionMap   ("AmbientOcclusionMap", 2D) = "white" {}
		[Toggle(_NORMALMAP)] _EnableNormalMap ("Enable Normal Map", Float) = 0.0
		[Normal][NoScaleOffset]_NormalMap     ("Normal Map", 2D) = "bump" {}
		_NormalMapScale                       ("Normal Map Scale", Float) = 1.0
		[HDR]_Emission                        ("Emission Color", Color) = (0, 0, 0, 1)
		[Header(Thinfilm)]
		_Thinfilm_Strength   ("Film Strength", Float) = 0.5
		_Thinfilm_Color_Freq ("Film Frequency", Float) = 10
	}
	SubShader {
		Tags { "RenderPipeline" = "UniversalRenderPipeline" "IgnoreProjector" = "True" }

		HLSLINCLUDE
		#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

		CBUFFER_START(UnityPerMaterial)
		float4 _BaseMap_ST;
		half4 _BaseColor, _Emission;
		half _Metallic, _AmbientOcclusion, _Smoothness;
		half _NormalMapScale, _Thinfilm_Color_Freq, _Thinfilm_Strength;
		CBUFFER_END
		ENDHLSL

		Pass {
			Tags { "LightMode" = "UniversalForward" }

			HLSLPROGRAM
			#pragma vertex SurfaceVertex
			#pragma fragment SurfaceFragment
			#include "Common.hlsl"

			// -------------------------------------
			// Universal Render Pipeline keywords
			#pragma multi_compile _ _MAIN_LIGHT_SHADOWS
			#pragma multi_compile _ _MAIN_LIGHT_SHADOWS_CASCADE
			#pragma multi_compile _ _SHADOWS_SOFT
			#pragma multi_compile _ DIRLIGHTMAP_COMBINED
			#pragma multi_compile _ LIGHTMAP_ON

			#pragma shader_feature _NORMALMAP

			TEXTURE2D(_BaseMap); SAMPLER(sampler_BaseMap);
			TEXTURE2D(_NormalMap); SAMPLER(sampler_NormalMap);
			TEXTURE2D(_AmbientOcclusionMap);

			void SurfaceFunction (Varyings IN, out SurfaceData surfaceData)
			{
				surfaceData = (SurfaceData)0;
				float2 uv = TRANSFORM_TEX(IN.uv, _BaseMap);

				half3 baseColor = SAMPLE_TEXTURE2D(_BaseMap, sampler_BaseMap, uv).rgb * _BaseColor.rgb;
				surfaceData.diffuse = ComputeDiffuseColor(baseColor.rgb, _Metallic);
				surfaceData.reflectance = ComputeFresnel0(baseColor.rgb, _Metallic, 0.5 * 0.5 * 0.16);
				surfaceData.ao = SAMPLE_TEXTURE2D(_AmbientOcclusionMap, sampler_BaseMap, uv).g * _AmbientOcclusion;
				surfaceData.perceptualRoughness = 1.0 - _Smoothness;
#ifdef _NORMALMAP
				surfaceData.normalWS = GetPerPixelNormalScaled(TEXTURE2D_ARGS(_NormalMap, sampler_NormalMap), uv, IN.normalWS, IN.tangentWS, _NormalMapScale);
#else
				surfaceData.normalWS = normalize(IN.normalWS);
#endif
				surfaceData.emission = _Emission.rgb;
				surfaceData.alpha = 1.0;
				surfaceData.positionWS = IN.positionWS;
			}
			////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
			float ITF_range (float v, float oMin, float oMax, float iMin, float iMax)
			{
				return iMin + ((v - oMin)/(oMax - oMin)) * (iMax - iMin);
			}
			float ITF_noise (float3 pos)
			{
				float mult = 1;
				float oset = 45;
				return	sin(pos.x * mult * 2 + 12 + oset) + cos(pos.z * mult + 21 + oset) *
						sin(pos.y * mult * 2 + 23 + oset) + cos(pos.y * mult + 32 + oset) *
						sin(pos.z * mult * 2 + 34 + oset) + cos(pos.x * mult + 43 + oset);
			}
			float3 ITF_color (float orient, float3 P)
			{
				float freq = _Thinfilm_Color_Freq;
				float oset = 25;
				float noiseMult = 1;

				float3 c;
				c.r = abs(cos(orient * freq + ITF_noise(P) * noiseMult + 1 + oset));
				c.g = abs(cos(orient * freq + ITF_noise(P) * noiseMult + 2 + oset));
				c.b = abs(cos(orient * freq + ITF_noise(P) * noiseMult + 3 + oset));
				return c;
			}
			////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
			half4 LightingFunction (SurfaceData surfaceData, LightingData lightingData)
			{
				half3 iridColor = ITF_color(lightingData.NdotV, surfaceData.positionWS) * ITF_range(pow(abs(1.0 - lightingData.NdotV), 1.0 / 0.75), 0.0, 1.0, 0.1, 1.0);
				iridColor *= _Thinfilm_Strength;

				half perceptualRoughness = max(surfaceData.perceptualRoughness, 0.089);
				half roughness = PerceptualRoughnessToRoughness(perceptualRoughness);

				half3 environmentReflection = lightingData.environmentReflections;
				environmentReflection *= EnvironmentBRDF(surfaceData.reflectance, roughness, lightingData.NdotV);

				half3 environmentLighting = lightingData.environmentLighting * surfaceData.diffuse;
				half3 diffuse = surfaceData.diffuse * Lambert();

				half DV = DV_SmithJointGGX(lightingData.NdotH, lightingData.NdotL, lightingData.NdotV, roughness);

				half3 F = F_Schlick(surfaceData.reflectance, lightingData.LdotH);
				half3 specular = DV * F;
				half3 finalColor = (diffuse + specular * iridColor) * lightingData.light.color * lightingData.NdotL;
				finalColor += environmentReflection + environmentLighting + surfaceData.emission;
				return half4(finalColor, surfaceData.alpha);
			}
			ENDHLSL
		}
		UsePass "Universal Render Pipeline/Lit/ShadowCaster"
		UsePass "Universal Render Pipeline/Lit/DepthOnly"
		UsePass "Universal Render Pipeline/Lit/Meta"
	}
	FallBack Off
}