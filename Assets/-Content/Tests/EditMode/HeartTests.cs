using NUnit.Framework;
using UnityEditor;
using UnityEngine;

namespace Tests
{
	public class HeartTests
	{
		[Test]
		public void should_remove_life_when_taking_damage()
		{
			// Arrange
			Heart heart = AssetDatabase.LoadAssetAtPath<Heart>("Assets/-Content/Prefabs/Buildings/Roads/Heart.prefab");

			RSO_Heart rsoHeart = ScriptableObject.CreateInstance<RSO_Heart>();
			rsoHeart.value = 20;
			heart.test_setRsoHeart(rsoHeart);

			// Act
			heart.UpdateHeart(-1);

			// Assert
			Assert.AreEqual(19, rsoHeart.value);
		}

		[Test]
		public void should_change_gamestart_when_reach_zero()
		{
			// Arrange
			Heart heart = AssetDatabase.LoadAssetAtPath<Heart>("Assets/-Content/Prefabs/Buildings/Roads/Heart.prefab");

			RSO_Heart rsoHeart = ScriptableObject.CreateInstance<RSO_Heart>();
			rsoHeart.value = 0;
			heart.test_setRsoHeart(rsoHeart);

			RSO_GameState rsoGameState = ScriptableObject.CreateInstance<RSO_GameState>();
			rsoGameState.value = GameState.WAVE;
			heart.test_setRsoGameState(rsoGameState);

			// Act
			heart.HandleHeart();

			// Assert
			Assert.AreEqual(GameState.GAME_OVER, rsoGameState.value);
		}
	}
}