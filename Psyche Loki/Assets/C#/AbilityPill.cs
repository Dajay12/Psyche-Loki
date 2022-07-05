using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityPill : MonoBehaviour
{
    [SerializeField] PlayerAbility refToAbility;
    [SerializeField] GameObject player;
    public enum Abilities { Invicinble, BioExplosion, Restrain, None }
    public Abilities typesOfAblities;

    public bool follow;

    void Awake()
    {
        refToAbility = player.GetComponent<PlayerAbility>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            //gameObject.SetActive(false);
            follow = true;
            if (typesOfAblities == Abilities.Invicinble)
            {
                refToAbility.abilityUse = PlayerAbility.AbilityUse.Invincibility;
            }
            if (typesOfAblities == Abilities.BioExplosion)
            {
                refToAbility.abilityUse = PlayerAbility.AbilityUse.BioExplosion;
            }
            if (typesOfAblities == Abilities.Restrain)
            {
                refToAbility.abilityUse = PlayerAbility.AbilityUse.Restrain;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {

    }

    void Update()
    {
        if (refToAbility.abilityUse != PlayerAbility.AbilityUse.None && follow)
        {
            this.transform.position = refToAbility.storage.position;
            refToAbility.occupied = true;
        }
    }
}
