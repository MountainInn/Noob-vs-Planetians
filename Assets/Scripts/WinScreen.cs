using UnityEngine;
using UnityEngine.UI;
using HyperCasual.Core;
using UnityEngine.Events;

public class WinScreen : View
{
    [SerializeField] public UnityEvent onMultiplyClicked;
    [SerializeField] public UnityEvent onContinueClicked;
    [Space]
    [SerializeField] public Button multiplyButton;
    [SerializeField] public Button continueButton;

    void Awake()
    {
        multiplyButton.onClick.AddListener(() => onMultiplyClicked?.Invoke());
        continueButton.onClick.AddListener(() => onContinueClicked?.Invoke());
    }

    public override void Show()
    {
        base.Show();
    }

    public override void Hide()
    {
        base.Hide();
    }
}
