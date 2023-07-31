using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonController : MonoBehaviour
{
    public static ThirdPersonController instance;

    public enum ControlsType { KeyBoardControl, SwipeControl };
    public ControlsType controlsType;

    public Transform cameraTransform, cameraParentTransform;
    public CharacterController characterController;
    public Transform circleImage;

    public float cameraSensitivity;
    public float moveSpeed, jumpSpeed, gravity;
    public float moveInputDeadZone;

    [Range(0, 1f)]
    public float StartAnimTime = 0.3f;
    [Range(0, 1f)]
    public float StopAnimTime = 0.15f;

    public bool isControllingEnabled;
    internal Animator anim;

    int leftFingerId, rightFingerId;
    float halfScreenWidth;

    Vector2 lookInput;
    float cameraPitch;

    Vector2 moveTouchStartPosition;
    Vector2 moveInput;
    float animSpeed;

    bool isJump, isInAir, isRotation, isMovement;
    Vector3 jumpDirection, iniPos, currPos;

    private void Awake()
    {
        instance = this;
    }

    private void OnEnable()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        leftFingerId = -1;
        rightFingerId = -1;

        halfScreenWidth = Screen.width / 2;

        moveInputDeadZone = Mathf.Pow(Screen.height / moveInputDeadZone, 2);

        anim = this.GetComponent<Animator>();
        iniPos = transform.localPosition;
        currPos = iniPos;
    }

    // Update is called once per frame
    void Update()
    {
        if (controlsType == ControlsType.KeyBoardControl)
        {
            if (isControllingEnabled)
            {
                MoveUsingKeys();
                LookUsingKeys();

                if (Input.GetKeyDown(KeyCode.Space))
                {
                    isJump = true;
                }
                JumpControls();
            }
            else if (isRotation)
            {
                LookUsingKeys();
            }
            else if (isMovement)
            {
                MoveUsingKeys();

                if (Input.GetKeyDown(KeyCode.Space))
                {
                    isJump = true;
                }
                JumpControls();
            }
            else
            {
                animSpeed = 0;
                anim.SetFloat("Blend", animSpeed, StopAnimTime, Time.deltaTime);
                //SoundHandler.instance.StopSound("PlayerRunning");
                circleImage.gameObject.SetActive(false);
                leftFingerId = -1;
                rightFingerId = -1;
                //Jump
                jumpDirection.x = 0;
                jumpDirection.y = 0;
            }
        }
        else if (controlsType == ControlsType.SwipeControl)
        {
            if (isControllingEnabled)
            {
                GetTouchInput();

                if (rightFingerId != -1)
                {
                    LookAround();
                }

                if (leftFingerId != -1)
                {
                    Move();
                }
                else
                {
                    animSpeed = 0;
                    anim.SetFloat("Blend", animSpeed, StopAnimTime, Time.deltaTime);
                    //SoundHandler.instance.StopSound("PlayerRunning");
                    circleImage.gameObject.SetActive(false);
                    //Jump
                    jumpDirection.x = 0;
                    jumpDirection.y = 0;
                }
                JumpControls();
                isRotation = false;
                isMovement = false;
            }
            else if (isRotation)
            {
                GetTouchInput();

                if (rightFingerId != -1)
                {
                    LookAround();
                }

                animSpeed = 0;
                anim.SetFloat("Blend", animSpeed, StopAnimTime, Time.deltaTime);
                //AudioHandler.instance.StopAudio_PlayerAudioSource();
                circleImage.gameObject.SetActive(false);
                jumpDirection.x = 0;
                jumpDirection.y = 0;
            }
            else if (isMovement)
            {
                GetTouchInput();

                if (leftFingerId != -1)
                {
                    Move();
                }
                else
                {
                    animSpeed = 0;
                    anim.SetFloat("Blend", animSpeed, StopAnimTime, Time.deltaTime);
                    //SoundHandler.instance.StopSound("PlayerRunning");
                    circleImage.gameObject.SetActive(false);
                    //Jump
                    jumpDirection.x = 0;
                    jumpDirection.y = 0;
                }
                JumpControls();
            }
            else
            {
                animSpeed = 0;
                anim.SetFloat("Blend", animSpeed, StopAnimTime, Time.deltaTime);
                //SoundHandler.instance.StopSound("PlayerRunning");
                circleImage.gameObject.SetActive(false);
                leftFingerId = -1;
                rightFingerId = -1;
                //Jump
                jumpDirection.x = 0;
                jumpDirection.y = 0;
            }
        }
    }

    void GetTouchInput()
    {
        for (int i = 0; i < Input.touchCount; i++)
        {
            Touch _touch = Input.GetTouch(i);

            switch (_touch.phase)
            {
                case TouchPhase.Began:
                    if (_touch.position.x < halfScreenWidth && leftFingerId == -1)
                    {
                        leftFingerId = _touch.fingerId;
                        moveTouchStartPosition = _touch.position;
                        circleImage.gameObject.SetActive(true);
                        circleImage.position = new Vector3(_touch.position.x, _touch.position.y, transform.position.z);
                    }
                    else if (_touch.position.x > halfScreenWidth && rightFingerId == -1)
                    {
                        rightFingerId = _touch.fingerId;
                    }
                    break;
                case TouchPhase.Moved:
                    if (_touch.fingerId == rightFingerId)
                    {
                        lookInput = _touch.deltaPosition * cameraSensitivity * Time.deltaTime;
                    }
                    else if (_touch.fingerId == leftFingerId)
                    {
                        moveInput = _touch.position - moveTouchStartPosition;

                        circleImage.gameObject.SetActive(true);
                        circleImage.position = new Vector3(_touch.position.x, _touch.position.y, transform.position.z);
                    }
                    break;
                case TouchPhase.Ended:
                case TouchPhase.Canceled:
                    circleImage.gameObject.SetActive(false);
                    if (_touch.fingerId == leftFingerId)
                    {
                        leftFingerId = -1;
                        animSpeed = 0;
                    }
                    else if (_touch.fingerId == rightFingerId)
                    {
                        rightFingerId = -1;
                    }
                    break;
                case TouchPhase.Stationary:
                    if (_touch.fingerId == rightFingerId)
                    {
                        lookInput = Vector2.zero;
                    }
                    break;
            }
        }
    }

    void GetMouseMovement()
    {
        if (Input.GetMouseButton(1))
        {
            lookInput = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"))* cameraSensitivity;
        }
        else
        {
            lookInput = Vector2.zero;
        }
    }

    public void StopRotation()
    {
        isControllingEnabled = false;
        leftFingerId = -1;
        rightFingerId = -1;
        lookInput = Vector2.zero;
        moveInput = Vector2.zero;
        moveTouchStartPosition = Vector2.zero;
        Move();
    }

    void LookAround()
    {
        //Vertical
        cameraPitch = Mathf.Clamp(cameraPitch - lookInput.y, -90f, 90f);
        cameraTransform.localRotation = Quaternion.Euler(cameraPitch, 0, 0);

        //Horizontal
        cameraParentTransform.Rotate(transform.up, lookInput.x);
    }

    void Move()
    {
        if (moveInput.sqrMagnitude <= moveInputDeadZone)
        {
            Debug.Log("not move dir");
            return;
        }

        if (cameraParentTransform.localEulerAngles != Vector3.zero)
        {
            transform.eulerAngles = cameraParentTransform.eulerAngles;
            cameraParentTransform.localEulerAngles = Vector3.zero;
        }

        Vector2 moveDir = moveInput.normalized * moveSpeed * Time.fixedDeltaTime;
        characterController.Move(transform.right * moveDir.x + transform.forward * moveDir.y);

        Vector2 normalizeMoveInput = moveInput;
        normalizeMoveInput.Normalize();
        animSpeed = normalizeMoveInput.sqrMagnitude;
        anim.SetFloat("Blend", animSpeed, StartAnimTime, Time.fixedDeltaTime);
        //TunnelMachineHandler.instance.PlayAudio_RunningFootStep();

        Debug.Log("Move dir");
        //jump
        jumpDirection.x = moveDir.x;
        jumpDirection.y = moveDir.y;

        RealCamera.instance.isPlayerMoved = true;
    }

    public void StandPlayer()
    {
        anim.SetTrigger("stand");
    }

    #region Jump
    public void Click_Jump()
    {
        isJump = true;
    }

    void JumpControls()
    {
        if (characterController.isGrounded)
        {
            isInAir = false;
            currPos = transform.localPosition;
        }
        //else
        //{
        //    currPos = iniPos;
        //}

        //Jump
        if (isJump && !isInAir)
        {
            isInAir = true;
            isJump = false;
            if (transform.localPosition.y <= currPos.y + 0.1f)
            {
                jumpDirection.z = jumpSpeed;
                anim.SetTrigger("jump");
            }
        }
        //jumpDirection.z += Physics.gravity.y * gravity * Time.deltaTime;
        jumpDirection.z -= gravity * Time.deltaTime;
        characterController.Move(transform.right * jumpDirection.x + transform.forward * jumpDirection.y + transform.up * jumpDirection.z);
    }
    #endregion

    #region Pc move controlls

    void MoveUsingKeys()
    {
        Vector2 movementAxis = Vector2.zero;

        if(!Input.GetKey(KeyCode.UpArrow) && !Input.GetKey(KeyCode.DownArrow) && !Input.GetKey(KeyCode.LeftArrow) && !Input.GetKey(KeyCode.RightArrow))
        {
            movementAxis = Vector2.zero;
        }
        else
        {
            if (Input.GetKey(KeyCode.UpArrow))
            {
                movementAxis = new Vector2(movementAxis.x, 1);
            }
            if (Input.GetKey(KeyCode.DownArrow))
            {
                movementAxis = new Vector2(movementAxis.x, -1);
            }
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                movementAxis = new Vector2(-1, movementAxis.y);
            }
            if (Input.GetKey(KeyCode.RightArrow))
            {
                movementAxis = new Vector2(1, movementAxis.y);
            }
        }

        if (movementAxis.y != 0 || movementAxis.x != 0)
        {
            //Vector2 moveInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

            if (cameraParentTransform.localEulerAngles != Vector3.zero)
            {
                transform.eulerAngles = cameraParentTransform.eulerAngles;
                cameraParentTransform.localEulerAngles = Vector3.zero;
            }

            Vector2 moveDir = movementAxis.normalized * moveSpeed * 2 * Time.deltaTime;
            characterController.Move(transform.right * moveDir.x + transform.forward * moveDir.y);

            Vector2 normalizeMoveInput = movementAxis;
            normalizeMoveInput.Normalize();
            animSpeed = normalizeMoveInput.sqrMagnitude * 2;
            anim.SetFloat("Blend", animSpeed, StartAnimTime, Time.deltaTime);
            //TunnelMachineHandler.instance.PlayAudio_RunningFootStep();

            jumpDirection.x = moveDir.x;
            jumpDirection.y = moveDir.y;

            RealCamera.instance.isPlayerMoved = true;
        }
        else
        {
            animSpeed = 0;
            anim.SetFloat("Blend", animSpeed, StopAnimTime, Time.deltaTime);
            //SoundHandler.instance.StopSound("PlayerRunning");
            circleImage.gameObject.SetActive(false);
            leftFingerId = -1;
            rightFingerId = -1;
            //Jump
            jumpDirection.x = 0;
            jumpDirection.y = 0;
        }
    }

    void LookUsingKeys()
    {
        GetMouseMovement();

        Vector2 rotationAxis = Vector2.zero;

        if (!Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.S) && !Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D))
        {
            rotationAxis = Vector2.zero;
        }
        else
        {
            if (Input.GetKey(KeyCode.W))
            {
                rotationAxis = new Vector2(rotationAxis.x, 1);
            }
            if (Input.GetKey(KeyCode.S))
            {
                rotationAxis = new Vector2(rotationAxis.x, -1);
            }
            if (Input.GetKey(KeyCode.A))
            {
                rotationAxis = new Vector2(-1, rotationAxis.y);
            }
            if (Input.GetKey(KeyCode.D))
            {
                rotationAxis = new Vector2(1, rotationAxis.y);
            }
        }

        if (rotationAxis.x != 0 || rotationAxis.y != 0)
        {
            lookInput = new Vector2(lookInput.x + rotationAxis.x , lookInput.y + rotationAxis.y) * cameraSensitivity / 4;
        }

        //Vertical
        cameraPitch = Mathf.Clamp(cameraPitch - lookInput.y, -90f, 90f);
        cameraTransform.localRotation = Quaternion.Euler(cameraPitch, 0, 0);

        //Horizontal
        cameraParentTransform.Rotate(transform.up, lookInput.x);
    }

    #endregion

    ///
    //To use keys only for movement.
    ///
    //void MoveUsingKeys()
    //{
    //    if (Input.GetAxis("Vertical") != 0 || Input.GetAxis("Horizontal") != 0)
    //    {
    //        Vector2 moveInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

    //        if (cameraParentTransform.localEulerAngles != Vector3.zero)
    //        {
    //            transform.eulerAngles = cameraParentTransform.eulerAngles;
    //            cameraParentTransform.localEulerAngles = Vector3.zero;
    //        }

    //        Vector2 moveDir = moveInput.normalized * moveSpeed * 2 * Time.deltaTime;
    //        characterController.Move(transform.right * moveDir.x + transform.forward * moveDir.y);

    //        Vector2 normalizeMoveInput = moveInput;
    //        normalizeMoveInput.Normalize();
    //        animSpeed = normalizeMoveInput.sqrMagnitude * 2;
    //        anim.SetFloat("Blend", animSpeed, StartAnimTime, Time.deltaTime);
    //        //TunnelMachineHandler.instance.PlayAudio_RunningFootStep();

    //        jumpDirection.x = moveDir.x;
    //        jumpDirection.y = moveDir.y;

    //        RealCamera.instance.isPlayerMoved = true;
    //    }
    //    else
    //    {
    //        animSpeed = 0;
    //        anim.SetFloat("Blend", animSpeed, StopAnimTime, Time.deltaTime);
    //        //SoundHandler.instance.StopSound("PlayerRunning");
    //        circleImage.gameObject.SetActive(false);
    //        leftFingerId = -1;
    //        rightFingerId = -1;
    //        //Jump
    //        jumpDirection.x = 0;
    //        jumpDirection.y = 0;
    //    }
    //}

    //void LookUsingKeys()
    //{
    //    GetMouseMovement();

    //    //Vertical
    //    cameraPitch = Mathf.Clamp(cameraPitch - lookInput.y, -90f, 90f);
    //    cameraTransform.localRotation = Quaternion.Euler(cameraPitch, 0, 0);

    //    //Horizontal
    //    cameraParentTransform.Rotate(transform.up, lookInput.x);
    //}
}

