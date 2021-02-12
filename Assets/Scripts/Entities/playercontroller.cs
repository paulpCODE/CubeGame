using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Cinemachine;

public class playercontroller : MonoBehaviour, IGravityChangeable, IDestroyable
{
    [Header("Moving")]
    [SerializeField] float speed;
    [SerializeField] float jumpHeight;

    [Space]
    [Header("Dash")]
    [Tooltip("cooldown in seconds")]
    [SerializeField] float dashCoolDown;
    [SerializeField] float dashSpeed;
    [SerializeField] private float totalDashTime; // отнимаем от него

    [Space]
    [Header("Cinemamachine")]
    [SerializeField] GameObject CinemachineCamera;
    [SerializeField] private float cameraShake;


    #region Actions
    public static Action<bool, float> OnCanDashChanged;
    public static Action<sbyte> OnAxisChanged;
    #endregion


    #region Private Variables

    #region Cinemamachine
    private CinemachineVirtualCamera vCamera;
    private CinemachineBasicMultiChannelPerlin vCameraShaker;
    //private float cameraAnimationMotion = 0.5f;
    #endregion


    #region Dash
    private bool canDash;
    private bool isDashInProcess = false;
    private float dashTime; // if <=0 dashTime = startDashTime
    #endregion


    private Rigidbody2D rb;
    private BoxCollider2D boxCollider;

    private Vector2 axis;
    private Vector3 startPosition;
    private Vector2 previousVelocity;

    private sbyte lookDirection; // 1 - right, (-1) - left

    private bool isDoubleJumpBoostActive = false;
    #endregion

    #region Get Set
    /// <summary>
    /// variable to store player input 
    /// </summary>
    public Vector2 Axis {
        get { return axis; }
        private set
        {
            if(axis.x!= 0)
            {
                if(OnAxisChanged != null)
                {
                    OnAxisChanged((sbyte)axis.x);
                }
            }
            if(axis == value)return;
           
            

            
            if( (axis.x == 1 || axis.x == -1) && value.x == 0) // switch from 1 to 0 or from -1 to 0 
            {
                var yPrevVelocity = rb.velocity.y;
                rb.velocity = new Vector2(0, yPrevVelocity);
            }
            axis = value;
        }
    }
    public bool CanDash
    {
        get
        {
            return canDash;
        }
        set
        {
            // отправить сигнал, что поменялось значение onCanDashChanged
            if (OnCanDashChanged != null)
                OnCanDashChanged(value, dashCoolDown);

            canDash = value;
        }
    }
    #endregion



    #region MonobehaviourCallbacks
    void Awake()
    {
        vCamera = CinemachineCamera.GetComponent<CinemachineVirtualCamera>();
        vCameraShaker = vCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

        rb = gameObject.GetComponent<Rigidbody2D>();
        boxCollider = gameObject.GetComponent<BoxCollider2D>();
        dashTime = totalDashTime;
        CanDash = true;
        startPosition = transform.position;
    }

    private void OnEnable()
    {
        DoubleJump.OnDoubleJumpEntered += SphereBoost;
        OnCanDashChanged += ReloadDash;
        OnAxisChanged += setLookDirection;
    }
    private void OnDisable()
    {
        DoubleJump.OnDoubleJumpEntered -= SphereBoost;
        OnCanDashChanged -= ReloadDash;
        OnAxisChanged -= setLookDirection;
    }

    void Update()
    {
        Input();
    }

    private void FixedUpdate()
    {
        if(!isDashInProcess)
            rb.AddForce(Axis * speed); // move 

    }

    #endregion


    #region Private Methods
    private void Input()
    {
        if (!isDashInProcess)
        {
            Axis = new Vector2(UnityEngine.Input.GetAxisRaw("Horizontal"), 0);
        }
        // jump 
        if ((IsGrounded() || isDoubleJumpBoostActive)
            && UnityEngine.Input.GetButtonDown("Jump"))
        {
            Jump();
        }

        // dash 
            Dash();
        
    }

    private int ReturnGravityDirection()
    {
        if (rb.gravityScale < 0)
            return -1;
        return 1;
    }

    private void setLookDirection(sbyte newLookdirection)
    {
        lookDirection = newLookdirection;
    }


    #region Jump
    private void Jump()
    {
        Debug.Log("jump!");
        rb.AddForce(Vector2.up * jumpHeight * ReturnGravityDirection() * 100);

        isDoubleJumpBoostActive = false;
    }

    private void SphereBoost()
    {
        isDoubleJumpBoostActive = true;
    }

    private bool IsGrounded()
    {
        if (Physics2D.Raycast(new Vector3(transform.position.x + transform.localScale.x / 2 - 0.1f, transform.position.y, 0),
            Vector2.down * ReturnGravityDirection(),
            transform.localScale.y / 2 + 0.1f,
            LayerMask.GetMask("Ground")))
            return true;

        if (Physics2D.Raycast(new Vector3(transform.position.x - transform.localScale.x / 2 + 0.1f, transform.position.y, 0),
            Vector2.down * ReturnGravityDirection(),
            transform.localScale.y / 2 + 0.1f,
            LayerMask.GetMask("Ground")))
            return true;

        return false;
    }
    #endregion


    #region Dash
    /// <summary>
    /// is called in update
    /// </summary>
    private void Dash() 
    {
        var scaleDifference = 0.3f;
        if (!CanDash)
        {
            return;
        }
        // if we just start to dash
        if(!isDashInProcess  && UnityEngine.Input.GetKeyDown(KeyCode.LeftShift))
        {
            isDashInProcess = true;
            previousVelocity = rb.velocity;


            
            // #critical player scale 
            vCameraShaker.m_AmplitudeGain = cameraShake;
            var playerScale = gameObject.transform.localScale;
            playerScale = new Vector3(playerScale.x + scaleDifference, playerScale.y - scaleDifference, 0);
            gameObject.transform.localScale = playerScale;
        }
        // active dash code in update 
        else if(isDashInProcess)
        {
            // #critical половину времени деша сужать, половину назад расширять 

            dashTime -= Time.deltaTime;
            rb.velocity = new Vector2(lookDirection * dashSpeed, 0);
        }
        // if dash finished 
        if(dashTime<=0)
        {
            // restore values
            // #critical player scale 
            vCameraShaker.m_AmplitudeGain = 0;


            var playerScale = gameObject.transform.localScale;
            playerScale = new Vector3(playerScale.x - scaleDifference, playerScale.y + scaleDifference, 0);
            gameObject.transform.localScale = playerScale;

            dashTime = totalDashTime;
            rb.velocity = previousVelocity;
            CanDash = false;
            isDashInProcess = false;
        }
    }

    void ReloadDash(bool newValue, float coolDownTime)
    {
        // c true na false отправляю на перезарядку 
        if (newValue == false)
            StartCoroutine(DashReloadRoutine(newValue, coolDownTime));
    }
    IEnumerator DashReloadRoutine(bool newValue, float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        // Maybe beter use reference 
        CanDash = true;
    }
    #endregion
    #endregion



    #region Public Methods

    public void ChangeGravity()
    {
        rb.gravityScale *= -1;
    }

    public void DestroyObject()
    {
        transform.position = startPosition;
    }

    #endregion




}
