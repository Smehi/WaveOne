using UnityEngine;
using WaveOne.StartPoints.Shapes;

namespace WaveOne.StartPoints.StartPointPickers
{
    [RequireComponent(typeof(ListOfSpheres))]
    public class ReverseOrderSphere : ReverseOrder<Sphere>
    {
    }
}