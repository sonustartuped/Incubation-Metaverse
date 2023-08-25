using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepeatAnimation : MonoBehaviour
{
    public Animator animator;
    public string animationName = "YourAnimationName";
    public float repeatDuration = 5f; // Duration in seconds

    private float elapsedTime = 0f;

    private void Start()
    {
        if (animator == null)
        {
            animator = GetComponent<Animator>();
            if (animator == null)
            {
                Debug.LogWarning("No Animator component found on this object.");
            }
        }
    }

    private void Update()
    {
        if (animator != null)
        {
            if (elapsedTime < repeatDuration)
            {
                elapsedTime += Time.deltaTime;

                if (!animator.GetCurrentAnimatorStateInfo(0).IsName(animationName))
                {
                    animator.Play(animationName, 0, 0f);
                }
            }
            else
            {
                // Animation duration exceeded, you can stop or reset the animation here
                // Example: animator.Play("IdleAnimation", 0, 0f);
            }
        }
    }
}
