using UnityEngine;
using DG.Tweening;

public class Tentacle : MonoBehaviour
{
    [Header("Parent")]    
    [SerializeField] InteractiveCollider parentEnemyInterCollider;
    [SerializeField] Damage parentEnemyDamage;
    [Header("Tentacle")]    
    [SerializeField] Transform tentacleFbx;
    [SerializeField] SkinnedMeshRenderer tentacleRenderer;
    [SerializeField] Animation tentacleAnimation;
    [Header("Options")]
    [SerializeField] float fadeDuration = .08f;

    PlayerCharacter player;
    
    void Awake()
    {
        parentEnemyInterCollider
            .onPlayerEnter
            .AddListener(player =>
            {
                this.player = player;

                FacePlayer(player);

                tentacleRenderer.material.DOFade(1, fadeDuration);

                tentacleAnimation.Play();
            });

        parentEnemyInterCollider
            .GetComponent<Health>()
            .onDie
            .AddListener(() =>
            {
                tentacleAnimation.Stop();

                FadeOut();
            });
    }

    void FacePlayer(PlayerCharacter player)
    {
        Vector3 dirToPlayer =
            (player.transform.position - parentEnemyDamage.transform.position)
            .normalized
            .WithY(0);
        
        transform.rotation = Quaternion.LookRotation(dirToPlayer, Vector3.up);

        Vector3 eulers = tentacleFbx.transform.localRotation.eulerAngles;

        float dot = Vector3.Dot(parentEnemyDamage.transform.right, dirToPlayer);

        eulers.x = (dot < 0) ? 180 : 0;

        tentacleFbx.transform.localRotation = Quaternion.Euler(eulers);
    }

    public void TentacleDoDamage()
    {
        parentEnemyDamage.__DoDamage(player);

        FadeOut();
    }

    void FadeOut()
    {
        tentacleRenderer
            .material
            .DOFade(0, fadeDuration);
    }
}
