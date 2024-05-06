using System.Collections.Generic;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;

namespace Tests
{
	public class EnemyTests
	{
		[Test]
		public void should_remove_life_when_taking_damage()
		{
			// Arrange
			Enemy enemy = AssetDatabase.LoadAssetAtPath<Enemy>("Assets/-Content/Prefabs/Enemies/Enemy_Prefab.prefab");
			enemy.test_setCurrentHealth(3);

			EnemyConfig enemyConfig = AssetDatabase.LoadAssetAtPath<EnemyConfig>("Assets/-Content/Data/Config/Enemies/Enemy_Minion.asset");
			enemy.test_setEnemyConfig(enemyConfig);

			GameObject mesh = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/-Content/Prefabs/Enemies/Skeleton_Minion.prefab");
			enemy.test_setMesh(mesh);
			enemy.test_setAnimator(mesh.GetComponent<Animator>());

			// Act
			enemy.UpdateHealth(-1);

			// Assert
			Assert.AreEqual(2, enemy.test_getCurrentHealth());
		}

		[Test]
		public void should_increment_enemy_killed_when_enemy_dies()
		{
			// Arrange
			Enemy enemy = AssetDatabase.LoadAssetAtPath<Enemy>("Assets/-Content/Prefabs/Enemies/Enemy_Prefab.prefab");

			RSO_EnemyKilled rsoEnemyKilled = ScriptableObject.CreateInstance<RSO_EnemyKilled>();
			enemy.test_setRsoEnemyKilled(rsoEnemyKilled);

			// Act
			enemy.HandleDeath();

			// Assert
			Assert.AreEqual(1, rsoEnemyKilled.value);
		}

		[Test]
		public void should_return_distance_from_last_waypoint()
		{
			// Arrange
			Enemy enemy = AssetDatabase.LoadAssetAtPath<Enemy>("Assets/-Content/Prefabs/Enemies/Enemy_Prefab.prefab");
			enemy.transform.position = new Vector3(1, 0, 1);
			enemy.test_setNextWaypoint(2);

			RSO_Path rsoPath = ScriptableObject.CreateInstance<RSO_Path>();
			rsoPath.value = new List<Vector3>
			{
				new Vector3(0, 0, 0), 
				new Vector3(1, 0, 0), 
				new Vector3(1, 0, 1)
			};
			enemy.test_setRsoPath(rsoPath);

			// Act
			float distance = enemy.GetDistanceFromLastWaypoint();

			// Assert
			Assert.AreEqual(2, distance);
		}

		[Test]
		public void should_decrement_next_waypoint_when_waypoint_reached()
		{
			// Arrange
			Enemy enemy = AssetDatabase.LoadAssetAtPath<Enemy>("Assets/-Content/Prefabs/Enemies/Enemy_Prefab.prefab");
			enemy.transform.position = new Vector3(1, 0, 0);
			enemy.test_setNextWaypoint(1);

			RSO_Path rsoPath = ScriptableObject.CreateInstance<RSO_Path>();
			rsoPath.value = new List<Vector3>
			{
				new Vector3(0, 0, 0), 
				new Vector3(1, 0, 0), 
				new Vector3(1, 0, 1)
			};
			enemy.test_setRsoPath(rsoPath);

			// Act
			enemy.test_checkForDistance();

			// Assert
			Assert.AreEqual(0, enemy.test_getNextWaypoint());
		}

		[Test]
		public void should_set_path_as_completed_when_last_waypoint_reached()
		{
			// Arrange
			Enemy enemy = AssetDatabase.LoadAssetAtPath<Enemy>("Assets/-Content/Prefabs/Enemies/Enemy_Prefab.prefab");
			enemy.transform.position = new Vector3(0, 0, 0);
			enemy.test_setNextWaypoint(0);

			RSO_Path rsoPath = ScriptableObject.CreateInstance<RSO_Path>();
			rsoPath.value = new List<Vector3>
			{
				new Vector3(0, 0, 0), 
				new Vector3(1, 0, 0), 
				new Vector3(1, 0, 1)
			};
			enemy.test_setRsoPath(rsoPath);

			// Act
			enemy.test_checkForDistance();

			// Assert
			Assert.AreEqual(true, enemy.test_getCompleted());
		}
	}
}