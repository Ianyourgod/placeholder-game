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

    private bool is_jumping = false;

    // Start is called before the first frame update
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

        if (Input.GetButtonDown("Vertical")) {
            is_jumping = true;
        }

        rigid_body.velocity = new Vector2(horizontal * speed, rigid_body.velocity.y);
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
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, 0.5f, player_layer);
        return hits.Length > 1;
    }
}
