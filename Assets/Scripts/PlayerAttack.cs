using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class PlayerAttack : MonoBehaviour
{

    [SerializeField] private Animator animator;
    [SerializeField] private PlayerController playerController;
    private bool isAttacking;

    [SerializeField] private Transform attackPoint;
    [SerializeField] private int attackDamage = 10;
    [SerializeField] private float attackRange = 1.0f;

    [SerializeField] private float attackDuration = 0.8f;
    [SerializeField] private float attackMoment = 0.3f;
    [SerializeField] private string attackAnimationName = "Attack";

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Mouse mouse = Mouse.current;

        if (mouse == null || isAttacking) return;
        if (playerController.IsNormalState() == false) return;

        if (mouse.leftButton.wasPressedThisFrame)
        {
            StartCoroutine(AttackAnimation());
        }
    }

    private IEnumerator AttackAnimation()
    {
        isAttacking = true;

        playerController.ChangeState(PlayerState.Attack);

        int attackState = Animator.StringToHash(attackAnimationName);

        if (animator.HasState(0, attackState))
        {
            animator.CrossFade(attackAnimationName, 0.1f);
        }
        else
        {
            Debug.LogWarning(attackAnimationName + "애니메이션 상태가 없습니다.");
        }

        animator.CrossFade("Attack01", 0.1f);

        yield return new WaitForSeconds(0.8f);

        HitEnemy();

        yield return new WaitForSeconds(Mathf.Max(0f, attackDuration - attackMoment));

        animator.CrossFade("Bland Tree", 0.1f);
        playerController.ChangeState(PlayerState.Normal);
        isAttacking = false;
    }

    private void HitEnemy()
    {
        Collider[] targets = Physics.OverlapSphere(attackPoint.position, attackRange);

        foreach (Collider target in targets)
        {
            EnemyHealth enemy = target.GetComponentInParent<EnemyHealth>();

            if (enemy == null) continue;

            enemy.TakeDamage(attackDamage);
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null) return;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
