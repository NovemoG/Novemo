using DG.Tweening;

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
            DOTween.Kill(tweenId);
            
            var fillPercentage = (float)current / needed;
            
            DOTween.To(() => slider.value, x => slider.value = x, fillPercentage, 0.05f).intId = tweenId;
        }
    }
}