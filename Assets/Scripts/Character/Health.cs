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
    CanvasGroup lowHPOverlay;
    public AudioClip hurtSound;
    AudioSource hurtSoundComponent;
    GameManager gameManager;
    bool died = false;
    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        hurtSoundComponent = gameObject.AddComponent<AudioSource>();
        _currentHealth = maxHealth;
        if (this.tag == "Player")
        {
            healthBar = GameObject.Find("HUD").transform.Find("HP").GetComponent<HealthBar>();
            lowHPOverlay = GameObject.Find("HUD").transform.Find("LowHPOverlay").GetComponent<CanvasGroup>();
            lowHPOverlay.alpha = 0.0f;
        }
        else
            hurtSoundComponent.spatialBlend = 1.0f;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeDamage(float damage)
    {
        if (died)
            return;

        if (hurtSound != null)
            hurtSoundComponent.PlayOneShot(hurtSound);

        _currentHealth = Mathf.Clamp(_currentHealth - damage, 0.0f, _currentHealth);
        Debug.Log($"Damage taken: {damage}");
        if (healthBar != null) healthBar.UpdateHealthBar(_currentHealth);
        if (gameObject.tag == "Player" && _currentHealth < 40.0) {lowHPOverlay.alpha = (40.0f - _currentHealth) / 40.0f;
        Debug.Log(lowHPOverlay.alpha); }
        if(_currentHealth <= 0.0f)
        {
            Die();
        }
    }

    private void Die()
    {
        died = true;
        if (gameObject.tag == "Enemy")
        {
            gameManager.EnemyDied(); // Decreases the amount of alive enemies in the game manager.
            GameObject.Instantiate(deathParticles.gameObject, this.transform.position, this.transform.rotation);
            Destroy(gameObject);
        }
        else if(gameObject.tag == "Player")
        {
            Time.timeScale = 0.2f;
            if (hurtSound != null)
                hurtSoundComponent.PlayOneShot(hurtSound);
            gameManager.PlayerDied();
        }
    }
}
