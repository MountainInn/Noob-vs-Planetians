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

public class Flow : MonoBehaviour
{
    static public Flow instance => _inst;
    static Flow _inst;
    Flow(){ _inst = this; }


    [SerializeField] WorldLevel[] levels;
    [Space]
    [SerializeField] SequenceManager m_SequenceManagerPrefab;
    [SerializeField] GameObject[] levelManagers;

    public Branch currentBranch => branch;
    Branch branch = Branch.NONE;

    System.Threading.CancellationTokenSource onAppQuitCancellation = new CancellationTokenSource();

    SceneController sceneController;
    int
        levelCount,
        currentLevelIndex;

    public enum Branch {
        NONE,
        Preparation,
        LevelInProgress, Lose, Win,
        Ressurect, Retry,
        Continue, MultiplyMoney,
        StartLoadingLevel
    }

    void OnEnable()
    {
        YandexGame.GetDataEvent += Load;
    }
    void OnDisable()
    {
        YandexGame.GetDataEvent -= Load;
    }
    void Save()
    {
        YandexGame.savesData.levelCount = levelCount;
        YandexGame.savesData.currentLevelIndex = currentLevelIndex;
        YandexGame.SaveProgress();
    }
    void Load()
    {
        levelCount = YandexGame.savesData.levelCount;
        currentLevelIndex = YandexGame.savesData.currentLevelIndex;
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

        await LoadLevel();

        await
            UniTask
            .WaitUntil(() => PlayerCharacter.instance != null);

        UIManager.Instance.Initialize();

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

        PlayerCharacter.instance.Ressurect();

        return Branch.LevelInProgress;
    }

    async UniTask<Branch> Retry()
    {
        GameManager.Instance.Retry();

        return Branch.Preparation;
    }

    async UniTask<Branch> MultiplyMoney()
    {
        RewardDispenser.instance.ShowMoneyMult();

        await RewardDispenser.instance.onClaimX5.OnInvokeAsync(onAppQuitCancellation.Token);

        return await Continue();
    }

    async UniTask<Branch> Continue()
    {
        levelCount++;
        currentLevelIndex++;

        MoneyCache.instance.Claim();

        return Branch.StartLoadingLevel;
    }

    async UniTask<Branch> ShowWinScreen()
    {
        PlayerCharacter.instance.FullStop();

        WinScreen winScreen = ShowScreen<WinScreen>();

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
        ShowScreen<Hud>();

        Branch result =
            await UniTask.WhenAny(
                GameManager.Instance.onLose.OnInvokeAsync(onAppQuitCancellation.Token),
                GameManager.Instance.onWin.OnInvokeAsync(onAppQuitCancellation.Token)
            )
            switch {
                0 => Branch.Lose,
                1 => Branch.Win,
                _ => throw new System.ArgumentException()
            };

        GetScreen<Hud>().Hide();

        return result;
    }


    async UniTask<Branch> LoadLevel()
    {
        currentLevelIndex %= levels.Length;

        WorldLevel wLevel = levels[currentLevelIndex];

        await sceneController.LoadNewScene(wLevel.name).ToUniTask();

        foreach (var prefab in levelManagers)
        {
            Object.Instantiate(prefab);
        }

        GameManager.Instance.LoadLevel(wLevel);

        PlayerController.Instance.SetMaxXPosition(20);

        PCHealthBar.instance.Resubscribe();

        return Branch.Preparation;
    }


    async UniTask<Branch> ShowUpgradeScreen()
    {
        UpgradeScreen upgradeScreen = ShowScreen<UpgradeScreen>();

        await upgradeScreen .startLevelButton .OnClickAsync();

        upgradeScreen.Hide();

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
