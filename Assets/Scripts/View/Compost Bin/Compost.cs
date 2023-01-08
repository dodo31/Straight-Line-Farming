using UnityEngine;

public class Compost : MonoBehaviour
{
    [SerializeField]
    private CompostBin bin;

    private CompostStates compostState;

    protected void Update()
    {
        switch (compostState)
        {
            case CompostStates.CLOSED:

                break;
            case CompostStates.OPENING:

                break;
            case CompostStates.OPEN:

                break;
            case CompostStates.CLOSING:

                break;
        }
    }

    public void Open()
    {
        bin.Open();
    }

    public void Close()
    {
        bin.Close();
    }
}