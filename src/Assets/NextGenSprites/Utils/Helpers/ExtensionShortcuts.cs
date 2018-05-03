// NextGen Sprites (copyright) 2016 Ruben de la Torre, www.studio-delatorre.com

using System;
using UnityEngine;
using UnityEngine.Rendering;

namespace NextGenSprites
{
	#region enums
	/// <summary>
	/// NGS Shader float values.
	/// </summary>
	public enum ShaderFloat
	{
		CurvatureDepth,
		CurvatureGloss,
		ReflectionStrength,
		ReflectionBlur,
		ReflectionScrollSpeedX,
		ReflectionScrollSpeedY,
		EmissionIntensity,
		EmissionLayer1BlendAnimation,
		EmissionLayer1PulseSpeed,
		EmissionLayer2Intensity,
		EmissionLayer2BlendAnimation,
		EmissionLayer2PulseSpeed,
		EmissionLayer3Intensity,
		EmissionLayer3BlendAnimation,
		EmissionLayer3PulseSpeed,
		TransmissionDensity,
		DissolveBlend,
		DissolveBorderWidth,
		DissolveGlowStrength,
		RefractionStrength,
		FlowIntensity,
		FlowSpeed,
		SpriteLayer0ScrollingX,
		SpriteLayer0ScrollingY,
		SpriteLayer1Opacity,
		SpriteLayer1ScrollingX,
		SpriteLayer1ScrollingY,
		SpriteLayer2Opacity,
		SpriteLayer2ScrollingX,
		SpriteLayer2ScrollingY,
		SpriteLayer3Opacity,
		SpriteLayer3ScrollingX,
		SpriteLayer3ScrollingY,
		SpriteAutoScrollingSpeed,
		SpriteBlending
	}

	public enum ShaderVector4
	{
		HSBC = 0
	}

	/// <summary>
	/// NGS Shader coloring.
	/// </summary>
	public enum ShaderColor
	{
		Sprite,
		SpriteLayer1,
		SpriteLayer2,
		SpriteLayer3,
		Curvature,
		Emission,
		EmissionLayer2,
		EmissionLayer3,
		DissolveGlow
	}

	/// <summary>
	/// NGS Shader textures.
	/// </summary>
	public enum ShaderTexture
	{
		Sprite,
		SpriteLayer1,
		SpriteLayer2,
		SpriteLayer3,
		StencilMask,
		Curvature,
		EmissionMask,
		Reflection,
		ReflectionMask,
		Transmission,
		Dissolve,
		Refraction,
		Flow,
		RenderTexture
	}

	/// <summary>
	/// All NGS Shader features.
	/// </summary>
	public enum ShaderFeature
	{
		SpriteMultiLayer,
		SpriteScrolling,
		SpriteStencil,
		Curvature,
		Reflection,
		Emission,
		EmissionPulse,
		Transmission,
		Dissolve,
		DoubleSided,
		PixelSnap,
		AutoScrolling,
		HueShift,
		RenderTexture
	}

	/// <summary>
	/// NGS Shader features that can be toggled at runtime.
	/// </summary>
	public enum ShaderFeatureRuntime
	{
		Curvature,
		Reflection,
		Emission,
		Dissolve,
	}
	#endregion

	public static class ExtensionShortcuts
	{
		#region Strings
		//These are the Shader keyword and properties collections
		readonly private static string[] FloatProperties = {
			"_CurvatureDepth",
			"_Specular",
			"_ReflectionStrength",
			"_ReflectionBlur",
			"_ReflectionScrollingX",
			"_ReflectionScrollingY",
			"_EmissionStrength",
			"_EmissionBlendAnimation1",
			"_EmissionPulseSpeed1",
			"_EmissionStrength2",
			"_EmissionBlendAnimation2",
			"_EmissionPulseSpeed2",
			"_EmissionStrength3",
			"_EmissionBlendAnimation3",
			"_EmissionPulseSpeed3",
			"_TransmissionDensity",
			"_DissolveBlend",
			"_DissolveBorderWidth",
			"_DissolveGlowStrength",
			"_RefractionStrength",
			"_FlowIntensity",
			"_FlowSpeed",
			"_Layer0ScrollingX",
			"_Layer0ScrollingY",
			"_Layer1Opacity",
			"_Layer1ScrollingX",
			"_Layer1ScrollingY",
			"_Layer2Opacity",
			"_Layer2ScrollingX",
			"_Layer2ScrollingY",
			"_Layer3Opacity",
			"_Layer3ScrollingX",
			"_Layer3ScrollingY",
			"_Layer0AutoScrollSpeed",
			"_SpriteBlending"
		};

		readonly private static string[] Vector4Properties = {
			"_HSBC"
		};

		readonly private static float[,] MinMaxFloatProperties = {
			{ -1f, 1f },    //Curvature Depth
			{ 0f, 0.7f },   //Curvature Shininess
			{ 0f, 1f },     //Reflection Strength
			{ 0f, 9f },     //Reflection Blur
			{ 0f, 5f },     //Reflection scrolling X Axis
			{ 0f, 5f },     //Reflection scrolling Y Axis
			{ 0f, 5f },     //Emission0 Strenght
			{ 0f, 1f },     //Emission1 Animation Blending
			{ 0f, 10f },    //Emission1 Pulse Speed
			{ 0f, 5f },     //Emission2 Strenght
			{ 0f, 1f },     //Emission2 Animation Blending
			{ 0f, 10f },    //Emission2 Pulse Speed
			{ 0f, 5f },     //Emission3 Strenght
			{ 0f, 1f },     //Emission3 Animation Blending
			{ 0f, 10f },    //Emission3 Pulse Speed
			{ 0f, 1f },     //Transmission Density
			{ 0f, 1f },     //Dissolve Blending
			{ 0f, 100f },   //Dissolve Border width
			{ 0f, 5f },     //Dissolve Border glow
			{ -1f, 1f },    //Refraction Strength
			{ -1f, 1f },    //Flow Intensity
			{ -10f, 10f },  //Flow Speed
			{ -1, 1 },      //Layer 0 Scrolling X
			{ -1, 1 },      //Layer 0 Scrolling Y
			{ 0, 1 },       //Layer 1 Opacity
			{ -1, 1 },      //Layer 1 Scrolling X
			{ -1, 1 },      //Layer 1 Scrolling Y
			{ 0, 1 },       //Layer 2 Opacity
			{ -1, 1 },      //Layer 2 Scrolling X
			{ -1, 1 },      //Layer 2 Scrolling Y
			{ 0, 1 },       //Layer 3 Opacity
			{ -1, 1 },      //Layer 3 Scrolling X
			{ -1, 1 },      //Layer 3 Scrolling Y
			{ 0, 5 }       //Auto Scrolling Speed
		};

		readonly private static string[] TextureProperties = {
			"_MainTex",
			"_Layer1",
			"_Layer2",
			"_Layer3",
			"_StencilMask",
			"_BumpMap",
			"_Illum",
			"_ReflectionTex",
			"_ReflectionMask",
			"_TransmissionTex",
			"_DissolveTex",
			"_RefractionNormal",
			"_FlowMap",
			"_RenderTexture"
		};

		readonly private static string[] TintProperties = {
			"_Color",
			"_Layer1Color",
			"_Layer2Color",
			"_Layer3Color",
			"_SpecColor",
			"_EmissionTint",
			"_EmissionTint2",
			"_EmissionTint3",
			"_DissolveGlowColor"
		};

		readonly private static string[] ShaderKeywordProperties = {
			"SPRITE_MULTILAYER_ON",
			"SPRITE_SCROLLING_ON",
			"SPRITE_STENCIL_ON",
			"CURVATURE_ON",
			"REFLECTION_ON",
			"EMISSION_ON",
			"EMISSION_PULSE_ON",
			"TRANSMISSION_ON",
			"DISSOLVE_ON",
			"DOUBLESIDED_ON",
			"PIXELSNAP_ON",
			"AUTOSCROLL_ON",
			"HSB_TINT_ON",
			"RENDER_TEXTURE_ON"
		};

		readonly private static string[] ShaderRuntimeKeywordProperties = {
			"CURVATURE_ON",
			"REFLECTION_ON",
			"EMISSION_ON",
			"DISSOLVE_ON",
		};
		#endregion

		/// <summary>
		/// NextGenSprites: 
		/// Returns a string to pass as ShaderProperty for controlling Material properties.
		/// </summary>
		/// <param name="slot">ShaderFloat Enum</param>
		/// <returns>string</returns>
		public static string GetString (this ShaderFloat slot)
		{
			return FloatProperties [(int)slot];
		}

		/// <summary>
		/// NextGenSprites: 
		/// Returns a string to pass as ShaderProperty for controlling Material properties.
		/// </summary>
		/// <param name="slot">ShaderVector4 Enum</param>
		/// <returns>string</returns>
		public static string GetString(this ShaderVector4 slot)
		{
			return Vector4Properties[(int)slot];
		}

		/// <summary>
		/// NextGenSprites: 
		/// Returns a string to pass as ShaderProperty for controlling Material properties.
		/// </summary>
		/// <param name="slot">ShaderTexture Enum</param>
		/// <returns>string</returns>
		public static string GetString (this ShaderTexture slot)
		{
			return TextureProperties [(int)slot];
		}

		/// <summary>
		/// NextGenSprites: 
		/// Returns a string to pass as ShaderProperty for controlling Material properties.
		/// </summary>
		/// <param name="slot">ShaderColor Enum</param>
		/// <returns>string</returns>
		public static string GetString (this ShaderColor slot)
		{
			return TintProperties [(int)slot];
		}

		/// <summary>
		/// NextGenSprites: 
		/// Returns a string to pass as ShaderProperty for controlling Material properties.
		/// </summary>
		/// <param name="slot">ShaderFeature Enum</param>
		/// <returns>string</returns>
		public static string GetString (this ShaderFeature slot)
		{
			return ShaderKeywordProperties [(int)slot];
		}

		/// <summary>
		/// NextGenSprites: 
		/// Returns a string to pass as ShaderProperty for controlling Material properties.
		/// </summary>
		/// <param name="slot">ShaderFeatureRuntime Enum</param>
		/// <returns>string</returns>
		public static string GetString(this ShaderFeatureRuntime slot)
		{
			return ShaderRuntimeKeywordProperties[(int)slot];
		}

		/// <summary>
		/// NextGenSprites: 
		/// Returns the recomended max value for the selected ShaderProperty.
		/// You are free to overshoot these values.
		/// </summary>
		/// <param name="slot">ShaderFloat Enum</param>
		/// <returns>float</returns>
		public static float GetMax(this ShaderFloat slot)
		{
			return MinMaxFloatProperties[(int)slot, 1];
		}

		/// <summary>
		/// NextGenSprites: 
		/// Returns the recomended min value for the selected ShaderProperty.
		/// You are free to overshoot these values.
		/// </summary>
		/// <param name="slot">ShaderFloat Enum</param>
		/// <returns>float</returns>
		public static float GetMin(this ShaderFloat slot)
		{
			return MinMaxFloatProperties[(int) slot, 0];
		}

		/// <summary>
		/// NextGenSprites: 
		/// Toggle shadow casting.
		/// </summary>
		/// <param name="go">Gameobject with the SpriteRenderer</param>
		/// <param name="toggle">toggle casting</param>
		public static void ToggleShadowCasting (this GameObject go, bool toggle)
		{
			var castMode = toggle ? ShadowCastingMode.On : ShadowCastingMode.Off;
			go.GetComponent <SpriteRenderer> ().shadowCastingMode = castMode;
		}

		/// <summary>
		/// NextGenSprites: 
		/// Copy from a Material all attribute and tint values to a Material Property Block.
		/// </summary>
		/// <param name="mBlock">The Material Property Block you target</param>
		/// <param name="mat">Source Material</param>
		public static void CopyToPropertyBlock(this MaterialPropertyBlock mBlock, Material mat)
		{
			//Properties
			var floatNames = Enum.GetNames(typeof(ShaderFloat));
			for (int i = 0; i < floatNames.Length; i++)
			{
				var shaderKey = FloatProperties[i];
				if (mat.HasProperty(shaderKey))
					mBlock.SetFloat(shaderKey, mat.GetFloat(shaderKey));
			}

			//Colors/Tints
			var colorNames = Enum.GetNames(typeof(ShaderColor));
			for (int i = 0; i < colorNames.Length; i++)
			{
				var shaderKey = TintProperties[i];
				if (mat.HasProperty(shaderKey))
					mBlock.SetColor(shaderKey, mat.GetColor(shaderKey));
			}

			//Textures
			var textureNames = Enum.GetNames(typeof(ShaderTexture));
			//Index beginns by 1 since the Sprite Texture is Pre-Rendered by the SpriteRenderer
			for (int i = 1; i < textureNames.Length; i++)
			{
				var shaderKey = TextureProperties[i];
				if (mat.HasProperty(shaderKey))
				{
					var tex = mat.GetTexture(shaderKey);
					if (tex)
						mBlock.SetTexture(shaderKey, tex);
				}
			}
		}
	}
}
