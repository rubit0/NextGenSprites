// NextGenSprites (copyright) 2015 Ruben de la Torre, www.studio-delatorre.com

using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEditor;

namespace NextGenSprites.Widget
{
    public class NgsWidget : EditorWindow
	{
		private string _userPath;
		private bool _useCustomFolder;
	    private SpriteRenderer _sRend;

		private string _productName;
		private bool _showShadows;
        private bool _showSettings;
	    private bool _isSprite;
        private bool _isNgs;

        [MenuItem ("Window/NextGenSprites/Widget")]
		private static void Init ()
		{
            var window = (NgsWidget)EditorWindow.GetWindow (typeof(NgsWidget),false, "NGS Widget");
            window.minSize = new Vector2(235f, 100f);
			window.Show ();
		}

		private void OnEnable ()
		{
			_productName = PlayerSettings.productName;

            //Load the user settings from Editor Prefs
			if (EditorPrefs.HasKey("NGS_UseFolder" + _productName))
		    {
				_useCustomFolder = EditorPrefs.GetBool("NGS_UseFolder" + _productName);
				_userPath = EditorPrefs.GetString("NGS_UserPath" + _productName);
            }
		    else
		    {
                //Otherwise we set custom folder to false
		        _useCustomFolder = false;
				EditorPrefs.SetBool("NGS_UseFolder" + _productName, _useCustomFolder);

                //Better safe then sorry, set the default custom folder to the root assets folder
				EditorPrefs.SetString("NGS_UserPath" + _productName, "Assets");
		    }
        }

        private void OnGUI()
	    {
            var inHierarchy = (Selection.activeObject != null && AssetDatabase.Contains(Selection.activeObject) == false);
            _sRend = (inHierarchy) ? Selection.activeGameObject.GetComponent<SpriteRenderer>() : null;
            _isSprite = (_sRend != null);

            if (_isSprite)
                _isNgs = HasNgsShader();
            else
                _isNgs = false;

            GUILayout.BeginHorizontal();
            EditorGUI.BeginDisabledGroup((_isSprite && !_isNgs) ? false : true);
            if (GUILayout.Button("Add Multi\n Lit Material"))
                ControlAddMaterial("Multi");
            if (GUILayout.Button("Add Single\n Lit Material"))
                ControlAddMaterial("Single");
            if (GUILayout.Button("Add\n Unlit Material"))
                ControlAddMaterial("Unlit");
            EditorGUI.EndDisabledGroup();
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            EditorGUI.BeginDisabledGroup((_isNgs) ? false : true);
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("Apply [to Selection]\n Properties Collection"))
                EditorApplication.ExecuteMenuItem("GameObject/NextGenSprites/Apply Properties Collection to selection");
            if (GUILayout.Button("Generate [from Selection]\n Properties Collection"))
                EditorApplication.ExecuteMenuItem("Assets/Create/NextGenSprites/Properties Collection from selection");
            EditorGUI.EndDisabledGroup();
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("Generate [Blank]\n Properties Collection"))
                EditorApplication.ExecuteMenuItem("Assets/Create/NextGenSprites/Properties Collection");
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();

            //Shadow Setter
            //Auto close shadow foldout if selection is not a sprite
            if (!_isSprite) { _showShadows = false; }

            EditorGUI.BeginDisabledGroup(!_isSprite);
            _showShadows = EditorGUILayout.Foldout(_showShadows, "Shadows");
            if (_showShadows)
            {
                if (_isSprite)
                    ControlShadows();
            }
            EditorGUI.EndDisabledGroup();

            //Settings
            _showSettings = EditorGUILayout.Foldout(_showSettings, "Settings");
            if (_showSettings)
                ControlSettings();
        }

		/// <summary>
		/// Returns a bool. Check if the Shared Material contains a NGS Shader
		/// </summary>
		private bool HasNgsShader ()
		{
			var shaderName = _sRend.sharedMaterial.shader.name;
		    if (shaderName.IndexOf("/", StringComparison.Ordinal) != -1)
		    {
			    var seek = shaderName.Substring (0, shaderName.IndexOf ('/'));
                return seek == "NextGenSprites";
            }
		    else
		    {
		        return false;
		    }
		}

		#region Controller GUIs

		//Create and attach a new Material and also save it on Disk
		private void ControlAddMaterial (string LitType)
		{
			GUILayout.BeginHorizontal ();
	
			var targetPath = "";
	
			if (_useCustomFolder)
            {
				var texName = _sRend.sprite.name;
	
				var relativePath = _userPath.Substring (_userPath.IndexOf ("Assets", StringComparison.Ordinal));
	
				targetPath += (relativePath + "/" + texName + ".mat");
			}
            else
            {
				var path = AssetDatabase.GetAssetPath (_sRend.sprite.texture).Split ('.');
				path [1] = ".mat";
				for (int i = 0; i < path.Length; i++)
                {
					targetPath += path [i];
				}
			}
			var material = new Material (Shader.Find ("NextGenSprites/Standard/" + LitType));
			AssetDatabase.CreateAsset (material, targetPath);
			Debug.Log ("A happy NextGenSprites Material has been stored at:\n " + AssetDatabase.GetAssetPath (material));
			_sRend.sharedMaterial = material;
			GUILayout.EndHorizontal ();
		}
        
	
		//Settings for the Widget and Saving of Materials
		private void ControlSettings ()
		{
            GUILayout.Space(10f);
            GUILayout.Label ("Material Save Folder", EditorStyles.boldLabel);
			DrawWideBox ();
			GUILayout.Space (5f);
	
	
			_useCustomFolder = EditorGUILayout.Toggle ("Custom Folder", _useCustomFolder);
            if(!_useCustomFolder)
                GUILayout.Label("Material will be saved along the Sprite", EditorStyles.helpBox);

            if (_useCustomFolder)
            {
				var showFolder = _userPath.Substring (_userPath.IndexOf ("Assets", StringComparison.Ordinal));
	
				EditorGUILayout.LabelField (("Path: " + showFolder), EditorStyles.whiteLabel);
				GUILayout.Space (5f);
				if (GUILayout.Button ("Select Folder"))
                {
					_userPath = EditorUtility.OpenFolderPanel ("Select your default Material Folder", "Assets", "");
                    if (string.IsNullOrEmpty(_userPath))
                        _userPath = "Assets";
					
					EditorPrefs.SetString ("NGS_UserPath" + _productName, _userPath);
				}
			}
			EditorPrefs.SetBool ("NGS_UseFolder" + _productName, _useCustomFolder);

            GUILayout.Space (5f);
            DrawWideBox();
			GUILayout.Space (15f);
		}

		private void ControlShadows ()
		{
			var castEnable = (_sRend.shadowCastingMode == ShadowCastingMode.On);
			var receiveEnable = _sRend.receiveShadows;
			var lblBtnCast = castEnable ? "Disable Cast Shadows" : "Enable Cast Shadows";
			var lblBtnRcv = receiveEnable ? "Disable Receive Shadows" : "Enable Receive Shadows";
	
			GUILayout.Label ("Shadow Settings", EditorStyles.boldLabel);
			DrawWideBox ();
			GUILayout.Space (5f);
			GUILayout.Label ("Note: Receiving Shadows has currently no effect.", EditorStyles.helpBox);
			if (GUILayout.Button (lblBtnCast))
            {
				_sRend.shadowCastingMode = !castEnable ? ShadowCastingMode.On : ShadowCastingMode.Off;
			}
			if (GUILayout.Button (lblBtnRcv))
            {
				_sRend.receiveShadows = !receiveEnable;
			}
	
			GUILayout.Space (5f);
			DrawWideBox ();
            GUILayout.Space(15f);
        }
        #endregion

        /// <summary>
        /// Draws super thin box as a visual separator
        /// </summary>
        /// <param name="height">Height of the Box</param>
        private void DrawWideBox (float height = 1f)
		{
			GUILayout.Box ("", GUILayout.Height (height), GUILayout.ExpandWidth (true));
		}

	}
}