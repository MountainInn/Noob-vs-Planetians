using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;

namespace HyperCasual.Runner
{
    /// <summary>
    /// A simple Input Manager for a Runner game.
    /// </summary>
    public class InputManager : MonoBehaviour
    {
        /// <summary>
        /// Returns the InputManager.
        /// </summary>
        public static InputManager Instance => s_Instance;
        static InputManager s_Instance;

        [SerializeField] float m_InputSensitivity = 1.5f;

        bool m_HasInput;
        Vector3 m_InputPosition;
        Vector3 m_PreviousInputPosition;

        bool
            keyboardInput,
            pressingLeft,
            pressingRight;

        void Update()
        {
            if (PlayerController.Instance == null)
            {
                return;
            }

            m_InputPosition = Mouse.current.position.ReadValue();

            if (Mouse.current.leftButton.isPressed)
            {
                if (!m_HasInput)
                {
                    m_PreviousInputPosition = m_InputPosition;
                }

                m_HasInput = true;
            }
            else if ((pressingLeft = (Keyboard.current.aKey.isPressed || Keyboard.current.leftArrowKey.isPressed))
                     ||
                     (pressingRight = (Keyboard.current.dKey.isPressed || Keyboard.current.rightArrowKey.isPressed)))
            {

                keyboardInput = true;
                m_HasInput = true;
            }
            else
            {
                m_HasInput = false;
            }

            if (m_HasInput)
            {
                if (keyboardInput)
                {
                    PlayerController.Instance.SetDeltaPosition(pressingLeft ? -100 : 100);
                }
                else
                {
                    float normalizedDeltaPosition = (m_InputPosition.x - m_PreviousInputPosition.x) / Screen.width * m_InputSensitivity;
                    PlayerController.Instance.SetDeltaPosition(normalizedDeltaPosition);
                }
            }
            else
            {
                PlayerController.Instance.CancelMovement();
            }

            m_PreviousInputPosition = m_InputPosition;

            keyboardInput = false;
        }
    }
}

