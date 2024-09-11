using Reggborn.Core;
using Reggborn.Objects;
using Reggborn.UI;
using UnityEngine;
using UnityEngine.UI;

public class NextLevelUI : SceneLevelUI, IUserInterface
{

    [SerializeField] private Transform nextLevelPanel;

    [SerializeField] private Button nextLevelBTN;



    private void Start()
    {
        Goal.OnGoalEnter += OnGoalEnterEvent;
    }
    private void OnDisable()
    {
        Goal.OnGoalEnter -= OnGoalEnterEvent;
    }
    private void OnGoalEnterEvent()
    {
        GameManager.Instance.ChangeState(GameState.NextOrRetryScene);
        Show();
    }

    public override void Show()
    {
        base.Show();
        nextLevelBTN.onClick.AddListener(LoadNextLevel);

        nextLevelPanel.gameObject.SetActive(true);
        defaultButton.Select();
    }

    public override void Close()
    {
        base.Close();

        nextLevelPanel.gameObject.SetActive(false);
    }

    private void LoadNextLevel()
    {
        GameManager.Instance.LoadNextScene();
    }




}