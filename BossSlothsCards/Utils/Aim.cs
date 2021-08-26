using UnityEngine;

namespace BossSlothsCards.Utils
{
    public class Aim
    {
        private static GameObject cube;

        public static float GetAimDirectionAs360(Player player)
        {
            CheckIfCube(player);
            return Vector2.SignedAngle(player.transform.position, cube.transform.position) + 180;
        }

        public static Vector2 GetAimDirectionAsVector(Player player)
        {
            CheckIfCube(player);
            var dir = (player.transform.position - cube.transform.position).normalized;
            return -dir;
        }

        private static void CheckIfCube(Player player)
        {
            if (cube == null)
            {
                var gun = player.GetComponent<Holding>().holdable;
                cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                cube.transform.parent = gun.transform;
                cube.transform.localPosition = new Vector3(0, 100, 0);
            }
        }
    }
}