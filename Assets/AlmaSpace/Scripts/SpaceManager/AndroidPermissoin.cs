using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Android;

public class AndroidPermissoin : MonoBehaviour
{
    void Start()
    {
        CheckPlatform();
    }

    private void CheckPlatform()
    {
        switch (Application.platform)
        {
            case RuntimePlatform.Android:
                StartCoroutine(RequestStoragePermission(PermissonIsDone));
                break;
            default:
                return;
        }
    }

    private IEnumerator RequestStoragePermission(Action<bool> callback)
    {
        if (Permission.HasUserAuthorizedPermission(Permission.ExternalStorageRead))
        {
            callback?.Invoke(true);
        }
        else
        {
            Permission.RequestUserPermission(Permission.ExternalStorageRead);
            yield return new WaitUntil(() => Permission.HasUserAuthorizedPermission(Permission.ExternalStorageRead));
            callback?.Invoke(Permission.HasUserAuthorizedPermission(Permission.ExternalStorageRead));
        }
    }

    private void PermissonIsDone(bool isDone)
    {
        if (isDone)
            Debug.Log("Разрешено");
        else
            Debug.Log("Разрешение отклонено");
    }
}
