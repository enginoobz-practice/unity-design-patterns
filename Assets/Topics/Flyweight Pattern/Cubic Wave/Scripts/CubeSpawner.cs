﻿using Unity.Entities;
using UnityEngine;
using Unity.Transforms;
using Unity.Rendering;
using Unity.Mathematics;

public class CubeSpawner : MonoBehaviour
{
    [SerializeField] private Mesh unitMesh;
    [SerializeField] private Material unitMaterial;
    [SerializeField] private GameObject gameObjectPrefab;

    [SerializeField] int xSize = 10;
    [SerializeField] int ySize = 10;
    [Range(0.1f, 2f)]
    [SerializeField] float spacing = 1f;

    private Entity entityPrefab;
    private World defaultWorld;
    private EntityManager entityManager;


    void Start()
    {
        // setup references to World and EntityManager
        defaultWorld = World.DefaultGameObjectInjectionWorld;
        entityManager = defaultWorld.EntityManager;

        // generate Entity Prefab
        if (gameObjectPrefab != null)
        {
            GameObjectConversionSettings settings = GameObjectConversionSettings.FromWorld(defaultWorld, null);
            entityPrefab = GameObjectConversionUtility.ConvertGameObjectHierarchy(gameObjectPrefab, settings);

            // spawn x by y grid of Entities
            InstantiateEntityGrid(xSize, ySize, spacing);
        }
    }

    // create a single Entity from an Entity prefab
    private void InstantiateEntity(float3 position)
    {
        if (entityManager == null)
        {
            Debug.LogWarning("InstantiateEntity WARNING: No EntityManager found!");
            return;
        }

        Entity myEntity = entityManager.Instantiate(entityPrefab);
        entityManager.SetComponentData(myEntity, new Translation
        {
            Value = position
        });
    }

    // create a grid of Entities in an x by y formation
    private void InstantiateEntityGrid(int dimX, int dimY, float spacing = 1f)
    {
        for (int i = 0; i < dimX; i++)
        {
            for (int j = 0; j < dimY; j++)
            {
                InstantiateEntity(new float3(i * spacing, j * spacing, 0f));
            }
        }
    }

    // create a single Entity using the Conversion Workflow
    private void ConvertToEntity(float3 position)
    {
        if (entityManager == null)
        {
            Debug.LogWarning("ConvertToEntity WARNING: No EntityManager found!");
            return;
        }

        if (gameObjectPrefab == null)
        {
            Debug.LogWarning("ConvertToEntity WARNING: Missing GameObject Prefab");
            return;
        }

        GameObjectConversionSettings settings = GameObjectConversionSettings.FromWorld(defaultWorld, null);
        entityPrefab = GameObjectConversionUtility.ConvertGameObjectHierarchy(gameObjectPrefab, settings);

        Entity myEntity = entityManager.Instantiate(entityPrefab);
        entityManager.SetComponentData(myEntity, new Translation
        {
            Value = position
        });
    }

    // create a single Entity using "pure ECS"
    private void MakeEntity(float3 position)
    {
        if (entityManager == null)
        {
            Debug.LogWarning("ConvertToEntity WARNING: No EntityManager found!");
            return;
        }

        if (unitMesh == null || unitMaterial == null)
        {
            Debug.LogWarning("ConvertToEntity WARNING: Missing mesh or material");
            return;
        }

        EntityArchetype archetype = entityManager.CreateArchetype(
            typeof(Translation),
            typeof(Rotation),
            typeof(RenderMesh),
            typeof(RenderBounds),
            typeof(LocalToWorld)
            );

        Entity myEntity = entityManager.CreateEntity(archetype);

        entityManager.AddComponentData(myEntity, new Translation
        {
            Value = position
        });

        entityManager.AddSharedComponentData(myEntity, new RenderMesh
        {
            mesh = unitMesh,
            material = unitMaterial
        });
    }
}