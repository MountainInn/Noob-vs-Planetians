using UniRx;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class Health : MonoBehaviour
{
    [SerializeField] public StackedNumber Value;
    [Header("View")]
    [SerializeField] ProgressBar healthBar;
    [SerializeField] TextMeshPro healthLabel;
    [Header("Events")]
    [SerializeField] public UnityEvent onHeal;
    [SerializeField] public UnityEvent onTakeDamage;
    [SerializeField] public UnityEvent onDie;

    [HideInInspector] public Volume Volume;


    void Awake()
    {
        Volume = new (Value.initial);
    }
    void Start()
    {
        Value.result
            .Subscribe((_) =>
            {
                if (Flow.instance.currentBranch == Flow.Branch.Preparation)
                    Volume.ResizeAndRefill(Value.AsFloorInt());
                else
                    Volume.Resize(Value.AsFloorInt());
            })
            .AddTo(this);

        if (healthBar)
            healthBar.Subscribe(gameObject, Volume);

        if (healthLabel)
            Volume
                .current
                .Subscribe(cur => healthLabel.text = $"{cur}")
                .AddTo(this);
    }

    public void __TakeDamage(Bullet bullet) => TakeDamage(bullet.damage);
    public void __TakeDamage(Enemy enemy) => TakeDamage(enemy.damage);
    public void __TakeDamage(Damage harm) => TakeDamage(harm);
    public void TakeDamage(Damage harm)
    {
        if (Volume.IsEmpty)
            return;

        Volume.Subtract(harm.Value.AsFloorInt());

        onTakeDamage?.Invoke();

        if (Volume.IsEmpty)
        {
            onDie?.Invoke();
        }
    }

    public void __Heal(Healing healing) => Heal(healing);
    public void Heal(Healing healing)
    {
        Volume.Add(healing.Value);

        onHeal?.Invoke();
    }
}
