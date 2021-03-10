using Unity.Entities;
using UnityEngine;

/// <summary>
/// Set movement and physics from the Unity inspector
/// </summary>
public class MovementAuthoring : MonoBehaviour, IConvertGameObjectToEntity
{
	[SerializeField] float moveSpeed = 20f;
	[SerializeField] float jumpForce = 1f;
	[SerializeField] float doubleJumpForceMultiplier = 2f;
	[SerializeField] float jumpTimeMax = 0.08f;

	/// <summary>
	/// Convert the data entered via Unity's inspector into a DOTS component instance.
	/// </summary>
	public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
	{
		var movement = new Movement
		{
			moveSpeed = moveSpeed,
			jumpForce = jumpForce,
			doubleJumpForceMultiplier = doubleJumpForceMultiplier,
			jumpTimeMax = jumpTimeMax,
			jumpDelta = 0f,
			//flags = Movement.Grounded
			grounded = false
		};

		dstManager.AddComponentData(entity, movement);
	}
}
