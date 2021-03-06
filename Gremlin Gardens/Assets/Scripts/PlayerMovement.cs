using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] Transform playerCamera = null;
    [SerializeField] float mouseSensitivity = 3.5f;
    [SerializeField] bool lockCursor = true;
    [SerializeField] float walkSpeed = 6.0f;
    [SerializeField] [Range(0.0f, 0.5f)] float moveSmoothTime = 0.3f;

    float cameraPitch = 0.0f;
    CharacterController controller = null;

    Vector2 currentDir = Vector2.zero;
    Vector2 currentDirVelocity = Vector2.zero;

    /// <summary>
    /// Object that the player is looking at.
    /// </summary>
    [HideInInspector]
    public GameObject centeredObject = null;
    [HideInInspector]
    public GameObject previousObject = null;
    [HideInInspector]
    public bool hitObjectIsNew = true;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
        //cursor is locked and in middle of screen
        if (lockCursor)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    void FixedUpdate()
    {
        UpdateMovement();
    }
    void Update()
    {
        //Get the object we're looking at
        if (Physics.Raycast(this.transform.position, this.transform.GetChild(0).transform.forward, out RaycastHit objectHit))
        {
            centeredObject = objectHit.transform.gameObject;
            if (centeredObject != previousObject)
            {
                hitObjectIsNew = true;
            }
            else
            {
                hitObjectIsNew = false;
            }
        }
        UpdateMouseLook();
    }

    private void LateUpdate()
    {
        previousObject = centeredObject;
    }

    void UpdateMouseLook()
    {
        Vector2 mouseDelta = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
        cameraPitch -= (mouseDelta.y * mouseSensitivity);
        //limits camera and applies it to player
        cameraPitch = Mathf.Clamp(cameraPitch, -90.0f, 90.0f);
        playerCamera.localEulerAngles = Vector3.right * cameraPitch;
        transform.Rotate(Vector3.up * mouseDelta.x * mouseSensitivity);
    }

    void UpdateMovement()
    {
        Vector2 targetDir = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        //normalize and smooth vector
        targetDir.Normalize();

        currentDir = Vector2.SmoothDamp(currentDir, targetDir, ref currentDirVelocity, moveSmoothTime);

        Vector3 velocity = (transform.forward * currentDir.y + transform.right * currentDir.x) * walkSpeed;
        controller.SimpleMove(velocity * Time.deltaTime);
    }
}