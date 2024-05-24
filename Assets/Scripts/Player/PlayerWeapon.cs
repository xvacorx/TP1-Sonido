using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeapon : MonoBehaviour
{
    private Animator animator;
    private PlayerActions playerActions;
    private bool isAttacking;

    private string currentAnimationState;

    private void Start()
    {
        animator = GetComponent<Animator>();
        playerActions = GetComponentInParent<PlayerActions>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
            Attack();

        SetAnimations();
    }

    private void ChangeAnimationState(string newState)
    {
        if (currentAnimationState == newState) return;

        currentAnimationState = newState;
        animator.CrossFadeInFixedTime(currentAnimationState, 0.1f);
    }

    private void SetAnimations()
    {
        if(!isAttacking)
        {
            if (playerActions.isInAir)
                ChangeAnimationState("Jump");
            else if (!playerActions.isRunning)
                ChangeAnimationState("Idle");
            else
                ChangeAnimationState("Run");
        }
    }

    private void Attack()
    {
        if (isAttacking) return;

        isAttacking = true;

        Invoke(nameof(ResetAttack), 0.912f);

        ChangeAnimationState("Attack");
    }

    private void ResetAttack()
    {
        isAttacking = false;
    }
}
