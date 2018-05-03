using UnityEngine;
using System;
using System.Collections.Generic;

namespace NextGenSprites
{
    public class DualMaterial
    {
        #region Custom Data Objects
        private class FloatPropertyRange
        {
            public ShaderFloat Property { get; set; }
            public float ValueMain { get; set; }
            public float ValueSecond { get; set; }
        }

        private class ColorPropertyRange
        {
            public ShaderColor Property { get; set; }
            public Color ValueMain { get; set; }
            public Color ValueSecond { get; set; }
        }
        #endregion

        /// <summary>
        /// Return the result of the two Material.
        /// </summary>
        public Material FusedMaterial { get { return _mainMaterial; } }

        private float _lerpAmount = 0f;

        /// <summary>
        /// Interpolate between the two internal Materials. The lerp amount is clamped to the range [0, 1].
        /// </summary>
        public float Lerp
        {
            get { return _lerpAmount; }
            set
            {
                _lerpAmount = Mathf.Clamp01(value);
                LerpMaterial();
            }
        }


        private Material _mainMaterial;
        private FloatPropertyRange[] FloatPropertyValues;
        private ColorPropertyRange[] ColorPropertyValues;

        /// <summary>
        /// The Dual Material will contain float and color properties of the supplied two Materials.
        /// You can Lerp between the two Materials via the MaterialLerp() method.
        /// Note: Both supplied Materials must use the same NextGenSprites shader.
        /// </summary>
        /// <param name="firstMaterial">First donor Material. This is also the dominant Material.</param>
        /// <param name="secondMaterial">Second donor Material.</param>
        /// <param name="materialName">Name of the new Material.</param>
        public DualMaterial(Material firstMaterial, Material secondMaterial, string materialName = "New Dual Material")
        {
            if(firstMaterial && secondMaterial)
            {
                //Check if both Material are the same
                if (string.CompareOrdinal(firstMaterial.shader.name, secondMaterial.shader.name) != 0)
                {
                    Debug.LogError("Invalid Materials. Both must use the same NextGenSprite shader");
                    _mainMaterial = null;
                }

                //Create the new Main Material
                _mainMaterial = new Material(Shader.Find(firstMaterial.shader.name));
                _mainMaterial.CopyPropertiesFromMaterial(firstMaterial);
                _mainMaterial.name = materialName;
                SetKeywords(firstMaterial);

                BuildProperties(firstMaterial, secondMaterial);
            }
            else
            {
                Debug.LogError("One or both of the provided Materials are null");
                _mainMaterial = null;
            }
        }



        #region Methods
        /// <summary>
        /// Linearly interpolates between the two Materials.
        /// </summary>
        private void LerpMaterial()
        {
            //Lerp Floats
            foreach (var item in FloatPropertyValues)
            {
                var target = Mathf.Lerp(item.ValueMain, item.ValueSecond, _lerpAmount);
                _mainMaterial.SetFloat(item.Property.GetString(), target);
            }

            //Lerp Colors
            foreach (var item in ColorPropertyValues)
            {
                var target = Color.Lerp(item.ValueMain, item.ValueSecond, _lerpAmount);
                _mainMaterial.SetColor(item.Property.GetString(), target);
            }
        }

        /// <summary>
        /// Get all NextGenSprites properties and assign them to the array.
        /// </summary>
        /// <param name="firstMat">first material</param>
        /// <param name="secondMat">second material</param>
        private void BuildProperties(Material firstMat, Material secondMat)
        {
            var FloatsList = new List<FloatPropertyRange>();
            var ColorsList = new List<ColorPropertyRange>();

            //Float Properties
            //We cast the Enum to an Array to use it lenghts for iterating through it
            var floatNames = Enum.GetNames(typeof(ShaderFloat));
            foreach (var item in floatNames)
            {
                //Parse the string back to the Enum
                var enumParsed = (ShaderFloat)Enum.Parse(typeof(ShaderFloat), item);

                if (firstMat.HasProperty(enumParsed.GetString()))
                {
                    var target = new FloatPropertyRange();

                    target.Property = enumParsed;
                    target.ValueMain = firstMat.GetFloat(enumParsed.GetString());
                    target.ValueSecond = secondMat.GetFloat(enumParsed.GetString());
                    FloatsList.Add(target);
                }
            }

            //Tints
            //We cast the Enum to an Array to use it lenghts for iterating through it
            var colorNames = Enum.GetNames(typeof(ShaderColor));
            foreach (var item in colorNames)
            {
                //Parse the string back to the Enum
                var enumParsed = (ShaderColor)Enum.Parse(typeof(ShaderColor), item);

                if (firstMat.HasProperty(enumParsed.GetString()))
                {
                    var target = new ColorPropertyRange();

                    target.Property = enumParsed;
                    target.ValueMain = firstMat.GetColor(enumParsed.GetString());
                    target.ValueSecond = secondMat.GetColor(enumParsed.GetString());
                    ColorsList.Add(target);
                }
            }

            FloatPropertyValues = FloatsList.ToArray();
            ColorPropertyValues = ColorsList.ToArray();
        }

        /// <summary>
        /// Enable all the keywords on the main material, based on the source material.
        /// </summary>
        /// <param name="sourceMaterial">Source material to check from.</param>
        private void SetKeywords(Material sourceMaterial)
        {
            var keywordNames = Enum.GetNames(typeof(ShaderFeature));
            foreach (var item in keywordNames)
            {
                //Parse the string back to the Enum
                var enumParsed = (ShaderFeature)Enum.Parse(typeof(ShaderFeature), item);

                if (sourceMaterial.IsKeywordEnabled(enumParsed.GetString()))
                {
                    _mainMaterial.EnableKeyword(enumParsed.GetString());
                }
            }
        }
        #endregion
    }
}
