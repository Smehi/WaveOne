using UnityEngine;
using WaveOne.StartPoints.Shapes;

namespace WaveOne.StartPoints.StartPointPickers
{
    [RequireComponent(typeof(ListOfBoxes))]
    public class ReverseOrderBox : ReverseOrder<Box>
    {
    }
}