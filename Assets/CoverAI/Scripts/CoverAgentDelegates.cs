using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CoverAgentDelegates : MonoBehaviour
{
    public float meleeAttackRange;
    public GameObject coverMaster;
    public GameObject enemy;
    public CoverMaster GetCoverMaster()
    {
        return this.coverMaster.GetComponent<CoverMaster>();
    }
    abstract public void MoveTo(Vector3 targetPosition);
    abstract public void Crouch();
    abstract public void Stand();
    abstract public void Charge();
    abstract public void RangedAttack(GameObject target);
    abstract public void MeleeAttack(GameObject target);
    abstract public void Die();
    abstract public bool IsDead();
    abstract public float DistanceTo(GameObject target);
}
