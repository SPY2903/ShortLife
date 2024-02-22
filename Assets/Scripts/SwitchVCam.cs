using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SwitchVCam : MonoBehaviour
{
    [SerializeField] private PlayerInput playerInput;
    [SerializeField] private int priorityBoostAmount = 10;
    [SerializeField] private Canvas canvas3rdPerson;
    [SerializeField] private Canvas canvasAim;
    private CinemachineVirtualCamera virtualCamera;
    private InputAction inputAction;
    private void Awake()
    {
        virtualCamera = GetComponent<CinemachineVirtualCamera>();
        inputAction = playerInput.actions["Aim"];
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void OnEnable()
    {
        inputAction.performed += _ => StartAim();
        inputAction.canceled += _ => CancelAim();
    }

    private void OnDisable()
    {
        inputAction.performed -= _ => StartAim();
        inputAction.canceled -= _ => CancelAim();
    }

    private void StartAim()
    {
        virtualCamera.Priority += priorityBoostAmount;
        canvas3rdPerson.enabled = false;
        canvasAim.enabled = true;
    }
    private void CancelAim()
    {
        virtualCamera.Priority -= priorityBoostAmount;
        canvas3rdPerson.enabled = true;
        canvasAim.enabled = false;
    }
}
