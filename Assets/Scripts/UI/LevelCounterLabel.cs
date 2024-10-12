using UnityEngine;
using TMPro;

public class LevelCounterLabel : MonoBehaviour
{
    static public LevelCounterLabel instance => _inst;
    static LevelCounterLabel _inst;
    LevelCounterLabel(){ _inst = this; }

    [SerializeField] TextMeshProUGUI label;

    public void SetCount(int levelCount)
    {
        label.text = $"{levelCount}";
    }
}
