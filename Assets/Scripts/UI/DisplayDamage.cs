using Characters;
using Enums;
using Managers;
using TMPro;
using UnityEngine;

namespace UI
{
    public class DisplayDamage : MonoBehaviour
    {
        public Transform indicatorCanvas;
        public GameObject indicatorPrefab;

        private Color[] _indicatorColors;
        private TextMeshProUGUI _indicatorText;
        private Character _character;

        private readonly Vector3[] _rightPath = 
        {
            new (0.3f, 0.15f), 
            new (0.5f, 0.35f), 
            new (0.4f, 0.65f),
            new (0.6f, 0),
        };
        
        private readonly Vector3[] _leftPath = 
        {
            new (-0.3f, 0.15f), 
            new (-0.5f, 0.35f), 
            new (-0.4f, 0.65f),
            new (-0.6f, 0),
        };

        private bool _path;

        private void Awake()
        {
            _character = GetComponent<Character>();
            _indicatorColors = GameManager.Instance.UIManager.indicatorColors;
            indicatorCanvas.GetComponent<Canvas>().worldCamera = GameManager.Instance.mainCamera;

            _character.DamageTaken += ShowDamage;
        }
        
        private void ShowDamage(Character source, DamageType damageType, float value, bool isCrit)
        {
            if (value < 3) return;
            _path = !_path;

            var indicator = Instantiate(indicatorPrefab, transform);
            
            _indicatorText = indicator.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
            if (isCrit)
            {
                _indicatorText.fontStyle = FontStyles.Bold;
                _indicatorText.text = "{crit} ";
            }
            _indicatorText.text += value.ToString("F0");
            _indicatorText.color = _indicatorColors[(int)damageType];
            
            var scale = Mathf.Clamp(value / 500, 0.5f, 1f);
            _indicatorText.transform.localScale = new Vector3(scale, scale, scale);

            LeanTween.move(indicator, _path ? _rightPath : _leftPath, 1.25f).setEase(LeanTweenType.easeOutQuad);
            LeanTween.scale(indicator, new Vector3(1, 1, 1), 1.5f).setDestroyOnComplete(true);
        }
    }
}