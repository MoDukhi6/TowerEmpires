using UnityEngine;

public class AttackRange : MonoBehaviour
{
    public float radius = 5f; // Adjust the attack range radius in the Inspector

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
