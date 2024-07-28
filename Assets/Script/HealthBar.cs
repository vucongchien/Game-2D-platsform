using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class HealthBar : MonoBehaviour
{
    
    public Slider healthSlider;
    public TMP_Text healthBarText;
    Damgeable playerDamgeable;
    // Start is called before the first frame update
    private void Awake()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if(player == null)
        {
            Debug.Log("can't find player ");
        }
        playerDamgeable = player.GetComponent<Damgeable>();
    }
    void Start()
    {
        
        healthSlider.value = CaculateSliderPercentage(playerDamgeable.Health,playerDamgeable.MaxHealth);
        healthBarText.text = playerDamgeable.Health + " / " + playerDamgeable.MaxHealth;
    }

    private float CaculateSliderPercentage(float currHealth,float maxHealth)
    {

        return currHealth / maxHealth;
    }
    private void OnEnable()
    {
        playerDamgeable.healthChange.AddListener(OnPlayerHealthChange);
    }
    private void OnDisable()
    {
        playerDamgeable.healthChange.RemoveListener(OnPlayerHealthChange);
    }
    private void OnPlayerHealthChange(int newHealth,int maxHealth)
    { 
        healthSlider.value = CaculateSliderPercentage(newHealth, maxHealth);
        healthBarText.text = newHealth + " / " + maxHealth;
    }
}
