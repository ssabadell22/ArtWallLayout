using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneModelManager : MonoBehaviour
{
    [SerializeField] private OVRSceneManager _ovrSceneManager;
    
    // Start is called before the first frame update
    void Start()
    {
        // From https://blog.learnxr.io/xr-development/meta-scene-understanding-with-unity
        // or SceneManager = GetComponent<OVRSceneManager>();
        // Bind the events associated with LoadSceneModel()
        _ovrSceneManager.SceneModelLoadedSuccessfully += OnSceneModelLoadedSuccessfully;
        _ovrSceneManager.NoSceneModelToLoad += OnNoSceneModelToLoad;
        // Bind the events associated with RequestSceneCapture()
//        _ovrSceneManager.SceneCaptureReturnedWithoutError += OnSceneCaptureReturnedWithoutError;
//        _ovrSceneManager.UnexpectedErrorWithSceneCapture += OnUnexpectedErrorWithSceneCapture;

//        OnStart();
    }
    // From https://blog.learnxr.io/xr-development/meta-scene-understanding-with-unity
    protected virtual void OnSceneModelLoadedSuccessfully()
    {
        // The scene model was captured successfully. At this point all prefabs have been instantiated.
//        _ovrSceneManager.Verbose?.Log(nameof(OVRSceneModelLoader),
//            $"{nameof(OVRSceneManager)}.{nameof(OVRSceneManager.LoadSceneModel)}() completed successfully.");
    }
    // From https://blog.learnxr.io/xr-development/meta-scene-understanding-with-unity
    protected virtual void OnNoSceneModelToLoad()
    {
#if UNITY_EDITOR_WIN
        UnityEditor.EditorUtility.DisplayDialog("Scene capture does not work over Link",
            "There is no scene model available, and scene capture cannot be invoked over link. " +
            "Please capture a scene with the HMD in standalone mode, then access the scene model over Link. " +
            "\n\nIf a scene model has already been captured, make sure the HMD is connected via Link and that it is donned.",
            "Ok");
#else        
        if (_sceneCaptureRequested)
        {
            SceneManager.Verbose?.Log(nameof(OVRSceneModelLoader),
                $"{nameof(OnSceneCaptureReturnedWithoutError)}() There is no scene model, but we have already requested scene capture once. No further action will be taken.");
        }
        else
        {
            // There's no Scene model, we have to ask the user to create one
            SceneManager.Verbose?.Log(nameof(OVRSceneModelLoader),
                $"{nameof(OnNoSceneModelToLoad)}() calling {nameof(OVRSceneManager)}.{nameof(OVRSceneManager.RequestSceneCapture)}()");
            _sceneCaptureRequested = SceneManager.RequestSceneCapture();
        }
#endif
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
