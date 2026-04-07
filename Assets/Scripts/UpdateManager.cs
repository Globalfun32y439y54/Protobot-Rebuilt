using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;

public class UpdateManager : MonoBehaviour
{
    [System.Serializable]
    public class UpdateInfo
    {
        public string latestVersion;
        public string downloadUrl;
        public string releaseNotes;
        public bool isUpdateAvailable;
    }

    public static UpdateManager Instance { get; private set; }

    [SerializeField] private string githubOwner = "Globalfun32y439y54";
    [SerializeField] private string githubRepo = "Protobot-Rebuilt";
    [SerializeField] private string currentVersion = "1.0.0";
    
    private UpdateInfo lastUpdateInfo;
    private bool isCheckingForUpdates = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void CheckForUpdates(System.Action<UpdateInfo> onComplete = null)
    {
        if (isCheckingForUpdates)
        {
            Debug.LogWarning("Update check already in progress");
            return;
        }

        StartCoroutine(CheckForUpdatesCoroutine(onComplete));
    }

    private IEnumerator CheckForUpdatesCoroutine(System.Action<UpdateInfo> onComplete)
    {
        isCheckingForUpdates = true;
        string apiUrl = $"https://api.github.com/repos/{githubOwner}/{githubRepo}/releases/latest";

        using (UnityWebRequest request = UnityWebRequest.Get(apiUrl))
        {
            // GitHub API requires a user agent
            request.SetRequestHeader("User-Agent", "Protobot-Rebuilt-Updater");
            
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                try
                {
                    string jsonResponse = request.downloadHandler.text;
                    JSONNode releaseData = JSON.Parse(jsonResponse);

                    string latestVersion = releaseData["tag_name"].Value.TrimStart('v');
                    string downloadUrl = releaseData["assets"][0]["browser_download_url"].Value;
                    string releaseNotes = releaseData["body"].Value;

                    lastUpdateInfo = new UpdateInfo
                    {
                        latestVersion = latestVersion,
                        downloadUrl = downloadUrl,
                        releaseNotes = releaseNotes,
                        isUpdateAvailable = CompareVersions(latestVersion, currentVersion) > 0
                    };

                    Debug.Log($"Update check complete. Current: {currentVersion}, Latest: {latestVersion}, Update available: {lastUpdateInfo.isUpdateAvailable}");
                    onComplete?.Invoke(lastUpdateInfo);
                }
                catch (System.Exception e)
                {
                    Debug.LogError($"Failed to parse GitHub release data: {e.Message}");
                    onComplete?.Invoke(null);
                }
            }
            else
            {
                Debug.LogError($"Failed to check for updates: {request.error}");
                onComplete?.Invoke(null);
            }
        }

        isCheckingForUpdates = false;
    }

    public void DownloadUpdate(UpdateInfo updateInfo)
    {
        if (updateInfo == null || string.IsNullOrEmpty(updateInfo.downloadUrl))
        {
            Debug.LogError("Invalid update info");
            return;
        }

        // Open the download URL in the default browser
        Application.OpenURL(updateInfo.downloadUrl);
    }

    public UpdateInfo GetLastUpdateInfo()
    {
        return lastUpdateInfo;
    }

    public string GetCurrentVersion()
    {
        return currentVersion;
    }

    public void SetCurrentVersion(string version)
    {
        currentVersion = version;
    }

    /// <summary>
    /// Compares two semantic versions
    /// Returns: 1 if v1 > v2, -1 if v1 < v2, 0 if equal
    /// </summary>
    private int CompareVersions(string v1, string v2)
    {
        string[] parts1 = v1.Split('.');
        string[] parts2 = v2.Split('.');

        int maxLength = System.Mathf.Max(parts1.Length, parts2.Length);

        for (int i = 0; i < maxLength; i++)
        {
            int num1 = i < parts1.Length && int.TryParse(parts1[i], out int n1) ? n1 : 0;
            int num2 = i < parts2.Length && int.TryParse(parts2[i], out int n2) ? n2 : 0;

            if (num1 > num2) return 1;
            if (num1 < num2) return -1;
        }

        return 0;
    }
}
