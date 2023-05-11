using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PictureFrameMoveConstrainer : MonoBehaviour
{
    [SerializeField] private Transform _picture;

    private Quaternion _startingRotation;
    private Quaternion _lastRotation;
    
    void Start()
    {
        _startingRotation = _picture.localRotation;
        _lastRotation = _picture.localRotation;
    }
    // Update is called once per frame
    void Update() // might need consider using FixedUpdate
    {
        // Assume starting local rotation is what we want to fix. Don't let it
        // change.
        if (_picture.localRotation == _lastRotation)
            return;
        var locrot = _picture.localRotation;
        locrot.x = _startingRotation.x;
        locrot.z = _startingRotation.z;
        _picture.localRotation = locrot;
        _lastRotation = _picture.localRotation;
    }

}
