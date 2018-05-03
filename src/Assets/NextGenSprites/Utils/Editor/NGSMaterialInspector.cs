// NextGenSprites (copyright) 2016 Ruben de la Torre, www.studio-delatorre.com

using NextGenSprites;
using UnityEditor;
using UnityEngine;

public class NGSMaterialInspector : ShaderGUI
{
	#region Shader MatBlock
	//Sprite
	private MaterialProperty _mainSprite;
	private MaterialProperty _mainSpriteTint;

	private MaterialProperty _HSBC;

	//Sprite Layers
	private MaterialProperty _spriteLayer1ScrollingX;
	private MaterialProperty _spriteLayer1ScrollingY;
	private MaterialProperty _spriteLayer2Tex;
	private MaterialProperty _spriteLayer2Color;
	private MaterialProperty _spriteLayer2Opacity;
	private MaterialProperty _spriteLayer2ScrollingX;
	private MaterialProperty _spriteLayer2ScrollingY;
	private MaterialProperty _spriteLayer3Tex;
	private MaterialProperty _spriteLayer3Color;
	private MaterialProperty _spriteLayer3Opacity;
	private MaterialProperty _spriteLayer3ScrollingX;
	private MaterialProperty _spriteLayer3ScrollingY;
	private MaterialProperty _spriteLayer4Tex;
	private MaterialProperty _spriteLayer4Color;
	private MaterialProperty _spriteLayer4Opacity;
	private MaterialProperty _spriteLayer4ScrollingX;
	private MaterialProperty _spriteLayer4ScrollingY;
	private MaterialProperty _spriteLayerStencil;
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
	//ReflectionT Scrolling
	private MaterialProperty _reflectionScrollingX;
	private MaterialProperty _reflectionScrollingY;
	//Emission i.e. Glow
	private MaterialProperty _emissionMask;
	private MaterialProperty _emissionStrength;
	private MaterialProperty _emissionTint;
	private MaterialProperty _emissionLayer1AninBlend;
	private MaterialProperty _emissionLayer1AninSpeed;
	private MaterialProperty _emissionLayer2Strength;
	private MaterialProperty _emissionLayer2Tint;
	private MaterialProperty _emissionLayer2AninBlend;
	private MaterialProperty _emissionLayer2AninSpeed;
	private MaterialProperty _emissionLayer3Strength;
	private MaterialProperty _emissionLayer3Tint;
	private MaterialProperty _emissionLayer3AninBlend;
	private MaterialProperty _emissionLayer3AninSpeed;
	//X-Ray i.e. Light Transmission
	private MaterialProperty _transmissionMap;
	private MaterialProperty _transmissionDensity;
	//Dissolve
	private MaterialProperty _dissolveMap;
	private MaterialProperty _dissolveBlend;
	private MaterialProperty _dissolveBorderWidth;
	private MaterialProperty _dissolveGlowColor;
	private MaterialProperty _dissolveGlowStrength;
	#endregion

	#region Inspector properties
	//Inspector Icons
	readonly Texture2D _btnSprite = (Texture2D)AssetDatabase.LoadAssetAtPath("Assets/NextGenSprites/Utils/Editor/Icons/btn_sprite.png", typeof(Texture2D));
	readonly Texture2D _btnCurvature = (Texture2D)AssetDatabase.LoadAssetAtPath("Assets/NextGenSprites/Utils/Editor/Icons/btn_curvature.png", typeof(Texture2D));
	readonly Texture2D _btnReflection = (Texture2D)AssetDatabase.LoadAssetAtPath("Assets/NextGenSprites/Utils/Editor/Icons/btn_reflection.png", typeof(Texture2D));
	readonly Texture2D _btnEmission = (Texture2D)AssetDatabase.LoadAssetAtPath("Assets/NextGenSprites/Utils/Editor/Icons/btn_emission.png", typeof(Texture2D));
	readonly Texture2D _btnTransmission = (Texture2D)AssetDatabase.LoadAssetAtPath("Assets/NextGenSprites/Utils/Editor/Icons/btn_xray.png", typeof(Texture2D));
	readonly Texture2D _btnDissolve = (Texture2D)AssetDatabase.LoadAssetAtPath("Assets/NextGenSprites/Utils/Editor/Icons/btn_dissolve.png", typeof(Texture2D));
	readonly Texture2D _btnExtras = (Texture2D)AssetDatabase.LoadAssetAtPath("Assets/NextGenSprites/Utils/Editor/Icons/btn_extras.png", typeof(Texture2D));
	readonly Texture2D _btnHelp = (Texture2D)AssetDatabase.LoadAssetAtPath("Assets/NextGenSprites/Utils/Editor/Icons/btn_help.png", typeof(Texture2D));
	readonly Texture2D _btnWiki = (Texture2D)AssetDatabase.LoadAssetAtPath("Assets/NextGenSprites/Utils/Editor/Icons/btn_wiki.png", typeof(Texture2D));
	readonly Texture2D _btnWidget = (Texture2D)AssetDatabase.LoadAssetAtPath("Assets/NextGenSprites/Utils/Editor/Icons/btn_widget.png", typeof(Texture2D));
	readonly Texture2D _btnLoadCollection = (Texture2D)AssetDatabase.LoadAssetAtPath("Assets/NextGenSprites/Utils/Editor/Icons/btn_loadCollection.png", typeof(Texture2D));
	readonly Texture2D _btnSaveCollection = (Texture2D)AssetDatabase.LoadAssetAtPath("Assets/NextGenSprites/Utils/Editor/Icons/btn_saveCollection.png", typeof(Texture2D));

	//Used by the top Toolbar
	private enum SettingsMode
	{
		Sprites,
		Curvature,
		Reflection,
		Emission,
		Translucency,
		Dissolve,
		Extras
	}

	private SettingsMode _setPanel;

	//Shaderselection Toolbox index
	private int _toolbarSelected;
	private int _toolbarSelectedUnlit;

	//Misc
	private bool _isUnlit;
	private Texture[] _panelIcons;
	private Material _matTarget;
	private bool _rebuildPanel = true;
	#endregion

	#region Shader Methods
	//Find all MatBlock from the Shader/Material
	private void FetchProperties(MaterialProperty[] props)
	{
		_mainSprite = FindProperty(ShaderTexture.Sprite.GetString(), props);
		_mainSpriteTint = FindProperty(ShaderColor.Sprite.GetString(), props);

		_HSBC = FindProperty(ShaderVector4.HSBC.GetString(), props);

		_spriteLayer1ScrollingX = FindProperty(ShaderFloat.SpriteLayer0ScrollingX.GetString(), props);
		_spriteLayer1ScrollingY = FindProperty(ShaderFloat.SpriteLayer0ScrollingY.GetString(), props);
		_spriteLayer2Tex = FindProperty(ShaderTexture.SpriteLayer1.GetString(), props);
		_spriteLayer2Color = FindProperty(ShaderColor.SpriteLayer1.GetString(), props);
		_spriteLayer2Opacity = FindProperty(ShaderFloat.SpriteLayer1Opacity.GetString(), props);
		_spriteLayer2ScrollingX = FindProperty(ShaderFloat.SpriteLayer1ScrollingX.GetString(), props);
		_spriteLayer2ScrollingY = FindProperty(ShaderFloat.SpriteLayer1ScrollingY.GetString(), props);
		_spriteLayer3Tex = FindProperty(ShaderTexture.SpriteLayer2.GetString(), props);
		_spriteLayer3Color = FindProperty(ShaderColor.SpriteLayer2.GetString(), props);
		_spriteLayer3Opacity = FindProperty(ShaderFloat.SpriteLayer2Opacity.GetString(), props);
		_spriteLayer3ScrollingX = FindProperty(ShaderFloat.SpriteLayer2ScrollingX.GetString(), props);
		_spriteLayer3ScrollingY = FindProperty(ShaderFloat.SpriteLayer2ScrollingY.GetString(), props);
		_spriteLayer4Tex = FindProperty(ShaderTexture.SpriteLayer3.GetString(), props);
		_spriteLayer4Color = FindProperty(ShaderColor.SpriteLayer3.GetString(), props);
		_spriteLayer4Opacity = FindProperty(ShaderFloat.SpriteLayer3Opacity.GetString(), props);
		_spriteLayer4ScrollingX = FindProperty(ShaderFloat.SpriteLayer3ScrollingX.GetString(), props);
		_spriteLayer4ScrollingY = FindProperty(ShaderFloat.SpriteLayer3ScrollingY.GetString(), props);
		_spriteLayerStencil = FindProperty(ShaderTexture.StencilMask.GetString(), props);

		_reflectionScreenMap = FindProperty(ShaderTexture.Reflection.GetString(), props);
		_reflectionMask = FindProperty(ShaderTexture.ReflectionMask.GetString(), props);
		_reflectionStrength = FindProperty(ShaderFloat.ReflectionStrength.GetString(), props);
		_reflectionBlur = FindProperty(ShaderFloat.ReflectionBlur.GetString(), props);
		_reflectionScrollingX = FindProperty(ShaderFloat.ReflectionScrollSpeedX.GetString(), props);
		_reflectionScrollingY = FindProperty(ShaderFloat.ReflectionScrollSpeedY.GetString(), props);

		_dissolveMap = FindProperty(ShaderTexture.Dissolve.GetString(), props);
		_dissolveBlend = FindProperty(ShaderFloat.DissolveBlend.GetString(), props);
		_dissolveBorderWidth = FindProperty(ShaderFloat.DissolveBorderWidth.GetString(), props);
		_dissolveGlowColor = FindProperty(ShaderColor.DissolveGlow.GetString(), props);
		_dissolveGlowStrength = FindProperty(ShaderFloat.DissolveGlowStrength.GetString(), props);

		_curvatureMap = FindProperty(ShaderTexture.Curvature.GetString(), props);
		_curvatureDepth = FindProperty(ShaderFloat.CurvatureDepth.GetString(), props);
		_curvatureHighlight = FindProperty(ShaderColor.Curvature.GetString(), props);
		_curvatureGloss = FindProperty(ShaderFloat.CurvatureGloss.GetString(), props);

		//Stop here if we use an Unlit shader, since these Props are not used
		if (_isUnlit)
			return;

		_emissionStrength = FindProperty(ShaderFloat.EmissionIntensity.GetString(), props);
		_emissionMask = FindProperty(ShaderTexture.EmissionMask.GetString(), props);
		_emissionTint = FindProperty(ShaderColor.Emission.GetString(), props);
		_emissionLayer1AninBlend = FindProperty(ShaderFloat.EmissionLayer1BlendAnimation.GetString(), props);
		_emissionLayer1AninSpeed = FindProperty(ShaderFloat.EmissionLayer1PulseSpeed.GetString(), props);
		_emissionLayer2Strength = FindProperty(ShaderFloat.EmissionLayer2Intensity.GetString(), props);
		_emissionLayer2Tint = FindProperty(ShaderColor.EmissionLayer2.GetString(), props);
		_emissionLayer2AninBlend = FindProperty(ShaderFloat.EmissionLayer2BlendAnimation.GetString(), props);
		_emissionLayer2AninSpeed = FindProperty(ShaderFloat.EmissionLayer2PulseSpeed.GetString(), props);
		_emissionLayer3Strength = FindProperty(ShaderFloat.EmissionLayer3Intensity.GetString(), props);
		_emissionLayer3Tint = FindProperty(ShaderColor.EmissionLayer3.GetString(), props);
		_emissionLayer3AninBlend = FindProperty(ShaderFloat.EmissionLayer3BlendAnimation.GetString(), props);
		_emissionLayer3AninSpeed = FindProperty(ShaderFloat.EmissionLayer3PulseSpeed.GetString(), props);

		_transmissionMap = FindProperty(ShaderTexture.Transmission.GetString(), props);
		_transmissionDensity = FindProperty(ShaderFloat.TransmissionDensity.GetString(), props);
	}
	#endregion

	public override void OnGUI(MaterialEditor materialEditor, MaterialProperty[] properties)
	{
		if (_rebuildPanel)
		{
			//Get Material reference
			_matTarget = materialEditor.target as Material;
			//Build Icons for the toolbar
			BuildPanelIcons();
		}

		//This must happen at every OnGUI callback
		FetchProperties(properties);

		GUILayout.Space(2f);

		//Draw our Panels
		//This is the Top Toolbar to select each shader feature
		ShaderSelectionToolbar();

		DrawWideBox();

		//Main view for the properties
		MainView(materialEditor);
	}

	#region GUI Methods

	//This is the Main view with all shader controlls
	private void MainView(MaterialEditor materialEditor)
	{
		CheckIsShadowy(_matTarget);

		GUILayout.Space(5f);

		switch (_setPanel)
		{
			case SettingsMode.Sprites:
				EditorGUILayout.BeginHorizontal();
				GUILayout.FlexibleSpace();
				ButtonOpenUrl(_btnHelp, "http://wiki.next-gen-sprites.com/doku.php?id=shaders:feature:sprite");
				GUILayout.Space(10f);
				EditorGUILayout.EndHorizontal();

				GUILayout.Space(5f);

				materialEditor.TextureProperty(_mainSprite, "Sprite", scaleOffset: false);

				DrawWideBox();
				GUILayout.Space(10f);

				GUILayout.BeginHorizontal();
				materialEditor.ColorProperty(_mainSpriteTint, "Tint");
				if (GUILayout.Button("Reset Tint"))
				{
					_matTarget.SetColor(ShaderColor.Sprite.GetString(), Color.white);
				}
				GUILayout.EndHorizontal();
				GUILayout.Space(15f);

				ToggleFeature(_matTarget, "Hue Shift", ShaderFeature.HueShift, disableGroup: true);

				EditorGUILayout.BeginHorizontal();
				GUILayout.FlexibleSpace();
				ButtonOpenUrl(_btnHelp, "http://wiki.next-gen-sprites.com/doku.php?id=shaders:feature:sprite#hue_shift");
				GUILayout.Space(10f);
				EditorGUILayout.EndHorizontal();
				GUILayout.Space(5f);

				var hsbcTemp = _HSBC.vectorValue;

				hsbcTemp.x = EditorGUILayout.Slider("Hue", hsbcTemp.x, 0f, 1f);
				hsbcTemp.y = EditorGUILayout.Slider("Saturation", hsbcTemp.y, 0f, 1f);
				hsbcTemp.z = EditorGUILayout.Slider("Brightness", hsbcTemp.z, 0f, 1f);
				hsbcTemp.w = EditorGUILayout.Slider("Contrast", hsbcTemp.w, 0f, 1f);

				GUILayout.BeginHorizontal();
				if (GUILayout.Button("Reset Hue"))
					hsbcTemp = new Vector4(0f, 0.5f, 0.5f, 0.5f);

				GUILayout.FlexibleSpace();
				GUILayout.EndHorizontal();
				_HSBC.vectorValue = hsbcTemp;

				EditorGUI.EndDisabledGroup();

				ToggleFeature(_matTarget, "Main Layer Scrolling", ShaderFeature.SpriteScrolling);
				GUILayout.Space(5f);
				materialEditor.RangeProperty(_spriteLayer1ScrollingX, "X-Axis");
				materialEditor.RangeProperty(_spriteLayer1ScrollingY, "Y-Axis");
				GUILayout.BeginHorizontal();
				if (GUILayout.Button("Reset Scrolling"))
				{
					_matTarget.SetFloat(ShaderFloat.SpriteLayer0ScrollingX.GetString(), 0f);
					_matTarget.SetFloat(ShaderFloat.SpriteLayer0ScrollingY.GetString(), 0f);
				}
				GUILayout.FlexibleSpace();
				GUILayout.EndHorizontal();

				EditorGUI.EndDisabledGroup();


				GUILayout.Space(5f);

				DrawWideBox();
				GUILayout.Space(5f);
				ToggleFeature(_matTarget, "Multi Layer", ShaderFeature.SpriteMultiLayer, disableGroup: false);
				if (_matTarget.IsKeywordEnabled(ShaderFeature.SpriteMultiLayer.GetString()))
				{
					EditorGUILayout.BeginHorizontal();
					GUILayout.FlexibleSpace();
					ButtonOpenUrl(_btnHelp, "http://wiki.next-gen-sprites.com/doku.php?id=shaders:feature:sprite#multi_layer");
					GUILayout.Space(10f);
					EditorGUILayout.EndHorizontal();

					ToggleFeature(_matTarget, "Stencil Mask", ShaderFeature.SpriteStencil, disableGroup: true);
					materialEditor.TextureProperty(_spriteLayerStencil, "Stencil Mask");
					EditorGUILayout.BeginHorizontal();
					GUILayout.FlexibleSpace();
					EditorGUILayout.HelpBox("Red: Layer 1# | Green: Layer 2# | Blue: Layer 3#", MessageType.None);
					GUILayout.FlexibleSpace();
					EditorGUILayout.EndHorizontal();
					DrawWideBox();
					EditorGUI.EndDisabledGroup();

					GUILayout.Box("Layer 1#", EditorStyles.miniButton);
					materialEditor.TextureProperty(_spriteLayer2Tex, "Sprite");
					GUILayout.Space(10f);
					materialEditor.RangeProperty(_spriteLayer2Opacity, "Opacity");
					materialEditor.ColorProperty(_spriteLayer2Color, "Tint");
					materialEditor.RangeProperty(_spriteLayer2ScrollingX, "Scrolling X-Axis");
					materialEditor.RangeProperty(_spriteLayer2ScrollingY, "Scrolling Y-Axis");

					GUILayout.Space(10f);
					GUILayout.BeginHorizontal();
					GUILayout.FlexibleSpace();
					if (GUILayout.Button("Reset"))
					{
						_matTarget.SetFloat(ShaderFloat.SpriteLayer1Opacity.GetString(), 1f);
						_matTarget.SetColor(ShaderColor.SpriteLayer1.GetString(), Color.white);
						_matTarget.SetFloat(ShaderFloat.SpriteLayer1ScrollingX.GetString(), 0f);
						_matTarget.SetFloat(ShaderFloat.SpriteLayer1ScrollingY.GetString(), 0f);
					}
					GUILayout.EndHorizontal();

					GUILayout.Space(10f);

					DrawWideBox();
					GUILayout.Box("Layer 2#", EditorStyles.miniButton);
					materialEditor.TextureProperty(_spriteLayer3Tex, "Sprite");
					GUILayout.Space(10f);
					materialEditor.RangeProperty(_spriteLayer3Opacity, "Opacity");
					materialEditor.ColorProperty(_spriteLayer3Color, "Tint");
					materialEditor.RangeProperty(_spriteLayer3ScrollingX, "Scrolling X-Axis");
					materialEditor.RangeProperty(_spriteLayer3ScrollingY, "Scrolling Y-Axis");

					GUILayout.Space(10f);
					GUILayout.BeginHorizontal();
					GUILayout.FlexibleSpace();
					if (GUILayout.Button("Reset"))
					{
						_matTarget.SetFloat(ShaderFloat.SpriteLayer2Opacity.GetString(), 1f);
						_matTarget.SetColor(ShaderColor.SpriteLayer2.GetString(), Color.white);
						_matTarget.SetFloat(ShaderFloat.SpriteLayer2ScrollingX.GetString(), 0f);
						_matTarget.SetFloat(ShaderFloat.SpriteLayer2ScrollingY.GetString(), 0f);
					}
					GUILayout.EndHorizontal();

					GUILayout.Space(10f);

					DrawWideBox();
					GUILayout.Box("Layer 3#", EditorStyles.miniButton);
					materialEditor.TextureProperty(_spriteLayer4Tex, "Sprite");
					GUILayout.Space(10f);
					materialEditor.RangeProperty(_spriteLayer4Opacity, "Opacity");
					materialEditor.ColorProperty(_spriteLayer4Color, "Tint");
					materialEditor.RangeProperty(_spriteLayer4ScrollingX, "Scrolling X-Axis");
					materialEditor.RangeProperty(_spriteLayer4ScrollingY, "Scrolling Y-Axis");

					GUILayout.Space(10f);
					GUILayout.BeginHorizontal();
					GUILayout.FlexibleSpace();
					if (GUILayout.Button("Reset"))
					{
						_matTarget.SetFloat(ShaderFloat.SpriteLayer3Opacity.GetString(), 1f);
						_matTarget.SetColor(ShaderColor.SpriteLayer3.GetString(), Color.white);
						_matTarget.SetFloat(ShaderFloat.SpriteLayer3ScrollingX.GetString(), 0f);
						_matTarget.SetFloat(ShaderFloat.SpriteLayer3ScrollingY.GetString(), 0f);
					}
					GUILayout.EndHorizontal();
				}
				break;
			case SettingsMode.Curvature:
				GUILayout.BeginHorizontal();
				ToggleFeature(_matTarget, "Curvature", ShaderFeature.Curvature);
				GUILayout.FlexibleSpace();
				ButtonOpenUrl(_btnHelp, "http://wiki.next-gen-sprites.com/doku.php?id=shaders:feature:curvature");
				GUILayout.Space(10f);
				GUILayout.EndHorizontal();

				GUILayout.Space(5f);

				materialEditor.TextureProperty(_curvatureMap, "Curvature Map");
				GUILayout.Space(10f);
				DrawWideBox();
				GUILayout.Space(10f);

				materialEditor.ColorProperty(_curvatureHighlight, "Highlight Tint");
				GUILayout.Space(10f);

				DrawWideBox();
				GUILayout.Space(10f);

				materialEditor.RangeProperty(_curvatureDepth, "Depth");
				GUILayout.Space(10f);

				materialEditor.RangeProperty(_curvatureGloss, "Surface Glossiness");
				GUILayout.Space(10f);

				DrawWideBox();
				GUILayout.Space(5f);

				GUILayout.BeginHorizontal();
				GUILayout.FlexibleSpace();
				if (GUILayout.Button("Reset Properties"))
				{
					_matTarget.SetFloat(ShaderFloat.CurvatureDepth.GetString(), 0.5f);
					_matTarget.SetColor(ShaderColor.Curvature.GetString(), Color.gray);
					_matTarget.SetFloat(ShaderFloat.CurvatureGloss.GetString(), 0.5f);
				}
				GUILayout.EndHorizontal();

				EditorGUI.EndDisabledGroup();
				break;
			case SettingsMode.Reflection:
				GUILayout.BeginHorizontal();
				ToggleFeature(_matTarget, "Reflection", ShaderFeature.Reflection);
				GUILayout.FlexibleSpace();
				ButtonOpenUrl(_btnHelp, "http://wiki.next-gen-sprites.com/doku.php?id=shaders:feature:reflection");
				GUILayout.Space(10f);
				GUILayout.EndHorizontal();
				GUILayout.Space(5f);

				materialEditor.TextureProperty(_reflectionScreenMap, "Reflection");
				EditorGUILayout.HelpBox("Texture should have Wrap Mode set to Repeat.", MessageType.None);

				DrawWideBox();
				materialEditor.RangeProperty(_reflectionStrength, "Reflection Strength");
				GUILayout.Space(5f);
				materialEditor.RangeProperty(_reflectionBlur, "Blur");
				GUILayout.Space(5f);

				DrawWideBox();
				materialEditor.RangeProperty(_reflectionScrollingX, "Scrolling speed X");
				GUILayout.Space(10f);
				materialEditor.RangeProperty(_reflectionScrollingY, "Scrolling speed Y");
				DrawWideBox();

				GUILayout.Space(5f);

				materialEditor.TextureProperty(_reflectionMask, "Mask");

				DrawWideBox();
				GUILayout.Space(5f);

				GUILayout.BeginHorizontal();
				GUILayout.FlexibleSpace();

				if (GUILayout.Button("Reset Properties"))
				{
					_matTarget.SetFloat(ShaderFloat.ReflectionScrollSpeedX.GetString(), 0.25f);
					_matTarget.SetFloat(ShaderFloat.ReflectionScrollSpeedY.GetString(), 0.25f);
					_matTarget.SetFloat(ShaderFloat.ReflectionStrength.GetString(), 0f);
					_matTarget.SetFloat(ShaderFloat.ReflectionBlur.GetString(), 0f);
				}
				GUILayout.EndHorizontal();

				EditorGUI.EndDisabledGroup();
				break;
			case SettingsMode.Emission:
				var animationOn = _matTarget.IsKeywordEnabled(ShaderFeature.EmissionPulse.GetString());

				GUILayout.BeginHorizontal();
				ToggleFeature(_matTarget, "Emission", ShaderFeature.Emission);
				GUILayout.FlexibleSpace();
				ButtonOpenUrl(_btnHelp, "http://wiki.next-gen-sprites.com/doku.php?id=shaders:feature:emission");
				GUILayout.Space(10f);
				GUILayout.EndHorizontal();
				GUILayout.Space(5f);

				GUILayout.BeginHorizontal();
				EditorGUILayout.HelpBox("Mask Channels\n\nRed: Layer 1#\nGreen: Layer 2#\nBlue: Layer 3#", MessageType.None);
				materialEditor.TextureProperty(_emissionMask, "Emission Mask");
				GUILayout.EndHorizontal();

				GUILayout.Space(5f);
				DrawWideBox();
				GUILayout.Box("Layer 1# - Mask Channel Red", EditorStyles.miniButton);
				GUILayout.Space(10f);
				materialEditor.RangeProperty(_emissionStrength, "Intensity");
				GUILayout.Space(10f);
				materialEditor.ColorProperty(_emissionTint, "Tint");
				GUILayout.Space(5f);

				EditorGUI.BeginDisabledGroup(!animationOn);
				materialEditor.RangeProperty(_emissionLayer1AninSpeed, "Pulse Speed");
				materialEditor.RangeProperty(_emissionLayer1AninBlend, "Blend Animation");
				EditorGUI.EndDisabledGroup();
				GUILayout.Space(5f);

				DrawWideBox();
				GUILayout.Box("Layer 2# - Mask Channel Green", EditorStyles.miniButton);
				GUILayout.Space(10f);
				materialEditor.RangeProperty(_emissionLayer2Strength, "Intensity");
				GUILayout.Space(10f);
				materialEditor.ColorProperty(_emissionLayer2Tint, "Tint");
				GUILayout.Space(5f);

				EditorGUI.BeginDisabledGroup(!animationOn);
				materialEditor.RangeProperty(_emissionLayer2AninSpeed, "Pulse Speed");
				materialEditor.RangeProperty(_emissionLayer2AninBlend, "Blend Animation");
				EditorGUI.EndDisabledGroup();
				GUILayout.Space(5f);

				DrawWideBox();
				GUILayout.Box("Layer 3# - Mask Channel Blue", EditorStyles.miniButton);
				GUILayout.Space(10f);
				materialEditor.RangeProperty(_emissionLayer3Strength, "Intensity");
				GUILayout.Space(10f);
				materialEditor.ColorProperty(_emissionLayer3Tint, "Tint");
				GUILayout.Space(5f);
				EditorGUI.BeginDisabledGroup(!animationOn);
				materialEditor.RangeProperty(_emissionLayer3AninSpeed, "Pulse Speed");
				materialEditor.RangeProperty(_emissionLayer3AninBlend, "Blend Animation");
				EditorGUI.EndDisabledGroup();
				GUILayout.Space(5f);

				DrawWideBox();
				GUILayout.Space(5f);
				GUILayout.BeginHorizontal();
				ToggleFeature(_matTarget, "Pulse Animation", ShaderFeature.EmissionPulse, disableGroup: false);
				GUILayout.FlexibleSpace();
				if (GUILayout.Button("Reset Properties"))
				{
					_matTarget.SetFloat(ShaderFloat.EmissionIntensity.GetString(), 0f);
					_matTarget.SetColor(ShaderColor.Emission.GetString(), Color.white);
					_matTarget.SetFloat(ShaderFloat.EmissionLayer1BlendAnimation.GetString(), 1f);
					_matTarget.SetFloat(ShaderFloat.EmissionLayer1PulseSpeed.GetString(), 0f);
					_matTarget.SetFloat(ShaderFloat.EmissionLayer2Intensity.GetString(), 0f);
					_matTarget.SetColor(ShaderColor.EmissionLayer2.GetString(), Color.white);
					_matTarget.SetFloat(ShaderFloat.EmissionLayer2BlendAnimation.GetString(), 1f);
					_matTarget.SetFloat(ShaderFloat.EmissionLayer2PulseSpeed.GetString(), 0f);
					_matTarget.SetFloat(ShaderFloat.EmissionLayer3Intensity.GetString(), 0f);
					_matTarget.SetColor(ShaderColor.EmissionLayer3.GetString(), Color.white);
					_matTarget.SetFloat(ShaderFloat.EmissionLayer3BlendAnimation.GetString(), 1f);
					_matTarget.SetFloat(ShaderFloat.EmissionLayer3PulseSpeed.GetString(), 0f);
				}
				GUILayout.EndHorizontal();
				break;
			case SettingsMode.Translucency:
				GUILayout.BeginHorizontal();
				ToggleFeature(_matTarget, "Transmission", ShaderFeature.Transmission);
				GUILayout.FlexibleSpace();
				ButtonOpenUrl(_btnHelp, "http://wiki.next-gen-sprites.com/doku.php?id=shaders:feature:transmission");
				GUILayout.Space(10f);
				GUILayout.EndHorizontal();
				GUILayout.Space(5f);

				materialEditor.TextureProperty(_transmissionMap, "Texture");
				GUILayout.Space(10f);
				DrawWideBox();
				GUILayout.Space(10f);

				materialEditor.RangeProperty(_transmissionDensity, "Density");
				EditorGUI.EndDisabledGroup();
				break;
			case SettingsMode.Dissolve:
				GUILayout.BeginHorizontal();
				ToggleFeature(_matTarget, "Dissolve", ShaderFeature.Dissolve);
				GUILayout.FlexibleSpace();
				ButtonOpenUrl(_btnHelp, "http://wiki.next-gen-sprites.com/doku.php?id=shaders:feature:dissolve");
				GUILayout.Space(10f);
				GUILayout.EndHorizontal();
				GUILayout.Space(5f);

				materialEditor.TextureProperty(_dissolveMap, "Dissolve Pattern", scaleOffset: false);
				DrawWideBox();
				GUILayout.Space(15f);
				materialEditor.RangeProperty(_dissolveBlend, "Blending");
				GUILayout.Space(10f);
				materialEditor.RangeProperty(_dissolveBorderWidth, "Border Width");
				GUILayout.Space(10f);
				materialEditor.RangeProperty(_dissolveGlowStrength, "Border Glow");
				GUILayout.Space(10f);
				materialEditor.ColorProperty(_dissolveGlowColor, "Glow Color");

				EditorGUI.EndDisabledGroup();
				break;
			case SettingsMode.Extras:
				ShaderLightingSelection(_matTarget, materialEditor);

				GUILayout.Space(5f);
				//check Shader Keywords for Pixel Snapping
				GUILayout.BeginHorizontal();
				ToggleFeature(_matTarget, "Pixel Snapping", ShaderFeature.PixelSnap, disableGroup: false, height: 30f);
				ToggleFeature(_matTarget, "Doublesided", ShaderFeature.DoubleSided, disableGroup: false, height: 30f);
				GUILayout.EndHorizontal();
				GUILayout.Space(5f);

				DrawWideBox();
				GUILayout.Space(5f);

				GUILayout.BeginHorizontal();
				GUILayout.FlexibleSpace();

				if (GUILayout.Button(_btnWiki, GUILayout.Height(64f), GUILayout.MaxWidth(140f)))
					EditorApplication.delayCall += ()=> { Application.OpenURL("http://wiki.next-gen-sprites.com/"); };
				
				if (GUILayout.Button(_btnWidget, GUILayout.Height(64f), GUILayout.MaxWidth(140f)))
					EditorApplication.delayCall += ()=> { EditorApplication.ExecuteMenuItem("Window/NextGenSprites/Widget"); };
				
				GUILayout.FlexibleSpace();
				GUILayout.EndHorizontal();

				GUILayout.Space(10f);

				EditorGUILayout.BeginHorizontal();
				GUILayout.FlexibleSpace();

				if (GUILayout.Button(_btnSaveCollection, GUILayout.Height(64f), GUILayout.MaxWidth(140f)))
					EditorApplication.delayCall += ()=> { EditorApplication.ExecuteMenuItem("Assets/Create/NextGenSprites/Properties Collection from selection"); };
				
				if (GUILayout.Button(_btnLoadCollection, GUILayout.Height(64f), GUILayout.MaxWidth(140f)))
					EditorApplication.delayCall += ()=> { EditorApplication.ExecuteMenuItem("GameObject/NextGenSprites/Apply Properties Collection to selection"); };

				GUILayout.FlexibleSpace();
				EditorGUILayout.EndHorizontal();
				
				GUILayout.Space(10f);
				DrawWideBox();
				GUILayout.Space(10f);

				EditorGUILayout.BeginHorizontal();
				GUILayout.FlexibleSpace();
				EditorGUILayout.HelpBox("Version 1.3.9", MessageType.None);
				EditorGUILayout.HelpBox("Copyright 2016 Ruben de la Torre", MessageType.None);
				EditorGUILayout.EndHorizontal();
				break;
		}
	}

	//Check if its Unlit and build the Buttons Array
	private void BuildPanelIcons()
	{
		//check if shader is Unlit by looking if there is no Emission property
		_isUnlit = _matTarget.HasProperty(ShaderFloat.EmissionIntensity.GetString()) == false;

		//Lets set our Panel Icons Array
		if (_isUnlit)
		{
			_panelIcons = new Texture[]
			{ _btnSprite, _btnCurvature, _btnReflection, _btnDissolve, _btnExtras };
		}
		else
		{
			_panelIcons = new Texture[]
			{ _btnSprite, _btnCurvature, _btnReflection, _btnEmission, _btnTransmission, _btnDissolve, _btnExtras };
		}

		//Prevent OnGUI invoking this method repeatedly 
		_rebuildPanel = false;
	}

	//The Shader Lighting selection in the Extras Panel
	private void ShaderLightingSelection(Material mat, MaterialEditor matEditor)
	{
		//This is the Header
		GUILayout.BeginHorizontal();
		GUILayout.Box("Lighting Style Selector", EditorStyles.centeredGreyMiniLabel, GUILayout.ExpandWidth(true));
		ButtonOpenUrl(_btnHelp, "http://wiki.next-gen-sprites.com/doku.php?id=shaders:lighting");
		GUILayout.EndHorizontal();
		GUILayout.Space(5f);

		var indexSpecialStyle = 0;
		var indexLitStyle = 0;

		//Labels for Buttons
		var labelSpecialStyle = new[] { "Standard", "Shadow Receiver" };
		var labelLitStyle = new[] { "Multi Lit", "Single Lit", "Unlit" };

		//Used to seek and assign Shaders by Name
		var special = new[] { "Standard", "Standard Shadowy" };
		var litStyle = new[] { "Multi", "Single", "Unlit" };

		var splited = mat.shader.name.Split('/');
		var lastWord = splited[splited.Length - 1];
		var midWord = splited[splited.Length - 2];

		indexSpecialStyle = (string.CompareOrdinal(midWord, labelSpecialStyle[0]) == 0) ? 0 : 1;

		//The Toolbar uses an index, we get it by comparing the string
		for (int i = 0; i < litStyle.Length; i++)
		{
			if(string.CompareOrdinal(lastWord, litStyle[i]) == 0)
			{
				indexLitStyle = i;
				break;
			}
		}

		indexSpecialStyle = GUILayout.Toolbar(indexSpecialStyle, labelSpecialStyle, GUILayout.ExpandWidth(true), GUILayout.Height(32f));
		if(GUI.changed)
		{
			_rebuildPanel = true;

			GUILayout.Space(8f);
			DrawWideBox();

			//Build String and assign it as Shader
			var mainResult = "NextGenSprites/" + special[indexSpecialStyle] + "/" + litStyle[indexLitStyle];

			//Check if it realy is a different Shader then the current
			if (string.CompareOrdinal(_matTarget.shader.name, mainResult) != 0)
				matEditor.SetShader(Shader.Find(mainResult));
		}

		indexLitStyle = GUILayout.Toolbar(indexLitStyle, labelLitStyle, GUILayout.ExpandWidth(true), GUILayout.Height(32f));
		if (GUI.changed)
		{
			_rebuildPanel = true;

			GUILayout.Space(8f);
			DrawWideBox();

			//Build String and assign it as Shader
			var mainResult = "NextGenSprites/" + special[indexSpecialStyle] + "/" + litStyle[indexLitStyle];

			//Check if it realy is a different Shader then the current
			if (string.CompareOrdinal(_matTarget.shader.name, mainResult) != 0)
				matEditor.SetShader(Shader.Find(mainResult));
		}
	}

	//The Toolbar at the Top
	private void ShaderSelectionToolbar()
	{
		GUILayout.BeginHorizontal();
		GUILayout.FlexibleSpace();
		//Toolbar height is 40 = 48 - 8
		if (!_isUnlit)
		{
			_toolbarSelected = GUILayout.Toolbar(_toolbarSelected, _panelIcons, GUILayout.MaxWidth(Screen.width - 42f),
				GUILayout.Height(48f));
			_setPanel = (SettingsMode)_toolbarSelected;

		}
		else
		{
			_toolbarSelectedUnlit = GUILayout.Toolbar(_toolbarSelectedUnlit, _panelIcons,
				GUILayout.MaxWidth(Screen.width - 42f), GUILayout.Height(48f));

			//For Unlit we need to use a different index and a switch as workaround
			switch (_toolbarSelectedUnlit)
			{
				case 4:
					_setPanel = SettingsMode.Extras;
					break;
				case 3:
					_setPanel = SettingsMode.Dissolve;
					break;
				case 2:
					_setPanel = SettingsMode.Reflection;
					break;
				case 1:
					_setPanel = SettingsMode.Curvature;
					break;
				default:
					_setPanel = SettingsMode.Sprites;
					break;
			}
		}

		GUILayout.FlexibleSpace();
		GUILayout.EndHorizontal();
		GUILayout.Space(1f);
	}

	private void CheckIsShadowy(Material matTarget)
	{
		if(string.CompareOrdinal(matTarget.shader.name.Split('/')[1], "Standard Shadowy") == 0)
		{
			GUILayout.Space(5f);
			EditorGUILayout.HelpBox("This Shadowy Shader may be occluded by other Shader types.", MessageType.Warning);
			GUILayout.Space(5f);
			DrawWideBox();
		}
	}

	//Toogle a Shader feature
	private void ToggleFeature(Material _matTarget, string label, ShaderFeature feature, float height = 20f, bool disableGroup = true)
	{
		var setter = _matTarget.IsKeywordEnabled(feature.GetString());
		var lbl = (setter) ? ("Disable " + label) : ("Enable " + label);
		if (GUILayout.Button(lbl, GUILayout.Height(height)))
		{
			setter = !setter;
			if (setter)
			{
				_matTarget.EnableKeyword(feature.GetString());
			}
			else
			{
				_matTarget.DisableKeyword(feature.GetString());
			}
		}

		//Set this to false to not disabled the remaining GUI components
		if (disableGroup)
			EditorGUI.BeginDisabledGroup(!setter);
	}

	//Simple URL Button
	private void ButtonOpenUrl(Texture tex, string url, float width = 32f, float height = 24f)
	{
		if (GUILayout.Button(tex, GUILayout.Width(width), GUILayout.Height(height)))
			Application.OpenURL(url);
	}

	//Draw super thin box as separator
	private void DrawWideBox(float height = 1f)
	{
		GUILayout.Box("", GUILayout.Height(height), GUILayout.ExpandWidth(true));
	}

	#endregion
}