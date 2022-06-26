using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyMaker : MonoBehaviour
{
    public Sprite currentSprite;

    public Animator currentAnimator;

    void Start()
    {
        Debug.Log("Rehan");
    }

    void Update()
    {

    }

    public void EnemyMaker(Sprite _sprite, Animator _animator)
    {
        currentSprite = _sprite;
        currentAnimator = _animator;
    }

    public void Attacker()
    {
        Debug.Log("Dajay");
    }

    public void Gunner()
    {
        Debug.Log("Donray");
    }

    public void Healer()
    {
        Debug.Log("Jheanelle");
    }
}
