using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using static VolumeBox.Utils.MessageBoxSkin;

namespace VolumeBox.Utils
{
    public class MessageBox: MonoBehaviour
    {
        [SerializeField] private float m_FadeInDuration = 0.2f;
        [SerializeField] private float m_FadeOutDuration = 0.2f;
        [SerializeField] private string m_DefaultOkCaption;
        [SerializeField] private string m_DefaultNoCaption;
        [SerializeField] private string m_DefaultCancelCaption;
        [Space]
        [SerializeField] private CanvasGroup m_CanvasGroup;
        [Space]
        [SerializeField] private Transform m_SkinRoot;
        [Space]
        [SerializeField] private List<MessageBoxSkin> m_Skins;

        #region Singleton
        private static MessageBox instance;
        private static object lockObject = new object();
        private static bool destroyed = false;
        private static bool reinstantiateIfDestroyed = true;

        public static bool HasInstance => instance != null;
        public static bool ReinstantiateIfDestroyed
        {
            get
            {
                return reinstantiateIfDestroyed;
            }
            set
            {
                if (value)
                {
                    destroyed = false;
                }

                reinstantiateIfDestroyed = value;
            }
        }

        public static MessageBox Instance
        {
            get
            {
                if (!reinstantiateIfDestroyed && destroyed) return null;

                lock (lockObject)
                {
                    if (instance == null)
                    {
                        instance = FindObjectOfType<MessageBox>();

                        if (instance == null)
                        {
                            var singleton = new GameObject("[SINGLETON] " + typeof(MessageBox));
                            destroyed = false;
                            instance = singleton.AddComponent<MessageBox>();
                        }
                    }
                    return instance;
                }
            }
        }

        public static void DontDestroy()
        {
            DontDestroyOnLoad(Instance.gameObject);
        }

        private void OnDestroy()
        {
            destroyed = true;
        }
        #endregion

        private MessageBoxSkin m_CurrentSkin;
        private MessageBoxData? m_CurrentData;
        private bool m_IsOpened = false;

        public UnityEvent OnShowEvent;
        public UnityEvent OnCloseEvent;
        

        private void Awake()
        {
            SetSkinInternal(m_Skins[0]);
        }

        public static void SetSkin(string skinName)
        {
            Instance.SetSkinInternal(skinName);
        }

        public static void SetSkin(MessageBoxSkin skin)
        {
            Instance.SetSkinInternal(skin);
        }

        public void SetSkinInternal(string skinName)
        {
            var skin = m_Skins.FirstOrDefault(s => s.Definition.SkinName == skinName);

            SetSkinInternal(skin);
        }

        public void SetSkinInternal(MessageBoxSkin skin)
        {
            if (skin == null)
            {
                Debug.LogWarning("Skin you trying to set is null");
                return;
            }

            if (m_CurrentSkin != null)
            {
                m_CurrentSkin.Definition.OkClickEvent.RemoveAllListeners();
                m_CurrentSkin.Definition.NoClickEvent.RemoveAllListeners();
                m_CurrentSkin.Definition.CancelClickEvent.RemoveAllListeners();
                Destroy(m_CurrentSkin.gameObject);
            }

            m_CurrentSkin = Instantiate(skin.gameObject, m_SkinRoot).GetComponent<MessageBoxSkin>();
            m_CurrentSkin.Rect.anchoredPosition = Vector2.zero;
            m_CurrentSkin.Rect.localScale = Vector3.one;
            m_CurrentSkin.Definition.OkClickEvent.AddListener(OkClick);
            m_CurrentSkin.Definition.NoClickEvent.AddListener(NoClick);
            m_CurrentSkin.Definition.CancelClickEvent.AddListener(CancelClick);
        }

        public static void Show(MessageBoxData data)
        {
            Instance.ShowInternal(data);
        }

        private void Update()
        {
            //if (Input.GetKeyDown(KeyCode.Space))
            //{
            //    StartCoroutine(HideCoroutine());
            //}
            //if (Input.GetKeyUp(KeyCode.Space))
            //{
            //    StartCoroutine(ShowCoroutine());
            //}
        }

        public void ShowInternal(MessageBoxData data)
        {
            if (m_IsOpened)
            {
                return;
            }

            if (m_CurrentSkin == null)
            {
                Debug.LogWarning("Current Message Box skin is null");
                return;
            }

            m_CurrentData = ValidateData(data);
            m_CurrentSkin.SetCaptions(data);
            m_CurrentSkin.OnShowEvent.Invoke();
            OnShowEvent.Invoke();

            StartCoroutine(nameof(ShowCoroutine));

            m_IsOpened = true;
        }

        private MessageBoxData ValidateData(MessageBoxData data)
        {
            if (data.OkAction == null)
            {
                data.OkAction = Close;
            }

            if (data.NoAction == null)
            {
                data.NoAction = Close;
            }

            if(data.CancelAction == null)
            {
                data.CancelAction = Close;
            }

            if (data.OkCaption == string.Empty)
            {
                data.OkCaption = m_DefaultOkCaption;
            }

            if (data.CancelCaption == string.Empty)
            {
                data.CancelCaption = m_DefaultCancelCaption;
            }

            if (data.NoCaption == string.Empty)
            {
                data.NoCaption = m_DefaultNoCaption;
            }

            return data;
        }

        private IEnumerator ShowCoroutine()
        {
            if (m_CanvasGroup == null) yield break;

            StopCoroutine(nameof(HideCoroutine));

            m_CanvasGroup.interactable = true;
            m_CanvasGroup.blocksRaycasts = true;

            float alpha = m_CanvasGroup.alpha;
            float stack = 0;

            while (alpha < 1)
            {
                stack += Time.deltaTime / m_FadeInDuration;
                alpha = Mathf.Lerp(0, 1, stack);
                m_CanvasGroup.alpha = alpha;
                yield return null;
            }
        }

        private IEnumerator HideCoroutine()
        {
            if (m_CanvasGroup == null) yield break;

            StopCoroutine(nameof(ShowCoroutine));

            float alpha = m_CanvasGroup.alpha;
            float stack = 0;

            while (alpha > 0)
            {
                stack += Time.deltaTime / m_FadeOutDuration;
                alpha = Mathf.Lerp(1, 0, stack);
                m_CanvasGroup.alpha = alpha;
                yield return null;
            }

            m_CanvasGroup.interactable = false;
            m_CanvasGroup.blocksRaycasts = false;
        }

        public void Close()
        {
            if (!m_IsOpened)
            {
                return;
            }

            StartCoroutine(nameof(HideCoroutine));

            m_CurrentSkin.OnCloseEvent.Invoke();
            OnCloseEvent.Invoke();

            m_IsOpened = false;
        }

        public void OkClick()
        {
            if (IsDataValuable())
            {
                m_CurrentData.Value.OkAction.Invoke();
            }

            Close();
        }

        public void CancelClick()
        {
            if(IsDataValuable())
            {
                m_CurrentData.Value.CancelAction.Invoke();
            }

            Close();
        }

        public void NoClick()
        {
            if (IsDataValuable())
            {
                m_CurrentData.Value.NoAction.Invoke();
            }

            Close();
        }

        private bool IsDataValuable()
        {
            if (m_CurrentData == null)
            {
                Debug.LogWarning("Current Message Box data is null");
                return false;
            }

            return true;
        }

    }

    [Serializable]
    public struct MessageBoxData
    {
        public string OkCaption;
        public string CancelCaption;
        public string NoCaption;
        public string MainCaption;
        public string HeaderCaption;
        public OkButtonState OkButtonState;
        public Action OkAction;
        public Action NoAction;
        public Action CancelAction;
    }
}
