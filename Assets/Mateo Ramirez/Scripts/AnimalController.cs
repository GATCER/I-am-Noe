
using UnityEngine;

public class AnimalController : MonoBehaviour
{
    [Header("Configuración")]
    [SerializeField] private AnimalVariantData variantData;

    [Header("Estado individual")]
    [SerializeField]
    private AnimalInstanceData instanceData =
        new AnimalInstanceData();

    public AnimalVariantData VariantData => variantData;

    public AnimalSpeciesData Species =>
        variantData != null ? variantData.Species : null;

    public AnimalGender Gender =>
        variantData != null
            ? variantData.Gender
            : AnimalGender.Desconocido;

    public AnnoyanceLevel AnnoyanceLevel =>
        variantData != null
            ? variantData.AnnoyanceLevel
            : AnnoyanceLevel.Bajo;

    public AnimalInstanceData InstanceData => instanceData;

    private void Awake()
    {
        if (variantData == null)
        {
            Debug.LogError(
                $"El animal {name} no tiene AnimalVariantData asignado.",
                this
            );
        }
    }

    public void Initialize(AnimalVariantData data)
    {
        variantData = data;
        instanceData = new AnimalInstanceData();
    }

    public void SetSick(bool isSick)
    {
        instanceData.SetSick(isSick);
    }

    public void TakeDamage(float damage)
    {
        instanceData.SetHealth(
            instanceData.Health - Mathf.Max(0f, damage)
        );
    }
}
