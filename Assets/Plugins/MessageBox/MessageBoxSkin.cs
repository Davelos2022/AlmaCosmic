using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace VolumeBox.Utils
{
    public class MessageBoxSkin: MonoBehaviour
    {
        [SerializeField] private MessageBoxSkinDefinition m_Definition;
        [SerializeField] private RectTransform m_Rect;
        [SerializeField] private Color m_PositiveColor = Color.green;
        [SerializeField] private Color m_NeutralColor = Color.white;
        [SerializeField] private Color m_NegativeColor = Color.red;
        [SerializeField] private bool m_UseSpriteOkButton;
        [SerializeField] private Sprite m_PositiveSprite;
        [SerializeField] private Sprite m_NeutralSprite;
        [SerializeField] private Sprite m_NegativeSprite;
        [SerializeField] private bool m_UseTextures;
        [SerializeField] private Texture2D m_PositiveTexture;
        [SerializeField] private Texture2D m_NeutralTexture;
        [SerializeField] private Texture2D m_NegativeTexture;

        public RectTransform Rect => m_Rect;
        public MessageBoxSkinDefinition Definition => m_Definition;

        public UnityEvent OnShowEvent;
        public UnityEvent OnCloseEvent;
        [Space]
        public UnityEvent<string> SetMainCaptionEvent;
        public UnityEvent<string> SetHeaderCaptionevent;
        public UnityEvent<string> SetCancelCaptionEvent;
        public UnityEvent<string> SetNoCaptionEvent;
        public UnityEvent<string> SetOkCaptionEvent;

        public void SetCaptions(MessageBoxData data)
        {
            if(m_Definition.CancelButtonText != null)
            {
                m_Definition.CancelButtonText.text = data.CancelCaption;
                SetCancelCaptionEvent.Invoke(data.CancelCaption);
            }

            if(m_Definition.OkButtonText != null)
            {
                m_Definition.OkButtonText.text = data.OkCaption;
                SetOkCaptionEvent.Invoke(data.OkCaption);
            }

            if(m_Definition.NoButtonText != null)
            {
                m_Definition.NoButtonText.text = data.NoCaption;
                SetNoCaptionEvent.Invoke(data.NoCaption);
            }

            if(m_Definition.MainText != null)
            {
                m_Definition.MainText.text = data.MainCaption;
                SetMainCaptionEvent.Invoke(data.MainCaption);
            }
            
            if(m_Definition.HeaderText != null)
            {
                m_Definition.HeaderText.text = data.HeaderCaption;
                SetHeaderCaptionevent.Invoke(data.HeaderCaption);
            }

            switch (data.OkButtonState)
            {
                case OkButtonState.Neutral:
                    if(m_UseSpriteOkButton)
                    {
                        if (m_UseTextures)
                        {
                            m_Definition.OkImageTextureStateEvent.Invoke(m_NeutralTexture);
                        }
                        else
                        {
                            m_Definition.OkImageSpriteStateEvent.Invoke(m_NeutralSprite);
                        }
                    }
                    else
                    {
                            m_Definition.OkImageColorStateEvent.Invoke(m_NeutralColor);
                    }
                    break;
                case OkButtonState.Positive:
                    if (m_UseSpriteOkButton)
                    {
                        if (m_UseTextures)
                        {
                            m_Definition.OkImageTextureStateEvent.Invoke(m_PositiveTexture);
                        }
                        else
                        {
                            m_Definition.OkImageSpriteStateEvent.Invoke(m_PositiveSprite);
                        }
                    }
                    else
                    {
                        m_Definition.OkImageColorStateEvent.Invoke(m_PositiveColor);
                    }
                    break;
                case OkButtonState.Negative:
                    if (m_UseSpriteOkButton)
                    {
                        if (m_UseTextures)
                        {
                            m_Definition.OkImageTextureStateEvent.Invoke(m_NegativeTexture);
                        }
                        else
                        {
                            m_Definition.OkImageSpriteStateEvent.Invoke(m_NegativeSprite);
                        }
                    }
                    else
                    {
                        m_Definition.OkImageColorStateEvent.Invoke(m_NegativeColor);
                    }
                    break;
            }

        }

        public void OkClick()
        {
            m_Definition.OkClickEvent.Invoke();
        }

        public void NoClick()
        {
            m_Definition.NoClickEvent.Invoke();
        }

        public void CancelClick()
        {
            m_Definition.CancelClickEvent.Invoke();
        }
    }

    [Serializable]
    public class MessageBoxSkinDefinition
    {
        public string SkinName;
        public UnityEvent OkClickEvent;
        public UnityEvent NoClickEvent;
        public UnityEvent CancelClickEvent;
        public UnityEvent<Texture2D> OkImageTextureStateEvent;
        public UnityEvent<Sprite> OkImageSpriteStateEvent;
        public UnityEvent<Color> OkImageColorStateEvent;
        public TMP_Text OkButtonText;
        public TMP_Text CancelButtonText;
        public TMP_Text NoButtonText;
        public TMP_Text MainText;
        public TMP_Text HeaderText;
    }

    public enum OkButtonState
    {
        Neutral,
        Positive,
        Negative,
    }
}
