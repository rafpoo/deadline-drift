using System.Collections;
using UnityEngine;

public enum SIDE { LEFT, MID, RIGHT }

[RequireComponent(typeof(CharacterController))]
public class Character : MonoBehaviour
{
    public SIDE side = SIDE.MID;
    public float laneDistance = 3f;
    public float laneChangeSpeed = 8f;
    public float jumpForce = 10f;
    public float gravity = -30f;
    public float forwardSpeed = 2f;
    public float xValue = 3f; // bukan -3f


    private CharacterController controller;
    private Animator anim;
    private Vector3 verticalVelocity;
    private float targetX;

    private bool gravityEnabled = false;

    IEnumerator Start()
    {
        controller = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();
        targetX = transform.position.x;
        anim.SetBool("IsRunning", true);

        yield return new WaitForSeconds(0.1f);
        gravityEnabled = true;
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
                targetX = -laneDistance; // kiri = negatif
                side = SIDE.LEFT;
            }
            else if (side == SIDE.RIGHT)
            {
                targetX = 0f;
                side = SIDE.MID;
            }
            anim.SetTrigger("DodgeLeft");
        }
        else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (side == SIDE.MID)
            {
                targetX = laneDistance; // kanan = positif
                side = SIDE.RIGHT;
            }
            else if (side == SIDE.LEFT)
            {
                targetX = 0f;
                side = SIDE.MID;
            }
            anim.SetTrigger("DodgeRight");
        }


        if (Input.GetKeyDown(KeyCode.Space) && controller.isGrounded)
        {
            Jump();
        }
    }

    void Jump()
    {
        verticalVelocity.y = jumpForce;
        anim.SetBool("IsRunning", false);
        anim.ResetTrigger("Jump");
        anim.SetTrigger("Jump");
    }

    void ApplyGravity()
    {
        if (!gravityEnabled) return;

        if (controller.isGrounded && verticalVelocity.y < 0)
        {
            verticalVelocity.y = -1f;
        }
        else
        {
            verticalVelocity.y += gravity * Time.deltaTime;
        }
    }

    void MoveCharacter()
    {
        float nextX = Mathf.Lerp(transform.position.x, targetX, Time.deltaTime * laneChangeSpeed);
        float moveX = nextX - transform.position.x;

        Vector3 move = new Vector3(moveX, 0f, forwardSpeed * Time.deltaTime);
        move.y = verticalVelocity.y * Time.deltaTime;

        controller.Move(move);
    }

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.collider.CompareTag("Obstacle"))
        {
            // Hit normal mengarah ke belakang player?
            Vector3 hitDir = hit.normal; // arah permukaan yang ditabrak

            // Asumsi player menghadap ke depan (Z+)
            bool frontalHit = Vector3.Dot(hitDir, Vector3.back) > 0.5f;

            if (frontalHit)
            {
                Debug.Log("💥 Player menabrak obstacle dari depan!");
                GameOver();
            }
        }
    }

    void GameOver()
    {
        Debug.Log("GAME OVER!");

        anim.SetBool("IsRunning", false);
        anim.SetTrigger("Death"); // pastikan punya animasi "Death"

        // Nonaktifkan pergerakan
        enabled = false;

        // (Opsional) Panggil UI manager / restart scene
        // SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }



    public void OnDodgeEnd()
    {
        anim.SetBool("IsRunning", true);
        anim.CrossFade("Run", 0.1f);
    }

    public void OnJumpEnd()
    {
        anim.SetBool("IsRunning", true);
        anim.CrossFade("Run", 0.1f);
    }
}
