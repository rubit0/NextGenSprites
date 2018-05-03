// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'
// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// NextGenSprites (copyright) 2016 Ruben de la Torre, www.studio-delatorre.com
// Version 1.3.7

Shader "NextGenSprites/FX/Glass" {
	Properties{
		//Sprite Props
		[PerRendererData]_MainTex("Sprite", 2D) = "white" {}
		_Color("Sprite Tint", Color) = (1,1,1,0.2)
		_SpriteBlending("Sprite Color Blend", Range(0, 1)) = 0
		_EmissionStrength("Emission Strength", Range(0, 5)) = 0

		//Refraction Props
		[Normal]_RefractionNormal("Refraction Normal", 2D) = "bump" {}
		_RefractionStrength("Refraction Strength", Range(-1, 1)) = 0.5

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
			#include "UnityCG.cginc"
			#pragma multi_compile_fwdbase_fullshadows
			#pragma target 3.0
			uniform float4 _LightColor0;
			uniform sampler2D _GrabTexture;
			uniform sampler2D _MainTex; uniform float4 _MainTex_ST;
			uniform float4 _Color;
			uniform float _SpriteBlending;
			uniform float _EmissionStrength;
			uniform sampler2D _RefractionNormal; uniform float4 _RefractionNormal_ST;
			uniform float _RefractionStrength;
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
			VertexOutput vert(VertexInput v) {
				VertexOutput o = (VertexOutput)0;
				o.uv0 = v.texcoord0;
				o.vertexColor = v.vertexColor;
				o.normalDir = UnityObjectToWorldNormal(v.normal);
				o.tangentDir = normalize(mul(unity_ObjectToWorld, float4(v.tangent.xyz, 0.0)).xyz);
				o.bitangentDir = normalize(cross(o.normalDir, o.tangentDir) * v.tangent.w);
				o.posWorld = mul(unity_ObjectToWorld, v.vertex);
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
				i.normalDir = normalize(i.normalDir);
				i.screenPos = half4(i.screenPos.xy / i.screenPos.w, 0, 0);
				i.screenPos.y *= _ProjectionParams.x;
				half2 UVtoTex = (i.uv0*1.0);
				half3 _RefractionNormal_var = UnpackNormal(tex2D(_RefractionNormal,TRANSFORM_TEX(UVtoTex, _RefractionNormal)));
				half2 sceneUVs = half2(1,grabSign)*i.screenPos.xy*0.5 + 0.5 + (_RefractionNormal_var.rgb.rg*(_RefractionStrength*0.2));
				half3x3 tangentTransform = half3x3(i.tangentDir, i.bitangentDir, i.normalDir);
				half3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
				half3 normalLocal = lerp(half3(0,0,1),_RefractionNormal_var.rgb,_RefractionStrength);
				half3 normalDirection = normalize(mul(normalLocal, tangentTransform));
				half3 lightDirection = normalize(_WorldSpaceLightPos0.xyz);
				half3 lightColor = _LightColor0.rgb;
				half3 halfDirection = normalize(viewDirection + lightDirection);

				half attenuation = 1;
				half3 attenColor = attenuation * _LightColor0.xyz;

				// Specular
				half gloss = 0.5;
				half specPow = exp2(gloss * 10.0 + 1.0);
				half NdotL = max(0, dot(normalDirection, lightDirection));
				half3 specularColor = half3(_RefractionNormal_var.r,_RefractionNormal_var.r,_RefractionNormal_var.r);
				half3 directSpecular = (floor(attenuation) * _LightColor0.xyz) * pow(max(0,dot(halfDirection,normalDirection)),specPow)*specularColor;
				half3 specular = directSpecular;

				// Diffuse
				NdotL = max(0.0,dot(normalDirection, lightDirection));
				half3 directDiffuse = max(0.0, NdotL) * attenColor;
				half3 ambience = UNITY_LIGHTMODEL_AMBIENT.rgb;

				half4 _MainTex_var = tex2D(_MainTex, i.uv0.rg);
				_MainTex_var.rgb *= (_Color.rgb * i.vertexColor.rgb);
				half3 diffuse = _MainTex_var * (directDiffuse + ambience);

				// Emission
				half3 emissive = (_EmissionStrength * _MainTex_var);

				// Final diffuse Color
				half3 finalDiffuseColor = diffuse + specular + emissive;
				//Grab Scene Frame to Texture and tint it
				half3 sceneColor = tex2D(_GrabTexture, sceneUVs).rgb;
				sceneColor *= (_Color.rgb * i.vertexColor.rgb);

				half alphaCombined = (_MainTex_var.a * _Color.a * i.vertexColor.a);
				return fixed4(lerp(sceneColor.rgb, finalDiffuseColor, _SpriteBlending), alphaCombined);
			}
			ENDCG
		}
		Pass {
			Name "FORWARD_DELTA"
			Tags {
				"LightMode" = "ForwardAdd"
			}
			Blend One One
			ZWrite Off

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#define UNITY_PASS_FORWARDADD
			#pragma multi_compile _ PIXELSNAP_ON
			#include "UnityCG.cginc"
			#include "AutoLight.cginc"
			#pragma multi_compile_fwdadd_fullshadows
			#pragma target 3.0
			uniform float4 _LightColor0;
			uniform sampler2D _GrabTexture;
			uniform sampler2D _MainTex; uniform float4 _MainTex_ST;
			uniform float4 _Color;
			uniform float _SpriteBlending;
			uniform sampler2D _RefractionNormal; uniform float4 _RefractionNormal_ST;
			uniform float _RefractionStrength;
			uniform float _OpacitySprite;
			uniform float _EmissionStrength;
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
				LIGHTING_COORDS(6,7)
			};
			VertexOutput vert(VertexInput v) {
				VertexOutput o = (VertexOutput)0;
				o.uv0 = v.texcoord0;
				o.vertexColor = v.vertexColor;
				o.normalDir = UnityObjectToWorldNormal(v.normal);
				o.tangentDir = normalize(mul(unity_ObjectToWorld, float4(v.tangent.xyz, 0.0)).xyz);
				o.bitangentDir = normalize(cross(o.normalDir, o.tangentDir) * v.tangent.w);
				o.posWorld = mul(unity_ObjectToWorld, v.vertex);
				float3 lightColor = _LightColor0.rgb;
				o.pos = UnityObjectToClipPos(v.vertex);
				#ifdef PIXELSNAP_ON
					o.pos = UnityPixelSnap(o.pos);
				#endif
				o.screenPos = o.pos;
				TRANSFER_VERTEX_TO_FRAGMENT(o)
				return o;
			}
			float4 frag(VertexOutput i) : COLOR {
				#if UNITY_UV_STARTS_AT_TOP
					half grabSign = -_ProjectionParams.x;
				#else
					half grabSign = _ProjectionParams.x;
				#endif

				i.normalDir = normalize(i.normalDir);
				half3x3 tangentTransform = half3x3(i.tangentDir, i.bitangentDir, i.normalDir);
				half3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
				half3 normalDirection = i.normalDir;
				half3 lightDirection = normalize(lerp(_WorldSpaceLightPos0.xyz, _WorldSpaceLightPos0.xyz - i.posWorld.xyz,_WorldSpaceLightPos0.w));
				half3 lightColor = _LightColor0.rgb;

				// Lighting
				half attenuation = LIGHT_ATTENUATION(i);
				half3 attenColor = attenuation * _LightColor0.xyz;

				// Diffuse
				half NdotL = max(0.0,dot(normalDirection, lightDirection));
				half3 directDiffuse = max(0.0, NdotL) * attenColor;

				half4 _MainTex_var = tex2D(_MainTex,i.uv0.rg);
				_MainTex_var.rgb *= (_Color.rgb * i.vertexColor.rgb);
				half3 diffuse = directDiffuse * _MainTex_var.rgb;
				half alphaCombined = (_MainTex_var.a * _Color.a * i.vertexColor.a);

				// Final Color
				return fixed4(diffuse * _SpriteBlending, alphaCombined);
			}
			ENDCG
		}
	}
	FallBack "Diffuse"
	CustomEditor "NGSMaterialInspectorGeneric"
}