using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using TMPro;

namespace HyperCasual.Runner
{
    public class Gate : Spawnable, ITarget
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

        bool m_Applied;
        Vector3 m_TextInitialScale;

        enum GateType {
            DamageBonus
        }

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

            totalValue = startingValue;

            labelType.text = $"{System.Enum.GetName(typeof(GateType), m_GateType)}";
            labelIncrement.text = $"{incrementPerHit:^;v;~}";
        }

        void OnTriggerEnter(Collider col)
        {
            if (col.CompareTag(k_PlayerTag)
                && !m_Applied)
            {
                ActivateGate();
            }
        }

        public void OnHit(Bullet bullet)
        {
            totalValue += incrementPerHit;

            labelTotalValue.text = $"{totalValue}";

            transform
                .DOPunchScale(Vector3.one * 1.1f, .2f);
        }

        void ActivateGate()
        {
            switch (m_GateType)
            {
                case GateType.DamageBonus:

                    PlayerCharacter.instance.harm.damage
                        .SetAddendUntil(nameof(GateType.DamageBonus),
                                        totalValue,
                                        GameManager.Instance.onStartGame);

                    break;
            }

            m_Applied = true;
        }
    }
}
