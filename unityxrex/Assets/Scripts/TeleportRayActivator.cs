using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class TeleportRayActivator : MonoBehaviour
{
    [SerializeField] private XRController _leftTeleportRay;
    [SerializeField] private XRController _rightTeleportRay;
    [SerializeField] private InputHelpers.Button _teleportActivationBtn;
    [SerializeField] private float _activationThreshold = 0.1f;

    void Update()
    {
        CheckForRayActivation(_leftTeleportRay);
        CheckForRayActivation(_rightTeleportRay);
    }
    private void CheckForRayActivation(XRController controller)
    {
        if (controller)
        {
            controller.gameObject.SetActive(CheckIfTeleportActivated(controller));
        }
    }
    private bool CheckIfTeleportActivated(XRController controller)
    {
        InputHelpers.IsPressed(controller.inputDevice, _teleportActivationBtn, out bool isActive, _activationThreshold);
        return isActive;
    }


}
