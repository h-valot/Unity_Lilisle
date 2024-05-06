using NUnit.Framework;
using UnityEditor;

namespace Tests
{
	public class HeartTests
	{
		[Test]
		public void should_remove_life_when_taking_damage()
		{
			// Arrange
			Heart heart = AssetDatabase.LoadAssetAtPath<Heart>("Assets/-Content/Prefabs/Buildings/Roads/Heart.prefab");

			RSO_Heart rsoHeart = AssetDatabase.LoadAssetAtPath<RSO_Heart>("Assets/-Content/Data/RSO/RSO_Heart.asset");
			rsoHeart.value = 20;

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

			RSO_Heart rsoHeart = AssetDatabase.LoadAssetAtPath<RSO_Heart>("Assets/-Content/Data/RSO/RSO_Heart.asset");
			rsoHeart.value = 0;

			RSO_GameState rsoGameState = AssetDatabase.LoadAssetAtPath<RSO_GameState>("Assets/-Content/Data/RSO/RSO_GameState.asset");
			rsoGameState.value = GameState.WAVE;

			// Act
			heart.HandleHeart();

			// Assert
			Assert.AreEqual(GameState.GAME_OVER, rsoGameState.value);
		}
	}
}