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

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            //gameObject.SetActive(false);

            if (!refToAbility.occupied)
            {
                follow = true;

                if (typesOfAblities == Abilities.Invicinble) refToAbility.abilityUse = PlayerAbility.AbilityUse.Invincibility;
                if (typesOfAblities == Abilities.BioExplosion) refToAbility.abilityUse = PlayerAbility.AbilityUse.BioExplosion;    
                if (typesOfAblities == Abilities.Restrain) refToAbility.abilityUse = PlayerAbility.AbilityUse.Restrain;
            }
            else if (refToAbility.occupied)
            {
                refToAbility.swapped = true;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            refToAbility.swapped = false;
        }
    }

    void Update()
    {
        if (refToAbility.abilityUse != PlayerAbility.AbilityUse.None && follow)
        {
            this.transform.position = refToAbility.storage.position;
            refToAbility.occupied = true;
        }
        else
        {
            follow = false;
        }

        if (refToAbility.swapped && refToAbility.ability) {
            refToAbility.swapped = false;
            refToAbility.occupied = false;
            refToAbility.abilityUse = PlayerAbility.AbilityUse.None;
            refToAbility.ability = false;
        }
    }
}
