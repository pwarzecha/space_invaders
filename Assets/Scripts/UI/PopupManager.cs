using UnityEngine;
using TMPro;
using System.Collections.Generic;
using System;
using Unity.VisualScripting;
using PrimeTween;
public enum PopupType
{
    WaveStart,
    WarmupPhase,
    FormationPhase,
}

[Serializable]
public struct PopupMessage
{
    public PopupType popupType;
    public string message;
}

public class PopupManager : Singleton<PopupManager>
{
    [SerializeField] private Transform _popupPanel; 
    [SerializeField] private Transform _hideTransform;
    [SerializeField] private Transform _targetTransform;
    [SerializeField] private TextMeshProUGUI _popupText; 
    [SerializeField] private float _moveDuration = 0.5f; 
    [SerializeField] private Ease _moveEase = Ease.OutCubic;
    [SerializeField] private List<PopupMessage> _popupMessages;

    public void ShowPopup(PopupType type)
    {
        string message = GetMessageForPopup(type);

        if (!string.IsNullOrEmpty(message))
        {
            _popupText.text = message;
            _popupPanel.gameObject.SetActive(true);

            Tween.Position(_popupPanel, _targetTransform.position, _moveDuration, _moveEase).OnComplete(() =>
                Tween.Position(_popupPanel, _hideTransform.position, _moveDuration, _moveEase, startDelay: 2f).OnComplete(() => {
                    _popupPanel.gameObject.SetActive(false);
                })
            );
        }
    }

    private string GetMessageForPopup(PopupType type)
    {
        foreach (var popup in _popupMessages)
        {
            if (popup.popupType == type)
            {
                return popup.message;
            }
        }

        Debug.LogWarning($"No message found for PopupType: {type}");
        return string.Empty;
    }
}