using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private PlayerControls controls;
    [SerializeField] float speed;
    Animator animator;
    private Rigidbody2D rb;
    private bool facingRight;

    #region Dash
    [SerializeField] private float dashSpeed;
    private float dashTime;
    [SerializeField] private float startDashTime;
    private int direction;
    [SerializeField] private CameraShake cameraShake;
    #endregion

    #region Attack
    [SerializeField] private GameObject swordLeft;
    [SerializeField] private GameObject swordRight;
    #endregion

    private void Awake()
    {
        controls = new PlayerControls();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        facingRight = true;

        #region Dash
        dashTime = startDashTime;
        //cameraShake = GameObject.FindGameObjectWithTag("CameraShake").GetComponent<CameraShake>();
        #endregion
    }

    private void OnEnable()
    {
        controls.Enable();
    }

    private void OnDisable()
    {
        controls.Disable();
    }

    void Start()
    {

    }

    private void FixedUpdate()
    {
        float moveLeftRightInput = controls.Movement.LeftRight.ReadValue<float>();

        if ((moveLeftRightInput < 0 && facingRight) || (moveLeftRightInput > 0 && !facingRight))
        {
            Flip();
        }

        Vector3 currentPos = transform.position;
        currentPos.x += moveLeftRightInput * speed * Time.deltaTime;
        transform.position = currentPos;

        animator.SetBool("IsMoving", moveLeftRightInput != 0);

        #region Dash
        controls.Movement.Dash.performed += _ => Dash(moveLeftRightInput);
        #endregion

        #region Attack
        controls.Attack.SwordAttack.performed += SwordAttack_performed;
        #endregion
    }

    private void SwordAttack_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        animator.SetTrigger("SwordAttack");
    }

    private void Dash(float moveLeftRightInput)
    {
        if (direction == 0)
        {
            if (moveLeftRightInput > 0)
            {
                direction = 1;
            }
            else
            {
                direction = 2;
            }
            //cameraShake.ShakeCamera();

            animator.SetTrigger("Dashing");
        }
        else
        {
            if (dashTime <= 0)
            {
                direction = 0;
                dashTime = startDashTime;
                rb.velocity = Vector2.zero;
            }
            else
            {
                dashTime -= Time.deltaTime;
                switch (direction)
                {
                    case 1:
                        rb.velocity = Vector2.right * dashSpeed;
                        break;
                    case 2:
                        rb.velocity = Vector2.left * dashSpeed;
                        break;
                }
            }
        }
        rb.velocity = Vector2.zero;
    }

    private void Flip()
    {
        facingRight = !facingRight;
        transform.Rotate(Vector3.up * 180);
    }

    private void SwordAttack()
    {
        if (facingRight)
        {
            swordRight.SetActive(true);
        }
        else
        {
            swordLeft.SetActive(true);
        }
    }

    private void SwordAttackDone()
    {
        if (facingRight)
        {
            swordRight.SetActive(false);
        }
        else
        {
            swordLeft.SetActive(false);
        }
    }

    void Update()
    {

    }

}
