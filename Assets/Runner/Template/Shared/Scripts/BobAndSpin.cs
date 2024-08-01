using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HyperCasual.Core
{
    public class BobAndSpin : MonoBehaviour
    {
        public bool UsePositionBasedOffset = true;
        public float PositionBasedScale = 2.0f;

        [Header("Bob")]
        public bool Bob = true;
        public float BobSpeed = 5.0f;
        public float BobHeight = 0.2f;

        [Header("Spin")]
        public bool Spin = true;
        public float SpinSpeed = 180.0f;
        public Transform spinTarget;

        Transform m_Transform;
        Vector3 m_StartPosition;
        Quaternion m_StartRotation;

        void Awake()
        {
            m_Transform = transform;
            m_StartPosition = m_Transform.position;
            m_StartRotation = spinTarget.rotation;
        }

        void Update()
        {
            float offset = (UsePositionBasedOffset) ? m_StartPosition.z * PositionBasedScale + Time.time : Time.time;

            if (Bob)
            {
                m_Transform.position = m_StartPosition + Vector3.up * Mathf.Sin(offset * BobSpeed) * BobHeight;
            }

            if (Spin)
            {
                spinTarget.rotation = m_StartRotation * Quaternion.AngleAxis(offset * SpinSpeed, Vector3.up);
            }
        }
    }
}
