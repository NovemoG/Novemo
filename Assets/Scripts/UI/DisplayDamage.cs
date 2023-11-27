using DG.Tweening;
using Enums;
using Managers;
using TMPro;
using UnityEngine;
using Character = Characters.Character;
using FontStyles = TMPro.FontStyles;

namespace UI
{
    public class DisplayDamage : MonoBehaviour
    {
        public Transform indicatorCanvas;
        public GameObject indicatorPrefab;

        private Color[] _indicatorColors;
        private TextMeshProUGUI _indicatorText;
        private Character _character;

        private readonly Vector2[] _rightPath = 
        {
            new (0.4f, 0.4f), 
            new (0.43f, 0.37f),
            new (0.46f, 0.33f),
            new (0.5f, 0.3f),
        };
        
        private readonly Vector2[] _leftPath = 
        {
            new (-0.4f, 0.4f), 
            new (-0.43f, 0.37f),
            new (-0.46f, 0.33f),
            new (-0.5f, 0.3f),
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
            
            var path = new Vector3[4];
            var charPosition = source.transform.position;
             
            for (int i = 0; i < 4; i++)
            {
                path[i] = _path ?
                    new Vector2(charPosition.x + _rightPath[i].x, charPosition.y + _rightPath[i].y) :
                    new Vector2(charPosition.x + _leftPath[i].x, charPosition.y + _leftPath[i].y);
            }
            
            var indicator = Instantiate(indicatorPrefab, path[0], new Quaternion(), indicatorCanvas).transform;
            
            _indicatorText = indicator.GetChild(0).GetComponent<TextMeshProUGUI>();
            if (isCrit)
            {
                _indicatorText.fontStyle = FontStyles.Bold;
                _indicatorText.text = "{crit} ";
            }
            _indicatorText.text += value.ToString("F0");
            _indicatorText.color = _indicatorColors[(int)damageType];
            
            var scale = Mathf.Clamp(value / 500, 0.5f, 1f);

            if (isCrit) scale += 0.25f;
            
            _indicatorText.transform.localScale = new Vector3(scale, scale, scale);

            indicator.DOPath(path, 1.25f);
            indicator.DOScale(new Vector3(0.25f, 0.25f, 0.25f), 1f).SetDelay(0.25f).OnComplete(() => Destroy(indicator.gameObject));
        }
    }
}