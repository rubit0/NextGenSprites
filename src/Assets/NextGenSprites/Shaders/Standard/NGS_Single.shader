// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'
// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// NextGenSprites (copyright) 2016 Ruben de la Torre, www.studio-delatorre.com
// Version 1.3.7

Shader "NextGenSprites/Standard/Single" {
	Properties {
		//Sprite Props
		[PerRendererData]_MainTex ("Sprite", 2D) = "white" {}
		_Color ("Sprite Tint", Color) = (1,1,1,1)

		//HSB Tinting
		_HSBC("HSB Tinting", Vector) = (0, 0.5, 0.5, 0.5)

		//Sprite Layers
		_StencilMask("Stencil Mask", 2D) = "white" {}
		_Layer0ScrollingX("Main Sprite X-Axis Scrolling", Range(-1, 1)) = 0
		_Layer0ScrollingY("Main Sprite Y-Axis Scrolling", Range(-1, 1)) = 0
		_Layer1("Layer 1", 2D) = "black" {}
		_Layer1Opacity("Layer 2 Opacity", Range(0, 1)) = 1
		_Layer1Color("Layer 2 Tint", Color) = (1,1,1,1)
		_Layer1ScrollingX("Layer 2 X-Axis Scrolling", Range(-1, 1)) = 0
		_Layer1ScrollingY("Layer 2 Y-Axis Scrolling", Range(-1, 1)) = 0
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

		//Curvature Props
		[Normal]_BumpMap ("Curvature Normal", 2D) = "bump" {}
		_CurvatureDepth ("Curvature Depth", Range(-1, 1)) = 0.5
		_Specular ("Curvature Gloss", Range(0, 1)) = 0.2
		_SpecColor ("Curvature Highlight", Color) = (0.5,0.5,0.5,1)

		//Reflection Props
		_ReflectionTex("Reflection Texture", 2D) = "white" {}
		_ReflectionBlur("Reflection Blur", Range(0, 9)) = 0
		_ReflectionMask("Reflection Mask", 2D) = "white" {}
		_ReflectionStrength("Reflection Strength", Range(0, 1)) = 0
		_ReflectionScrollingX("Scrolling Reflection X", Range(0, 5)) = 0.25
		_ReflectionScrollingY("Scrolling Reflection Y", Range(0, 5)) = 0.25

		//Emission Props
		_Illum("Emission Mask", 2D) = "white" {}
		_EmissionStrength("Emission Strength", Range(0, 5)) = 0
		_EmissionTint("Emission Tint", Color) = (1,1,1,1)
		_EmissionBlendAnimation1("1# Blend Pulse Animation", Range(0, 1)) = 0
		_EmissionPulseSpeed1("1# Pulse Speed", Range(0, 10)) = 1
		_EmissionStrength2("2# Emission Strength", Range(0, 5)) = 0
		_EmissionTint2("2# Emission Tint", Color) = (1,1,1,1)
		_EmissionBlendAnimation2("2# Blend Pulse Animation", Range(0, 1)) = 0
		_EmissionPulseSpeed2("2# Pulse Speed", Range(0, 10)) = 1
		_EmissionStrength3("3# Emission Strength", Range(0, 5)) = 0
		_EmissionTint3("3# Emission Tint", Color) = (1,1,1,1)
		_EmissionBlendAnimation3("3# Blend Pulse Animation", Range(0, 1)) = 0
		_EmissionPulseSpeed3("3# Pulse Speed", Range(0, 10)) = 1

		//Transmission Props
		_TransmissionDensity ("Transmission Density", Range(0, 1)) = 0
		_TransmissionTex ("Transmission Texture", 2D) = "white" {}

		//Dissolve Props
		_DissolveTex ("Dissolve Texture", 2D) = "white" {}
		_DissolveBlend ("Dissolve Blending", Range(0, 1)) = 0
		_DissolveBorderWidth ("Dissolve Border width", Range(0, 100)) = 10
		_DissolveGlowColor ("Dissolve Glow color", Color) = (1,1,1,1)
		_DissolveGlowStrength ("Dissolve Glow strength", Range(0, 5)) = 1

		//Misc
		[HideInInspector]_Cutoff ("Alpha cutoff", Range(0,1)) = 0.5
		[MaterialToggle] PixelSnap ("Pixel snap", float) = 0
	}
	SubShader {
		Tags {
			"IgnoreProjector"="True"
			"Queue"="Transparent"
			"RenderType"="Transparent"
			"CanUseSpriteAtlas"="True"
			"PreviewType"="Plane"
		}
		Pass {
			Name "FORWARD"
			Tags {
				"LightMode"="ForwardBase"
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
			#pragma shader_feature SPRITE_MULTILAYER_ON
			#pragma shader_feature SPRITE_SCROLLING_ON
			#pragma shader_feature SPRITE_STENCIL_ON
			#pragma multi_compile _ CURVATURE_ON
			#pragma shader_feature DOUBLESIDED_ON
			#pragma multi_compile _ DISSOLVE_ON
			#pragma multi_compile _ TRANSMISSION_ON
			#pragma multi_compile _ REFLECTION_ON
			#pragma multi_compile _ EMISSION_ON
			#pragma shader_feature EMISSION_PULSE_ON
			#include "UnityCG.cginc"
			#include "Assets/NextGenSprites/Shaders/NGS.cginc"
			#pragma fragmentoption ARB_precision_hint_fastest
			#pragma multi_compile_fwdbase_fullshadows
			#pragma target 3.0
			#pragma glsl
			uniform half4 _LightColor0;
			uniform sampler2D _MainTex; uniform float4 _MainTex_ST;
			uniform sampler2D _Layer1; uniform float4 _Layer1_ST;
			uniform sampler2D _Layer2; uniform float4 _Layer2_ST;
			uniform sampler2D _Layer3; uniform float4 _Layer3_ST;
			uniform sampler2D _StencilMask; uniform float4 _StencilMask_ST;
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
			uniform half _ReflectionScrollingX;
			uniform half _ReflectionScrollingY;
			uniform sampler2D _Illum; uniform float4 _Illum_ST;
			uniform half _EmissionStrength;
			uniform half4 _EmissionTint;
			uniform half _EmissionBlendAnimation1;
			uniform half _EmissionPulseSpeed1;
			uniform half _EmissionStrength2;
			uniform half4 _EmissionTint2;
			uniform half _EmissionBlendAnimation2;
			uniform half _EmissionPulseSpeed2;
			uniform half _EmissionStrength3;
			uniform half4 _EmissionTint3;
			uniform half _EmissionBlendAnimation3;
			uniform half _EmissionPulseSpeed3;
			uniform half _TransmissionDensity;
			uniform sampler2D _TransmissionTex; uniform float4 _TransmissionTex_ST;
			uniform sampler2D _DissolveTex; uniform float4 _DissolveTex_ST;
			uniform half _DissolveBlend;
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
			VertexOutput vert (VertexInput v) {
				VertexOutput o = (VertexOutput)0;
				o.uv0 = v.texcoord0;

				//Scroll UV
				#if SPRITE_SCROLLING_ON
					o.uv0 = ScrollUV(o.uv0, _Layer0ScrollingX, _Layer0ScrollingY);
				#endif

				o.normalDir = UnityObjectToWorldNormal(v.normal);

				o.posWorld = mul(unity_ObjectToWorld, v.vertex);
				o.pos = UnityObjectToClipPos(v.vertex);

				//Flip Normals if double sided
				#if DOUBLESIDED_ON
					half3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - o.posWorld);
					half nSign = sign(dot(viewDirection, o.normalDir));
					o.normalDir *= nSign;
				#endif

				//Tangents for Normal mapping
				#if CURVATURE_ON
					o.tangentDir = normalize( mul( unity_ObjectToWorld, float4( v.tangent.xyz, 0.0 ) ).xyz );
					o.bitangentDir = normalize(cross(o.normalDir, o.tangentDir) * v.tangent.w);
				#endif

				//Vertex Lighting
				half3 lightDirection = normalize(_WorldSpaceLightPos0.xyz);
				half NdotL = max(0, dot(o.normalDir, lightDirection));
				float3 diffuse = 1 * _LightColor0.rgb * NdotL;
				float3 surface = diffuse + UNITY_LIGHTMODEL_AMBIENT.rgb;
				o.vertexColor = v.vertexColor * float4(surface * _Color.rgb, _Color.a);
				
				#ifdef PIXELSNAP_ON
					o.pos = UnityPixelSnap(o.pos);
				#endif
				return o;
			}

			float4 frag(VertexOutput i) : COLOR {
				//Define empty variables to reduce conditional operators
				half3 emissive = 0;
				half3 specular = 0;
				half3 normalDirection = normalize(i.normalDir);

				//Texture to RGBA
				#if SPRITE_MULTILAYER_ON
					half _time = _Time.w;

					half2 layer1UV = ScrollUV(i.uv0, _Layer1ScrollingX, _Layer1ScrollingY);
					half2 layer2UV = ScrollUV(i.uv0, _Layer2ScrollingX, _Layer2ScrollingY);
					half2 layer3UV = ScrollUV(i.uv0, _Layer3ScrollingX, _Layer3ScrollingY);

					//Layer 0 / Main Sprite
					half4 _MainTex_var = tex2D(_MainTex, TRANSFORM_TEX(i.uv0, _MainTex));

					//Use MainTex as canvas for the additional layers
					half3 canvas = _MainTex_var.rgb * i.vertexColor.rgb;

					//Layer 1
					half4 layer1Tex = tex2D(_Layer1, TRANSFORM_TEX(layer1UV, _Layer1));
					layer1Tex.rgb = BlendWithCanvas(layer1Tex, canvas, _Layer1Color.rgb);


					//Apply stencil by the Red channel
					#if SPRITE_STENCIL_ON
						half3 stencil = tex2D(_StencilMask, TRANSFORM_TEX(i.uv0, _StencilMask));
						layer1Tex.rgb = lerp(canvas, layer1Tex.rgb, stencil.r);
					#endif

					canvas = lerp(canvas, layer1Tex.rgb, _Layer1Opacity);

					//Layer2
					half4 layer2Tex = tex2D(_Layer2, TRANSFORM_TEX(layer2UV, _Layer2));
					layer2Tex.rgb = BlendWithCanvas(layer2Tex, canvas, _Layer2Color.rgb);


					//Apply stencil by the Green channel
					#if SPRITE_STENCIL_ON
						layer2Tex.rgb = lerp(canvas, layer2Tex.rgb, stencil.g);
					#endif

					canvas = lerp(canvas, layer2Tex.rgb, _Layer2Opacity);

					//Layer3
					half4 layer3Tex = tex2D(_Layer3, TRANSFORM_TEX(layer3UV, _Layer3));
					layer3Tex.rgb = BlendWithCanvas(layer3Tex, canvas, _Layer3Color.rgb);

					//Apply stencil by the Blue channel
					#if SPRITE_STENCIL_ON
						layer3Tex.rgb = lerp(canvas, layer3Tex.rgb, stencil.b);
					#endif

					canvas = lerp(canvas, layer3Tex.rgb, _Layer3Opacity);

					#if HSB_TINT_ON
						canvas = ConvertToHSB(canvas, _HSBC);
					#endif
					
					//Result
					half3 diffuseColor = canvas * i.vertexColor.rgb;
				#else
					//Just tint
					half4 _MainTex_var = tex2D(_MainTex, TRANSFORM_TEX(i.uv0, _MainTex));

					#if HSB_TINT_ON
						_MainTex_var.rgb = ConvertToHSB(_MainTex_var.rgb, _HSBC);
					#endif

					half3 diffuseColor = _MainTex_var.rgb * i.vertexColor.rgb;
				#endif
				
				//Get Viewdirection
				#if CURVATURE_ON ||  REFLECTION_ON || DOUBLESIDED_ON || TRANSMISSION_ON
					half3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
					half3 lightDirection = normalize(_WorldSpaceLightPos0.xyz);
				#endif

				//Curvature
				#if CURVATURE_ON
					half3 _BumpMap_var = UnpackNormal(tex2D (_BumpMap, TRANSFORM_TEX(i.uv0, _BumpMap)));
					half3 normalLocal = lerp(half3 (0, 0, 1), _BumpMap_var.rgb, _CurvatureDepth);
					half3x3 tangentTransform = half3x3(i.tangentDir, i.bitangentDir, i.normalDir);
					normalDirection = normalize(mul (normalLocal, tangentTransform));
					half gloss = (_Specular * -1.0 + 1.0);
					half specPow = exp2(gloss * 10.0 + 1.0);
					half3 halfDirection = normalize (viewDirection + lightDirection);
					//We use 1 because we don't care for attenation in directional lighting
					specular = (1 * _LightColor0.xyz) * pow(max (0, dot( halfDirection, normalDirection)), specPow) * _SpecColor.rgb;
				#endif

				//Apply perburted Normals if Curvature or Transmission on
				#if CURVATURE_ON || TRANSMISSION_ON
					half NdotL = max(0, dot(normalDirection, lightDirection));
					diffuseColor *= max(0.0, NdotL);
				#endif

				//Transmission
				#if TRANSMISSION_ON
					half4 _TransmissionTex_var = tex2D(_TransmissionTex, TRANSFORM_TEX(i.uv0, _TransmissionTex));
					half density = (_TransmissionDensity * -1) + 1;
					half3 backLight = ((-NdotL + 1) * 5) * lerp(diffuseColor, _TransmissionTex_var.rgb, density);
					//Multiply by _LightColor since in backlighting the MainTex is pure black
					diffuseColor += (backLight * _LightColor0.rgb);
				#endif

				//Reflection
				#if REFLECTION_ON
					half3 viewReflectDirection = reflect( -viewDirection, normalDirection );
					half4 objPos = mul ( unity_ObjectToWorld, half4(0, 0, 0, 1) );
					half2 ScrollUV = (half2((_ReflectionScrollingX * objPos.r * 0.05), (0.05 * objPos.g * _ReflectionScrollingY)) + (viewReflectDirection.rg * 0.5 + 0.5));
					half4 _ReflectionTex_var = tex2Dlod(_ReflectionTex, half4(TRANSFORM_TEX(ScrollUV, _ReflectionTex), 0.0, _ReflectionBlur));
					half4 _ReflectionMask_var = tex2D(_ReflectionMask, TRANSFORM_TEX(i.uv0, _ReflectionMask));
					half3 ReflectionMasked = _ReflectionStrength * _ReflectionMask_var.rgb;
					diffuseColor = lerp(diffuseColor, _ReflectionTex_var.rgb, ReflectionMasked);
					#if EMISSION_ON
						half4 _Illum_var = tex2D(_Illum, TRANSFORM_TEX(i.uv0, _Illum));
						emissive = (_EmissionStrength * diffuseColor * _EmissionTint.rgb) * _Illum_var.rgb;
					#endif
				#endif

				//Emission
				#if EMISSION_ON
					#if EMISSION_PULSE_ON
						half3 _IllumMask = tex2D(_Illum, TRANSFORM_TEX(i.uv0, _Illum)).rgb;
						half3 IllumLayer1 = _EmissionTint * (lerp(_IllumMask.r, _IllumMask.r * abs(sin(_Time.y * _EmissionPulseSpeed1)), _EmissionBlendAnimation1)) * _EmissionStrength;
						half3 IllumLayer2 = _EmissionTint2 * (lerp(_IllumMask.g, _IllumMask.g * abs(sin(_Time.y * _EmissionPulseSpeed2)), _EmissionBlendAnimation2)) * _EmissionStrength2;
						half3 IllumLayer3 = _EmissionTint3 * (lerp(_IllumMask.b, _IllumMask.b * abs(sin(_Time.y * _EmissionPulseSpeed3)), _EmissionBlendAnimation3)) * _EmissionStrength3;
						half3 IllumCol = IllumLayer1 + IllumLayer2 + IllumLayer3;
						emissive = (_MainTex_var * IllumCol);
					#else
						half3 _IllumMask = tex2D(_Illum, TRANSFORM_TEX(i.uv0, _Illum)).rgb;
						half3 IllumLayer1 = _EmissionTint * _IllumMask.r * _EmissionStrength;
						half3 IllumLayer2 = _EmissionTint2 * _IllumMask.g * _EmissionStrength2;
						half3 IllumLayer3 = _EmissionTint3 * _IllumMask.b * _EmissionStrength3;
						half3 IllumCol = IllumLayer1+ IllumLayer2 + IllumLayer3;
						emissive = (_MainTex_var * IllumCol);
					#endif
				#endif

				//Dissolve
				#if DISSOLVE_ON
					half4 _DissolveTex_var = tex2D(_DissolveTex, TRANSFORM_TEX(i.uv0, _DissolveTex));
					emissive += DissolveBlending(_DissolveTex_var.r, _DissolveBorderWidth, _DissolveBlend, _DissolveGlowColor.rgb, _DissolveGlowStrength);
				#endif

				//Final Color
				half3 finalColor = diffuseColor + specular + emissive;
				return fixed4(finalColor, (_MainTex_var.a * i.vertexColor.a));
			}
			ENDCG
		}
		Pass {
			Name "ShadowCaster"
			Tags {
				"LightMode"="ShadowCaster"
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
			VertexOutput vert (VertexInput v) {
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
			float4 frag(VertexOutput i) : COLOR {
				#if DISSOLVE_ON
					half4 _DissolveTex_var = tex2D(_DissolveTex, TRANSFORM_TEX(i.uv0, _DissolveTex));
					clip(DissolveClip(_DissolveTex_var.r, _DissolveBlend));
				#else
					half4 _MainTex_var = tex2D(_MainTex, i.uv0);
					clip((_MainTex_var.a * i.vertexColor.a) - 0.5);
				#endif
				SHADOW_CASTER_FRAGMENT(i)
			}
			ENDCG
		}
	}
	FallBack "Sprites/Diffuse"
	CustomEditor "NGSMaterialInspector"
}