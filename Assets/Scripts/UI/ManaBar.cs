using DG.Tweening;

namespace UI
{
    public class ManaBar : Bar
    {
        protected override void Awake()
        {
            base.Awake();
            
            Character.ManaChange += UpdateManaBar;
        }

        private void UpdateManaBar(float current, float max, float change)
        {
            DOTween.Kill(tweenId);
            
            var fillPercentage = current / max;

            DOTween.To(() => slider.value, x => slider.value = x, fillPercentage, 0.05f).intId = tweenId;
        }
    }
}