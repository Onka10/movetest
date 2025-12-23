using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class FPSController : MonoBehaviour
{
    [Header("移動設定")]
    public float moveSpeed = 5f;
    public float gravity = -9.81f;

    [Header("回転設定")]
    public float rotateSpeed = 120f;

    [Header("しゃがみ設定")]
    public float standHeight = 1.8f;
    public float crouchHeight = 1.0f;
    public float crouchSpeedRate = 0.5f;

    float yVelocity;
    bool isCrouching;

    CharacterController controller;

    void Start()
    {
        controller = GetComponent<CharacterController>();

        // 念のため初期値設定
        controller.height = standHeight;
        controller.center = new Vector3(0f, standHeight / 2f, 0f);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    void Update()
    {
        Rotate();
        Crouch();
        Move();
    }

    void Rotate()
    {
        float rotateDirection = 0f;

        if (Input.GetKey(KeyCode.Space))
            rotateDirection += 1f; // 右旋回

        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
            rotateDirection -= 1f; // 左旋回

        if (rotateDirection != 0f)
        {
            transform.Rotate(0f, rotateDirection * rotateSpeed * Time.deltaTime, 0f);
        }
    }

    void Crouch()
    {
        if (Input.GetKey(KeyCode.E))
        {
            if (!isCrouching)
            {
                isCrouching = true;
                controller.height = crouchHeight;
                controller.center = new Vector3(0f, crouchHeight / 2f, 0f);
            }
        }
        else
        {
            if (isCrouching)
            {
                isCrouching = false;
                controller.height = standHeight;
                controller.center = new Vector3(0f, standHeight / 2f, 0f);
            }
        }
    }

    void Move()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        float speed = isCrouching
            ? moveSpeed * crouchSpeedRate
            : moveSpeed;

        Vector3 move = transform.right * x + transform.forward * z;
        move *= speed;

        if (controller.isGrounded && yVelocity < 0)
        {
            yVelocity = -2f;
        }

        yVelocity += gravity * Time.deltaTime;

        Vector3 velocity = move + Vector3.up * yVelocity;
        controller.Move(velocity * Time.deltaTime);
    }
}
