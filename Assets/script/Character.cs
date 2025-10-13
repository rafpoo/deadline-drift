using UnityEngine;

public enum SIDE { LEFT, MID, RIGHT }

public class Character : MonoBehaviour
{
    public SIDE side = SIDE.MID;
    public float xValue = 2f;
    public float laneSpeed = 5f;

    private CharacterController CharController;
    private Vector3 velocity = Vector3.zero;
    private float gravity = -9.81f;
    private float newXPos = 0f;
    private float currentX;

    void Start()
    {
        CharController = GetComponent<CharacterController>();
        // newXPos = transform.position.x;
        currentX = newXPos;
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
        }
        else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
        {
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
        }
    }

    void ApplyGravity()
    {
        if (!CharController.isGrounded)
            velocity.y += gravity * Time.deltaTime;
        else
            velocity.y = -1f; // kecil negatif agar tetap grounded
    }

    void MoveCharacter()
    {
        float nextX = Mathf.MoveTowards(currentX, newXPos, laneSpeed * Time.deltaTime);

        Vector3 move = Vector3.zero;
        move.x = nextX - currentX;
        move += velocity * Time.deltaTime;

        CharController.Move(move);
        currentX = nextX;
        Debug.Log($"currentX={currentX}, newXPos={newXPos}");
    }

}
