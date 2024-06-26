using UnityEngine;

[CreateAssetMenu(fileName = "GunSO", menuName = "SO/GunSO")]
public class GunSO  : ScriptableObject
{
    [SerializeField] public int damage;
    [SerializeField] public float rate;
    [SerializeField] public int range;

    [SerializeField] public GameObject view;

    // [SerializeField] Mesh mesh;
    // [SerializeField] Material material;

}
