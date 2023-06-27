using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloneSkill : Skill
{
    #region Fields
    [Header("Clone info")]
    [SerializeField] private GameObject _clonePrefab;
    [SerializeField] private float _cloneDuration;
    [Space]
    [SerializeField] private bool _canAttack;
    #endregion

    public void CreateClone(Transform clonePosition, Vector3 offset)
    {
        GameObject newClone = Instantiate(_clonePrefab);
        newClone.GetComponent<CloneSkillController>().SetupClone(clonePosition, _cloneDuration, _canAttack, offset, FindClosestEnemy(newClone.transform));
    }
}
