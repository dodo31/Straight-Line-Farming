using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/NameGenerator")]
public class NameGenerator : ScriptableObject
{
    public string[] namesBase;
    private int[,,] map;
    public void DoMap()
    {
        map = new int[27, 27, 27];
        for (int i = 0; i < namesBase.Length; i++)
        {
            string name = namesBase[i];
            name = name.ToLower();
            if (IsAllInBounds(name))
            {
                for (int j = -2; j < name.Length - 1; j++)
                {
                    map[CharToInt(name, j), CharToInt(name, j + 1), CharToInt(name, j + 2)]++;
                }
            }
        }
    }
    public string GenName(int minLength = 4, int maxLength = 9)
    {
        if (map == null) DoMap();
        string name = "";
        System.Random random = new();
        int tot = 0;
        while (true)
        {
            int total = 0;
            int c1 = CharToInt(name, name.Length - 2);
            int c2 = CharToInt(name, name.Length - 1);
            int i;
            for (i = 0; i < 27; i++)
            {
                total += map[c1, c2, i];
            }
            int rando = random.Next(total);
            for (i = 0; i < 27; i++)
            {
                rando -= map[c1, c2, i];
                if (rando < 0)
                {
                    break;
                }
            }
            if (i == 26)
            {
                break;
            }

            name += (char)('a' + i);
            tot++;
        }
        if (name.Length > maxLength || name.Length < minLength) return GenName();
        return ToFirstUpperCase(name);
    }

    private string ToFirstUpperCase(string name)
    {
        return name[..1].ToUpperInvariant() + name[1..];
    }

    private bool IsAllInBounds(string name)
    {
        for (int pos = 0; pos < name.Length; pos++)
        {
            if (CharToInt(name, pos) < 0 || CharToInt(name, pos) > 25)
            {
                return false;
            }
        }
        return true;
    }
    private int CharToInt(string name, int pos)
    {
        if (pos < 0 || pos >= name.Length)
        {
            return 26;
        }

        return name[pos] - 'a';
    }

}
