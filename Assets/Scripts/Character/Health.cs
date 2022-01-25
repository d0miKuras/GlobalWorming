using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{

    [Tooltip("The maximum health of the unit.")]
    public float maxHealth = 100f;
    private float _currentHealth;
    public HealthBar healthBar;
    public ParticleSystem deathParticles;
    // Start is called before the first frame update
    void Start()
    {
        _currentHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeDamage(float damage)
    {
        _currentHealth = Mathf.Clamp(_currentHealth - damage, 0.0f, _currentHealth);
        Debug.Log($"Damage taken: {damage}");
        if(healthBar != null) healthBar.UpdateHealthBar(_currentHealth);
        if(_currentHealth <= 0.0f)
        {
            Die();
        }
    }

    private void Die()
    {
        if (gameObject.tag == "Enemy")
        {
            GameObject.Find("GameManager").GetComponent<GameManager>().EnemyDied(); // Decreases the amount of alive enemies in the game manager.
            GameObject.Instantiate(deathParticles.gameObject, this.transform.position, this.transform.rotation);
        }
        Destroy(gameObject);
    }
}
