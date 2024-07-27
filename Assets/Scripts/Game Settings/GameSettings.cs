using UnityEngine;

[CreateAssetMenu(fileName = "GameSettings", menuName = "SO/GameSettings")]
public class GameSettings : ScriptableObject
{
    [Header("Wheel")]
    public float cursorRotationSpeed = 30;
    
}
