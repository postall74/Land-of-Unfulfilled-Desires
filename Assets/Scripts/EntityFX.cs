using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityFX : MonoBehaviour
{
    private SpriteRenderer _spriteRenderer;

    [Header("Flash FX")]
    [SerializeField] private float _flashDuration = 0.2f;
    [SerializeField] private Material _hitMat;
    [HideInInspector] private Material _originalMat;

    private void Start()
    {
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        _originalMat = _spriteRenderer.material;
    }

    public IEnumerator FlashFX()
    {
        _spriteRenderer.material = _hitMat;
        yield return new WaitForSeconds(_flashDuration);
        _spriteRenderer.material = _originalMat;
    }

    private void RedColorBlink() => _spriteRenderer.color = _spriteRenderer.color != Color.white ? Color.white : Color.red;

    private void CancelRedBlink()
    {
        CancelInvoke();
        _spriteRenderer.color = Color.white;
    }
}