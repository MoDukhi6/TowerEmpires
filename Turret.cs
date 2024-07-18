#if UNITY_EDITOR
using UnityEditor;
#endif

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    [Header("Attributes")]
    [SerializeField] private Transform LookingBackPoint;
    [SerializeField] private LayerMask enemyMask;
    [Header("Attributes")]
    [SerializeField] private float targetingRange = 3f;

    private Transform target;

    private void Update()
    {
        if (target == null)
        {
            FindTarget();
            return;
        }

        RotateTowardsTarget();
    }

    private void FindTarget()
    {
        RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, targetingRange, (Vector2)transform.position, 0f, enemyMask);

        if (hits.Length > 0)
        {
            target = hits[0].transform;
        }
    }

    private void RotateTowardsTarget()
    {
        Vector3 targetDirection = target.position - transform.position;
        Quaternion targetRotation = Quaternion.LookRotation(targetDirection, Vector3.up);
        // Limit rotation to only rotate in the y-axis
        targetRotation.eulerAngles = new Vector3(0f, targetRotation.eulerAngles.y, 0f);
        transform.rotation = targetRotation;
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Handles.color = Color.black;
        Handles.DrawWireDisc(transform.position, transform.forward, targetingRange);
    }
#endif
}
