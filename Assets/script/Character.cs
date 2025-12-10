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
    public float gameOverDelay = 2f;
    public float invincibleDuration = 3f;


    private CharacterController controller;
    private Animator anim;
    private Vector3 verticalVelocity;
    private float targetX;
    private bool isDead = false;
    private bool gravityEnabled = false;
    private bool isInvincible = false;

    IEnumerator Start()
    {
        transform.position = new Vector3(0f, transform.position.y, transform.position.z); // pastikan di tengah
        controller = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();
        targetX = 0f; // pastikan target lane adalah MID
        anim.SetBool("IsRunning", true);

        yield return new WaitForSeconds(0.1f);
        gravityEnabled = true;
    }


    void Update()
    {
        if (isDead) return;

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

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Collectible"))
        {
            GameManager.Instance.AddScore(1); // tambah skor 1
            Destroy(other.gameObject); // hapus kertas
        }
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
        if (hit.gameObject.CompareTag("Obstacle"))
        {
            Debug.Log("Tersentuh obstacle: " + hit.gameObject.name);
            // Jika mendarat dari atas → aman
            if (verticalVelocity.y < 0 && hit.normal.y > 0.5f)
                return;

            // Jika kena dari samping/depan → mati
            Die();
        }

    }


    void Reset()
    {
        // side = SIDE.MID;
        // targetX = 0f;
        if (side == SIDE.LEFT)
        {
            targetX = laneDistance;
            anim.SetTrigger("DodgeRight");
        }
        else if (side == SIDE.RIGHT)
        {
            targetX = -laneDistance;
            anim.SetTrigger("DodgeLeft");
        }
        else
        {
            targetX = 0f;
        }
        side = SIDE.MID;
    }

    void Die()
    {
        if (isDead) return;

        if (isInvincible)
            return;

        StartCoroutine(StartInvincible()); // aktifkan invincibility

        GameManager.Instance.deathCount += 1;
        StageManager.Instance.OnPlayerHit(GameManager.Instance.deathCount);

        Debug.Log("Character died! Total deaths: " + GameManager.Instance.deathCount);
        // isDead = true;
        if (GameManager.Instance.deathCount >= 4)
        {
            isDead = true;

            // Matikan movement
            GetComponent<CharacterController>().enabled = false;

            // Jalankan animasi
            anim.SetBool("IsRunning", false);
            anim.SetTrigger("Death");
            TileManager.Instance.StopTiles();

            // Beri sedikit delay sebelum game over muncul
            Invoke(nameof(TriggerGameOver), gameOverDelay);
        }
    }

    IEnumerator StartInvincible()
    {
        isInvincible = true;

        // (Optional) player flashing  👇
        StartCoroutine(FlashPlayer());

        yield return new WaitForSeconds(invincibleDuration);

        isInvincible = false;
    }

    IEnumerator FlashPlayer()
    {
        Renderer rend = GetComponentInChildren<Renderer>();

        float flashSpeed = 0.15f;

        while (isInvincible)
        {
            rend.enabled = false;
            yield return new WaitForSeconds(flashSpeed);

            rend.enabled = true;
            yield return new WaitForSeconds(flashSpeed);
        }
    }



    void TriggerGameOver()
    {

        if (GameManager.Instance != null)
        {
            GameManager.Instance.GameOver();
        }
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
