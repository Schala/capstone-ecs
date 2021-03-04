using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

/// <summary>
/// Handles startup conversion to Unity's new data oriented tech stack, and tracks game-wide variables
/// </summary>
public class GameManager : MonoBehaviour
{
	public static GameManager Instance { get; private set; } = null;

	[SerializeField] GameObject playerPrefab = null;

	EntityManager entityManager;
	World world = null;
	BlobAssetStore blobAssetStore = null;
	Entity playerEntityPrefab = Entity.Null;

	/// <summary>
	/// Runs upon game creation, ensuring only one game manager instance and setting up data oriented tech stack
	/// </summary>
	private void Awake()
	{
		if (Instance != null)
			Destroy(gameObject);

		Instance = this;
		DontDestroyOnLoad(gameObject);

		world = World.DefaultGameObjectInjectionWorld;
		entityManager = world.EntityManager;
		blobAssetStore = new BlobAssetStore();
		var settings = GameObjectConversionSettings.FromWorld(world, blobAssetStore);
		playerEntityPrefab = GameObjectConversionUtility.ConvertGameObjectHierarchy(playerPrefab, settings);
	}

	/// <summary>
	/// Clean up anything in memory
	/// </summary>
	private void OnDestroy() => blobAssetStore.Dispose();

	/// <summary>
	/// Spawns the player
	/// </summary>
	void SpawnPlayer()
	{
		var playerEntity = entityManager.Instantiate(playerEntityPrefab);

		entityManager.AddComponent(playerEntity, typeof(PlayerInput));
		entityManager.AddComponentData(playerEntity, new Translation
		{
			Value = new float3(0f, 2.25f, -0.25f)
		});

		CameraController.Instance.target = playerEntity;
	}

	/// <summary>
	/// Spawn the player at game start
	/// </summary>
	private void Start()
	{
		SpawnPlayer();
	}
}
