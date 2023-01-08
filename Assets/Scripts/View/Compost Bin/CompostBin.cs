using UnityEngine;

public class CompostBin : MonoBehaviour
{
    [SerializeField]
    private Animator binAnimator;

    public void Open()
    {
        binAnimator.SetTrigger("OPEN");
        binAnimator.ResetTrigger("CLOSE");
    }

    public void Close()
    {
        binAnimator.SetTrigger("CLOSE");
        binAnimator.ResetTrigger("OPEN");
    }
}