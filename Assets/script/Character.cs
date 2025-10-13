using UnityEngine;

public enum SIDE { LEFT, MID, RIGHT }

public class Character : MonoBehaviour
{
    public SIDE side = SIDE.MID;
    public float xValue = 3f;
    public float laneSpeed = 5.5f;
    public float jumpForce = 1.4f;
    public float gravity = -20f;

    private CharacterController CharController;
    private Vector3 velocity = Vector3.zero;

    private float newXPos = 0f;
    private float currentX;
    private Animator anim;

    void Start()
    {
        CharController = GetComponent<CharacterController>();
        // newXPos = transform.position.x;
        currentX = newXPos;
        anim = GetComponent<Animator>();
        anim.SetBool("IsRunning", true);
    }

    void Update()
    {
        HandleInput();
        ApplyGravity();
        MoveCharacter();
    }

    void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
        {

            if (side == SIDE.MID)
            {
                newXPos = -xValue;
                side = SIDE.LEFT;
            }
            else if (side == SIDE.RIGHT)
            {
                newXPos = 0f;
                side = SIDE.MID;
            }
            anim.SetBool("IsRunning", false);
            anim.SetTrigger("DodgeLeft");
        }
        else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            anim.SetTrigger("DodgeRight");
            if (side == SIDE.MID)
            {
                newXPos = xValue;
                side = SIDE.RIGHT;
            }
            else if (side == SIDE.LEFT)
            {
                newXPos = 0f;
                side = SIDE.MID;
            }
            anim.SetBool("IsRunning", false);
            anim.SetTrigger("DodgeRight");
        }
        else if (Input.GetKeyDown(KeyCode.Space) && CharController.isGrounded)
        {
            velocity.y = jumpForce;
            anim.SetBool("IsRunning", false);
            anim.SetTrigger("Jump");
        }
    }

    void ApplyGravity()
    {
        if (CharController.isGrounded && velocity.y < 0)
        {
            velocity.y = -1f; // Small negative value to keep the character grounded
        }
        else
        {
            velocity.y += gravity * Time.deltaTime;
        }
    }

    void MoveCharacter()
    {
        float nextX = Mathf.Lerp(currentX, newXPos, laneSpeed * Time.deltaTime);

        Vector3 move = Vector3.zero;
        move.x = nextX - currentX;
        move.y = velocity.y;
        move += velocity * Time.deltaTime;

        CharController.Move(move);
        currentX = nextX;
        Debug.Log($"currentX={currentX}, newXPos={newXPos}");
    }

    public void OnDodgeEnd()
    {
        Debug.Log("Dodge animation ended");
        anim.ResetTrigger("DodgeLeft");
        anim.ResetTrigger("DodgeRight");
        anim.SetBool("IsRunning", true);
        anim.CrossFade("Run", 0.1f);
    }

    public void OnJumpEnd()
    {
        Debug.Log("Jump animation ended");
        anim.ResetTrigger("Jump");
        anim.SetBool("IsRunning", true);
        anim.CrossFade("Run", 0.1f);
    }
}
