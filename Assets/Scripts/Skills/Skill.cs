using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill : MonoBehaviour
{
    #region Fields
    [SerializeField] protected float cooldown;
    protected float cooldownTimer;
    protected Player player;
    #endregion

    public virtual bool CanUseSkill()
    {
        if (cooldownTimer < 0)
        {
            UseSkill();
            cooldownTimer = cooldown;
            return true;
        }

        Debug.Log("Skill is on cooldown");
        return false;
    }    

    public virtual void UseSkill()
    {

    }

    protected virtual void Start()
    {
        player = PlayerManager.instance.Player;
    }

    protected virtual void Update()
    {
        cooldownTimer -= Time.deltaTime;
    }

    protected virtual Transform FindClosestEnemy(Transform checkTransform)
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(checkTransform.position, 25);
        Transform closestEnemy = null;
        float closestDistance = Mathf.Infinity;

        foreach (var hit in colliders)
        {
            if (hit.GetComponent<Enemy>() != null)
            {
                float distanceToEnemy = Vector2.Distance(checkTransform.position, hit.transform.position);

                if (distanceToEnemy < closestDistance)
                {
                    closestDistance = distanceToEnemy;
                    closestEnemy = hit.transform;
                }
            }
        }

        return closestEnemy;
    }
}
