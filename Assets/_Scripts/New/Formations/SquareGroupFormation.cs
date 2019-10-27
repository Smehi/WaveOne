using System.Collections.Generic;
using UnityEngine;

namespace SemihOrhan.WaveOne.Formations
{
    public class SquareGroupFormation : MonoBehaviour, IFormation
    {
        /// <summary>
        /// Makes a square formation around the vector (0, 0, 0), favoring width over depth.
        /// </summary>
        /// <param name="obj">The GameObject that is being spawned.</param>
        /// <param name="groupSize">The size of the group.</param>
        /// <returns>Returns an array of positions that is made from the parameters given.</returns>
        public List<Vector3> MakeFormation(GameObject obj, int groupSize)
        {
            List<Vector3> positions = new List<Vector3>();

            int width = Mathf.CeilToInt(Mathf.Sqrt(groupSize));
            int depth = Mathf.CeilToInt((float)groupSize / (float)width);

            Collider col = obj.GetComponent<Collider>();

            float spreadWidth = col.bounds.size.x * 1.15f;
            float spreadDepth = col.bounds.size.z * 1.15f;

            // Simple calculation take width = 2 & spreadWidth = 1:
            // 2 - 1 = 1
            // 1 / -2f = -0.5
            // -0.5 * 1 = -0.5
            // each x or z the spread will be added so the middle of the square is always (0, 0, 0).
            float bottomLeftX = ((width - 1) / -2f) * spreadWidth;
            float bottomLeftZ = ((depth - 1) / -2f) * spreadDepth;

            // Loop through the depth and rows and make the positions
            for (float z = 0; z < depth; z++)
            {
                for (float x = 0; x < width; x++)
                {
                    // Add the (x * spread) and (z * spread) to their start positions
                    positions.Add(new Vector3(bottomLeftX + (x * spreadWidth), 0, bottomLeftZ + (z * spreadDepth)));
                }
            }

            return positions;
        }
    }
}
