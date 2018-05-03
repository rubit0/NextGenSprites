// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'
// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// NextGenSprites (copyright) 2016 Ruben de la Torre, www.studio-delatorre.com
// Version 1.3.4

Shader "NextGenSprites/FX/Lava" {
	Properties{
		// Sprite Props
		[PerRendererData]_MainTex("Sprite", 2D) = "black" {}
		_Color("Sprite Tint", Color) = (1,1,1,1)

		//HSB Tinting
		_HSBC("HSB Tinting", Vector) = (0, 0.5, 0.5, 0.5)

		_EmissionStrength("Emission Strength", Range(0, 5)) = 0
		_Layer0ScrollingX("Scrolling Speed by World Position", Range(-1, 1)) = 0
		_Layer0ScrollingY("Scrolling Speed by World Position", Range(-1, 1)) = 0
		_Layer0AutoScrollSpeed("Auto Scrolling by Time", Range(0, 5)) = 0

		// Curvature Props
		[Normal]_BumpMap("Normal Map", 2D) = "bump" {}

		// Flow Props
		_FlowMap("Flow Map", 2D) = "white" {}
		_FlowIntensity("Flow Intensity", Range(-1, 1)) = 0.5
		_FlowSpeed("Flow Speed", Range(-10, 10)) = 0.5
		[HideInInspector]_Cutoff("Alpha cutoff", Range(0,1)) = 0.5
		[MaterialToggle] PixelSnap("Pixel snap", Float) = 0
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
			#pragma multi_compile _ PIXELSNAP_ON
			#pragma shader_feature HSB_TINT_ON
			#pragma shader_feature AUTOSCROLL_ON
			#include "UnityCG.cginc"
			#include "Assets/NextGenSprites/Shaders/NGS.cginc"
			#pragma multi_compile_fwdbase_fullshadows
			#pragma target 3.0
			uniform float4 _LightColor0;
			uniform float4 _TimeEditor;
			uniform half _Layer0ScrollingX;
			uniform half _Layer0ScrollingY;
			uniform half _Layer0AutoScrollSpeed;
			uniform float4 _Color;
			uniform half4 _HSBC;
			uniform float _EmissionStrength;
			uniform sampler2D _FlowMap; uniform float4 _FlowMap_ST;
			uniform float _FlowIntensity;
			uniform float _FlowSpeed;
			uniform sampler2D _BumpMap; uniform float4 _BumpMap_ST;
			uniform sampler2D _MainTex; uniform float4 _MainTex_ST;
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

			half2 ScrolledUV(half2 uv, half scrollSpeed, half scrollX, half scrollY)
			{
				half4 objPos = mul(unity_ObjectToWorld, half4(0, 0, 0, 1));
				#if AUTOSCROLL_ON
					half time = _Time.y * 0.5;
					return half2(uv.r + (scrollX * (scrollSpeed * time)), uv.g + (scrollY * (scrollSpeed * time)));
				#else
					return half2(uv.r + (objPos.x * (scrollX * 2)), uv.g + (objPos.y * (scrollY * 2)));
				#endif
			}

			VertexOutput vert(VertexInput v) {
				VertexOutput o = (VertexOutput)0;
				o.uv0 = v.texcoord0;
				o.vertexColor = v.vertexColor;
				o.normalDir = UnityObjectToWorldNormal(v.normal);
				o.tangentDir = normalize(mul(unity_ObjectToWorld, float4(v.tangent.xyz, 0.0)).xyz);
				o.bitangentDir = normalize(cross(o.normalDir, o.tangentDir) * v.tangent.w);
				o.posWorld = mul(unity_ObjectToWorld, v.vertex);

				o.uv0 = ScrolledUV(o.uv0, _Layer0AutoScrollSpeed, _Layer0ScrollingX, _Layer0ScrollingY);

				float3 lightColor = _LightColor0.rgb;
				o.pos = UnityObjectToClipPos(v.vertex);
				#ifdef PIXELSNAP_ON
					o.pos = UnityPixelSnap(o.pos);
				#endif
				return o;
			}
			float4 frag(VertexOutput i) : COLOR {
				i.normalDir = normalize(i.normalDir);
				half3x3 tangentTransform = half3x3(i.tangentDir, i.bitangentDir, i.normalDir);
				half3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);

				// Flowmap Setup
				half4 _FlowMap_var = tex2D(_FlowMap,TRANSFORM_TEX(i.uv0, _FlowMap));
				half2 FlowFinal = ((_FlowMap_var.rgb.rg*1.0 + -0.5)*(_FlowIntensity*(-1.0)));
				half4 TimeTick = _Time + _TimeEditor;
				half TimeAppended = (TimeTick.r*_FlowSpeed);
				half FracTime = frac(TimeAppended);

				// Flowmap Lerping
				half2 FirstSprite = (i.uv0 + (FlowFinal*FracTime));
				half3 _BumpMap1 = UnpackNormal(tex2D(_BumpMap,TRANSFORM_TEX(FirstSprite, _BumpMap)));
				half2 TargetSprite = (i.uv0 + (FlowFinal*frac((TimeAppended + 0.5))));
				half3 _BumpMap2 = UnpackNormal(tex2D(_BumpMap,TRANSFORM_TEX(TargetSprite, _BumpMap)));
				half FlowHalf = 0.5;
				half UVLerpController = abs(((FlowHalf - FracTime) / FlowHalf));
				half3 normalLocal = lerp(_BumpMap1.rgb,_BumpMap2.rgb,UVLerpController);
				half3 normalDirection = normalize(mul(normalLocal, tangentTransform));
				half3 lightDirection = normalize(_WorldSpaceLightPos0.xyz);
				half3 lightColor = _LightColor0.rgb;

				// Lighting
				half attenuation = 1;
				half3 attenColor = attenuation * _LightColor0.xyz;

				// Diffuse
				half NdotL = max(0.0,dot(normalDirection, lightDirection));
				half3 directDiffuse = max(0.0, NdotL) * attenColor;
				half3 indirectDiffuse = half3(0,0,0);
				indirectDiffuse += UNITY_LIGHTMODEL_AMBIENT.rgb;
				half4 _FirstTex = tex2D(_MainTex,TRANSFORM_TEX(FirstSprite, _MainTex));
				half4 _SecondTex = tex2D(_MainTex,TRANSFORM_TEX(TargetSprite, _MainTex));
				half Colorcombined = (_Color.a*i.vertexColor.a*lerp(_FirstTex.a,_SecondTex.a,UVLerpController));
				half3 diffuseColor = ((lerp(_FirstTex.rgb,_SecondTex.rgb,UVLerpController)*_Color.rgb*i.vertexColor.rgb)*Colorcombined);
				half3 diffuse = (directDiffuse + indirectDiffuse) * diffuseColor;

				#if HSB_TINT_ON
					diffuse = ConvertToHSB(diffuse, _HSBC);
				#endif

					// Emission
					half3 emissive = (_EmissionStrength*diffuseColor);

					// Final Color
					half3 finalColor = diffuse + emissive;
					return fixed4(finalColor,Colorcombined);
				}
				ENDCG
			}
	}
	FallBack "Diffuse"
	CustomEditor "NGSMaterialInspectorGeneric"
}