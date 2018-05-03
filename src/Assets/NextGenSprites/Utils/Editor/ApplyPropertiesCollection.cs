// NextGen Sprites (copyright) 2015 Ruben de la Torre, www.studio-delatorre.com

using UnityEngine;
using UnityEditor;
using NextGenSprites.PropertiesCollections;
using System;

namespace NextGenSprites.Helpers
{
	//This is a helper class to apply the Props from Editor elements
	public class ApplyPropertiesCollection : ScriptableObject
	{
		[MenuItem("GameObject/NextGenSprites/Apply Properties Collection to selection")]
		public static void ApplyPropertyCollection()
		{
			var inHierarchy = (Selection.activeObject != null && AssetDatabase.Contains(Selection.activeObject) == false);
			var _sRend = (inHierarchy) ? Selection.activeGameObject.GetComponent<SpriteRenderer>() : null;

			if (_sRend && CheckSelection(_sRend))
			{
				var sourcePath = EditorUtility.OpenFilePanel(
					string.Format("Load Propterties Collection to [{0}]", Selection.activeGameObject.name),
					"Assets",
					"asset");

				if (string.IsNullOrEmpty(sourcePath))
					return;

				//Make Path relative
				sourcePath = sourcePath.Substring(sourcePath.IndexOf("Assets", StringComparison.Ordinal));

				var obj = AssetDatabase.LoadAssetAtPath(sourcePath, typeof(PropertiesCollection)) as PropertiesCollection;
				var targetMaterial = _sRend.sharedMaterial;

				//Apply it all to the Material of the selected Object
				foreach (var item in obj.Floats)
				{
					targetMaterial.SetFloat(item.Target.GetString(), item.Value);
				}

				foreach (var item in obj.Vector4s)
				{
					targetMaterial.SetVector(item.Target.GetString(), item.Value);
				}

				foreach (var item in obj.Tints)
				{
					targetMaterial.SetColor(item.Target.GetString(), item.Value);
				}

				foreach (var item in obj.Textures)
				{
					targetMaterial.SetTexture(item.Target.GetString(), item.Value);
				}

				foreach (var item in obj.Features)
				{
					if (item.Value)
						targetMaterial.EnableKeyword(item.Target.GetString());
					else
						targetMaterial.DisableKeyword(item.Target.GetString());
				}

				Debug.Log(string.Format("Property Collection {0} has been applied to the target Material {1}", obj.CollectionName, targetMaterial.name));
			}
			else
			{
				Debug.LogWarning("The selected GameObject is not a Sprite or has no Material with a NextGenSprites Shader.");
				return;
			}
		}

		//Check if the Sprite Renderer has a NGS Shader
		private static bool CheckSelection(SpriteRenderer _sRend)
		{
			var shaderName = _sRend.sharedMaterial.shader.name;
			if (shaderName.IndexOf("/", StringComparison.Ordinal) != -1)
			{
				var seek = shaderName.Substring(0, shaderName.IndexOf('/'));
				if (string.CompareOrdinal(seek, "NextGenSprites") == 0)
				{
					return true;
				}
				else
				{
					Debug.LogWarning("The Material of the selected GameObject has no NextGenSprites Shader.");
					return false;
				}
			}
			else
			{
				return false;
			}
		}
	} 
}