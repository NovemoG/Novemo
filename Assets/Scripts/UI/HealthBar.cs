using UnityEngine;

namespace UI
{
    public class HealthBar : Bar
    {
        public RectTransform healthBack;

        private GameObject _healthBackObject;
        private float _previousHealth;

        private int _id;

        protected override void Awake()
        {
            base.Awake();
            
            _healthBackObject = healthBack.gameObject;
            _previousHealth = Character.CurrentHealth;

            Character.HealthChange += UpdateHealthBar;
        }

        private void UpdateHealthBar(float current, float max, float change)
        {
            LeanTween.cancel(TweenId);

            var fillPercentage = current / max;
            var healthBackPosition = -(650f - (650f * fillPercentage));
            
            TweenId = LeanTween.value(slider.value, fillPercentage, 0.05f).setOnUpdate(f => slider.value = f).uniqueId;

            if (_previousHealth > current || healthBack.anchoredPosition.x > healthBackPosition)
            {
                var descr = LeanTween.descr(_id);
                
                if (descr != null)
                {
                    descr.setTo(new Vector3(healthBackPosition, 0, 0));
                }
                else
                {
                    _id = LeanTween.moveX(healthBack, healthBackPosition, 0.35f).setDelay(0.5f).uniqueId;
                }
            }
            else
            {
                LeanTween.cancel(_healthBackObject);
                LeanTween.moveX(healthBack, healthBackPosition, 0.05f);
            }

            _previousHealth = current;
        }
    }
}