using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class FirstPersonController : Singleton<FirstPersonController>
{
    public float walkSpeed = 5.0f;
    public float rotationSpeed = 180.0f;
    public float gravity = 8f;
    private CharacterController characterController;
    private Transform cameraTransform;
    public float jumpSpeed = 8.0f;
    private float verticalVelocity = 0;
    float horizontal;
    float vertical;
    Animator animator;

    public float Horizontal
    {
        get => horizontal; set
        {
            if (horizontal != value)
            {
                horizontal = value;
                if (horizontal != 0 || vertical != 0)
                {
                }
                else
                {
                    animator.SetBool("IsRun", false);
                }
            }

        }
    }

    public float Vertical { get => vertical; set {

            if (vertical != value)
            {
                vertical = value;
                if (horizontal != 0 || vertical != 0)
                {
                
                }
                else
                {
                    animator.SetBool("IsRun", false);
                }
            }
        } }

    void Start()
    {
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
        animator = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();
        // 获取角色的 Rigidbody 组件
        cameraTransform = Camera.main.transform;
    }
    void FixedUpdate()
    {
        // 应用重力
    }


    int Sub_Str(string str)
    {
        return int.Parse(str.Substring(str.Length - 1));

    }
    void Update()
    {


        if (Input.GetKey(KeyCode.LeftShift))
        {
            walkSpeed = 10;
        }
        else
        {
            walkSpeed = 5;
        }
        // 获取摄像头的前方向
        Vector3 cameraForward = cameraTransform.forward;
        // 保持y轴方向为0，只保留水平方向
        cameraForward.y = 0;
        cameraForward.Normalize();

        // 获取水平和垂直输入
        Horizontal = Input.GetAxis("Horizontal");
        Vertical = Input.GetAxis("Vertical");
        Vector3 moveDirection = cameraForward * Vertical + cameraTransform.right * Horizontal;



        if (moveDirection.magnitude >= 0.1f)
        {
            animator.SetBool("IsRun",true);
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);

            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime * 2);
            // 移动角色
            characterController.Move(moveDirection.normalized * walkSpeed * Time.deltaTime);
        }
        // 在地面时处理跳跃
        if (characterController.isGrounded)
        {
            verticalVelocity = 0;
            if (Input.GetKeyDown(KeyCode.Space))
            {
                verticalVelocity = jumpSpeed;
            }
        }

        // 应用重力并移动玩家
        verticalVelocity -= gravity * Time.deltaTime;
        moveDirection.y = verticalVelocity;
        characterController.Move(moveDirection * Time.deltaTime);
    }
}

