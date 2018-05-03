using UnityEngine;
using System.Collections;
using System;

namespace NextGenSprites.PropertiesCollections
{
    /// <summary>
    /// The Remote classes are meant to be used when you spawn the same sprite several times from a pool.
    /// </summary>
    [AddComponentMenu("NextGenSprites/Properties Collection/Remote/Controller - Receiver")]
    [RequireComponent(typeof(SpriteRenderer))]
    [HelpURL("http://wiki.next-gen-sprites.com/doku.php?id=scripting:propertiescollection#controller")]
    public class PropertiesCollectionProxyController : MonoBehaviour
    {
        public PropertiesCollectionProxyManager CollectionManager;
        public bool FindManagerByReference;
        public string ManagerReferenceId;

        private SpriteRenderer _spriteRenderer;
        private string _lastId = "";

        void Start()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();

            if (CollectionManager == null)
            {
                if (FindManagerByReference)
                {
                    var man = FindObjectsOfType<PropertiesCollectionProxyManager>();

                    foreach (var item in man)
                    {
                        if (string.CompareOrdinal(ManagerReferenceId, item.ReferenceId) == 0)
                        {
                            CollectionManager = item;
                            break;
                        }
                    }

                    if (CollectionManager = null)
                    {
                        Debug.LogError(string.Format("Could not find an Manager with the Id: {0}", ManagerReferenceId));
                    }
                }
                else
                {
                    Debug.LogError("There is no Manager assigned!");
                }
            }
        }

        /// <summary>
        /// Update the Material by giving the desired Properties Collection Name as a string.
        /// </summary>
        /// <param name="CollectionName">Make sure to match the name correctly</param>
        public void UpdateMaterial(string CollectionName)
        {
            //Check if the Collection has been already applied the last time
            if (string.CompareOrdinal(CollectionName, _lastId) == 0)
            {
                return;
            }
            else
            {
                //Check if the Manager has been initialised in first place
                if (CollectionManager._cachedMaterials.Count < 1)
                    CollectionManager.InitManager();

                //Apply the Material by the correct Id Name from the Properties Collection Asset
                if (CollectionManager._cachedMaterials.ContainsKey(CollectionName))
                {
                    _spriteRenderer.material = CollectionManager._cachedMaterials[CollectionName];
                }

                _lastId = CollectionName;
            }
        }

        /// <summary>
        /// Updates the Material 'smoothly' by lerping over a period of time.
        /// </summary>
        /// <param name="CollectionName">The Collection we need to match</param>
        /// <param name="LerpDuration">Duration for the lerp in seconds</param>
        public void UpdateMaterialSmooth(string CollectionName, float LerpDuration = 1f)
        {
            //Check if the Collection has been already applied the last time
            if (string.CompareOrdinal(CollectionName, _lastId) == 0)
            {
                return;
            }
            else
            {
                //Check if the Manager has been initialised in first place
                if (CollectionManager._cachedMaterials.Count < 1)
                    CollectionManager.InitManager();

                //Apply the Material by the correct Id Name from the Properties Collection Asset
                if (CollectionManager._cachedMaterials.ContainsKey(CollectionName))
                {
                    var sourceMat = _spriteRenderer.material;
                    var targetMat = CollectionManager._cachedMaterials[CollectionName];

                    StartCoroutine(SmoothMaterialLerp(sourceMat, targetMat, LerpDuration));
                }

                _lastId = CollectionName;
            }
        }

        /// <summary>
        /// !DO NOT USE THIS OVERLOAD!
        /// This overload is intented to be invoked by a OnClick GUI action through an Inspector.
        /// First the collections name and then the duration in seconds, you must seperate them by a colon.
        /// E.g.: "IdleAnimationSet : 1.5"
        /// Updates the Material 'smoothly' by lerping over a period of time.
        /// </summary>
        /// <param name="Arguments">Arguments are separated by colon. First collection name and then duration.</param>
        public void UpdateMaterialSmooth(string Arguments)
        {
            //create an array from the arguments string separated by a colon.
            var argumentsSplitted = Arguments.Split(new[] { ':', ' ' }, StringSplitOptions.RemoveEmptyEntries);

            if (argumentsSplitted.Length > 1)
            {
                float time;
                //convert string to a float
                float.TryParse(argumentsSplitted[1], out time);

                //if the conversion fails TryParse() will return -1 which we check here
                if (time == 0)
                {
                    Debug.LogWarning("Invalid Time argument. Check spelling?");
                }
                else
                {
                    var CollectionName = argumentsSplitted[0];
                    var LerpDuration = time;

                    //Check if the Collection has been already applied the last time
                    if (string.CompareOrdinal(CollectionName, _lastId) == 0)
                    {
                        return;
                    }
                    else
                    {
                        //Check if the Manager has been initialised in first place
                        if (CollectionManager._cachedMaterials.Count < 1)
                            CollectionManager.InitManager();

                        //Apply the Material by the correct Id Name from the Properties Collection Asset
                        if (CollectionManager._cachedMaterials.ContainsKey(CollectionName))
                        {
                            var sourceMat = _spriteRenderer.material;
                            var targetMat = CollectionManager._cachedMaterials[CollectionName];

                            StartCoroutine(SmoothMaterialLerp(sourceMat, targetMat, LerpDuration));
                        }

                        _lastId = CollectionName;
                    }
                }
            }
        }

        IEnumerator SmoothMaterialLerp(Material origin, Material target, float duration)
        {
            var elapsedTime = 0f;

            while (elapsedTime < duration)
            {
                _spriteRenderer.material.Lerp(origin, target, (elapsedTime / duration));
                elapsedTime += Time.deltaTime;
                yield return null;
            }
        }
    } 
}
