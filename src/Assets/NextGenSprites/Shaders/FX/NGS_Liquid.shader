// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'
// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// NextGenSprites (copyright) 2016 Ruben de la Torre, www.studio-delatorre.com
// Version 1.3.7

Shader "NextGenSprites/FX/Liquid" {
	Properties{
		//Sprite Props
		[PerRendererData]_MainTex("Sprite", 2D) = "white" {}
		_Color("Sprite Tint", Color) = (1,1,1,0)
		_SpriteBlending("Sprite Color Blending", Range(0, 1)) = 0
		_RenderTexture("Render Texture", 2D) = "white" {}
		_EmissionStrength("Emission Strength", Range(0, 5)) = 0
		_Layer0ScrollingX("Scrolling Speed by World Position", Range(-1, 1)) = 0
		_Layer0ScrollingY("Scrolling Speed by World Position", Range(-1, 1)) = 0
		_Layer0AutoScrollSpeed("Auto Scrolling by Time", Range(0, 5)) = 0

		//Refraction Props
		[Normal]_RefractionNormal("Refraction Normal", 2D) = "bump" {}
		_RefractionStrength("Refraction Strength", Range(-1, 1)) = 0.5

		//Flow Props
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
		GrabPass{ }
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
			#pragma shader_feature AUTOSCROLL_ON
			#pragma shader_feature RENDER_TEXTURE_ON
			#include "UnityCG.cginc"
			#pragma multi_compile_fwdbase_fullshadows
			#pragma target 3.0
			uniform float4 _LightColor0;
			uniform sampler2D _GrabTexture;
			uniform sampler2D _RenderTexture; uniform sampler2D _RenderTexture_ST;
			uniform float4 _TimeEditor;
			uniform half _Layer0ScrollingX;
			uniform half _Layer0ScrollingY;
			uniform half _Layer0AutoScrollSpeed;
			uniform float4 _Color;
			uniform float _SpriteBlending;
			uniform float _RefractionStrength;
			uniform float _EmissionStrength;
			uniform sampler2D _FlowMap; uniform float4 _FlowMap_ST;
			uniform float _FlowIntensity;
			uniform float _FlowSpeed;
			uniform sampler2D _RefractionNormal; uniform float4 _RefractionNormal_ST;
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
				float4 screenPos : TEXCOORD5;
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
				o.normalDir = normalize(o.normalDir);
				o.tangentDir = normalize(mul(unity_ObjectToWorld, float4(v.tangent.xyz, 0.0)).xyz);
				o.bitangentDir = normalize(cross(o.normalDir, o.tangentDir) * v.tangent.w);
				o.posWorld = mul(unity_ObjectToWorld, v.vertex);

				o.uv0 = ScrolledUV(o.uv0, _Layer0AutoScrollSpeed, _Layer0ScrollingX, _Layer0ScrollingY);

				float3 lightColor = _LightColor0.rgb;
				o.pos = UnityObjectToClipPos(v.vertex);
				#ifdef PIXELSNAP_ON
					o.pos = UnityPixelSnap(o.pos);
				#endif
				o.screenPos = o.pos;
				return o;
			}
			float4 frag(VertexOutput i) : COLOR {
				#if UNITY_UV_STARTS_AT_TOP
					half grabSign = -_ProjectionParams.x;
				#else
					half grabSign = _ProjectionParams.x;
				#endif
				i.screenPos = half4(i.screenPos.xy / i.screenPos.w, 0, 0);
				i.screenPos.y *= _ProjectionParams.x;

				// Flowmap Setup
				half4 _FlowMap_var = tex2D(_FlowMap,TRANSFORM_TEX(i.uv0, _FlowMap));
				half2 FlowUVDest = ((_FlowMap_var.rgb.rg * 1 + -0.5) * (_FlowIntensity * (-1)));
				half4 TimeTick = _Time + _TimeEditor;
				half TimeAppended = (TimeTick.r * _FlowSpeed);
				half FracTime = frac(TimeAppended);

				//Flowmap Lerping
				half2 FirstSprite = (i.uv0 + (FlowUVDest * FracTime));
				half RefractionUVSet = 1;
				half2 NormalUVPlusFirst = (FirstSprite * RefractionUVSet);
				half3 _Normal1 = UnpackNormal(tex2D(_RefractionNormal, TRANSFORM_TEX(NormalUVPlusFirst, _RefractionNormal)));
				half2 TargetSprite = (i.uv0 + (FlowUVDest * frac((TimeAppended + 0.5))));
				half2 NormalUVPlusSecond = (TargetSprite * RefractionUVSet);
				half3 _Normal2 = UnpackNormal(tex2D(_RefractionNormal, TRANSFORM_TEX(NormalUVPlusSecond, _RefractionNormal)));
				half FlowHalf = 0.5;
				half UVLerpController = abs(((FlowHalf - FracTime) / FlowHalf));
				half3 NormalCurrent = lerp(_Normal1.rgb, _Normal2.rgb, UVLerpController);
				half2 sceneUVs = half2(1, grabSign) * i.screenPos.xy * 0.5 + 0.5 + (NormalCurrent.rg * (_RefractionStrength * 0.2));

				#if !RENDER_TEXTURE_ON


				half3x3 tangentTransform = half3x3(i.tangentDir, i.bitangentDir, i.normalDir);
				half3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
				half3 normalLocal = lerp(half3(0, 0, 1), NormalCurrent, _RefractionStrength);
				half3 normalDirection = normalize(mul(normalLocal, tangentTransform));
				half3 lightDirection = normalize(_WorldSpaceLightPos0.xyz);
				half3 lightColor = _LightColor0.rgb;
				half3 halfDirection = normalize(viewDirection + lightDirection);

				// Lighting
				half attenuation = 1;
				half3 attenColor = attenuation * _LightColor0.xyz;

				// Gloss
				half gloss = 0.5;
				half specPow = exp2(gloss * 10 + 1);

				// Specular
				half NdotL = max(0, dot(normalDirection, lightDirection));
				half3 directSpecular = (floor(attenuation) * _LightColor0.xyz) * pow(max(0, dot(halfDirection, normalDirection)), specPow)*NormalCurrent.r;
				half3 specular = directSpecular;

				// Diffuse
				half3 directDiffuse = max(0.0, NdotL) * attenColor;
				half3 indirectDiffuse = UNITY_LIGHTMODEL_AMBIENT.rgb;

				//Flowmap Magic
				half4 _FirstTex = tex2D(_MainTex, TRANSFORM_TEX(FirstSprite, _MainTex));
				half4 _SecondTex = tex2D(_MainTex, TRANSFORM_TEX(TargetSprite, _MainTex));
				half3 DiffuseCurrent = lerp(_FirstTex.rgb, _SecondTex.rgb, UVLerpController);

				//Also Alpha channel with Flowmap applied
				half alphaCombined = (lerp(_FirstTex.a, _SecondTex.a, UVLerpController) * _Color.a * i.vertexColor.a);

				//Tinting the Sprite
				DiffuseCurrent *= (_Color.rgb * i.vertexColor.rgb) * alphaCombined;
				half3 diffuse = (directDiffuse + indirectDiffuse) * DiffuseCurrent;

				// Emissive
				half3 emissive = (_EmissionStrength * DiffuseCurrent);

				// Grab Scene Frame to Texture and tint it
				half4 sceneColor = tex2D(_GrabTexture, sceneUVs);
				sceneColor.rgb *= (_Color.rgb * i.vertexColor.rgb);

				// Final Color
				half3 finalColor = diffuse + specular + emissive;
				return fixed4(lerp(sceneColor.rgb, finalColor, _SpriteBlending), alphaCombined);
				#else
				// Final Color from Render Texture
				half4 sceneColor = tex2D(_RenderTexture, sceneUVs);
				return fixed4(sceneColor.rgb, 1);
				#endif
			}
			ENDCG
		}
	}
	FallBack "Diffuse"
	CustomEditor "NGSMaterialInspectorGeneric"
}