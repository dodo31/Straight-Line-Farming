using System.Collections;
using UnityEngine;

public class PlantController : MonoBehaviour
{

    [SerializeField]
    private int growFrameCount = 12;


    [SerializeField]
    private int ejectionFrameCount = 15;


    [SerializeField]
    private float ejectionAngleRange = 40;

    [SerializeField]
    private float ejectionStrength = 30f;

    public PlantTypes plantType;

    [SerializeField]
    private SpriteRenderer plantRenderer;

    private PlantStates plantState;

    private void Awake()
    {
        StartCoroutine(PlayGrow());
    }

    private IEnumerator PlayGrow()
    {
        plantState = PlantStates.GROWING;

        for (int i = 0; i <= ejectionFrameCount; i++)
        {
            float indexRaw = i / (ejectionFrameCount * 1f);
            float indexExpo = -Mathf.Pow(-(indexRaw - 1), 2) + 1;

            float size = indexExpo;
            transform.localScale = Vector3.one * size;

            yield return new WaitForFixedUpdate();
        }

        plantState = PlantStates.IDLE;
    }

    public void EjectAndDestroy()
    {
        StartCoroutine(PlayEjection());
    }

    private IEnumerator PlayEjection()
    {
        plantState = PlantStates.EJECTING;

        float startPosY = transform.localPosition.y;
        float ejectionAngle = UnityEngine.Random.Range(-ejectionAngleRange, ejectionAngleRange);

        for (int i = 0; i <= ejectionFrameCount; i++)
        {
            float indexRaw = i / (ejectionFrameCount * 1f);
            float indexExpo = -Mathf.Pow(-(indexRaw - 1), 2) + 1;

            float angle = ejectionAngle * indexExpo;

            float directionX = Mathf.Cos((angle + 90) * Mathf.Deg2Rad);
            float directionY = Mathf.Sin((angle + 90) * Mathf.Deg2Rad);

            float deltaX = directionX * ejectionStrength;
            float deltaY = directionY * ejectionStrength;

            transform.localPosition += new Vector3(deltaX, deltaY, 0);
            transform.localRotation = Quaternion.Euler(new Vector3(0, 0, angle));

            float opacity = 1 - indexRaw;
            plantRenderer.color = new Color(1, 1, 1, opacity);

            yield return new WaitForFixedUpdate();
        }

        DestroyImmediate(gameObject);
    }

    public void Destroy()
    {
        DestroyImmediate(gameObject);
    }

    public void SetPlantType(PlantTypes plantType)
    {
        this.plantType = plantType;
    }

    public PlantTypes GetPlantType()
    {
        return plantType;
    }

    public void SetPlantSprite(Sprite plantSprite)
    {
        plantRenderer.sprite = plantSprite;
    }
}