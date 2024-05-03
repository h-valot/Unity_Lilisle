using System;
using UnityEngine;

public class Health : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Animator _animator;
    [SerializeField] private Enemy _enemy;

	private int _currentHealth;

	public void Initialize(int baseHealth)
	{
		_currentHealth = baseHealth;
	}

    public void UpdateHealth(int amount)
    {
        _currentHealth += amount;

		if (_currentHealth <= 0)
		{
			_enemy.HandleDeath();
		}
		
		_animator.SetBool("GetHit", true);
		_animator.SetBool("GetHit", false);
    }
}