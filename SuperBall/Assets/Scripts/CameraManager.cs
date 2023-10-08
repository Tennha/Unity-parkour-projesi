using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;

public class CameraManager : MonoBehaviour
{
    #region Variables

    [SerializeField] private Transform target;
    private float _distanceToPlayer;
    private Vector2 _input;
    private Vector2 _inputJ;

    [SerializeField] private MouseSensitivity mouseSensitivity;
    [SerializeField] private JoystickSensitivity JoystickSensitivity;
    [SerializeField] private CameraAngle cameraAngle;

    private CameraRotation _cameraRotation;

    #endregion

    private void Awake() => _distanceToPlayer = Vector3.Distance(transform.position, target.position);

    public void LookMouse(InputAction.CallbackContext context)
    {
        _input = context.ReadValue<Vector2>();
    } public void LookJoystcik(InputAction.CallbackContext context)
    {
        _inputJ = context.ReadValue<Vector2>();
    }


    private void Update()
    {
        #region mouse:
        _cameraRotation.Yaw += _input.x * mouseSensitivity.horizontal * BoolToInt(mouseSensitivity.invertHorizontal) * Time.deltaTime;
        _cameraRotation.Pitch += _input.y * mouseSensitivity.vertical * BoolToInt(mouseSensitivity.invertVertical) * Time.deltaTime;
        _cameraRotation.Pitch = Mathf.Clamp(_cameraRotation.Pitch, cameraAngle.min, cameraAngle.max);
        #endregion
        #region Joystick:
        _cameraRotation.Yaw += _inputJ.x * JoystickSensitivity.Jhorizontal * BoolToInt(JoystickSensitivity.JinvertHorizontal) * Time.deltaTime;
        _cameraRotation.Pitch += _inputJ.y * JoystickSensitivity.Jvertical * BoolToInt(JoystickSensitivity.JinvertVertical) * Time.deltaTime;
        _cameraRotation.Pitch = Mathf.Clamp(_cameraRotation.Pitch, cameraAngle.min, cameraAngle.max);
        #endregion
    }

    private void LateUpdate()
    {
        transform.eulerAngles = new Vector3(_cameraRotation.Pitch, _cameraRotation.Yaw, 0.0f);
        transform.position = target.position - transform.forward * _distanceToPlayer;
    }

    private static int BoolToInt(bool b) => b ? 1 : -1;
}

[Serializable]
public struct MouseSensitivity
{
    public float horizontal;
    public float vertical;
    public bool invertHorizontal;
    public bool invertVertical;
}
[Serializable]
public struct JoystickSensitivity
{
    public float Jhorizontal;
    public float Jvertical;
    public bool JinvertHorizontal;
    public bool JinvertVertical;
}
public struct CameraRotation
{
    public float Pitch;
    public float Yaw;
}

[Serializable]
public struct CameraAngle
{
    public float min;
    public float max;
}



