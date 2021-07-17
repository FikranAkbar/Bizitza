using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable] public class _UnityEventCheckLastHealth : UnityEvent { }

public class HealthUnit : MonoBehaviour
{

    public _UnityEventCheckLastHealth checkLastHealth;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayDecreaseHealthAnimation()
    {
        Animator animator = GetComponent<Animator>();
        animator.Play("Base Layer.Health_Decrease");
    }

    public void AlertObservers(string message)
    {
        if (message.Equals("Animation Ended"))
        {
            Destroy(gameObject);
        }
    }
}
