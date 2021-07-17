using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimationEventHandler : MonoBehaviour
{
    private EnemyController enemyController;
    private EnemyPatternHandler enemyPatternHandler;
    private Transform playerHealths;

    // Start is called before the first frame update
    void Start()
    {
        enemyController = transform.parent.transform.GetChild(0).GetComponent<EnemyController>();
        enemyPatternHandler = transform.parent.transform.GetChild(2).GetChild(0).GetChild(0).GetComponent<EnemyPatternHandler>();
        if (GameObject.FindGameObjectWithTag("Healths") != null)
        {
            playerHealths = GameObject.FindGameObjectWithTag("Healths").transform.GetChild(0);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (playerHealths != null)
        {
            print("Player Health Count: " + playerHealths.childCount);
        }
    }
    public void AlertObservers(string message)
    {
        if (message.Equals("Enemy Dead"))
        {
            Destroy(gameObject.transform.parent.gameObject);
        }

        if (message.Equals("Enemy Hit"))
        {
            enemyController.isAttacked = false;
            enemyController.PlayIdleAnim();
        }

        if (message.Equals("Enemy Attack"))
        { 
            Destroy(gameObject.transform.parent.gameObject);
        }

        if (message.Equals("Boss Attack"))
        {
            //enemyController.enemyBehaviour = null;
            enemyController.PlayIdleAnim();
            if (playerHealths.childCount > 0)
            {
                enemyController.PushEnemy();
            }
        }
    }
}
