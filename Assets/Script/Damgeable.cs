using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Damgeable : MonoBehaviour
{
    public UnityEvent<int, Vector2> damageableHit;
    public UnityEvent<int, int> healthChange;
    Animator animator;
    [SerializeField]
    private bool _isAlive=true;
    private bool IsAlive
    {
        get
        {
            return _isAlive;
        }
        set
        {
            _isAlive = value;
            animator.SetBool(AnimationStrings.isAlive,value);
            Debug.Log("die");
        }
    }

    [SerializeField]
    private int _maxHealth;
    public int MaxHealth
    {
        get
        {
            return _maxHealth;
        }
        set 
        {
            _maxHealth = value;
        }
    }

    [SerializeField]
    private int _health=100;
    [SerializeField]
    private bool isInvincible=false;


    private float timeSinceHit=0;
    [SerializeField]
    private float isInvincibleTime=0.25f;

    public int Health
    {
        get
        {
            return _health;
        }
        set
        {
            _health = value;
            healthChange?.Invoke(_health, MaxHealth);
            if (_health <= 0)
            {
                IsAlive = false;
            }
        }
    }

    public bool LockVelocity
    {
        get
        {
            return animator.GetBool(AnimationStrings.lockVelocity);
        }
        set
        {
            animator.SetBool(AnimationStrings.lockVelocity, value);
        }
    }

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void Update()
    {
        if (isInvincible)
        {
            if (timeSinceHit > isInvincibleTime)
            {
                isInvincible = false;
                timeSinceHit = 0;
            }
            timeSinceHit += Time.deltaTime;
        }
    }

    public bool Hit(int damage, Vector2 knockback)
    {
        if(IsAlive && !isInvincible)
        {
            Health -= damage;
            isInvincible = true;

            animator.SetTrigger(AnimationStrings.hitTrigger);
            damageableHit?.Invoke(damage, knockback);
            CharacterEvents.characterDamaged.Invoke(gameObject, damage);


            return true;
        }
        return false;
    }
    public void Heal(int healRestore)
    {
        if (IsAlive)
        {
            int maxHealth=Mathf.Max(MaxHealth-Health,0);
            int actualHeal=  Mathf.Min(maxHealth, healRestore);
            Health += actualHeal;
            CharacterEvents.characterHealed(gameObject, actualHeal);
        }
    }
}
