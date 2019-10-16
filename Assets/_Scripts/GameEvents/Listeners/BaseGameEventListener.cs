using UnityEngine;
using UnityEngine.Events;

namespace SemihOrhan.WaveOne.Events
{
#pragma warning disable 0649
    public abstract class BaseGameEventListener<T, E, UER> : MonoBehaviour,
        IGameEventListener<T> where E : BaseGameEvent<T> where UER : UnityEvent<T>
    {
        [SerializeField] private E gameEvent;
        [SerializeField] private UER unityEventResponse;

        private void OnEnable()
        {
            if (!GameEvent)
                return;

            GameEvent.RegisterListener(this);
        }

        private void OnDisable()
        {
            if (!GameEvent)
                return;

            GameEvent.UnregisterListener(this);
        }

        public void OnEventRaised(T item)
        {
            if (unityEventResponse != null)
                unityEventResponse.Invoke(item);
        }

        public E GameEvent
        {
            get { return gameEvent; }
            set { gameEvent = value; }
        }
    }
}
