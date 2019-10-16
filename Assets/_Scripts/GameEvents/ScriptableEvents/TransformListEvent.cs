using System.Collections.Generic;
using UnityEngine;

namespace SemihOrhan.WaveOne.Events
{
    [CreateAssetMenu(fileName = "New Transform List Event", menuName = "Game Events/Transform List Event")]
    public class TransformListEvent : BaseGameEvent<List<Transform>>
    {
    }
}