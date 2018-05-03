// NextGen Sprites (copyright) 2015 Ruben de la Torre, www.studio-delatorre.com

using UnityEngine;

namespace NextGenSprites.PropertiesCollections
{
    [System.Serializable]
    [HelpURL("http://wiki.next-gen-sprites.com/doku.php?id=scripting:propertiescollection")]
    public class PropertiesCollection : ScriptableObject
    {
        [System.Serializable]
        public class TextureTargets
        {
            public ShaderTexture Target;
            public Texture Value;
        }

        [System.Serializable]
        public class FloatTargets
        {
            public ShaderFloat Target;
            public float Value;
        }

        [System.Serializable]
        public class Vector4Targets
        {
            public ShaderVector4 Target;
            public Vector4 Value;
        }

        [System.Serializable]
        public class TintTargets
        {
            public ShaderColor Target;
            public Color Value;
        }

        [System.Serializable]
        public class FeatureTargets
        {
            public ShaderFeatureRuntime Target;
            public bool Value;
        }

        public string CollectionName;
        public TextureTargets[] Textures;
        public FloatTargets[] Floats;
        public Vector4Targets[] Vector4s;
        public TintTargets[] Tints;
        public FeatureTargets[] Features;
    }
}
