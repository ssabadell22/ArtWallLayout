using System.Collections;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

namespace MetaAdvancedFeatures.SceneUnderstanding
{
    public class OVRSceneManagerAddons : MonoBehaviour
    {
        [SerializeField] private Transform[] _pictureFramesToAlign;
        
        protected OVRSceneManager SceneManager { get; private set; }

        private void Awake()
        {
            SceneManager = GetComponent<OVRSceneManager>();
        }

        void Start()
        {
            SceneManager.SceneModelLoadedSuccessfully += OnSceneModelLoadedSuccessfully;
        }

        private void OnSceneModelLoadedSuccessfully()
        {
            StartCoroutine(AddCollidersAndFixClassifications());
        }

        private IEnumerator AddCollidersAndFixClassifications()
        {
            // [Note] jackyangzzh: to avoid racing condition, wait for end of frame
            //                     for all prefabs to be populated properly before continuing
            yield return new WaitForEndOfFrame();

            MeshRenderer[] allObjects = FindObjectsOfType<MeshRenderer>();

            Debug.Log($"Adding box colliders to {allObjects.Length} objects");
            foreach (var obj in allObjects)
            {
                if (obj.GetComponent<Collider>() == null)
                {
                    obj.AddComponent<BoxCollider>();
                }
            }
            
            // fix to desks - for some reason they are upside down with Meta's default code
            // Stew: I think this depends on how it is defined. If you start the desk from the
            // right side, it does not need to be flipped.
            //OVRSemanticClassification[] allClassifications = FindObjectsOfType<OVRSemanticClassification>()
            //    .Where(c => c.Contains(OVRSceneManager.Classification.Desk))
            //    .ToArray();
            //Debug.Log($"Found {allClassifications.Length} desks to flip");
            //foreach(var classification in allClassifications)
            //{
            //    // Fixed this from Dilmer's source
            //    Transform deskTM = classification.transform;
            //    deskTM.localScale = new Vector3(deskTM.localScale.x, deskTM.localScale.y, deskTM.localScale.z * -1);
            //    transform.localScale = deskTM.localScale;
            //}
            
            OVRSemanticClassification[] wallClassifications = FindObjectsOfType<OVRSemanticClassification>()
                .Where(c => c.Contains(OVRSceneManager.Classification.WallFace))
                .ToArray();
            //Debug.Log($"Found {wallClassifications.Length} walls");
            foreach(var classification in wallClassifications)
            {
                // Testing if adding rigidbody to room scene walls will give me collisions
                //if (classification.GetComponent<Rigidbody>() == null)
                //{
                //    Debug.Log("Adding rigidbody to wall");
                //    classification.AddComponent<Rigidbody>();
                //    if (classification.TryGetComponent(out Rigidbody rb))
                //    {
                //        rb.useGravity = false;
                //        rb.isKinematic = true;
                //        rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
                //    }
                //}
                
                // This works but I lost passthrough, so turning it off
                //var mesh = classification.GetComponentInChildren<MeshRenderer>();
                //if (mesh)
                //    mesh.enabled = false;
                //else
                //    Debug.Log("No mesh on wall");
            }

            // doesn't work
            /*
            OVRSemanticClassification[] doorClassifications = FindObjectsOfType<OVRSemanticClassification>()
                .Where(c => c.Contains(OVRSceneManager.Classification.DoorFrame))
                .ToArray();
            //Debug.Log($"Found {wallClassifications.Length} walls");
            foreach(var classification in doorClassifications)
            {
                var doorRot = classification.transform.rotation;
                //doorRot.y += 180f;
                foreach (var obj in _pictureFramesToAlign)
                    obj.rotation = doorRot;
                break;
            }
            */

        }
    }
}
