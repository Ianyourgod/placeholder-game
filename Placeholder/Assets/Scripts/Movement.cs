using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Movement : MonoBehaviour
{
    [SerializeField] private float speed = 5.0f;
    [SerializeField] private float jump_force = 5.0f;
    [SerializeField] private Rigidbody2D rigid_body;
    [SerializeField] private LayerMask ground_layer;
    [SerializeField] private LayerMask player_layer;
    [SerializeField] private LayerMask box_layer;
    [SerializeField] private Animator animator;

    [SerializeField] private bool is_evil = false;
    private string animation_name;

    private float animation_cooldown = 0.0f;

    private bool is_jumping = false;

    void Awake() {
        animation_name = is_evil ? "Bad" : "Good";
    }

    void Start()
    {

    }

    private int three_ops(float value) {
        if (value > 0) {
            return 1;
        } else if (value < 0) {
            return -1;
        } else {
            return 0;
        }
    }

    // Update is called once per frame
    void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");

        AnimatorClipInfo clip = animator.GetCurrentAnimatorClipInfo(0)[0];

        if (Time.time > animation_cooldown && clip.clip.name != $"{animation_name}PlayerIdle") {
            animator.Play($"{animation_name}PlayerIdle");
        }

        if (Input.GetButtonDown("Vertical")) {
            is_jumping = true;
            // play jump animation
            animator.Play($"{animation_name}PlayerJump");
            animation_cooldown = Time.time + 0.35f; // 0.35 seconds
        }

        rigid_body.velocity = new Vector2(horizontal * speed, rigid_body.velocity.y);

        if (horizontal != 0) {
            (bool ahead, Collider2D box) = box_ahead(new Vector2(three_ops(horizontal), 0));
            if (ahead) {
                box.transform.position = new Vector3(box.transform.position.x + horizontal * speed, box.transform.position.y, box.transform.position.z);
                // set box velocity to 0
            }

            if (clip.clip.name != $"{animation_name}PlayerRun" && Time.time > animation_cooldown) {
                animator.Play($"{animation_name}PlayerRun");
                animation_cooldown = Time.time + 0.5f; // 0.1 seconds
            }
        }

        if (horizontal < 0) {
            transform.localScale = new Vector3(-1, 1, 1);
        } else if (horizontal > 0) {
            transform.localScale = new Vector3(1, 1, 1);
        }

        if (is_jumping && touching_ground()) {
            rigid_body.AddForce(new Vector2(0f, jump_force));
            is_jumping = false;
        }

        if (touching_enemy()) {
            // reload the scene
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    bool touching_ground() {
        // raycast down
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 1.0f, ground_layer);
        return hit.collider != null;
    }

    bool touching_enemy() {
        // check circle
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, 0.25f, player_layer);
        return hits.Length > 1;
    }

    (bool, Collider2D) box_ahead(Vector2 direction) {
        // check circle
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position + new Vector3(direction.x, direction.y, 0), 0.25f, box_layer);
        bool ahead = hits.Length > 0;
        return (ahead, ahead ? hits[0] : null);
    }
}
