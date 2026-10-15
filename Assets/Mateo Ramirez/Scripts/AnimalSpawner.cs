
using System.Collections.Generic;
using UnityEngine;

public class AnimalSpawner : MonoBehaviour
{
    [Header("Variantes disponibles")]
    [SerializeField]
    private List<AnimalVariantData> availableVariants =
        new List<AnimalVariantData>();

    [Header("Cantidad por especie")]
    [SerializeField, Min(2)]
    private int minAnimalsPerSpecies = 3;

    [SerializeField, Min(2)]
    private int maxAnimalsPerSpecies = 4;

    [Header("Zona de aparición")]
    [SerializeField]
    private Transform spawnArea;

    [SerializeField]
    private Vector3 areaSize = new Vector3(10f, 0f, 10f);

    [Header("Generación")]
    [SerializeField]
    private bool spawnOnStart = true;

    private readonly List<AnimalController> spawnedAnimals =
        new List<AnimalController>();

    public IReadOnlyList<AnimalController> SpawnedAnimals =>
        spawnedAnimals;

    private void Start()
    {
        if (spawnOnStart)
        {
            SpawnAnimals();
        }
    }

    [ContextMenu("Generar animales")]
    public void SpawnAnimals()
    {
        if (availableVariants == null ||
            availableVariants.Count == 0)
        {
            Debug.LogWarning(
                "AnimalSpawner no tiene variantes asignadas.",
                this
            );
            return;
        }

        List<AnimalSpeciesData> speciesList =
            new List<AnimalSpeciesData>();

        // Obtener las especies sin repetirlas.
        foreach (AnimalVariantData variant in availableVariants)
        {
            if (variant == null || variant.Species == null)
                continue;

            if (!speciesList.Contains(variant.Species))
            {
                speciesList.Add(variant.Species);
            }
        }

        if (speciesList.Count == 0)
        {
            Debug.LogWarning(
                "No hay variantes válidas con una especie asignada.",
                this
            );
            return;
        }

        int minCount = Mathf.Max(2, minAnimalsPerSpecies);
        int maxCount = Mathf.Max(minCount, maxAnimalsPerSpecies);

        foreach (AnimalSpeciesData species in speciesList)
        {
            List<AnimalVariantData> speciesVariants =
                GetVariantsForSpecies(species);

            if (speciesVariants.Count == 0)
                continue;

            List<AnimalVariantData> selectedVariants =
                SelectVariantsForSpecies(
                    speciesVariants,
                    Random.Range(minCount, maxCount + 1)
                );

            foreach (AnimalVariantData variant in selectedVariants)
            {
                SpawnOneAnimal(variant);
            }
        }

        Debug.Log(
            $"Generación terminada. Animales creados: " +
            $"{spawnedAnimals.Count}",
            this
        );
    }

    private List<AnimalVariantData> GetVariantsForSpecies(
        AnimalSpeciesData species)
    {
        List<AnimalVariantData> result =
            new List<AnimalVariantData>();

        foreach (AnimalVariantData variant in availableVariants)
        {
            if (variant != null &&
                variant.Species == species &&
                variant.AnimalPrefab != null)
            {
                result.Add(variant);
            }
        }

        return result;
    }

    private List<AnimalVariantData> SelectVariantsForSpecies(
        List<AnimalVariantData> variants,
        int desiredCount)
    {
        List<AnimalVariantData> selected =
            new List<AnimalVariantData>();

        AnimalVariantData male = GetRandomVariantByGender(
            variants,
            AnimalGender.Macho
        );

        AnimalVariantData female = GetRandomVariantByGender(
            variants,
            AnimalGender.Hembra
        );

        // Intentar incluir ambos sexos si están disponibles.
        if (male != null && female != null)
        {
            selected.Add(male);
            selected.Add(female);
        }
        else if (male != null)
        {
            selected.Add(male);
        }
        else if (female != null)
        {
            selected.Add(female);
        }

        // Completar la población con variantes aleatorias.
        while (selected.Count < desiredCount)
        {
            AnimalVariantData chosen =
                variants[Random.Range(0, variants.Count)];

            selected.Add(chosen);
        }

        return selected;
    }

    private AnimalVariantData GetRandomVariantByGender(
        List<AnimalVariantData> variants,
        AnimalGender gender)
    {
        List<AnimalVariantData> matching =
            new List<AnimalVariantData>();

        foreach (AnimalVariantData variant in variants)
        {
            if (variant.Gender == gender)
            {
                matching.Add(variant);
            }
        }

        if (matching.Count == 0)
            return null;

        return matching[Random.Range(0, matching.Count)];
    }

    private void SpawnOneAnimal(AnimalVariantData variant)
    {
        Vector3 position = GetRandomPosition();

        GameObject animal = Instantiate(
            variant.AnimalPrefab,
            position,
            Quaternion.identity
        );

        AnimalController controller =
            animal.GetComponent<AnimalController>();

        if (controller == null)
        {
            Debug.LogError(
                $"El prefab {variant.AnimalPrefab.name} " +
                "no tiene AnimalController.",
                animal
            );

            Destroy(animal);
            return;
        }

        controller.Initialize(variant);
        spawnedAnimals.Add(controller);
    }

    private Vector3 GetRandomPosition()
    {
        Vector3 center = spawnArea != null
            ? spawnArea.position
            : transform.position;

        float x = Random.Range(-areaSize.x / 2f, areaSize.x / 2f);
        float z = Random.Range(-areaSize.z / 2f, areaSize.z / 2f);

        return center + new Vector3(x, 0f, z);
    }

    private void OnDrawGizmosSelected()
    {
        Vector3 center = spawnArea != null
            ? spawnArea.position
            : transform.position;

        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(
            center,
            new Vector3(areaSize.x, 0.1f, areaSize.z)
        );
    }
}
