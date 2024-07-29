using UniRx;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class Health : MonoBehaviour
{
    [SerializeField] public StackedNumber Value;
    [Header("View")]
    [SerializeField] public ProgressBar healthBar;
    [SerializeField] public TextMeshPro healthLabel;
    [Header("Events")]
    [SerializeField] public UnityEvent onHeal;
    [SerializeField] public UnityEvent onTakeDamage;
    [SerializeField] public UnityEvent onDie;

    [SerializeField] public Volume Volume;

    protected void Awake()
    {
        Volume = new (Value.initial);

        if (healthBar)
            healthBar.Subscribe(gameObject, Volume);

        if (healthLabel)
            Volume
                .current
                .Subscribe(cur => healthLabel.text = $"{cur}")
                .AddTo(this);

        Value.result
            .Subscribe((_) =>
            {
                Volume.ResizeAndRefill(Value.AsFloorInt());
            })
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
