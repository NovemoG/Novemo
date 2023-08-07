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
            LeanTween.cancel(TweenId);
            
            var fillPercentage = current / max;

            TweenId = LeanTween.value(slider.value, fillPercentage, 0.05f).setOnUpdate(f => slider.value = f).uniqueId;
        }
    }
}