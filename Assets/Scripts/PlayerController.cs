using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
[RequireComponent(typeof(CharacterController),typeof(PlayerInput))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private float playerSpeed = 2f;
    [SerializeField] private float speedUp = 10f;
    [SerializeField] private float jumpHeight = 1f;
    [SerializeField] private float gravityValue = -9.81f;
    [SerializeField] private float rotationSpeed = 5f;
    [SerializeField] private Transform aimTransform;
    [SerializeField] private float aimDistance= 10f;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform barrelTransform;
    [SerializeField] private Transform bulletParent;
    [SerializeField] private float bulletHitMissDistance = 25f;
    [SerializeField] private ScriptableobjectPlayer playerData;

    private CharacterController controller;
    private PlayerInput playerInput;
    private Transform cameraTransform;
    private InputAction moveAction;
    private InputAction lookAction;
    private InputAction aimAction;
    private InputAction shootAction;
    private InputAction shiftAction;
    private Animator anim;
    private Vector3 playerVelocity;
    private float currentSpeed;
    private bool groundedPlayer;

    // Start is called before the first frame update
    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        playerInput = GetComponent<PlayerInput>();
        anim = GetComponent<Animator>();
        cameraTransform = Camera.main.transform;
        moveAction = playerInput.actions["Move"];
        lookAction = playerInput.actions["Look"];
        aimAction = playerInput.actions["Aim"];
        shootAction = playerInput.actions["Shoot"];
        shiftAction = playerInput.actions["Shift"];
        currentSpeed = playerSpeed;
        Cursor.lockState = CursorLockMode.Locked;

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(playerData._health != 0)
        {
            aimTransform.position = cameraTransform.position + cameraTransform.forward * aimDistance;
            groundedPlayer = controller.isGrounded;
            if (groundedPlayer && playerVelocity.y < 0)
            {
                playerVelocity.y = 0f;
            }
            Vector2 input = moveAction.ReadValue<Vector2>();

            Vector3 move = new Vector3(input.x, 0, input.y);
            move = move.x * cameraTransform.right.normalized + move.z * cameraTransform.forward.normalized;
            move.y = 0;
            controller.Move(move * Time.deltaTime * currentSpeed);
            playerVelocity.y += gravityValue * Time.deltaTime;
            controller.Move(playerVelocity * Time.deltaTime);
            Quaternion targetRotation = Quaternion.Euler(0, cameraTransform.eulerAngles.y, 0);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }

    private void OnEnable()
    {
        GetComponent<HealthBar>().SetMaxHealth(playerData._health);
        shootAction.performed += _ => Shoot();
        shiftAction.performed += _ => SpeedUp();
        shiftAction.canceled += _ => SpeedDown();
    }

    private void OnDisable()
    {
        shootAction.canceled -= _ => Shoot();
        shiftAction.performed -= _ => SpeedUp();
        shiftAction.canceled -= _ => SpeedDown();
    }
    private void Shoot()
    {
        if (aimAction.IsPressed())
        {
            RaycastHit hit;
            GameObject bullet = Instantiate(bulletPrefab, barrelTransform.position, bulletPrefab.transform.rotation, 
                bulletParent);
            BulletController bulletController = bullet.GetComponent<BulletController>();
            if (Physics.Raycast(cameraTransform.position, cameraTransform.forward, out hit))
            {
                bulletController.SetTarget(hit.point);
                bulletController.SetHit(true);
            }
            else
            {
                bulletController.SetTarget(cameraTransform.position + cameraTransform.forward * bulletHitMissDistance);
                bulletController.SetHit(false);
            }
        }
    }
    private void SpeedUp()
    {
        currentSpeed = speedUp;
    }
    private void SpeedDown()
    {
        currentSpeed = playerSpeed;
    }
}
