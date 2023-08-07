using Characters;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class Bar : MonoBehaviour
    {
        public Slider slider;
        
        protected Character Character;
        protected int TweenId;

        protected virtual void Awake()
        {
            Character = GetComponent<Character>();
        }
    }
}