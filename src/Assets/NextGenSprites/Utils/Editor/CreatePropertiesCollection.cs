// NextGen Sprites (copyright) 2015 Ruben de la Torre, www.studio-delatorre.com

using UnityEngine;
using UnityEditor;
using NextGenSprites.PropertiesCollections;
using System;
using System.Collections.Generic;

namespace NextGenSprites.Helpers
{
    public class CreatePropertiesCollection
    {
        [MenuItem("Assets/Create/NextGenSprites/Properties Collection")]
        public static void CreateCollection()
        {
            var path = EditorUtility.SaveFilePanelInProject(
                "Save a blank Properties Collection",
                "PropsCollection_New.asset",
                "asset",
                "Save a new Properties Collection");

            if (string.IsNullOrEmpty(path))
                return;

            var obj = ScriptableObject.CreateInstance<PropertiesCollection>();
            AssetDatabase.CreateAsset(obj, path);
            Debug.Log(string.Format("A happy new Properties Collection has been saved in:\n " + path));
            Selection.activeObject = obj;
        }

        [MenuItem("Assets/Create/NextGenSprites/Properties Collection from selection")]
        public static void CreateCollectionFromSelection()
        {
            var inHierarchy = (Selection.activeObject != null && AssetDatabase.Contains(Selection.activeObject) == false);
            var _sRend = (inHierarchy) ? Selection.activeGameObject.GetComponent<SpriteRenderer>() : null;

            if(_sRend && CheckSelection(_sRend))
            {
                CreatCollectionFromMaterial(_sRend.sharedMaterial);
            }
            else
            {
                Debug.LogWarning("The selected GameObject is not a Sprite.");
                return;
            }
        }

        //This method usually get's invoked by the Widget
        public static void CreatCollectionFromMaterial(Material sourceMat)
        {
            //Set up the path and give a prompt
            var path = EditorUtility.SaveFilePanelInProject(
                string.Format("Save a new Properties Collection from [{0}]", Selection.activeGameObject.name),
                string.Format("PropsCollection_{0}.asset", sourceMat.name),
                "asset",
                string.Format("Save a new Properties Collection from [{0}]", sourceMat.name));

            //Abbort if the user presses the Cancel button
            if (string.IsNullOrEmpty(path))
                return;

            //Create a new Properties Collection object
            var obj = ScriptableObject.CreateInstance<PropertiesCollection>();

            //We make some helper lists that will cast later back into an array
            var listFloatTargets = new List<PropertiesCollection.FloatTargets>();
            var listTintTargets = new List<PropertiesCollection.TintTargets>();
            var listTextureTargets = new List<PropertiesCollection.TextureTargets>();
            var listFeatureTargets = new List<PropertiesCollection.FeatureTargets>();
            var listVector4Targets = new List<PropertiesCollection.Vector4Targets>();

            //Float Properties
            //We cast the Enum to an Array to use it lenghts for iterating through it
            var floatNames = Enum.GetNames(typeof(ShaderFloat));
            foreach (var item in floatNames)
            {
                //Parse the string back to the Enum
                var enumParsed = (ShaderFloat)Enum.Parse(typeof(ShaderFloat), item);

                if (sourceMat.HasProperty(enumParsed.GetString()))
                {
                    var target = new PropertiesCollection.FloatTargets();

                    target.Target = enumParsed;
                    target.Value = sourceMat.GetFloat(enumParsed.GetString());
                    listFloatTargets.Add(target);
                }
            }

            //Vector4 Properties
            //We cast the Enum to an Array to use it lenghts for iterating through it
            var vector4Names = Enum.GetNames(typeof(ShaderVector4));
            foreach (var item in vector4Names)
            {
                //Parse the string back to the Enum
                var enumParsed = (ShaderVector4)Enum.Parse(typeof(ShaderVector4), item);


                if(sourceMat.HasProperty(enumParsed.GetString()))
                {
                    var target = new PropertiesCollection.Vector4Targets();

                    target.Target = enumParsed;
                    target.Value = sourceMat.GetVector(enumParsed.GetString());
                    listVector4Targets.Add(target);
                }
            }

            //Tints
            //We cast the Enum to an Array to use it lenghts for iterating through it
            var colorNames = Enum.GetNames(typeof(ShaderColor));
            foreach (var item in colorNames)
            {
                //Parse the string back to the Enum
                var enumParsed = (ShaderColor)Enum.Parse(typeof(ShaderColor), item);

                if (sourceMat.HasProperty(enumParsed.GetString()))
                {
                    var target = new PropertiesCollection.TintTargets();

                    target.Target = enumParsed;
                    target.Value = sourceMat.GetColor(enumParsed.GetString());
                    listTintTargets.Add(target);
                }
            }

            //Textures
            //We cast the Enum to an Array to use it lenghts for iterating through it
            var texNames = Enum.GetNames(typeof(ShaderTexture));
            foreach (var item in texNames)
            {
                //Parse the string back to the Enum
                var enumParsed = (ShaderTexture)Enum.Parse(typeof(ShaderTexture), item);

                if (sourceMat.HasProperty(enumParsed.GetString()))
                {
                    var target = new PropertiesCollection.TextureTargets();

                    target.Target = enumParsed;
                    var tex = sourceMat.GetTexture(enumParsed.GetString());
                    if(tex != null)
                        target.Value = tex;
                    
                    listTextureTargets.Add(target);
                }
            }

            //Keywords
            //We cast the Enum to an Array to use it lenghts for iterating through it
            var featureNames = Enum.GetNames(typeof(ShaderFeatureRuntime));
            foreach (var item in featureNames)
            {
                //Parse the string back to the Enum
                var enumParsed = (ShaderFeatureRuntime)Enum.Parse(typeof(ShaderFeatureRuntime), item);

                if(sourceMat.IsKeywordEnabled(enumParsed.GetString()))
                {
                    var target = new PropertiesCollection.FeatureTargets();

                    target.Target = enumParsed;
                    target.Value = true;

                    listFeatureTargets.Add(target);
                }
            }

            //Set the Collection Name by the Material Name
            obj.CollectionName = sourceMat.name;

            //Fill the Properties Collection Array from the Helper Lists
            obj.Floats = listFloatTargets.ToArray();
            obj.Vector4s = listVector4Targets.ToArray();
            obj.Tints = listTintTargets.ToArray();
            obj.Textures = listTextureTargets.ToArray();
            obj.Features = listFeatureTargets.ToArray();

            //Store the object as Asset on our Project
            AssetDatabase.CreateAsset(obj, path);
            Debug.Log(string.Format("A happy new Properties Collection has been saved in:\n " + path));

            //We are done and focus the active object in the Editor to this
            Selection.activeObject = obj;
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
