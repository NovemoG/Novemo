using Characters;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace UI
{
    public class Bar : MonoBehaviour
    {
        public int tweenId;
        public Slider slider;
        
        protected Character Character;

        protected virtual void Awake()
        {
            Character = GetComponent<Character>();
        }
    }
}