using UnityEngine;

public class EnemyHealth : MonoBehaviour
{

    [SerializeField] private int maxealth = 30;
    [SerializeField] private int currentHealth;

    [SerializeField] private GameObject dropItemPrefab;
    [SerializeField] private Vector3 dropOffset = new Vector3(0f, 0.5f, 0f);
    private bool isDead;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentHealth = maxealth;
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return;

        currentHealth -= damage;
        Debug.Log(name + " 남은 체력 : " + currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        isDead = true;

        if (dropItemPrefab != null)
        {
            Instantiate(dropItemPrefab, transform.position + dropOffset, Quaternion.identity);
        }

        Destroy(gameObject);
    }
}
