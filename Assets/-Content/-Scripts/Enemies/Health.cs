using System;
using UnityEngine;

public class Health : MonoBehaviour
{
	private int _currentHealth;

	public void Initialize(int baseHealth)
	{
		_currentHealth = baseHealth;
	}

    public void UpdateHealth(int amount, Action<int> health)
    {
        _currentHealth += amount;

		if (_currentHealth <= 0)
		{
        	gameObject.SetActive(false);
		}
		 
		health(_currentHealth);
    }
}