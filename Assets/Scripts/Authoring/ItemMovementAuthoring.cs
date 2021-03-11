using Unity.Entities;
using UnityEngine;

/// <summary>
/// Set item movement data from the Unity inspector.
/// </summary>
public class ItemMovementAuthoring : MonoBehaviour, IConvertGameObjectToEntity
{
	[SerializeField] float heightFactor = 20f;
	[SerializeField] float deltaDegrees = 20f;

	/// <summary>
	/// Convert the item movement data into a data component.
	/// </summary>
	public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
	{
		var movement = new ItemMovement
		{
			heightFactor = heightFactor,
			deltaDegrees = deltaDegrees
		};

		dstManager.AddComponentData(entity, movement);
	}
}
