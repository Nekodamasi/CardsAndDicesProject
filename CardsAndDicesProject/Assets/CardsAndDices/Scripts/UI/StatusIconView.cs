using UnityEngine;
using TMPro;
using DG.Tweening;

namespace CardsAndDices
{
    /// <summary>
    /// Manages the visual representation and animation of a single status icon.
    /// </summary>
    public class StatusIconView : MonoBehaviour
    {
        [Header("Components")]
        [SerializeField] private SpriteSelector _faceSpriteSelector;

        [SerializeField] private TextMeshProUGUI _valueText;

        [SerializeField] private StatusIconData _statusIconData;

        [Header("Animation")]
        [SerializeField] private float _shakeDuration = 0.5f;
        [SerializeField] private float _shakeStrength = 1.0f;
        [SerializeField] private int _shakeVibrato = 10;

        private int _lastDisplayedValue;
        private Vector3 _originalPosition;
        public StatusIconData StatusIconData => _statusIconData;

        public void OnAwake()
        {
            _originalPosition = transform.localPosition;
            Initialize();
            _lastDisplayedValue = 0;
        }

        /// <summary>
        /// Initializes the status icon with its data.
        /// </summary>
        public void Initialize()
        {
            _faceSpriteSelector.SelectSprite(_statusIconData.IconSpriteId);
            _valueText.gameObject.SetActive(_statusIconData.ShowValue);
        }

        /// <summary>
        /// Updates the displayed value and plays a shake animation if the value has changed.
        /// </summary>
        /// <param name="value">The new value to display.</param>
        public void UpdateDisplay(int value)
        {
            Debug.Log("<color=red>ここにきてる？</color>");
            if (_statusIconData == null) return;

            if (_statusIconData.ShowValue)
            {
                _valueText.text = string.Format(_statusIconData.FormatString, value);
            }

            if (value != _lastDisplayedValue)
            {
                PlayShakeAnimation();
            }
            _lastDisplayedValue = value;
        }

        /// <summary>
        /// Plays a shake animation on the icon.
        /// </summary>
        public void PlayShakeAnimation()
        {
            // Ensure it returns to the original position before starting a new shake
            transform.localPosition = _originalPosition;
            transform.DOShakePosition(_shakeDuration, _shakeStrength, _shakeVibrato);
        }
    }
}
