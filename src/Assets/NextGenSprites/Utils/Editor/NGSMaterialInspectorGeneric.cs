// NextGenSprites (copyright) 2016 Ruben de la Torre, www.studio-delatorre.com

using System;
using System.Linq;
using NextGenSprites;
using UnityEditor;
using UnityEngine;

public class NGSMaterialInspectorGeneric : ShaderGUI
{
	private bool _fxCheckDone;
	private string _headerURL = "http://wiki.next-gen-sprites.com/doku.php?id=shaders:feature:sprite";
	private Material _targetMaterial;

	#region Shader MatBlock
	//Sprite
	private MaterialProperty _mainSprite;
	private MaterialProperty _mainSpriteTint;

	//Curvatur
	private MaterialProperty _curvatureMap;
	private MaterialProperty _curvatureDepth;
	private MaterialProperty _curvatureHighlight;
	private MaterialProperty _curvatureGloss;

	//Reflection
	private MaterialProperty _reflectionScreenMap;
	private MaterialProperty _reflectionMask;
	private MaterialProperty _reflectionStrength;
	private MaterialProperty _reflectionBlur;

	//Render Texture
	private MaterialProperty _renderTexture;

	//Glass and Liquid Sprite Blending
	private MaterialProperty _spriteBlending;
	
	//Reflection Scrolling
	private MaterialProperty _reflectionScrollingX;
	private MaterialProperty _reflectionScrollingY;

	//Emission
	private MaterialProperty _emissionStrength;
	private MaterialProperty _emissionTint;
	private MaterialProperty _emissionMask;

	//Dissolve
	private MaterialProperty _dissolveMap;
	private MaterialProperty _dissolveBlend;
	private MaterialProperty _dissolveBorderWidth;
	private MaterialProperty _dissolveGlowColor;
	private MaterialProperty _dissolveGlowStrength;

	//Refraction
	private MaterialProperty _refractionNormal;
	private MaterialProperty _refractionStrength;

	//Flow
	private MaterialProperty _flowMap;
	private MaterialProperty _flowIntensity;
	private MaterialProperty _flowSpeed;
	private MaterialProperty _FlowScrollX;
	private MaterialProperty _FlowScrollY;
	private MaterialProperty _FlowScrollAuto;

	//MegaStack
	private MaterialProperty _megaStack0ScrollingX;
	private MaterialProperty _megaStack0ScrollingY;

	private MaterialProperty _megaStack1Tex;
	private MaterialProperty _megaStack1Color;
	private MaterialProperty _megaStack1Opacity;
	private MaterialProperty _megaStack1ScrollingX;
	private MaterialProperty _megaStack1ScrollingY;

	private MaterialProperty _megaStack2Tex;
	private MaterialProperty _megaStack2Color;
	private MaterialProperty _megaStack2Opacity;
	private MaterialProperty _megaStack2ScrollingX;
	private MaterialProperty _megaStack2ScrollingY;

	private MaterialProperty _megaStack3Tex;
	private MaterialProperty _megaStack3Color;
	private MaterialProperty _megaStack3Opacity;
	private MaterialProperty _megaStack3ScrollingX;
	private MaterialProperty _megaStack3ScrollingY;

	private MaterialProperty _megaStack4Tex;
	private MaterialProperty _megaStack4Color;
	private MaterialProperty _megaStack4Opacity;
	private MaterialProperty _megaStack4ScrollingX;
	private MaterialProperty _megaStack4ScrollingY;

	private MaterialProperty _megaStack5Tex;
	private MaterialProperty _megaStack5Color;
	private MaterialProperty _megaStack5Opacity;
	private MaterialProperty _megaStack5ScrollingX;
	private MaterialProperty _megaStack5ScrollingY;

	private MaterialProperty _megaStack6Tex;
	private MaterialProperty _megaStack6Color;
	private MaterialProperty _megaStack6Opacity;
	private MaterialProperty _megaStack6ScrollingX;
	private MaterialProperty _megaStack6ScrollingY;

	private MaterialProperty _megaStack7Tex;
	private MaterialProperty _megaStack7Color;
	private MaterialProperty _megaStack7Opacity;
	private MaterialProperty _megaStack7ScrollingX;
	private MaterialProperty _megaStack7ScrollingY;

	private MaterialProperty _megaStack8Tex;
	private MaterialProperty _megaStack8Color;
	private MaterialProperty _megaStack8Opacity;
	private MaterialProperty _megaStack8ScrollingX;
	private MaterialProperty _megaStack8ScrollingY;

	private MaterialProperty _megaStack9Tex;
	private MaterialProperty _megaStack9Color;
	private MaterialProperty _megaStack9Opacity;
	private MaterialProperty _megaStack9ScrollingX;
	private MaterialProperty _megaStack9ScrollingY;
	#endregion

	#region Private Inspector properties
	private bool _hasSpriteTint;
	private bool _hasCurvature;
	private bool _hasCurvatureMap;
	private bool _hasReflection;
	private bool _hasEmission;
	private bool _hasEmissionTint;
	private bool _hasDissolve;
	private bool _hasRefraction;
	private bool _hasFlow;
	private bool _hasMegaStack;
	private bool _hasHueShift;
	private bool _hasRenderTexture;
	private string[] _shaderCompileKeywords;
	#endregion

	#region Inspector design assets
	//Get all Icons
	readonly Texture2D _btnHelp = (Texture2D)AssetDatabase.LoadAssetAtPath ("Assets/NextGenSprites/Utils/Editor/Icons/btn_help.png", typeof(Texture2D));
	readonly Texture2D _bannerTop = (Texture2D)AssetDatabase.LoadAssetAtPath("Assets/NextGenSprites/Utils/Editor/Icons/generic_banner.png", typeof(Texture2D));
	#endregion

	#region Shader Methods

	//Find all MatBlock from the Shader/Material
	private void FetchProperties (MaterialProperty[] props, Material target)
	{
		_shaderCompileKeywords = target.shaderKeywords;

		_mainSprite = FindProperty (ShaderTexture.Sprite.GetString (), props);

		//Even though all Materials have a MainTex, UI elements get only tinted by Vertex Colors
		if (_hasSpriteTint)
			_mainSpriteTint = FindProperty(ShaderColor.Sprite.GetString(), props);

		if (_hasRenderTexture)
			_renderTexture = FindProperty(ShaderTexture.RenderTexture.GetString(), props);

		if (_hasCurvature)
		{
			_curvatureDepth = FindProperty(ShaderFloat.CurvatureDepth.GetString(), props);
			_curvatureHighlight = FindProperty(ShaderColor.Curvature.GetString(), props);
			_curvatureGloss = FindProperty(ShaderFloat.CurvatureGloss.GetString(), props);
			if(_hasCurvatureMap)
				_curvatureMap = FindProperty(ShaderTexture.Curvature.GetString(), props);
		}

		if (_hasEmission)
		{
			_emissionStrength = FindProperty (ShaderFloat.EmissionIntensity.GetString (), props);
			if (_hasEmissionTint)
			{
				_emissionTint = FindProperty(ShaderColor.Emission.GetString(), props);
				_emissionMask = FindProperty(ShaderTexture.EmissionMask.GetString(), props);
			}
		}

		if (_hasReflection)
		{
			_reflectionScreenMap = FindProperty(ShaderTexture.Reflection.GetString(), props);
			_reflectionMask = FindProperty(ShaderTexture.ReflectionMask.GetString(), props);
			_reflectionStrength = FindProperty(ShaderFloat.ReflectionStrength.GetString(), props);
			_reflectionBlur = FindProperty(ShaderFloat.ReflectionBlur.GetString(), props);
			_reflectionScrollingX = FindProperty(ShaderFloat.ReflectionScrollSpeedX.GetString(), props);
			_reflectionScrollingY = FindProperty(ShaderFloat.ReflectionScrollSpeedY.GetString(), props);
		}

		if (_hasDissolve)
		{
			_dissolveMap = FindProperty(ShaderTexture.Dissolve.GetString(), props);
			_dissolveBlend = FindProperty(ShaderFloat.DissolveBlend.GetString(), props);
			_dissolveBorderWidth = FindProperty(ShaderFloat.DissolveBorderWidth.GetString(), props);
			_dissolveGlowColor = FindProperty(ShaderColor.DissolveGlow.GetString(), props);
			_dissolveGlowStrength = FindProperty(ShaderFloat.DissolveGlowStrength.GetString(), props);
		}

		if (_hasRefraction)
		{
			_refractionNormal = FindProperty (ShaderTexture.Refraction.GetString (), props);
			_refractionStrength = FindProperty (ShaderFloat.RefractionStrength.GetString (), props);
			_spriteBlending = FindProperty (ShaderFloat.SpriteBlending.GetString(), props);
		}

		if (_hasFlow)
		{
			_flowMap = FindProperty (ShaderTexture.Flow.GetString (), props);
			_flowIntensity = FindProperty (ShaderFloat.FlowIntensity.GetString (), props);
			_flowSpeed = FindProperty (ShaderFloat.FlowSpeed.GetString (), props);
			_FlowScrollX = FindProperty(ShaderFloat.SpriteLayer0ScrollingX.GetString() , props);
			_FlowScrollY = FindProperty(ShaderFloat.SpriteLayer0ScrollingY.GetString(), props);
			_FlowScrollAuto = FindProperty(ShaderFloat.SpriteAutoScrollingSpeed.GetString(), props);
			
			//Only happens with the Lava shader
			if (_hasCurvatureMap)
				_curvatureMap = FindProperty(ShaderTexture.Curvature.GetString(), props);
		}

		if(_hasMegaStack)
		{
			_megaStack0ScrollingX = FindProperty("_Layer0ScrollingX", props);
			_megaStack0ScrollingY = FindProperty("_Layer0ScrollingY", props);

			_megaStack1Tex = FindProperty("_Layer1", props);
			_megaStack1Color = FindProperty("_Layer1Color", props);
			_megaStack1Opacity = FindProperty("_Layer1Opacity", props);
			_megaStack1ScrollingX = FindProperty("_Layer1ScrollingX", props);
			_megaStack1ScrollingY = FindProperty("_Layer1ScrollingY", props);

			_megaStack2Tex = FindProperty("_Layer2", props);
			_megaStack2Color = FindProperty("_Layer2Color", props);
			_megaStack2Opacity = FindProperty("_Layer2Opacity", props);
			_megaStack2ScrollingX = FindProperty("_Layer2ScrollingX", props);
			_megaStack2ScrollingY = FindProperty("_Layer2ScrollingY", props);

			_megaStack3Tex = FindProperty("_Layer3", props);
			_megaStack3Color = FindProperty("_Layer3Color", props);
			_megaStack3Opacity = FindProperty("_Layer3Opacity", props);
			_megaStack3ScrollingX = FindProperty("_Layer3ScrollingX", props);
			_megaStack3ScrollingY = FindProperty("_Layer3ScrollingY", props);

			_megaStack4Tex = FindProperty("_Layer4", props);
			_megaStack4Color = FindProperty("_Layer4Color", props);
			_megaStack4Opacity = FindProperty("_Layer4Opacity", props);
			_megaStack4ScrollingX = FindProperty("_Layer4ScrollingX", props);
			_megaStack4ScrollingY = FindProperty("_Layer4ScrollingY", props);

			_megaStack5Tex = FindProperty("_Layer5", props);
			_megaStack5Color = FindProperty("_Layer5Color", props);
			_megaStack5Opacity = FindProperty("_Layer5Opacity", props);
			_megaStack5ScrollingX = FindProperty("_Layer5ScrollingX", props);
			_megaStack5ScrollingY = FindProperty("_Layer5ScrollingY", props);

			_megaStack6Tex = FindProperty("_Layer6", props);
			_megaStack6Color = FindProperty("_Layer6Color", props);
			_megaStack6Opacity = FindProperty("_Layer6Opacity", props);
			_megaStack6ScrollingX = FindProperty("_Layer6ScrollingX", props);
			_megaStack6ScrollingY = FindProperty("_Layer6ScrollingY", props);

			_megaStack7Tex = FindProperty("_Layer7", props);
			_megaStack7Color = FindProperty("_Layer7Color", props);
			_megaStack7Opacity = FindProperty("_Layer7Opacity", props);
			_megaStack7ScrollingX = FindProperty("_Layer7ScrollingX", props);
			_megaStack7ScrollingY = FindProperty("_Layer7ScrollingY", props);

			_megaStack8Tex = FindProperty("_Layer8", props);
			_megaStack8Color = FindProperty("_Layer8Color", props);
			_megaStack8Opacity = FindProperty("_Layer8Opacity", props);
			_megaStack8ScrollingX = FindProperty("_Layer8ScrollingX", props);
			_megaStack8ScrollingY = FindProperty("_Layer8ScrollingY", props);

			_megaStack9Tex = FindProperty("_Layer9", props);
			_megaStack9Color = FindProperty("_Layer9Color", props);
			_megaStack9Opacity = FindProperty("_Layer9Opacity", props);
			_megaStack9ScrollingX = FindProperty("_Layer9ScrollingX", props);
			_megaStack9ScrollingY = FindProperty("_Layer9ScrollingY", props);
		}
	}

	private void TestShader (Material target)
	{
		_hasSpriteTint = (target.HasProperty(ShaderColor.Sprite.GetString()));
		_hasCurvature = (target.HasProperty(ShaderFloat.CurvatureDepth.GetString()));
		_hasCurvatureMap = (target.HasProperty(ShaderTexture.Curvature.GetString()));
		_hasReflection = (target.HasProperty(ShaderFloat.ReflectionStrength.GetString()));
		_hasEmission = (target.HasProperty(ShaderFloat.EmissionIntensity.GetString()));
		_hasEmissionTint = (target.HasProperty(ShaderColor.Emission.GetString()));
		_hasDissolve = (target.HasProperty(ShaderFloat.DissolveBlend.GetString()));
		_hasRefraction = (target.HasProperty (ShaderFloat.RefractionStrength.GetString ()));
		_hasFlow = (target.HasProperty (ShaderFloat.FlowSpeed.GetString ()));
		_hasMegaStack = (target.HasProperty(ShaderFloat.SpriteLayer1Opacity.GetString()));
		_hasHueShift = (target.HasProperty(ShaderVector4.HSBC.GetString()));
		_hasRenderTexture = (target.HasProperty(ShaderTexture.RenderTexture.GetString()));

		//Change the header URL if this is an FX Shader to give the correct wiki page
		if (!_fxCheckDone)
		{
			var shaderName = target.shader.name.Split('/');

			if (shaderName[shaderName.Length - 2] == "FX")
			{
				if (shaderName[shaderName.Length - 1] == "Lava")
					_headerURL = "http://wiki.next-gen-sprites.com/doku.php?id=shaders:fx:lava";
				if (shaderName[shaderName.Length - 1] == "Glass")
					_headerURL = "http://wiki.next-gen-sprites.com/doku.php?id=shaders:fx:glass";
				if (shaderName[shaderName.Length - 1] == "Liquid")
					_headerURL = "http://wiki.next-gen-sprites.com/doku.php?id=shaders:fx:liquid";
			}

			_fxCheckDone = true;
		}
	}
	#endregion

	public override void OnGUI (MaterialEditor materialEditor, MaterialProperty[] properties)
	{
		//Get Material reference
		_targetMaterial = materialEditor.target as Material;
		TestShader (_targetMaterial);
		FetchProperties (properties, _targetMaterial);

		//Draw our Panels
		DrawMainPanel (materialEditor, _targetMaterial);
	}

	#region GUI Methods

	private void DrawMainPanel (MaterialEditor materialEditor, Material targetMaterial)
	{
		GUILayout.Space (7f);

		GUILayout.BeginHorizontal ();
		GUILayout.Box(_bannerTop, EditorStyles.label, GUILayout.MaxWidth(224f), GUILayout.MaxHeight(32f));
		GUILayout.FlexibleSpace ();
		ButtonOpenUrl(_btnHelp, _headerURL);
		GUILayout.EndHorizontal();
		DrawWideBox(1f);

		SpriteControl(materialEditor);

		if (_hasHueShift)
			HueShiftControl(targetMaterial);

		if(_hasMegaStack)
		{
			MegaStackControl(materialEditor);
		}

		if(_hasRenderTexture)
		{
			ToggleShader(targetMaterial, "Render Texture", ShaderFeature.RenderTexture.GetString());
			RenderTextureControl(materialEditor);
		}

		if (_hasCurvature)
		{
			ToggleShader(targetMaterial, "Curvature", ShaderFeature.Curvature.GetString());
			CurvatureControl(materialEditor);
		}

		if (_hasReflection)
		{
			var noToggle = ShaderNameCheck(targetMaterial.shader.name, "UI/Unlit");
			if (!noToggle)
				ToggleShader(targetMaterial, "Reflection", ShaderFeature.Reflection.GetString());
			ReflectionControl(materialEditor, noToggle);
		}

		if (_hasRefraction)
		{
			var noToggle = ShaderNameCheck(targetMaterial.shader.name, "/FX/");
			RefractionControl(materialEditor, noToggle);
		}

		if (_hasEmission && !targetMaterial.IsKeywordEnabled(ShaderFeature.RenderTexture.GetString()))
		{
			var noToggle = ShaderNameCheck(targetMaterial.shader.name, "/FX/");
			if (!noToggle)
				ToggleShader(targetMaterial, "Emission", ShaderFeature.Emission.GetString());
			EmissionControl(materialEditor, noToggle);
		}

		if (_hasDissolve)
		{
			ToggleShader(targetMaterial, "Dissolve", ShaderFeature.Dissolve.GetString());
			DissolveControl(materialEditor);
		}

		if (_hasFlow)
		{
			FlowControl(materialEditor);
		}


		//check Shader Keywords for Pixel Snapping
		ToggleShader(targetMaterial, "Pixel Snapping", "PIXELSNAP_ON", disableGroup: false);

		GUILayout.Space(15f);

		EditorGUILayout.BeginHorizontal();
		GUILayout.FlexibleSpace();
		EditorGUILayout.HelpBox("Version 1.3.9", MessageType.None);
		EditorGUILayout.HelpBox("Copyright 2016 Ruben de la Torre", MessageType.None);
		EditorGUILayout.EndHorizontal();
	}

	private void SpriteControl(MaterialEditor target)
	{
		target.TextureProperty(_mainSprite, "Sprite", scaleOffset: false);
		if (_hasSpriteTint)
			target.ColorProperty(_mainSpriteTint, "Tint");

		GUILayout.Space(10f);
		DrawWideBox(1f);
	}

	private void RenderTextureControl(MaterialEditor target)
	{
		target.TextureProperty(_renderTexture, "Render Texture");

		GUILayout.Space(10f);
		DrawWideBox(1f);
		if (!_shaderCompileKeywords.Contains(ShaderFeature.RenderTexture.GetString()))
			EditorGUI.EndDisabledGroup();
	}

	private void CurvatureControl(MaterialEditor target)
	{
		Header("Curvature", "http://wiki.next-gen-sprites.com/doku.php?id=shaders:fx:lava#curvature");

		target.TextureProperty(_curvatureMap, "Curvature Map");
		target.RangeProperty(_curvatureDepth, "Depth");
		target.ColorProperty(_curvatureHighlight, "Highlight Color");
		target.RangeProperty(_curvatureGloss, "Gloss");

		GUILayout.Space(10f);
		DrawWideBox(1f);
		if (!_shaderCompileKeywords.Contains(ShaderFeature.Curvature.GetString()))
			EditorGUI.EndDisabledGroup();
	}


	private void ReflectionControl(MaterialEditor target, bool noDisablegroup)
	{
		Header("Reflection", "http://wiki.next-gen-sprites.com/doku.php?id=shaders:feature:reflection#reflection_texture");

		target.TextureProperty(_reflectionScreenMap, "Reflection Texture");
		target.RangeProperty(_reflectionStrength, "Strength");
		target.RangeProperty(_reflectionBlur, "Blur");

		GUILayout.Space(5f);

		target.TextureProperty(_reflectionMask, "Mask", scaleOffset: false);
		target.RangeProperty(_reflectionScrollingX, "X-Axis scrolling");
		target.RangeProperty(_reflectionScrollingY, "Y-Axis scrolling");

		GUILayout.Space(10f);
		DrawWideBox(1f);

		//Turn off the Disable group?
		if (noDisablegroup) return;
		if (!_shaderCompileKeywords.Contains(ShaderFeature.Reflection.GetString()))
			EditorGUI.EndDisabledGroup();
	}

	private void RefractionControl(MaterialEditor target, bool noDisablegroup)
	{
		Header("Refraction", "http://wiki.next-gen-sprites.com/doku.php?id=shaders:fx:liquid#refraction_map");

		target.TextureProperty(_refractionNormal, "Refraction Map");
		target.RangeProperty(_refractionStrength, "Strength");
		target.RangeProperty(_spriteBlending, "Blending");

		GUILayout.Space(10f);
		DrawWideBox(1f);
	}

	private void EmissionControl(MaterialEditor target, bool noDisablegroup)
	{
		Header("Emission", "http://wiki.next-gen-sprites.com/doku.php?id=shaders:feature:emission");

		target.RangeProperty(_emissionStrength, "Strength");
		if (_hasEmissionTint)
		{
			target.ColorProperty(_emissionTint, "Tint");
			GUILayout.Space(5f);
			target.TextureProperty(_emissionMask, "Mask");
		}


		GUILayout.Space(10f);
		DrawWideBox(1f);
		//Turn off the Disable group?
		if (noDisablegroup) return;
		if (!_shaderCompileKeywords.Contains(ShaderFeature.Emission.GetString()))
			EditorGUI.EndDisabledGroup();
	}

	private void DissolveControl(MaterialEditor target)
	{
		Header("Dissolve", "http://wiki.next-gen-sprites.com/doku.php?id=shaders:feature:dissolve");

		target.TextureProperty(_dissolveMap, "Dissolve Pattern", scaleOffset: false);
		target.RangeProperty(_dissolveBlend, "Blend");

		GUILayout.Space(5f);

		target.RangeProperty(_dissolveBorderWidth, "Border Width");
		target.ColorProperty(_dissolveGlowColor, "Border Glow Tint");
		target.RangeProperty(_dissolveGlowStrength, "Border Glow width");

		GUILayout.Space(10f);
		DrawWideBox(1f);
		if (!_shaderCompileKeywords.Contains(ShaderFeature.Dissolve.GetString()))
			EditorGUI.EndDisabledGroup();
	}

	private void FlowControl(MaterialEditor target)
	{
		Header("Flow", "http://wiki.next-gen-sprites.com/doku.php?id=shaders:fx:liquid#flow_map");

		if (_hasCurvatureMap)
			target.TextureProperty(_curvatureMap, "Curvature Map", scaleOffset: false);

		target.TextureProperty(_flowMap, "Flow Map", scaleOffset: false);
		target.RangeProperty(_flowIntensity, "Intensity");
		target.RangeProperty(_flowSpeed, "Speed");
		GUILayout.Space(10f);

		ToggleShader(_targetMaterial, "Auto scrolling", ShaderFeature.AutoScrolling.GetString());
		target.RangeProperty(_FlowScrollAuto, "Auto scroll Speed");
		EditorGUI.EndDisabledGroup();

		GUILayout.Space(5f);
		var modeLbl = (_targetMaterial.IsKeywordEnabled(ShaderFeature.AutoScrolling.GetString()) ? "Automatic" : "By Position");
		GUILayout.Box(string.Format("Current Scrolling Mode: {0}", modeLbl));
		GUILayout.Space(5f);

		target.RangeProperty(_FlowScrollX, "Scroll Speed X");
		target.RangeProperty(_FlowScrollY, "Scroll Speed Y");


		GUILayout.Space(10f);
		DrawWideBox(1f);
	}

	private void HueShiftControl(Material target)
	{
		Header("Hue Shift", "http://wiki.next-gen-sprites.com/doku.php?id=shaders:feature:sprite#hue_shift");

		ToggleShader(target, "Hue Shift", ShaderFeature.HueShift.GetString(), disableGroup: true);
		var hsbc = target.GetVector(ShaderVector4.HSBC.GetString());
		hsbc.x = EditorGUILayout.Slider("Hue", hsbc.x, 0f, 1f);
		hsbc.y = EditorGUILayout.Slider("Saturation", hsbc.y, 0f, 1f);
		hsbc.z = EditorGUILayout.Slider("Brightness", hsbc.z, 0f, 1f);
		hsbc.w = EditorGUILayout.Slider("Contrast", hsbc.w, 0f, 1f);
		GUILayout.BeginHorizontal();
		if (GUILayout.Button("Reset Hue"))
			hsbc = new Vector4(0f, 0.5f, 0.5f, 0.5f);

		GUILayout.FlexibleSpace();
		GUILayout.EndHorizontal();
		target.SetVector(ShaderVector4.HSBC.GetString(), hsbc);

		EditorGUI.EndDisabledGroup();
	}

	private void MegaStackControl(MaterialEditor target)
	{
		Header("Mega Stack", "http://wiki.next-gen-sprites.com/doku.php?id=shaders:feature:sprite#mega_stack");

		ToggleShader(_targetMaterial, "Sprite scrolling", ShaderFeature.SpriteMultiLayer.GetString(), disableGroup: false);

		var hasScrolling = _targetMaterial.IsKeywordEnabled(ShaderFeature.SpriteMultiLayer.GetString());

		GUILayout.Space(10f);

		if (hasScrolling)
		{
			GUILayout.Box("Layer #0");
			target.RangeProperty(_megaStack0ScrollingX, "Layer 0 Scrolling X");
			target.RangeProperty(_megaStack0ScrollingY, "Layer 0 Scrolling Y");
			GUILayout.Space(10f); 
		}

		GUILayout.Box("Layer #1");
		target.TextureProperty(_megaStack1Tex, "Texture", scaleOffset: false);
		target.ColorProperty(_megaStack1Color, "Tint");
		target.RangeProperty(_megaStack1Opacity, "Opacity");
		if (hasScrolling)
		{
			target.RangeProperty(_megaStack1ScrollingX, "Scrolling X");
			target.RangeProperty(_megaStack1ScrollingY, "Scrolling Y"); 
		}
		DrawWideBox(1f);
		GUILayout.Space(10f);

		GUILayout.Box("Layer #2");
		target.TextureProperty(_megaStack2Tex, "Texture", scaleOffset: false);
		target.ColorProperty(_megaStack2Color, "Tint");
		target.RangeProperty(_megaStack2Opacity, "Opacity");
		if (hasScrolling)
		{
			target.RangeProperty(_megaStack2ScrollingX, "Scrolling X");
			target.RangeProperty(_megaStack2ScrollingY, "Scrolling Y"); 
		}
		DrawWideBox(1f);
		GUILayout.Space(10f);

		GUILayout.Box("Layer #3");
		target.TextureProperty(_megaStack3Tex, "Texture", scaleOffset: false);
		target.ColorProperty(_megaStack3Color, "Tint");
		target.RangeProperty(_megaStack3Opacity, "Opacity");
		if (hasScrolling)
		{
			target.RangeProperty(_megaStack3ScrollingX, "Scrolling X");
			target.RangeProperty(_megaStack3ScrollingY, "Scrolling Y"); 
		}
		DrawWideBox(1f);
		GUILayout.Space(10f);

		GUILayout.Box("Layer #4");
		target.TextureProperty(_megaStack4Tex, "Texture", scaleOffset: false);
		target.ColorProperty(_megaStack4Color, "Tint");
		target.RangeProperty(_megaStack4Opacity, "Opacity");
		if (hasScrolling)
		{
			target.RangeProperty(_megaStack4ScrollingX, "Scrolling X");
			target.RangeProperty(_megaStack4ScrollingY, "Scrolling Y"); 
		}
		DrawWideBox(1f);
		GUILayout.Space(10f);

		GUILayout.Box("Layer #5");
		target.TextureProperty(_megaStack5Tex, "Texture", scaleOffset: false);
		target.ColorProperty(_megaStack5Color, "Tint");
		target.RangeProperty(_megaStack5Opacity, "Opacity");
		if (hasScrolling)
		{
			target.RangeProperty(_megaStack5ScrollingX, "Scrolling X");
			target.RangeProperty(_megaStack5ScrollingY, "Scrolling Y"); 
		}
		DrawWideBox(1f);
		GUILayout.Space(10f);

		GUILayout.Box("Layer #6");
		target.TextureProperty(_megaStack6Tex, "Texture", scaleOffset: false);
		target.ColorProperty(_megaStack6Color, "Tint");
		target.RangeProperty(_megaStack6Opacity, "Opacity");
		if (hasScrolling)
		{
			target.RangeProperty(_megaStack6ScrollingX, "Scrolling X");
			target.RangeProperty(_megaStack6ScrollingY, "Scrolling Y"); 
		}
		DrawWideBox(1f);
		GUILayout.Space(10f);

		GUILayout.Box("Layer #7");
		target.TextureProperty(_megaStack7Tex, "Texture", scaleOffset: false);
		target.ColorProperty(_megaStack7Color, "Tint");
		target.RangeProperty(_megaStack7Opacity, "Opacity");
		if (hasScrolling)
		{
			target.RangeProperty(_megaStack7ScrollingX, "Scrolling X");
			target.RangeProperty(_megaStack7ScrollingY, "Scrolling Y"); 
		}
		DrawWideBox(1f);
		GUILayout.Space(10f);

		GUILayout.Box("Layer #8");
		target.TextureProperty(_megaStack8Tex, "Texture", scaleOffset: false);
		target.ColorProperty(_megaStack8Color, "Tint");
		target.RangeProperty(_megaStack8Opacity, "Opacity");
		if (hasScrolling)
		{
			target.RangeProperty(_megaStack8ScrollingX, "Scrolling X");
			target.RangeProperty(_megaStack8ScrollingY, "Scrolling Y"); 
		}
		DrawWideBox(1f);
		GUILayout.Space(10f);

		GUILayout.Box("Layer #9");
		target.TextureProperty(_megaStack9Tex, "Texture", scaleOffset: false);
		target.ColorProperty(_megaStack9Color, "Tint");
		target.RangeProperty(_megaStack9Opacity, "Opacity");
		if (hasScrolling)
		{
			target.RangeProperty(_megaStack9ScrollingX, "Scrolling X");
			target.RangeProperty(_megaStack9ScrollingY, "Scrolling Y"); 
		}
		DrawWideBox(1f);
		GUILayout.Space(10f);

		DrawWideBox(1f);
	}

	#endregion

	#region Helpers

	private bool ShaderNameCheck(string name, string checkKey)
	{
		return name.Contains(checkKey);
	}

	private void Header(string title, string url = "http://wiki.next-gen-sprites.com/doku.php?id=start")
	{
		GUILayout.BeginHorizontal();
		GUILayout.FlexibleSpace();
		GUILayout.Box(title, EditorStyles.centeredGreyMiniLabel, GUILayout.Height(32f), GUILayout.ExpandWidth(true));
		GUILayout.FlexibleSpace();
		ButtonOpenUrl(_btnHelp, url);
		GUILayout.EndHorizontal();
		GUILayout.Space(5f);
	}

	private void ToggleShader(Material targetMaterial, string label, string keyword, float height = 20f, bool disableGroup = true)
	{
		var setter = Array.IndexOf(targetMaterial.shaderKeywords, keyword) != -1;
		var lbl = (setter) ? ("Disable " + label) : ("Enable " + label);
		if (GUILayout.Button(lbl, GUILayout.Height(height)))
		{
			setter = !setter;
			if (setter)
				targetMaterial.EnableKeyword(keyword);
			else
				targetMaterial.DisableKeyword(keyword);
		}
		if (disableGroup)
			EditorGUI.BeginDisabledGroup(!setter);
	}

	private void ButtonOpenUrl(Texture tex, string url, float width = 32f, float height = 24f)
	{
		if (GUILayout.Button(tex, GUILayout.Width(width), GUILayout.Height(height)))
			Application.OpenURL(url);
	}

	private void DrawWideBox(float height)
	{
		//Draw super thin box as separator
		GUILayout.Box("", GUILayout.Height(height), GUILayout.ExpandWidth(true));
	}

	#endregion
}