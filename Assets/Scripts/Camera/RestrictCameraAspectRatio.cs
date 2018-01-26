using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

[RequireComponent(typeof(Camera))]
public class RestrictCameraAspectRatio : MonoBehaviour {

    [SerializeField]
    float _targeAspectRatio = 4/3f;

    [SerializeField]
    [Tooltip("Normalized (0.0-1.0) center for ")]
    [Range(0f, 1f)]
    float _center = 0.5f;

    Camera _camera;
    Rect _originalViewport;

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake() {
        _camera = GetComponent<Camera>();
        Assert.IsNotNull(_camera);
        _originalViewport = _camera.rect;
    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update() {
        if (_camera == null)
            return;
        Rect newViewport = _originalViewport;
        var targetPixelWdith = _targeAspectRatio * _originalViewport.height * (float)Screen.height;
        var normalizedWidth = targetPixelWdith / (float)Screen.width;
        newViewport.x = _center - normalizedWidth / 2f;
        newViewport.width = normalizedWidth;
        _camera.rect = newViewport;
    }

}
