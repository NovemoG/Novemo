using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;

namespace UI
{
    public class HealthBar : Bar
    {
        public int backHealthId;
        public RectTransform healthBack;

        private GameObject _healthBackObject;
        private float _previousHealth;
        
        private TweenerCore<float, float, FloatOptions> _backHealthTween;

        protected override void Awake()
        {
            base.Awake();
            
            _healthBackObject = healthBack.gameObject;
            _previousHealth = Character.CurrentHealth;

            Character.HealthChange += UpdateHealthBar;
        }

        private void UpdateHealthBar(float current, float max, float change)
        {
            DOTween.Kill(tweenId);

            var fillPercentage = current / max;
            var healthBackPosition = -(650f - (650f * fillPercentage));
            
            DOTween.To(() => slider.value, x => slider.value = x, fillPercentage, 0.05f).intId = tweenId;
            
            if (_previousHealth > current || healthBack.anchoredPosition.x > healthBackPosition)
            {
                if (_backHealthTween != null)
                {
                    _backHealthTween.ChangeEndValue(healthBackPosition, true);
                }
                else
                {
                    _backHealthTween = DOTween.To(() => healthBack.anchoredPosition.x,
                                           x => healthBack.anchoredPosition = new Vector2(x, 0), healthBackPosition,
                                           0.35f).SetDelay(0.5f).OnComplete(() => _backHealthTween = null);
                    _backHealthTween.intId = backHealthId;
                }
            }
            else
            {
                DOTween.Kill(backHealthId);
                DOTween.To(() => healthBack.anchoredPosition.x, x => healthBack.anchoredPosition = new Vector2(x, 0),
                    healthBackPosition, 0.05f);
                _backHealthTween = null;
            }

            _previousHealth = current;
        }
    }
}