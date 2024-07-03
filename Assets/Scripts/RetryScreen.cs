using UnityEngine;
using UnityEngine.UI;
using HyperCasual.Core;
using UnityEngine.Events;

public class RetryScreen : View
{
    [SerializeField] public UnityEvent onRessurectClicked;
    [SerializeField] public UnityEvent onRetryClicked;
    [Space]
    [SerializeField] public Button ressurectButton;
    [SerializeField] public Button retryButton;

    void Awake()
    {
        ressurectButton.onClick.AddListener(() => onRessurectClicked?.Invoke());
        retryButton.onClick.AddListener(() => onRetryClicked?.Invoke());
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
