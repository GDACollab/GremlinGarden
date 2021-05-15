using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] Transform playerCamera = null;
    [SerializeField] public float mouseSensitivity = 3.5f;
    [SerializeField] bool lockCursor = true;
    [SerializeField] float walkSpeed = 6.0f;
    [SerializeField] [Range(0.0f, 0.5f)] float moveSmoothTime = 0.3f;

    float cameraPitch = 0.0f;
    CharacterController controller = null;
    public Text moneyText;

    public int startingMoney = 1000;

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
    /// <summary>
    /// Allow the player to move?
    /// </summary>
    public bool enableMovement = true;

    private float footstepTimer = 0.0f;
    public float timeBetweenSteps = 0.5f;
    private Vector3 velocity;
    public float distToGround = 2.0f;

    // Start is called before the first frame update
    void Start()
    {
        if (LoadingData.money == 0)
        {
            LoadingData.money = startingMoney;
        }
        controller = GetComponent<CharacterController>();
        UpdateMoney(0);
        //cursor is locked and in middle of screen
        if (lockCursor)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    void FixedUpdate()
    {
        if (enableMovement)
        {
            UpdateMovement();
        }
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
        if (enableMovement)
        {
            UpdateMouseLook();
        }

        //timer for playing footsteps
        //using magnitude > 100 cause using 0 caused footstpes to play when barely moving
        if (velocity.magnitude > 100)
        {
            footstepTimer += Time.deltaTime;
        }
        else
            footstepTimer = 0.0f;
        if (footstepTimer >= timeBetweenSteps && IsGrounded())
        {
            int sound = Random.Range(0, 4);
            this.GetComponents<AudioSource>()[sound].Play();
            footstepTimer = 0.0f;
        }

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

        velocity = (transform.forward * currentDir.y + transform.right * currentDir.x) * walkSpeed;
        controller.SimpleMove(velocity * Time.deltaTime);
    }

    public bool UpdateMoney(int changeAmount)
    {
        if (LoadingData.money + changeAmount < 0)
        {
            return false;
        }
        else
        {
            LoadingData.money += changeAmount;
            moneyText.text = "Money: " + LoadingData.money;
            return true;
        }
    }

    public int GetMoney()
    {
        return LoadingData.money;
    }

    public bool IsGrounded()
    {
        return Physics.Raycast(transform.position, -Vector3.up, distToGround + 0.1f);
    }

}

