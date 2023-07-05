using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BlackholeHotKeyController : MonoBehaviour
{
    #region Fields
    private KeyCode _myHotKey;
    private TextMeshProUGUI _myText;
    private Transform _myEnemy;
    private BlackholeSkillController _blackhole;
    #endregion

    #region Components
    private SpriteRenderer _sr;
    #endregion

    public void SetupHotKey(KeyCode myNewHotKey, Transform myEnemy, BlackholeSkillController myBlackhole)
    {
        _sr = GetComponent<SpriteRenderer>();
        _myText = GetComponentInChildren<TextMeshProUGUI>();
        _myEnemy = myEnemy;
        _blackhole = myBlackhole;
        _myHotKey = myNewHotKey;
        _myText.text = myNewHotKey.ToString();
    }

    private void Update()
    {
        if (Input.GetKeyDown(_myHotKey))
        {
            _blackhole.AddEnemyToList(_myEnemy);
            _myText.color = Color.clear;
            _sr.color = Color.clear;
        }
    }
}
