// NextGen Sprites (copyright) 2015 Ruben de la Torre, www.studio-delatorre.com
// This is a simple class to call UpdateMaterial() from outside.

using UnityEngine;
using NextGenSprites;

namespace NextGenSprites.PropertiesCollections
{
    /// <summary>
    /// The Remote classes are meant to be used when you spawn the same sprite several times from a pool.
    /// </summary>
    [AddComponentMenu("NextGenSprites/Properties Collection/Remote/Manager - Host")]
    [HelpURL("http://wiki.next-gen-sprites.com/doku.php?id=scripting:propertiescollection#manager")]
    public class PropertiesCollectionProxyManager : PropertiesCollectionBase
    {
        public string ReferenceId = "GIVE ME A NAME";
        public bool TargetThis = true;
        public SpriteRenderer SourceObject;

        void Start()
        {
            //Fill the Dictionary with the Collection before Play to prevent hickups
            InitManager();
        }

        /// <summary>
        /// Initialise the Manager. Best practise is to call it at Start or Awake.
        /// </summary>
        public void InitManager()
        {
            //First check if we have any Properties Collections in our array
            if (PropCollections.Length == 0)
            {
                Debug.LogError("There are no Properties Collections assigned!");
                return;
            }
            else
            {
                //Check if none of the Slots in the array is empty/null
                for (int i = 0; i < PropCollections.Length; i++)
                {
                    if (PropCollections[i] == null)
                    {
                        Debug.LogError(string.Format("No Properties Collection assigned at Element {0}!", i));
                        return;
                    }
                }
            }

            //Get the Sprite Renderer
            if (TargetThis)
            {
                SourceObject = GetComponent<SpriteRenderer>();
            }
            else
            {
                if (SourceObject == null)
                {
                    Debug.LogError("There is no Target Object assigned!");
                    return;
                }
            }

            //We cache the Materials by our Properties Collections
            InitMaterialCache(GetComponent<SpriteRenderer>().sharedMaterial, _cachedMaterials);
        }
    } 
}
