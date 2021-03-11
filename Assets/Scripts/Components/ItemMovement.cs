using System;
using Unity.Entities;

/// <summary>
/// Movement and physics data for an item entity
/// </summary>
[Serializable]
public struct ItemMovement : IComponentData
{
	public float heightFactor;
	public float deltaDegrees;
}
