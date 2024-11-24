using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FlagInteract : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other) {
        // note: dont let spikes touch the flag
        if (other.gameObject.layer == LayerMask.NameToLayer("deadly")) {
            // get Movement script
            Movement movement = other.gameObject.GetComponent<Movement>();
            // if is_evil is true, reset the level
            bool is_evil = movement.is_evil;

            if (is_evil) {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            } else {
                // get current scene name, remove last letter (number), add 1 to it, load that scene
                string current_scene = SceneManager.GetActiveScene().name;
                string next_scene = current_scene.Substring(0, current_scene.Length - 1) + (int.Parse(current_scene.Substring(current_scene.Length - 1)) + 1);
                SceneManager.LoadScene(next_scene);
            }   
        }
    }
}
