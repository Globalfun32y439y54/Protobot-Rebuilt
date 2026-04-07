using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UpdatePromptUI : MonoBehaviour
{
    [SerializeField] private GameObject updatePromptPanel;
    [SerializeField] private TextMeshProUGUI updateMessageText;
    [SerializeField] private TextMeshProUGUI versionText;
    [SerializeField] private TextMeshProUGUI releaseNotesText;
    [SerializeField] private Button downloadButton;
    [SerializeField] private Button remindLaterButton;
    [SerializeField] private Button skipButton;

    private UpdateManager.UpdateInfo currentUpdateInfo;

    private void Start()
    {
        if (updatePromptPanel != null)
            updatePromptPanel.SetActive(false);

        if (downloadButton != null)
            downloadButton.onClick.AddListener(OnDownloadClicked);

        if (remindLaterButton != null)
            remindLaterButton.onClick.AddListener(OnRemindLaterClicked);

        if (skipButton != null)
            skipButton.onClick.AddListener(OnSkipClicked);
    }

    public void ShowUpdatePrompt(UpdateManager.UpdateInfo updateInfo)
    {
        if (updateInfo == null || !updateInfo.isUpdateAvailable)
            return;

        currentUpdateInfo = updateInfo;

        if (updateMessageText != null)
            updateMessageText.text = $"A new version is available!";

        if (versionText != null)
            versionText.text = $"Current: {UpdateManager.Instance.GetCurrentVersion()}\nNew: {updateInfo.latestVersion}";

        if (releaseNotesText != null)
            releaseNotesText.text = updateInfo.releaseNotes;

        if (updatePromptPanel != null)
            updatePromptPanel.SetActive(true);

        Debug.Log("Update prompt displayed");
    }

    private void OnDownloadClicked()
    {
        if (currentUpdateInfo != null)
        {
            UpdateManager.Instance.DownloadUpdate(currentUpdateInfo);
            // Optionally close the prompt after downloading
            HideUpdatePrompt();
        }
    }

    private void OnRemindLaterClicked()
    {
        HideUpdatePrompt();
        Debug.Log("Update reminder postponed");
    }

    private void OnSkipClicked()
    {
        HideUpdatePrompt();
        PlayerPrefs.SetString("SkippedVersion", currentUpdateInfo.latestVersion);
        PlayerPrefs.Save();
        Debug.Log($"Skipped update version {currentUpdateInfo.latestVersion}");
    }

    public void HideUpdatePrompt()
    {
        if (updatePromptPanel != null)
            updatePromptPanel.SetActive(false);
    }
}
