using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class wavehadle : MonoBehaviour
{
    GameObject[] enemys;
    Transform tf;
    public bool nextzone;
    Vector3 nextpos;
    public GameObject target;
    public int amount = 2;
    public GameObject enemy;
    // Start is called before the first frame update
    void Start()
    {
        tf = gameObject.GetComponent<Transform>();
        //StartCoroutine(wait());
    }

    IEnumerator wait()
    {
        yield return new WaitForSeconds(3);
        spawnenemys();
    }

    public void spawnenemys() {
        for (int i = 0; i < amount; i++) {
            Instantiate(enemy, new Vector2(Random.Range(target.transform.position.x, target.transform.position.x), Random.Range(target.transform.position.y - 5, target.transform.position.y + 5)), target.transform.rotation);
        }
        amount++;
    }
    
    // Update is called once per frame
    void Update()
    {
        enemys = GameObject.FindGameObjectsWithTag("Enemy");

        if (amount == 10 && enemys.Length == 0) {
            SceneManager.LoadScene(2);
        }

        if (enemys.Length == 0 && nextzone == false) {
            nextpos = tf.position + new Vector3(-5.5f,0,0);
            nextzone = true;
            
        }
        
        if (nextzone == true) {
            tf.position = Vector2.MoveTowards(tf.position, nextpos, 0.004f);

            if (tf.position == nextpos) {
                nextzone = false;
                //StartCoroutine(wait());
                spawnenemys();
            }
        }
    }
}
