using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    /*
     Players will only have 3 health
     Health To Do List:
     1. Health code obiviously [completed]
     2. Health sprite Icons [completed] + Animation []
    */
    [Range(0, 10)]
    public int currentHealth;
    [Range(0,10)]
    public int numOfHearts;
    //int maxHealth = 3;

    public Image[] healthIcon;
    public Sprite fullHealth;
    public Sprite blankHealth;
    
    public bool alive;

    private void Awake()
    {
        alive = true; 
        
        if(currentHealth > numOfHearts) {
            currentHealth = numOfHearts;
        }
    }

    void Start()
    {
        //currentHealth = maxHealth;
    }

    void Update()
    {
       
        
        for(int i = 0; i < healthIcon.Length; i++)
        {
            if (i < currentHealth) {
                healthIcon[i].sprite = fullHealth;
            }
            else {
                healthIcon[i].sprite = blankHealth;
            }

            if (i < numOfHearts) { 
                healthIcon[i].enabled = true;
            }
            else { 
                healthIcon[i].enabled = false;
            }
        }

        if (currentHealth <= 0 && alive == true)
        {
            gameObject.SetActive(false);
            alive = false;
        }
    }

    public void TakeDamage(int damage)
    {
        if (alive == true) {
            currentHealth -= damage;
            numOfHearts--;
        }
    }
}
