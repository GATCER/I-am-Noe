
using UnityEngine;

public enum AnimalFoodType
{
    Herbivoro,
    Carnivoro,
    Omnivoro
}

[CreateAssetMenu(
    fileName = "NewAnimalSpecies",
    menuName = "Arca/Animales/Especie"
)]
public class AnimalSpeciesData : ScriptableObject
{
    [Header("Identificación")]
    [SerializeField] private string speciesId;
    [SerializeField] private string speciesName;

    [Header("Alimentación")]
    [SerializeField] private AnimalFoodType foodType;

    public string SpeciesId => speciesId;
    public string SpeciesName => speciesName;
    public AnimalFoodType FoodType => foodType;
}
