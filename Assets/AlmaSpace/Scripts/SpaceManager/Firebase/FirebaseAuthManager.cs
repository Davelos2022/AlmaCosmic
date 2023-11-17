using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using Firebase.Extensions;
using System;
using System.Threading.Tasks;
using Firebase.RemoteConfig;
using TMPro;
using UnityEngine.UI;

public class FirebaseAuthManager : MonoBehaviour
{
    public string _googlesite;
    public string _key;

    public GameObject LoadScreen;
    public GameObject MessageBox;
    public TextMeshProUGUI TextContentMessageBox;
    public Button GotItButton;
    public float TimeWaitSecond;


    private const string _completedAuthorization = "Приложение успешно активировано на устройстве";
    private const string _notCompletedAuthorization = "Приложение уже активировано на другом устройстве";
    private const string _notConnection = "Проблема с подключением к интернету!";

    private Firebase.DependencyStatus dependencyStatus = Firebase.DependencyStatus.UnavailableOther;
    protected bool isFirebaseInitialized = false;

    // When the app starts, check to make sure that we have
    // the required dependencies to use Firebase, and if not,
    // add them if possible.
    private void Awake()
    {
        Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            dependencyStatus = task.Result;
            if (dependencyStatus == Firebase.DependencyStatus.Available)
            {
                InitializeFirebase();
            }
            else
            {
                Debug.LogError(
                  "Could not resolve all Firebase dependencies: " + dependencyStatus);
            }
        });
    }

    // Initialize remote config, and set the default values.
    void InitializeFirebase()
    {
        // [START set_defaults]
        System.Collections.Generic.Dictionary<string, object> defaults =
          new System.Collections.Generic.Dictionary<string, object>();

        // These are the values that are used if we haven't fetched data from the
        // server
        // yet, or if we ask for values that the server doesn't have:
        defaults.Add("config_test_string", "default local string");
        defaults.Add("config_test_int", 1);
        defaults.Add("config_test_float", 1.0);
        defaults.Add("config_test_bool", false);

        Firebase.RemoteConfig.FirebaseRemoteConfig.DefaultInstance.SetDefaultsAsync(defaults)
          .ContinueWithOnMainThread(task =>
          {
              // [END set_defaults]
              Debug.Log("RemoteConfig configured and ready!");

              FetchDataAsync();
              isFirebaseInitialized = true;
          });

    }
    public Task FetchDataAsync()
    {
        Debug.Log("Fetching data...");
        System.Threading.Tasks.Task fetchTask =
        Firebase.RemoteConfig.FirebaseRemoteConfig.DefaultInstance.FetchAsync(
            TimeSpan.Zero);
        return fetchTask.ContinueWithOnMainThread(FetchComplete);
    }

    private void FetchComplete(Task fetchTask)
    {
        if (!fetchTask.IsCompleted)
        {
            Debug.LogError("Retrieval hasn't finished.");
            return;
        }

        var remoteConfig = FirebaseRemoteConfig.DefaultInstance;
        var info = remoteConfig.Info;
        if (info.LastFetchStatus != LastFetchStatus.Success)
        {
            Debug.LogError($"{nameof(FetchComplete)} was unsuccessful\n{nameof(info.LastFetchStatus)}: {info.LastFetchStatus}");
            return;
        }

        // Fetch successful. Parameter values must be activated to use.
        remoteConfig.ActivateAsync()
          .ContinueWithOnMainThread(
            task =>
            {
                Debug.Log($"Remote data loaded and ready for use. Last fetch time {info.FetchTime}.");
            });
    }
    private void OnEnable()
    {
        GotItButton.onClick.AddListener(ClosePanel);
        StartCoroutine(CheckUser());
    }

    private void OnDestroy()
    {
        GotItButton.onClick.RemoveListener(ClosePanel);
    }

    IEnumerator CheckUser()
    {
        TextContentMessageBox.text = "";
        LoadScreen.SetActive(true);

        yield return new WaitForSeconds(TimeWaitSecond);

        if (PlayerPrefs.HasKey("Version"))
        {
            ClosePanel();
            yield break;
        }
        else
        {
            UnityWebRequest request = new UnityWebRequest($"{_googlesite}");
            yield return request.SendWebRequest();

            if (request.isDone && request.error == null)
            {
                string result = null;

                while (string.IsNullOrWhiteSpace(result))
                {
                    result = FirebaseRemoteConfig.DefaultInstance.GetValue(_key).BooleanValue.ToString().ToLower();
                    yield return null;
                }

                if (result == "true")
                {
                    PlayerPrefs.SetString("Version", "Full");
                    TextContentMessageBox.text = _completedAuthorization;

                }
                else
                {
                    TextContentMessageBox.text = _notCompletedAuthorization;
                }
            }
            else
            {
                TextContentMessageBox.text = _notConnection;
            }

            MessageBox.SetActive(true);
        }
    }

    private void ClosePanel()
    {
        if (PlayerPrefs.HasKey("Version"))
        {
            MessageBox.SetActive(false);
            LoadScreen.SetActive(false);

            AlmaSpace.AlmaSpaceManager.Instance.AudioManager.PlauMusic();
        }
        else
        {
            Application.Quit();
        }  
    }
}