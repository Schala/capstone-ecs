using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public static CameraController Instance { get; private set; } = null;
	[SerializeField] float3 offset = float3.zero;

    public Entity target = Entity.Null;

	EntityManager entityManager;

	/// <summary>
	/// Set up our singleton instance and variables
	/// </summary>
	private void Awake()
	{
		if (Instance != null)
			Destroy(gameObject);

		Instance = this;
		entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
	}

	/// <summary>
	/// Update the camera to follow the player consistently
	/// </summary>
	void LateUpdate()
    {
		if (target == Entity.Null) return;

		var trans = entityManager.GetComponentData<Translation>(target);
		var direction = (Vector3)trans.Value - transform.position;
		transform.position = trans.Value + offset;
		transform.rotation = Quaternion.LookRotation(direction, transform.up);
    }
}
