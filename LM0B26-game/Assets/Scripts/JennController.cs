using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JennController : MonoBehaviour
{
    public float health = 100f;
    public int enemycount = 0;

    public void takeDamage(int damage) {
        health -= damage;
        if (health <= 0)
            die();
    }

    void die() {
        Debug.Log("Jenn died!");
        
        Destroy(gameObject);
    }
}
