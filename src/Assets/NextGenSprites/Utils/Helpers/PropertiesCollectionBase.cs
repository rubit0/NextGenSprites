// NextGen Sprites (copyright) 2015 Ruben de la Torre, www.studio-delatorre.com

using UnityEngine;
using System.Collections.Generic;

namespace NextGenSprites.PropertiesCollections
{
    /// <summary>
    /// Responsible for handling the Properties Collection and creating the Material cache.
    /// </summary>
    public class PropertiesCollectionBase : MonoBehaviour
    {
        public PropertiesCollection[] PropCollections;
        public Dictionary<string, Material> _cachedMaterials = new Dictionary<string, Material>();

        /// <summary>
        /// Apply all the Properties Collection to the Target Material by its Name/Id.
        /// This method is only used by this base class to generate the Materials for the Cache
        /// </summary>
        /// <param name="PropCollection">Our source helper Prop Collection Dictionary.</param>
        /// <param name="CollectionName">Set the Collection by its Name/Id.</param>
        /// <param name="Target">Target material.</param>
        private void SetProperties(Dictionary<string, PropertiesCollection> PropCollection, string CollectionName, Material Target)
        {
            //Querry the Dictionary by the CollectionId and apply all properties to the Target Material
            if (PropCollection.ContainsKey(CollectionName))
            {
                //Apply all Textures
                foreach (var tex in PropCollection[CollectionName].Textures)
                {
                    Target.SetTexture(tex.Target.GetString(), tex.Value);
                }

                //Apply all Float Values
                foreach (var val in PropCollection[CollectionName].Floats)
                {
                    Target.SetFloat(val.Target.GetString(), val.Value);
                }

                //Apply all Color tints
                foreach (var tints in PropCollection[CollectionName].Tints)
                {
                    Target.SetColor(tints.Target.GetString(), tints.Value);
                }

                //Apply for all Shader Features
                foreach (var feature in PropCollection[CollectionName].Features)
                {
                    if (feature.Value)
                        Target.EnableKeyword(feature.Target.GetString());
                    else
                        Target.DisableKeyword(feature.Target.GetString());
                }
            }
            else
            {
                Debug.LogError("There is no matching Id on this Collection");
            }
        }

        //Fill the Material Cache
        protected void InitMaterialCache(Material source, Dictionary<string, Material> target)
        {
            //This is a helper Dictionary to properly apply the Collection to the Materials
            Dictionary<string, PropertiesCollection> _propertiesDictionary = new Dictionary<string, PropertiesCollection>();

            //Fill the Dictionary with the Properties Collections Array
            foreach (var item in PropCollections)
            {
                _propertiesDictionary.Add(item.CollectionName, item);
            }

            foreach (var item in _propertiesDictionary)
            {
                var maty = new Material(source);
                maty.name = string.Format("{0} - [{1}]", maty.name, item.Key);
                SetProperties(_propertiesDictionary, item.Key, maty);
                target.Add(item.Key, maty);
            }
        }
    }
}