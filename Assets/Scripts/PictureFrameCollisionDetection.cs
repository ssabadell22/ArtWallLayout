using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PictureFrameCollisionDetection : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log($"Collision with {collision.gameObject.name}");
        var classified = collision.gameObject.GetComponent<OVRSemanticClassification>();
        if (!classified)
            return;
        Debug.Log("Classified Collision");
        if (!classified.Contains(OVRSceneManager.Classification.WallFace))
            return;
        Debug.Log("Wall Collision");

    }
}
