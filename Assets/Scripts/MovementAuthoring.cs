using Unity.Entities;
using UnityEngine;

/// <summary>
/// Set movement and physics from the Unity inspector
/// </summary>
public class MovementAuthoring : MonoBehaviour, IConvertGameObjectToEntity
{
	[SerializeField] float moveSpeed = 2.5f;

	/// <summary>
	/// Convert the data entered via Unity's inspector into a DOTS component instance.
	/// </summary>
	public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
	{
		//if (!enabled) return;

		var movement = new Movement
		{
			moveSpeed = moveSpeed
		};

		dstManager.AddComponentData(entity, movement);
	}
}
