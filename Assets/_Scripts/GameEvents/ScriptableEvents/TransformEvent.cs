using UnityEngine;

namespace WaveOne.Events
{
    [CreateAssetMenu(fileName = "New Transform Event", menuName = "Game Events/Transform Event")]
    public class TransformEvent : BaseGameEvent<Transform>
    {
    }
}