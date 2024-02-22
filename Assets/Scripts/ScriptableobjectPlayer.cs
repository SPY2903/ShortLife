using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "ScrAbleObject",menuName ="ScriptAbleObjects/Character")]
public class ScriptableobjectPlayer : ScriptableObject
{
    public int _health = 100;
    [SerializeField]
    private int _maxHealth = 100;

    private void OnEnable()
    {
        _health = _maxHealth;
    }

    public void UpdateHealth(int amout)
    {
        _health += amout;
        if (_health > _maxHealth) _health = _maxHealth;
        if (_health <= 0) _health = 0;
    }
}
