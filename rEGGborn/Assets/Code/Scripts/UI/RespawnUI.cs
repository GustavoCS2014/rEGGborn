using Reggborn.Core;
using Reggborn.Player;
using Reggborn.UI;
using UnityEngine;

public class RespawnUI : SceneLevelUI, IUserInterface
{

    [SerializeField] private Transform respawnPanel;

    private void Start()
    {
        PlayerController.OnPlayerDead += OnPlayerDeadEvent;
    }

    private void OnDisable()
    {
        PlayerController.OnPlayerDead -= OnPlayerDeadEvent;
    }

    private void OnPlayerDeadEvent()
    {
        GameManager.Instance.ChangeState(GameState.NextOrRetryScene);
        respawnPanel.gameObject.SetActive(true);
        defaultButton.Select();
    }

    public override void Show()
    {
        respawnPanel.gameObject.SetActive(true);
        defaultButton.Select();
    }

    public override void Close()
    {
        respawnPanel.gameObject.SetActive(false);
    }
}