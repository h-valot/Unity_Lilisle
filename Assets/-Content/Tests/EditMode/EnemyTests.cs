using System.Collections;
using UnityEngine.TestTools;
using NUnit.Framework;
using UnityEngine;

namespace Tests
{
	public class EnemyTests
	{
		[Test]
		public void should_remove_life_when_taking_damage()
		{
			// Arrange
			GameObject gameObject = new GameObject();
			Enemy enemy = gameObject.AddComponent<Enemy>();
			enemy.test_setCurrentHealth(3);

			// Act
			enemy.UpdateHealth(-1);

			// Assert
			Assert.AreEqual(2, enemy.test_getCurrentHealth());
		}
	}
}