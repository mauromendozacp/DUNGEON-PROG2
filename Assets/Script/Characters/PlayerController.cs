using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float speed = 0f;
    [SerializeField] private float turnSmoothVelocity = 0f;

    private CharacterController character = null;
    private Animator anim = null;

    private float vMove = 0f;
    private float hMove = 0f;

    private Vector3 direction = Vector3.zero;
    private float turnSmoothTime = 0f;
    private float velocityY = 0f;

    private float currentSpeed = 0f;

    private void Awake()
    {
        character = GetComponent<CharacterController>();
        anim = GetComponentInChildren<Animator>();
    }

    private void Start()
    {
        currentSpeed = speed;
    }

    private void Update()
    {
        UpdateInput();

        ApplyGravity();
        Movement();

        UpdateAnimation();
    }

    public void ResetPlayer(Vector3 resetPosition)
    {
        character.enabled = false;

        transform.SetPositionAndRotation(resetPosition, Quaternion.identity);

        character.enabled = true;
    }

    private void UpdateInput()
    {
        hMove = Input.GetAxis("Horizontal") * -1;
        vMove = Input.GetAxis("Vertical") * -1;

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            currentSpeed = speed * 3f;
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            currentSpeed = speed;
        }
    }

    private void ApplyGravity()
    {
        if (!character.isGrounded)
        {
            velocityY = -Physics.gravity.magnitude * Time.deltaTime;
        }
        else
        {
            velocityY = 0f;
        }
    }

    private void Movement()
    {
        direction = new Vector3(hMove, velocityY, vMove).normalized;

        if (direction.magnitude > Mathf.Epsilon)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
            float characterAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothTime, turnSmoothVelocity);

            transform.rotation = Quaternion.Euler(0f, characterAngle, 0f);

            character.Move(direction * currentSpeed * Time.deltaTime);
        }
    }

    private void UpdateAnimation()
    {
        anim.SetFloat("WalkVelocity", direction.magnitude, 0.05f, Time.deltaTime);
    }
}
