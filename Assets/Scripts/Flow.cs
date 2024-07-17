using UnityEngine;
using HyperCasual.Core;
using HyperCasual.Runner;
using HyperCasual.Gameplay;
using Cysharp.Threading.Tasks;
using System.Threading;
using System.Threading.Tasks;
using System.Collections;
using UnityEngine.SceneManagement;
using YG;
using DG.Tweening;
using Zenject;

public class Flow : MonoBehaviour
{
    static public Flow instance => _inst;
    static Flow _inst;
    Flow(){ _inst = this; }

    [SerializeField] Material skyboxMaterial;
    [Space]
    [SerializeField] WorldLevel[] levels;
    [Space]
    [SerializeField] SequenceManager m_SequenceManagerPrefab;
    [SerializeField] GameObject[] levelManagers;

    [Inject] void Construct(YandexSaveSystem sv)
    {
        sv.Register(
            save => {
                YandexGame.savesData.levelCount = levelCount;
                YandexGame.savesData.currentLevelIndex = currentLevelIndex;
            },
            load => {
                levelCount = YandexGame.savesData.levelCount;
                currentLevelIndex = YandexGame.savesData.currentLevelIndex;
            });
    }

    public Branch currentBranch => branch;
    Branch branch = Branch.NONE;

    System.Threading.CancellationTokenSource onAppQuitCancellation = new CancellationTokenSource();

    SceneController sceneController;
    int
        levelCount,
        currentLevelIndex;

    public int CurrentLevel => currentLevelIndex + 1;

    public enum Branch {
        NONE,
        Preparation,
        LevelInProgress, Lose, Win,
        Ressurect, Retry,
        Continue, MultiplyMoney,
        StartLoadingLevel
    }


    void Awake()
    {
        sceneController = new SceneController(SceneManager.GetActiveScene());
    }

    void Start()
    {
        Initialize().Forget();
    }

    async UniTask Initialize()
    {
        Instantiate(m_SequenceManagerPrefab);
        SequenceManager.Instance.Initialize();

        currentLevelIndex = 0;

        UIManager.Instance.Initialize();

        await LoadLevel();

        await
            UniTask
            .WaitUntil(() => PlayerCharacter.instance != null);

        UpgradeHold.instance.Initialize();

        UIManager.Instance.GetView<UpgradeScreen>().OtherInitialize();

        await UniTask.Yield(PlayerLoopTiming.Update);

        MainLoop().Forget();
    }

    void OnApplicationQuit()
    {
        onAppQuitCancellation.Cancel();
    }

    async UniTask MainLoop()
    {
        branch = Branch.Preparation;

        do
        {
            branch = branch switch
                {
                    Branch.Preparation => await ShowUpgradeScreen(),

                    Branch.LevelInProgress => await WaitForGameplayResult(),

                    Branch.Lose => await ShowRetryScreen(),
                    Branch.Ressurect => await Ressurect(),
                    Branch.Retry => await Retry(),

                    Branch.Win => await ShowWinScreen(),
                    Branch.MultiplyMoney => await MultiplyMoney(),
                    Branch.Continue => await Continue(),

                    Branch.StartLoadingLevel => await LoadLevel(),

                    _ => throw new System.ArgumentException("Unhandled case")
                };

            await UniTask.Yield(PlayerLoopTiming.Update);
        }
        while (true);
    }

    async UniTask<Branch> Ressurect()
    {
        RewardDispenser.instance.ShowResurrect();

        await RewardDispenser.instance.onResurrect.OnInvokeAsync(onAppQuitCancellation.Token);

        PlayerCharacter.instance.Resurrect();

        return Branch.LevelInProgress;
    }

    async UniTask<Branch> Retry()
    {
        await LoadLevel();

        return Branch.Preparation;
    }

    int moneyOnEnteringWinScreen;
    int moneyAfterAdMult;

    async UniTask<Branch> ShowWinScreen()
    {
        PlayerCharacter.instance.FullStop();

        WinScreen winScreen = ShowScreen<WinScreen>();

        Vault.instance.Multiply(FinishMult.instance.currentMultiplier);

        moneyOnEnteringWinScreen = Vault.instance.buffer;
        winScreen.SetMoneyBufferText(moneyOnEnteringWinScreen);

        Adometer.instance.StartArrow();

        Branch result =
            await UniTask.WhenAny(
                winScreen.multiplyButton.OnClickAsync(),
                winScreen.continueButton.OnClickAsync()
            )
            switch {
                0 => Branch.MultiplyMoney,
                1 => Branch.Continue,
                _ => throw new System.ArgumentException()
            };

        return result;
    }

    async UniTask<Branch> MultiplyMoney()
    {
        int multiplier = Adometer.instance.StopArrow();

        RewardDispenser.instance.ShowMoneyMult(multiplier);

        await RewardDispenser.instance.onClaimX5.OnInvokeAsync(onAppQuitCancellation.Token);

        WinScreen winScreen = GetScreen<WinScreen>();

        moneyAfterAdMult = Vault.instance.buffer;

        await
            DOVirtual
            .Int(moneyOnEnteringWinScreen, moneyAfterAdMult, .75f,
                 (val) => {
                     winScreen.SetMoneyBufferText(val);
                 }
            )
            .ToUniTask()
            ;

        await UniTask.WaitForSeconds(.75f);

        return await Continue();
    }

    async UniTask<Branch> Continue()
    {
        levelCount++;
        currentLevelIndex++;

        Vault.instance.Claim();

        return Branch.StartLoadingLevel;
    }

    async UniTask<Branch> ShowRetryScreen()
    {
        RetryScreen retryScreen = ShowScreen<RetryScreen>();

        Branch result =
            await UniTask.WhenAny(
                retryScreen.ressurectButton.OnClickAsync(),
                retryScreen.retryButton.OnClickAsync()
            )
            switch {
                0 => Branch.Ressurect,
                1 => Branch.Retry,
                _ => throw new System.ArgumentException()
            };

        retryScreen.Hide();

        return result;
    }

    async UniTask<Branch> WaitForGameplayResult()
    {
        await GameManager.Instance.onLose.OnInvokeAsync(onAppQuitCancellation.Token);

        Branch result =
            FinishMult.instance.hasReachedFinish
            ? Branch.Win
            : Branch.Lose;

        return result;
    }


    async UniTask<Branch> LoadLevel()
    {
        currentLevelIndex %= levels.Length;

        WorldLevel wLevel = levels[currentLevelIndex];

        MySplashScreen splash = ShowScreen<MySplashScreen>();

        await splash.fade.FadeIn();
        {
            await sceneController.LoadNewScene(wLevel.name).ToUniTask();

            foreach (var prefab in levelManagers)
            {
                Object.Instantiate(prefab);
            }

            RenderSettings.skybox = skyboxMaterial;

            WorldLevel instantiatedWLevel = GameObject.Instantiate(wLevel);

            instantiatedWLevel.Generate();

            PlayerController.Instance.SetMaxXPosition(15);
            PlayerController.Instance.ResetPlayer();

            PlayerCharacter.instance.RefillHealth();

            PCHealthBar.instance.Resubscribe();

            GunBelt.instance.Reset();

            LevelCounterLabel.instance.SetCount(CurrentLevel);
        }
        await splash.fade.FadeOut();

        return Branch.Preparation;
    }


    async UniTask<Branch> ShowUpgradeScreen()
    {
        UpgradeScreen upgradeScreen = ShowScreen<UpgradeScreen>();

        await upgradeScreen .startLevelButton .OnClickAsync();

        upgradeScreen.Hide();

        FinishMult.instance.Reset();

        PlayerCharacter.instance.FullForward();

        return Branch.LevelInProgress;
    }

    T GetScreen<T>()
        where T : View
    {
        return UIManager.Instance.GetView<T>();
    }

    T ShowScreen<T>()
        where T : View
    {
        UIManager.Instance.Show<T>();

        return UIManager.Instance.GetView<T>();
    }
}
