using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class ContinousMovement : MonoBehaviour
{
    public float CharacterSpeed = 1f;

    [SerializeField] private float _addHeight = 0.2f;
    [SerializeField] private XRNode _inputsource;
    [SerializeField] private float _gravity = -9.81f;
    [SerializeField] private LayerMask _groundLayer;

    private float _fallSpeed;
    private Vector2 _inputAxis;
    private CharacterController _character;
    private XRRig _rig;


    private void Start()
    {
        _rig = GetComponent<XRRig>();
        _character = GetComponent<CharacterController>();
    }
    private void Update()
    {
        GetInputAxis();
    }
    private void FixedUpdate()
    {
        MoveCharacter();
    }

    private void GetInputAxis()
    {
        //TODO: Oculus devices use primary2DAxis as joystick input instead, find a way to switch between the two depending on device connected
        InputDevice device = InputDevices.GetDeviceAtXRNode(_inputsource);
        device.TryGetFeatureValue(CommonUsages.secondary2DAxis, out _inputAxis);
    }

    private void MoveCharacter()
    {
        CharacterHeightFollowHeadset();
        Quaternion headYaw = Quaternion.Euler(0, _rig.cameraGameObject.transform.eulerAngles.y, 0); //Gets rotation of head
        Vector3 moveDirection = headYaw * new Vector3(_inputAxis.x, 0, _inputAxis.y); //multiplies the rotation of head with movement direction
        _character.Move(moveDirection * Time.fixedDeltaTime * CharacterSpeed);
        MimicGravity();
    }

    private void MimicGravity()
    {
        if (!GroundCheck())
        {
            _fallSpeed += _gravity * Time.fixedDeltaTime;
        }
        else { _fallSpeed = 0; }

        _character.Move(Vector3.up * _fallSpeed * Time.fixedDeltaTime);
    }

    private bool GroundCheck()
    {
        Vector3 rayStartPos = transform.TransformPoint(_character.center);
        float rayLength = _character.center.y + 0.01f;
        return Physics.SphereCast(rayStartPos, _character.radius, Vector3.down, out RaycastHit hitInfo, rayLength, _groundLayer);
    }
    private void CharacterHeightFollowHeadset()
    {
        _character.height = _rig.cameraInRigSpaceHeight + _addHeight;
        Vector3 capsuleCenter = transform.InverseTransformPoint(_rig.cameraGameObject.transform.position);
        _character.center = new Vector3(capsuleCenter.x, _character.height / 2 + _character.skinWidth, capsuleCenter.z);
    }
}
