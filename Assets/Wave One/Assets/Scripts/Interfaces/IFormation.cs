using System.Collections.Generic;
using UnityEngine;

namespace SemihOrhan.WaveOne.Formations
{
    public interface IFormation
    {
        List<Vector3> MakeFormation(GameObject gameObject, int groupSize);
    } 
}
