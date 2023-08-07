namespace UI
{
    public class ExperienceBar : Bar
    {
        protected override void Awake()
        {
            base.Awake();

            Character.ExperienceChange += UpdateExperienceBar;
        }

        private void UpdateExperienceBar(int current, int needed, int change)
        {
            LeanTween.cancel(TweenId);
            
            var fillPercentage = (float)current / needed;
            
            TweenId = LeanTween.value(slider.value, fillPercentage, 0.05f).setOnUpdate(f => slider.value = f).uniqueId;
        }
    }
}