using Unity.Entities;
using UnityEngine;

/// <summary>
/// Set AI movement and physics from the Unity inspector
/// </summary>
public class AIMovementAuthoring : MonoBehaviour, IConvertGameObjectToEntity
{
	[SerializeField] float moveSpeed = 1f;
	[SerializeField] float turnSpeed = 10f;
	[SerializeField] float waypointEpsilon = 0.1f;
	[SerializeField] GameObject[] waypoints = null;
	[SerializeField] bool flying = false;

	/// <summary>
	/// Convert the data entered via Unity's inspector into a DOTS component instance.
	/// </summary>
	public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
	{
		var movement = new AIMovement
		{
			moveSpeed = moveSpeed,
			turnSpeed = turnSpeed,
			waypointEpsilon = waypointEpsilon,
			nextWaypoint = 0,
			flying = flying
		};
		dstManager.AddComponentData(entity, movement);

		var wpBuffer = dstManager.AddBuffer<Waypoint>(entity);
		for (int i = 0; i < waypoints.Length; i++)
		{
			wpBuffer.Add(waypoints[i].transform.position);
			Destroy(waypoints[i]);
		}
	}
}
