using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerEventAnimationHandler : MonoBehaviour
{
    public Animator animator;
    public SpriteRenderer spriteRenderer;
    public LevelManager levelManager;

    public void AlertObservers(string message)
    {
        if (message.Equals("Animation Drawing Ended"))
        {
            animator.Play("Base Layer.Idle_Player");
        }

        if (message.Equals("Animation Hit Ended"))
        {
            if (FindObjectOfType<HealthCounter>().gameObject.transform.childCount == 0)
            {
                animator.Play("Base Layer.Dead_Player");
            } else
            {
                animator.Play("Base Layer.Idle_Player");
            }
        }

        if (message.Equals("Animation Dead Ended"))
        {
            DOTween.Sequence()
                .Append(spriteRenderer.DOFade(0f, 1f));
        }
    }
}
