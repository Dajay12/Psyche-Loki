using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityPill : MonoBehaviour
{
    [SerializeField] PlayerAbility refToAbility;
    [SerializeField] GameObject player;
    public enum Abilities { Invicinble, BioExplosion, Restrain, None }
    public Abilities typesOfAblities;

    void Awake()
    {
        refToAbility = player.GetComponent<PlayerAbility>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            gameObject.SetActive(false);
            if (typesOfAblities == Abilities.Invicinble)
            {
                print("I");
                refToAbility.abilityUse = PlayerAbility.AbilityUse.Invincibility;
            }
            if (typesOfAblities == Abilities.BioExplosion)
            {
                print("F");
                refToAbility.abilityUse = PlayerAbility.AbilityUse.BioExplosion;
            }
            if (typesOfAblities == Abilities.Restrain)
            {
                print("R");
                refToAbility.abilityUse = PlayerAbility.AbilityUse.Restrain;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {

    }

    void Update()
    {
        
    }
}
