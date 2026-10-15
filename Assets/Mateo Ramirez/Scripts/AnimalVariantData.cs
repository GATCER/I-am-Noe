
using UnityEngine;

public enum AnimalGender
{
    Desconocido,
    Macho,
    Hembra
}

public enum AnnoyanceLevel
{
    Bajo,
    Medio,
    Alto
}

public enum AnimalActionType
{
    Ninguna,
    AbrirJaulas,
    Escapar,
    RobarComida,
    DañarArca,
    ProducirEstiercol,
    AtacarOtrosAnimales,
    MorirAccidentalmente
}

[CreateAssetMenu(
    fileName = "NewAnimalVariant",
    menuName = "Arca/Animales/Variante"
)]
public class AnimalVariantData : ScriptableObject
{
    [Header("Datos generales")]
    [SerializeField] private AnimalSpeciesData species;
    [SerializeField] private string variantName;
    [SerializeField] private GameObject animalPrefab;

    [Header("Características")]
    [SerializeField] private AnimalGender gender;
    [SerializeField] private AnnoyanceLevel annoyanceLevel;

    [Header("Frecuencia de molestias (segundos)")]
    [SerializeField, Min(1f)]
    private float minActionInterval = 45f;

    [SerializeField, Min(1f)]
    private float maxActionInterval = 90f;

    [Header("Mecánicas disponibles")]
    [SerializeField]
    private AnimalActionType[] availableActions;

    public AnimalSpeciesData Species => species;
    public string VariantName => variantName;
    public GameObject AnimalPrefab => animalPrefab;
    public AnimalGender Gender => gender;
    public AnnoyanceLevel AnnoyanceLevel => annoyanceLevel;
    public float MinActionInterval => minActionInterval;
    public float MaxActionInterval => maxActionInterval;
    public AnimalActionType[] AvailableActions => availableActions;
}
