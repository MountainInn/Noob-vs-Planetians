using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using TMPro;
using HyperCasual.Runner;
using UnityEngine.Events;

public class Gate : InteractiveCollider
{
    const string k_PlayerTag = "Player";

    [SerializeField] GateType m_GateType;
    [Space]
    [SerializeField] int startingValue;
    [SerializeField] int incrementPerHit;
    [SerializeField] int totalValue;
    [Space]
    [SerializeField] RectTransform m_Text;
    [Space]
    [SerializeField] TextMeshPro labelType;
    [SerializeField] TextMeshPro labelIncrement;
    [SerializeField] TextMeshPro labelTotalValue;
    [Space]
    [SerializeField] Sprite sprite;

    bool m_Applied;
    Vector3 m_TextInitialScale;
    Vector3 initialScale;

    public override void SetScale(Vector3 scale)
    {
        // Ensure the text does not get scaled
        if (m_Text != null)
        {
            float xFactor = Mathf.Min(scale.y / scale.x, 1.0f);
            float yFactor = Mathf.Min(scale.x / scale.y, 1.0f);
            m_Text.localScale = Vector3.Scale(m_TextInitialScale, new Vector3(xFactor, yFactor, 1.0f));

            m_Transform.localScale = scale;
        }
    }

    public override void ResetSpawnable()
    {
        m_Applied = false;
    }

    protected override void Awake()
    {
        base.Awake();

        if (m_Text != null)
        {
            m_TextInitialScale = m_Text.localScale;
        }

        labelType.text = m_GateType switch
            {
                GateType.DamageBonus => "Damage",
                GateType.AttackRate => "Fire Rate",
                GateType.Range => "Range",
                _ => throw new System.ArgumentException()
            };

        RandomizeStats();

        totalValue = startingValue;

        labelIncrement.text = $"{incrementPerHit:+#;-#;~#}";
        labelTotalValue.text = $"{totalValue}";

        initialScale = transform.localScale;
    }

    void RandomizeStats()
    {
        float initialValueRoll = UnityEngine.Random.value;

        float incrementRoll = UnityEngine.Random.value / initialValueRoll;

        incrementPerHit = Mathf.Clamp((int)(incrementRoll * 10),
                                      1,
                                      10);

        startingValue = Mathf.Clamp((int)(initialValueRoll * 20),
                                    3,
                                    30);
    }

    Tween punchScaleTween;

    public void __IncreaseBonus() => IncreaseBonus();
    public void IncreaseBonus()
    {
        totalValue += incrementPerHit;

        labelTotalValue.text = $"{totalValue}";

        // punchScaleTween?.Kill();
        punchScaleTween =
            transform
            .DOPunchScale(Vector3.one * .1f, .2f);
    }

    enum GateType {
        DamageBonus, AttackRate, Range
    }

    public void __ActivateGate() => ActivateGate();
    public void ActivateGate()
    {
        float multiplier = totalValue / 100f;

        StackedNumber stat = m_GateType switch
            {
                GateType.DamageBonus => PlayerCharacter.instance.damage.Value,
                GateType.AttackRate => UpgradeHold.instance.upgradeAttackRate.stat,
                GateType.Range => UpgradeHold.instance.upgradeAttackRange.stat,
                _ => throw new System.ArgumentException()
            };

        string modifierName = m_GateType switch
            {
                GateType.DamageBonus => "Gate: Damage",
                GateType.AttackRate => "Gate: Fire Rate",
                GateType.Range => "Gate: Range",
                _ => throw new System.ArgumentException()
            };

        stat
            .GetModifier(modifierName)
            .Add(multiplier)
            .Until(GameManager.Instance.onStartGame)
            .Log()
            .Apply();

        LevelUpPS.instance.Fire(transform.position, 1);

        m_Applied = true;
    }
}
