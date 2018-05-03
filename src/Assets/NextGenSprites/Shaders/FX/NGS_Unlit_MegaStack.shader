// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'
// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// NextGenSprites (copyright) 2016 Ruben de la Torre, www.studio-delatorre.com
// Version 1.3.7

Shader "NextGenSprites/FX/MegaStack" {
	Properties{
		//Sprite Props
		[PerRendererData]_MainTex("Sprite", 2D) = "white" {}
		_Color("Sprite Tint", Color) = (1,1,1,1)

		//HSB Tinting
		_HSBC("HSB Tinting", Vector) = (0, 0.5, 0.5, 0.5)

		//Sprite Layers
		_Layer0ScrollingX("Main Sprite X-Axis Scrolling", Range(-1, 1)) = 0
		_Layer0ScrollingY("Main Sprite Y-Axis Scrolling", Range(-1, 1)) = 0

		_Layer1("Layer 1", 2D) = "black" {}
		_Layer1Color("Layer 1 Tint", Color) = (1,1,1,1)
		_Layer1Opacity("Layer 1 Opacity", Range(0, 1)) = 1
		_Layer1ScrollingX("Layer 1 X-Axis Scrolling", Range(-1, 1)) = 0
		_Layer1ScrollingY("Layer 1 Y-Axis Scrolling", Range(-1, 1)) = 0

		_Layer2("Layer 2", 2D) = "black" {}
		_Layer2Color("Layer 2 Tint", Color) = (1,1,1,1)
		_Layer2Opacity("Layer 2 Opacity", Range(0, 1)) = 1
		_Layer2ScrollingX("Layer 2 X-Axis Scrolling", Range(-1, 1)) = 0
		_Layer2ScrollingY("Layer 2 Y-Axis Scrolling", Range(-1, 1)) = 0

		_Layer3("Layer 3", 2D) = "black" {}
		_Layer3Color("Layer 3 Tint", Color) = (1,1,1,1)
		_Layer3Opacity("Layer 3 Opacity", Range(0, 1)) = 1
		_Layer3ScrollingX("Layer 3 X-Axis Scrolling", Range(-1, 1)) = 0
		_Layer3ScrollingY("Layer 3 Y-Axis Scrolling", Range(-1, 1)) = 0

		_Layer4("Layer 4", 2D) = "black" {}
		_Layer4Color("Layer 4 Tint", Color) = (1,1,1,1)
		_Layer4Opacity("Layer 4 Opacity", Range(0, 1)) = 1
		_Layer4ScrollingX("Layer 4 X-Axis Scrolling", Range(-1, 1)) = 0
		_Layer4ScrollingY("Layer 4 Y-Axis Scrolling", Range(-1, 1)) = 0

		_Layer5("Layer 5", 2D) = "black" {}
		_Layer5Color("Layer 5 Tint", Color) = (1,1,1,1)
		_Layer5Opacity("Layer 5 Opacity", Range(0, 1)) = 1
		_Layer5ScrollingX("Layer 5 X-Axis Scrolling", Range(-1, 1)) = 0
		_Layer5ScrollingY("Layer 5 Y-Axis Scrolling", Range(-1, 1)) = 0

		_Layer6("Layer 6", 2D) = "black" {}
		_Layer6Color("Layer 6 Tint", Color) = (1,1,1,1)
		_Layer6Opacity("Layer 6 Opacity", Range(0, 1)) = 1
		_Layer6ScrollingX("Layer 6 X-Axis Scrolling", Range(-1, 1)) = 0
		_Layer6ScrollingY("Layer 6 Y-Axis Scrolling", Range(-1, 1)) = 0

		_Layer7("Layer 7", 2D) = "black" {}
		_Layer7Color("Layer 7 Tint", Color) = (1,1,1,1)
		_Layer7Opacity("Layer 7 Opacity", Range(0, 1)) = 1
		_Layer7ScrollingX("Layer 7 X-Axis Scrolling", Range(-1, 1)) = 0
		_Layer7ScrollingY("Layer 7 Y-Axis Scrolling", Range(-1, 1)) = 0

		_Layer8("Layer 8", 2D) = "black" {}
		_Layer8Color("Layer 8 Tint", Color) = (1,1,1,1)
		_Layer8Opacity("Layer 8 Opacity", Range(0, 1)) = 1
		_Layer8ScrollingX("Layer 8 X-Axis Scrolling", Range(-1, 1)) = 0
		_Layer8ScrollingY("Layer 8 Y-Axis Scrolling", Range(-1, 1)) = 0

		_Layer9("Layer 9", 2D) = "black" {}
		_Layer9Color("Layer 9 Tint", Color) = (1,1,1,1)
		_Layer9Opacity("Layer 9 Opacity", Range(0, 1)) = 1
		_Layer9ScrollingX("Layer 9 X-Axis Scrolling", Range(-1, 1)) = 0
		_Layer9ScrollingY("Layer 9 Y-Axis Scrolling", Range(-1, 1)) = 0

		//Curvature Props
		[Normal]_BumpMap("Curvature Normal", 2D) = "bump" {}
		_CurvatureDepth("Curvature Depth", Range(-1, 1)) = 0.5
		_Specular("Curvature Gloss", Range(0, 1)) = 0.2
		_SpecColor("Curvature Highlight", Color) = (0.5,0.5,0.5,1)

		//Reflection Props
		_ReflectionTex("Reflection Texture", 2D) = "white" {}
		_ReflectionMask("Reflection Mask", 2D) = "white" {}
		_ReflectionStrength("Reflection Strength", Range(0, 1)) = 0
		_ReflectionBlur("Reflection Blur", Range(0, 9)) = 0
		_ReflectionScrollingX("Scrolling Reflection X", Range(0, 5)) = 0.25
		_ReflectionScrollingY("Scrolling Reflection Y", Range(0, 5)) = 0.25

		//Dissolve Props
		_DissolveTex("Dissolve Texture", 2D) = "white" {}
		_DissolveBlend("Dissolve Blending", Range(0, 1)) = 0
		_DissolveBorderWidth("Dissolve Border width", Range(0, 100)) = 10
		_DissolveGlowColor("Dissolve Glow color", Color) = (1,1,1,1)
		_DissolveGlowStrength("Dissolve Glow strength", Range(0, 5)) = 1

		[HideInInspector]_Cutoff("Alpha cutoff", Range(0,1)) = 0.5
		[MaterialToggle]PixelSnap("Pixel snap", float) = 0
	}
		SubShader{
			Tags {
				"IgnoreProjector" = "True"
				"Queue" = "Transparent"
				"RenderType" = "Transparent"
				"CanUseSpriteAtlas" = "True"
				"PreviewType" = "Plane"
			}
			Pass {
				Name "FORWARD"
				Tags {
					"LightMode" = "ForwardBase"
				}
				Blend SrcAlpha OneMinusSrcAlpha
				Cull Off
				ZWrite Off

				CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag
				#define UNITY_PASS_FORWARDBASE
				#pragma shader_feature PIXELSNAP_ON
				#pragma shader_feature HSB_TINT_ON
				#pragma shader_feature SPRITE_SCROLLING_ON
				#pragma multi_compile _ CURVATURE_ON
				#pragma shader_feature DOUBLESIDED_ON
				#pragma multi_compile _ REFLECTION_ON
				#pragma multi_compile _ DISSOLVE_ON
				#include "UnityCG.cginc"
				#include "Assets/NextGenSprites/Shaders/NGS.cginc"
				#pragma fragmentoption ARB_precision_hint_fastest
				#pragma multi_compile_fwdbase_fullshadows
				#pragma target 3.0
				#pragma glsl
				uniform sampler2D _MainTex; uniform float4 _MainTex_ST;
				uniform sampler2D _Layer1; uniform float4 _Layer1_ST;
				uniform sampler2D _Layer2; uniform float4 _Layer2_ST;
				uniform sampler2D _Layer3; uniform float4 _Layer3_ST;
				uniform sampler2D _Layer4; uniform float4 _Layer4_ST;
				uniform sampler2D _Layer5; uniform float4 _Layer5_ST;
				uniform sampler2D _Layer6; uniform float4 _Layer6_ST;
				uniform sampler2D _Layer7; uniform float4 _Layer7_ST;
				uniform sampler2D _Layer8; uniform float4 _Layer8_ST;
				uniform sampler2D _Layer9; uniform float4 _Layer9_ST;
				uniform half _Layer0ScrollingX;
				uniform half _Layer0ScrollingY;
				uniform half _Layer1Opacity;
				uniform half4 _Layer1Color;
				uniform half _Layer1ScrollingX;
				uniform half _Layer1ScrollingY;
				uniform half _Layer2Opacity;
				uniform half4 _Layer2Color;
				uniform half _Layer2ScrollingX;
				uniform half _Layer2ScrollingY;
				uniform half _Layer3Opacity;
				uniform half4 _Layer3Color;
				uniform half _Layer3ScrollingX;
				uniform half _Layer3ScrollingY;
				uniform half _Layer4Opacity;
				uniform half4 _Layer4Color;
				uniform half _Layer4ScrollingX;
				uniform half _Layer4ScrollingY;
				uniform half _Layer5Opacity;
				uniform half4 _Layer5Color;
				uniform half _Layer5ScrollingX;
				uniform half _Layer5ScrollingY;
				uniform half _Layer6Opacity;
				uniform half4 _Layer6Color;
				uniform half _Layer6ScrollingX;
				uniform half _Layer6ScrollingY;
				uniform half _Layer7Opacity;
				uniform half4 _Layer7Color;
				uniform half _Layer7ScrollingX;
				uniform half _Layer7ScrollingY;
				uniform half _Layer8Opacity;
				uniform half4 _Layer8Color;
				uniform half _Layer8ScrollingX;
				uniform half _Layer8ScrollingY;
				uniform half _Layer9Opacity;
				uniform half4 _Layer9Color;
				uniform half _Layer9ScrollingX;
				uniform half _Layer9ScrollingY;
				uniform half4 _Color;
				uniform half4 _HSBC;
				uniform sampler2D _BumpMap; uniform float4 _BumpMap_ST;
				uniform half _CurvatureDepth;
				uniform half _Specular;
				uniform half4 _SpecColor;
				uniform sampler2D _ReflectionMask; uniform float4 _ReflectionMask_ST;
				uniform half _ReflectionStrength;
				uniform half _ReflectionBlur;
				uniform sampler2D _ReflectionTex; uniform float4 _ReflectionTex_ST;
				uniform sampler2D _DissolveTex; uniform float4 _DissolveTex_ST;
				uniform half _DissolveBlend;
				uniform half _ReflectionScrollingX;
				uniform half _ReflectionScrollingY;
				uniform half _DissolveBorderWidth;
				uniform half4 _DissolveGlowColor;
				uniform half _DissolveGlowStrength;
				struct VertexInput {
					float4 vertex : POSITION;
					float3 normal : NORMAL;
					float4 tangent : TANGENT;
					float2 texcoord0 : TEXCOORD0;
					float4 vertexColor : COLOR;
				};
				struct VertexOutput {
					float4 pos : SV_POSITION;
					float2 uv0 : TEXCOORD0;
					float4 posWorld : TEXCOORD1;
					float3 normalDir : TEXCOORD2;
					float3 tangentDir : TEXCOORD3;
					float3 bitangentDir : TEXCOORD4;
					float4 vertexColor : COLOR;
				};
				VertexOutput vert(VertexInput v) {
					VertexOutput o = (VertexOutput)0;
					o.uv0 = v.texcoord0;

					//Scroll UV
					#if SPRITE_SCROLLING_ON
						o.uv0 = ScrollUV(o.uv0, _Layer0ScrollingX, _Layer0ScrollingY);
					#endif

					o.vertexColor = v.vertexColor * float4(_Color.rgb, _Color.a);
					o.normalDir = UnityObjectToWorldNormal(v.normal);
					o.posWorld = mul(unity_ObjectToWorld, v.vertex);
					o.pos = UnityObjectToClipPos(v.vertex);

					//Tangents for Normal mapping
					#if CURVATURE_ON
					o.tangentDir = normalize(mul(unity_ObjectToWorld, float4(v.tangent.xyz, 0.0)).xyz);
					o.bitangentDir = normalize(cross(o.normalDir, o.tangentDir) * v.tangent.w);
					#endif

					#ifdef PIXELSNAP_ON
						o.pos = UnityPixelSnap(o.pos);
					#endif
					return o;
				}
				float4 frag(VertexOutput i) : COLOR {
					#if DOUBLESIDED_ON || REFLECTION_ON || CURVATURE_ON
						i.normalDir = normalize(i.normalDir);
						half3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
						half3 normalDirection = i.normalDir;
					#endif

					//Flip Normals if double sided
					#if DOUBLESIDED_ON
						half nSign = sign(dot(viewDirection, i.normalDir));
						i.normalDir *= nSign;
						normalDirection *= nSign;
					#endif

					//Texture to RGBA
					half _time = _Time.w;

					half2 layer1UV = ScrollUV(i.uv0, _Layer1ScrollingX, _Layer1ScrollingY);
					half2 layer2UV = ScrollUV(i.uv0, _Layer2ScrollingX, _Layer2ScrollingY);
					half2 layer3UV = ScrollUV(i.uv0, _Layer3ScrollingX, _Layer3ScrollingY);
					half2 layer4UV = ScrollUV(i.uv0, _Layer4ScrollingX, _Layer4ScrollingY);
					half2 layer5UV = ScrollUV(i.uv0, _Layer5ScrollingX, _Layer5ScrollingY);
					half2 layer6UV = ScrollUV(i.uv0, _Layer6ScrollingX, _Layer6ScrollingY);
					half2 layer7UV = ScrollUV(i.uv0, _Layer7ScrollingX, _Layer7ScrollingY);
					half2 layer8UV = ScrollUV(i.uv0, _Layer8ScrollingX, _Layer8ScrollingY);
					half2 layer9UV = ScrollUV(i.uv0, _Layer9ScrollingX, _Layer9ScrollingY);

					//Layer 0 / Main Sprite
					half4 _MainTex_var = tex2D(_MainTex, TRANSFORM_TEX(i.uv0, _MainTex));

					//Use MainTex as canvas for the additional layers
					half3 canvas = _MainTex_var.rgb * i.vertexColor.rgb;

					//Layer 1
					half4 layer1Tex = tex2D(_Layer1, TRANSFORM_TEX(layer1UV, _Layer1));
					layer1Tex.rgb *= _Layer1Color.rgb;
					layer1Tex.rgb = lerp(layer1Tex.rgb, canvas, ((layer1Tex.a - 1) * -1));

					canvas = lerp(canvas, layer1Tex.rgb, _Layer1Opacity);

					//Layer 2
					half4 layer2Tex = tex2D(_Layer2, TRANSFORM_TEX(layer2UV, _Layer2));
					layer2Tex.rgb *= _Layer2Color.rgb;
					layer2Tex.rgb = lerp(layer2Tex.rgb, canvas, ((layer2Tex.a - 1) * -1));

					canvas = lerp(canvas, layer2Tex.rgb, _Layer2Opacity);

					//Layer 3
					half4 layer3Tex = tex2D(_Layer3, TRANSFORM_TEX(layer3UV, _Layer3));
					layer3Tex.rgb *= _Layer3Color.rgb;
					layer3Tex.rgb = lerp(layer3Tex.rgb, canvas, ((layer3Tex.a - 1) * -1));

					canvas = lerp(canvas, layer3Tex.rgb, _Layer3Opacity);

					//Layer 4
					half4 layer4Tex = tex2D(_Layer4, TRANSFORM_TEX(layer4UV, _Layer4));
					layer4Tex.rgb *= _Layer4Color.rgb;
					layer4Tex.rgb = lerp(layer4Tex.rgb, canvas, ((layer4Tex.a - 1) * -1));

					canvas = lerp(canvas, layer4Tex.rgb, _Layer4Opacity);

					//Layer 5
					half4 layer5Tex = tex2D(_Layer5, TRANSFORM_TEX(layer5UV, _Layer5));
					layer5Tex.rgb *= _Layer5Color.rgb;
					layer5Tex.rgb = lerp(layer5Tex.rgb, canvas, ((layer5Tex.a - 1) * -1));

					canvas = lerp(canvas, layer5Tex.rgb, _Layer5Opacity);

					//Layer 6
					half4 layer6Tex = tex2D(_Layer6, TRANSFORM_TEX(layer6UV, _Layer6));
					layer6Tex.rgb *= _Layer6Color.rgb;
					layer6Tex.rgb = lerp(layer6Tex.rgb, canvas, ((layer6Tex.a - 1) * -1));

					canvas = lerp(canvas, layer6Tex.rgb, _Layer6Opacity);

					//Layer 7
					half4 layer7Tex = tex2D(_Layer7, TRANSFORM_TEX(layer7UV, _Layer7));
					layer7Tex.rgb *= _Layer7Color.rgb;
					layer7Tex.rgb = lerp(layer7Tex.rgb, canvas, ((layer7Tex.a - 1) * -1));

					canvas = lerp(canvas, layer7Tex.rgb, _Layer7Opacity);

					//Layer 8
					half4 layer8Tex = tex2D(_Layer8, TRANSFORM_TEX(layer8UV, _Layer8));
					layer8Tex.rgb *= _Layer8Color.rgb;
					layer8Tex.rgb = lerp(layer8Tex.rgb, canvas, ((layer8Tex.a - 1) * -1));

					canvas = lerp(canvas, layer8Tex.rgb, _Layer8Opacity);

					//Layer 9
					half4 layer9Tex = tex2D(_Layer9, TRANSFORM_TEX(layer9UV, _Layer9));
					layer9Tex.rgb *= _Layer9Color.rgb;
					layer9Tex.rgb = lerp(layer9Tex.rgb, canvas, ((layer9Tex.a - 1) * -1));

					canvas = lerp(canvas, layer9Tex.rgb, _Layer9Opacity);

					#if HSB_TINT_ON
						canvas = ConvertToHSB(canvas, _HSBC);
					#endif

					//Result
					half3 emissive = canvas;


					//Curvature
					#if CURVATURE_ON
						half3 _BumpMap_var = UnpackNormal(tex2D(_BumpMap, TRANSFORM_TEX(i.uv0, _BumpMap)));
						half3 normalLocal = lerp(half3 (0, 0, 1), _BumpMap_var.rgb, _CurvatureDepth);
						half3x3 tangentTransform = half3x3(i.tangentDir, i.bitangentDir, i.normalDir);
						normalDirection = normalize(mul(normalLocal, tangentTransform));
						half gloss = (_Specular * -1.0 + 1.0);
						half specPow = exp2(gloss * 10.0 + 1.0);
						half3 halfDirection = normalize(viewDirection + 1);
						emissive += pow(max(0, dot(halfDirection, normalDirection)), specPow) * _SpecColor.rgb;

						//Apply perburted Normals
						emissive *= max(0, dot(normalDirection, half3 (0, 0, -1)));
					#endif

					//Reflection
					#if REFLECTION_ON
						half ScrollDamp = 0.05;
						half4 objPos = mul(unity_ObjectToWorld, half4(0,0,0,1));
						half3 viewReflectDirection = reflect(-viewDirection, normalDirection);
						half2 ScrollUV = (half2((_ReflectionScrollingX * objPos.r * ScrollDamp), (ScrollDamp * objPos.g * _ReflectionScrollingY)) + (viewReflectDirection.rg * 0.5 + 0.5));
						half4 _ReflectionTex_var = tex2Dlod(_ReflectionTex, half4(TRANSFORM_TEX(ScrollUV, _ReflectionTex), 0.0, _ReflectionBlur));
						half4 _ReflectionMask_var = tex2D(_ReflectionMask, TRANSFORM_TEX(i.uv0, _ReflectionMask));
						half3 reflectionStrength = (_ReflectionStrength * _ReflectionMask_var.rgb);
						emissive = (lerp(emissive, _ReflectionTex_var.rgb, reflectionStrength));
					#endif

					//Dissolve blending
					#if DISSOLVE_ON
						half4 _DissolveTex_var = tex2D(_DissolveTex, i.uv0);
						emissive += DissolveBlending(_DissolveTex_var.r, _DissolveBorderWidth, _DissolveBlend, _DissolveGlowColor.rgb, _DissolveGlowStrength);
					#endif

						//Final Emission
						half3 finalColor = emissive;
						return fixed4(finalColor,(_MainTex_var.a * i.vertexColor.a));
					}
					ENDCG
				}
			Pass{
				Name "ShadowCaster"
				Tags{
				"LightMode" = "ShadowCaster"
				}
				Cull Off
				Offset 1, 1

				CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag
				#define UNITY_PASS_SHADOWCASTER
				#include "UnityCG.cginc"
				#include "Lighting.cginc"
				#include "Assets/NextGenSprites/Shaders/NGS.cginc"
				#pragma fragmentoption ARB_precision_hint_fastest
				#pragma shader_feature SPRITE_SCROLLING_ON
				#pragma multi_compile_shadowcaster
				#pragma multi_compile _ DISSOLVE_ON
				#pragma target 3.0
				#pragma glsl
				uniform sampler2D _MainTex; uniform float4 _MainTex_ST;
				uniform half4 _Color;
				uniform half _Layer0ScrollingX;
				uniform half _Layer0ScrollingY;
				uniform sampler2D _DissolveTex; uniform float4 _DissolveTex_ST;
				uniform half _DissolveBlend;
				struct VertexInput {
				float4 vertex : POSITION;
				float2 texcoord0 : TEXCOORD0;
				float4 vertexColor : COLOR;
				};
				struct VertexOutput {
					V2F_SHADOW_CASTER;
					float2 uv0 : TEXCOORD1;
					float4 vertexColor : COLOR;
				};
				VertexOutput vert(VertexInput v) {
					VertexOutput o = (VertexOutput)0;
					o.uv0 = v.texcoord0;

					//Scroll UV
					#if SPRITE_SCROLLING_ON
						o.uv0 = ScrollUV(o.uv0, _Layer0ScrollingX, _Layer0ScrollingY);
					#endif

					o.vertexColor = v.vertexColor * float4(_Color.rgb, _Color.a);
					o.pos = UnityObjectToClipPos(v.vertex);
					TRANSFER_SHADOW_CASTER(o)
					return o;
				}
				float4 frag(VertexOutput i) : COLOR{
					#if DISSOLVE_ON
						half4 _DissolveTex_var = tex2D(_DissolveTex, TRANSFORM_TEX(i.uv0, _DissolveTex));
						clip(DissolveClip(_DissolveTex_var.r, _DissolveBlend));
					#else
						half4 _MainTex_var = tex2D(_MainTex, TRANSFORM_TEX(i.uv0, _MainTex));
						clip((_MainTex_var.a * i.vertexColor.a) - 0.5);
					#endif
					SHADOW_CASTER_FRAGMENT(i)
				}
			ENDCG
		}
	}
	FallBack "Sprites/Diffuse"
	CustomEditor "NGSMaterialInspectorGeneric"
}