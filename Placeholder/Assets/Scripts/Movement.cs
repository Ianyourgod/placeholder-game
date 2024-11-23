using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField] private float speed = 5.0f;
    [SerializeField] private float jump_force = 5.0f;
    [SerializeField] private Rigidbody2D rigid_body;

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

    private int BoolToInt(bool value) {
        return value ? 1 : 0;
    }

    private Vector2 get_direction() {
        int up = BoolToInt(Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow));
        int left = BoolToInt(Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow));
        int right = BoolToInt(Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow));

        float horizontal = (float) (right - left);
        float vertical = (float) (up);

        return new Vector2(horizontal, vertical);
    }

    bool is_grounded() {
        return rigid_body.velocity.y == 0;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 direction = get_direction();
        
        if (is_grounded()) {
            rigid_body.velocity = new Vector2(0, rigid_body.velocity.y);
        }

        if (direction.x != 0) {
            rigid_body.velocity = new Vector2(direction.x * speed, rigid_body.velocity.y);
        }

        if (direction.y != 0 && is_grounded()) {
            rigid_body.velocity = new Vector2(rigid_body.velocity.x, jump_force);
        }
    }
}
