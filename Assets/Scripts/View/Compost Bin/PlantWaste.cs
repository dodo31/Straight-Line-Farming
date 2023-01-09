using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PlantWaste : MonoBehaviour
{
    [SerializeField]
    private Animator wasteAnimator;

    [SerializeField]
    private Image plantImage;

    [SerializeField]
    private TMP_Text plantCountText;

    public void SetPlantSprite(Sprite plantSprite)
    {
        plantImage.sprite = plantSprite;
    }

    public void SetPlantCount(int plantCount)
    {
        plantCountText.text = plantCount.ToString();
    }

    protected void Update()
    {
        AnimatorStateInfo animatorStateInfo = wasteAnimator.GetCurrentAnimatorStateInfo(0);

        if (animatorStateInfo.IsName("Hidden"))
        {
            DestroyImmediate(gameObject);
        }
    }

    public void Disappear()
    {
        wasteAnimator.SetTrigger("DISAPPEAR");
    }

    public void Throw()
    {
        wasteAnimator.SetTrigger("THROW");
    }
}