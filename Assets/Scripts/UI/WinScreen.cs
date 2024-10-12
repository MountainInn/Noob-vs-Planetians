using UnityEngine;
using UnityEngine.UI;
using HyperCasual.Core;
using UnityEngine.Events;
using Cysharp.Threading.Tasks;
using TMPro;

public class WinScreen : View
{
    [SerializeField] public UnityEvent onMultiplyClicked;
    [SerializeField] public UnityEvent onContinueClicked;
    [Space]
    [SerializeField] public Button multiplyButton;
    [SerializeField] public Button continueButton;
    [Space]
    [SerializeField] public TextMeshProUGUI moneyBufferLabel;

    void Awake()
    {
        multiplyButton.onClick.AddListener(() => onMultiplyClicked?.Invoke());
        continueButton.onClick.AddListener(() => onContinueClicked?.Invoke());
    }

    public void SetMoneyBufferText(int buffer)
    {
        moneyBufferLabel.text = $"+{buffer}";
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
