using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MilkPourState : MonoBehaviour
{
    [SerializeField]
    private float _fillRate = 100;

    [SerializeField]
    private float _cameraTransitionDuration = 0.2f;

    [SerializeField]
    private Transform _cameraAnchor;

    [SerializeField]
    private List<MilkPourCup> _cups;

    private int _cupIndex;
    private MilkPourCup _currentCup;
    private gameState _state;
    private float _transitionTime;

    private enum gameState
    {
        Idle,
        Filling,
        CameraTransition,
        Stopped
    }
        
    void Start ()
    {
        _cupIndex = 0;
        _currentCup = _cups[0];
        _state = gameState.Idle;
        _transitionTime = 0;
    }

    void Update ()
    {
        switch (_state)
        {
            case gameState.Stopped:
                return;
            case gameState.CameraTransition:
                _transitionTime += Time.deltaTime;
                if (_transitionTime < _cameraTransitionDuration)
                {
                    // Smooth interpolation between last cup and current cup's X position.
                    _cameraAnchor.position = new Vector3(
                        Mathf.SmoothStep (_cups [_cupIndex - 1].transform.position.x,
                            _cups [_cupIndex].transform.position.x, _transitionTime / _cameraTransitionDuration),
                        _cameraAnchor.position.y,
                        _cameraAnchor.position.z);
                    return;
                }
                else
                {
                    // Snap camera anchor X onto current cup position.
                    _cameraAnchor.position = new Vector3 (_cups [_cupIndex].transform.position.x,
                        _cameraAnchor.position.y, _cameraAnchor.position.z);
                    _transitionTime = 0;
                    _state = gameState.Idle;
                }
                break;
        }

        _state = Input.GetKey (KeyCode.Space) ? gameState.Filling : gameState.Idle;

        switch(_state)
        {
            case gameState.Idle:
                //Having this check only in Idle means that this will not check until player stops filling.
                if (_currentCup.Fill > _currentCup.RequiredFill)
                {
                    // If the cup index isn't the last in the list, change cup and transition.
                    if (_cupIndex + 1 < _cups.Count)
                    {
                        _state = gameState.CameraTransition;
                        _cupIndex++;
                        _currentCup = _cups [_cupIndex];
                    }
                    else
                    {
                        MicrogameController.instance.setVictory(true, true);
                        _state = gameState.Stopped;
                    }
                }
                break;
			case gameState.Filling:
                _currentCup.Fill += _fillRate * Time.deltaTime;
                if (_currentCup.Fill > _currentCup.MaximumFill)
                {
                    MicrogameController.instance.setVictory(false, true);
                    _state = gameState.Stopped;
                }
                break;
        }
    }
}
