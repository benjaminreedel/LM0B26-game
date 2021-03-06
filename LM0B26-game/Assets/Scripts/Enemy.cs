using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public string enemytype;
    public int health = 30;
    int attackinterval;
    bool waiting = false;
    public Animator animator;
    public bool isgreg = false;
    GameObject player1;
    GameObject player2;
    GameObject target;
    public GameObject WanderLock;
    Rigidbody2D rb;
    Transform tf;
    Vector3 initpos;
    Vector3 wanderpos;
    Vector2 currentpos;
    Vector2 lastpos;


    public Transform attackPoint;
    public float attackRange = 0.5f;
    public LayerMask enemyLayers;


    private bool facingRight = true;
    float idletime;
    public enum enemystate
    {
        idle,
        walkontoscreen,
        walknear,
        walkup,
        attack,
        wander
    }
    public enemystate currentstate;

    // Start is called before the first frame update
    void Start()
    {
        player1 = GameObject.Find("Greg");
        player2 = GameObject.Find("Jenn");
        rb = gameObject.GetComponent<Rigidbody2D>();
        tf = gameObject.GetComponent<Transform>();
        initpos = findinitpos();
        target = randomplayer();
        currentpos = tf.position;

        switch (enemytype)
        {
            case "Normal" :
                health = 30;
                attackinterval = 4;
                break;
            default:
                health = 30;
                break;
        }

        idle(enemystate.walkontoscreen, Random.Range(2, 4));
    }

    // Update is called once per frame
    void Update()
    {
        flip();
        switch (currentstate) {
            case enemystate.walkontoscreen:
                walkontoscreen();
                break;
            case enemystate.walknear:
                walknear();
                break;
            case enemystate.walkup:
                walkup();
                break;
            case enemystate.attack:
                attack();
                break;
            case enemystate.wander:
                wander();
                break;
            default:
                break;
        }
    }

    public void idle(enemystate nextstate, float idletime)
    {
        StartCoroutine(_idle(nextstate, idletime));
    }

    IEnumerator _idle(enemystate nextstate, float idletime)
    {
        waiting = true;
        yield return new WaitForSeconds(idletime);
        waiting = false;
        currentstate = nextstate;
    }

    public Vector2 findinitpos()
    {
        Vector3 randompos = new Vector2(Random.Range(WanderLock.transform.position.x - 8, WanderLock.transform.position.x + 8), Random.Range(WanderLock.transform.position.y - 1.5f, WanderLock.transform.position.y + 1.5f));
        return randompos;

    }

    Vector2 findclosesplayer()
    {
        Vector2 diff = tf.position - player1.transform.position;
        Vector2 diff2 = tf.position - player2.transform.position;
        if (diff.x > diff2.x)
        {
            return player2.transform.position;
        } else {
            return player1.transform.position;
        }
    }

    GameObject randomplayer()
    {
        if (Random.Range(1,3) == 1) {
            isgreg = true;
            return player1;
        } else {
            Debug.Log("jen");
            return player2;
        }
    }

    public void walkontoscreen()
    {
        tf.position = Vector2.MoveTowards(tf.position, initpos, 0.015f);
        if (tf.position == initpos && waiting == false){
            if (isgreg == true) {
                if (target.GetComponent<GregController>().enemycount == 0) {
                    target.GetComponent<GregController>().enemycount++;
                    idle(enemystate.walknear, 1);
                } else {
                    wanderpos = new Vector2(Random.Range(target.transform.position.x - 4, target.transform.position.x + 4), Random.Range(target.transform.position.y - 2, target.transform.position.y + 2));
                    idle(enemystate.wander, 1);
                }
            } else {
                if (target.GetComponent<JennController>().enemycount == 0) {
                    target.GetComponent<JennController>().enemycount++;
                    idle(enemystate.walknear, 1);
                } else {
                    wanderpos = new Vector2(Random.Range(target.transform.position.x - 4, target.transform.position.x + 4), Random.Range(target.transform.position.y - 2, target.transform.position.y + 2));
                    idle(enemystate.wander, 1);
                }
            }
        }
    }

    public void wander()
    {
        tf.position = Vector2.MoveTowards(tf.position, wanderpos, 0.008f);
        if (tf.position == wanderpos) {
            if (isgreg == true) {
                if (target.GetComponent<GregController>().enemycount == 0) {
                    target.GetComponent<GregController>().enemycount++;
                    idle(enemystate.walknear, 1);
                } else {
                    wanderpos = new Vector2(Random.Range(WanderLock.transform.position.x - 8, WanderLock.transform.position.x + 8), Random.Range(WanderLock.transform.position.y - 1.8f, WanderLock.transform.position.y + 1.8f));
                }
            } else {
                if (target.GetComponent<JennController>().enemycount == 0) {
                    target.GetComponent<JennController>().enemycount++;
                    idle(enemystate.walknear, 1);
                } else {
                    wanderpos = new Vector2(Random.Range(WanderLock.transform.position.x - 8, WanderLock.transform.position.x + 8), Random.Range(WanderLock.transform.position.y - 1.8f, WanderLock.transform.position.y + 1.8f));
                }
            }
        }
    }
    
    public void flip()
    {
        lastpos = currentpos;
        currentpos = tf.position;
        if (currentpos != lastpos) {
            animator.SetFloat("Speed", 1);
        } else {
            animator.SetFloat("Speed", 0);
        }

        if (currentpos.x < lastpos.x && facingRight == true) {
            // Switch the way the player is labelled as facing.
		    facingRight = false;

		    // Multiply the player's x local scale by -1.
		    Vector3 theScale = transform.localScale;
		    theScale.x *= -1;
		    transform.localScale = theScale;
        } else if (currentpos.x > lastpos.x && facingRight == false) {
            // Switch the way the player is labelled as facing.
		    facingRight = true;

		    // Multiply the player's x local scale by -1.
		    Vector3 theScale = transform.localScale;
		    theScale.x *= -1;
		    transform.localScale = theScale;
        }
    }

    public void walknear()
    {
        if (tf.position != (target.transform.position + new Vector3(3,0,0))) {
            if (isgreg == true) {
                if (target.GetComponent<GregController>().enemycount < 2) {
                    tf.position = Vector2.MoveTowards(tf.position, (target.transform.position + new Vector3(3,0,0)), 0.01f);
                }
            } else {
                if (target.GetComponent<JennController>().enemycount < 2) {
                    tf.position = Vector2.MoveTowards(tf.position, (target.transform.position + new Vector3(3,0,0)), 0.01f);
                }
            }
        } else if (waiting == false) {
            idle(enemystate.walkup, attackinterval);
        }
    }

    public void walktowards() 
    {
        if (tf.position != (target.transform.position + new Vector3(3,0,0))) {
            tf.position = Vector2.MoveTowards(tf.position, (target.transform.position + new Vector3(3,0,0)), 0.01f);
        } else if (waiting == false) {
            idle(enemystate.walkup, attackinterval);
        }
    }

    public void walkup()
    {
        tf.position = Vector2.MoveTowards(tf.position, (target.transform.position + new Vector3(1,0,0)), 0.025f);
        if (waiting == false && tf.position == (target.transform.position + new Vector3(1,0,0))) {
            animator.SetTrigger("Attack");
            currentstate = enemystate.attack;
        }
    }

    public void attack()
    {
        if (waiting == false) {
            idle(enemystate.walknear, 1);
        }//punch then have a 50% chance to punch after every punch then go back to walk near
        //two attacks a punch and a kick
    }

    public void takeDamage(int damage) {
        health -= damage;
        if (health <= 0)
            die();
    }

    void die() {
        Debug.Log("I died!");
        if (isgreg == true) {
            if (target.GetComponent<GregController>().enemycount == 1)
            target.GetComponent<GregController>().enemycount--;
        } else {
            if (target.GetComponent<JennController>().enemycount == 1)
            target.GetComponent<JennController>().enemycount--;
        }
        Destroy(gameObject);
    }

    public void attackCheck() {
        Debug.Log("attackCheck");
        Collider2D[] hitPlayers = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);

        foreach (Collider2D player in hitPlayers) {
            Debug.Log("Enemy hit");
            if (isgreg == true)
                player.GetComponent<GregController>().takeDamage(10);
            else
                player.GetComponent<JennController>().takeDamage(10);
        }
    }

    void OnDrawGizmosSelected() {
        if (attackPoint == null) {
            return;
        }
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }

}