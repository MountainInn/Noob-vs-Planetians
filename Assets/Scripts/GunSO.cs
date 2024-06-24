using UnityEngine;

[CreateAssetMenu(fileName = "GunSO", menuName = "SO/GunSO")]
public class GunSO  : ScriptableObject
{
    [SerializeField] public int damage;
    [SerializeField] public float rate;
    [SerializeField] public float distance;

    [SerializeField] public GameObject view;

    // [SerializeField] Mesh mesh;
    // [SerializeField] Material material;

}
