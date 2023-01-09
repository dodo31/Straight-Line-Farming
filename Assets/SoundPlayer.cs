using UnityEngine;

public class SoundPlayer : MonoBehaviour
{
    public static SoundPlayer instance;
    [SerializeField]
    public AudioSource source;
    [SerializeField]
    public AudioClip[] hello;
    [SerializeField]
    public AudioClip[] angry;
    [SerializeField]
    public AudioClip[] vroom;
    [SerializeField]
    public AudioClip[] meep;
    [SerializeField]
    public AudioClip[] sow;
    [SerializeField]
    public AudioClip[] chilli;
    [SerializeField]
    public AudioClip[] wheat;
    [SerializeField]
    public AudioClip[] pumpkin;
    [SerializeField]
    public AudioClip[] corn;
    [SerializeField]
    public AudioClip[] farting;
    [SerializeField]
    public AudioClip[] thanks;
    [SerializeField]
    public AudioClip[] gling;
    public enum SoundType
    {
        HELLO, ANGRY, VROOM, MEEP, SOW, CHILLI, WHEAT, PUMPKIN, CORN, FARTING, THANKS, GLING
    }
    public void Awake()
    {
        instance = this;
    }
    public static void PlaySound(SoundType type)
    {
        instance.PlaySoundLocal(type);
    }
    private void PlaySoundLocal(SoundType type)
    {
        source.PlayOneShot(GetSound(type));
    }

    private AudioClip GetRandom(AudioClip[] clip)
    {
        return clip[Random.Range(0, clip.Length)];
    }
    private AudioClip GetSound(SoundType type) => type switch
    {
        SoundType.HELLO => GetRandom(hello),
        SoundType.ANGRY => GetRandom(angry),
        SoundType.VROOM => GetRandom(vroom),
        SoundType.MEEP => GetRandom(meep),
        SoundType.SOW => GetRandom(sow),
        SoundType.CHILLI => GetRandom(chilli),
        SoundType.WHEAT => GetRandom(wheat),
        SoundType.PUMPKIN => GetRandom(pumpkin),
        SoundType.CORN => GetRandom(corn),
        SoundType.FARTING => GetRandom(farting),
        SoundType.THANKS => GetRandom(thanks),
        SoundType.GLING => GetRandom(gling),
        _ => throw new System.NotImplementedException(),
    };
}
