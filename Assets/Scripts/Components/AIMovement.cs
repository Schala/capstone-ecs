using System;
using Unity.Entities;

/// <summary>
/// AI movement data
/// </summary>
[Serializable]
public struct AIMovement : IComponentData
{
	public float moveSpeed;
	public float turnSpeed;
	public float waypointEpsilon;
	public int nextWaypoint;
	public bool flying;
}
