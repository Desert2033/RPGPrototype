using UnityEngine;

public static class PhysicDebug
{
    public static void DrawDebug(Vector3 worldPos, float radius, float seconds)
    {
        Debug.DrawRay(worldPos, Vector3.up * radius, Color.red,seconds);
        Debug.DrawRay(worldPos, Vector3.down * radius, Color.red,seconds);
        Debug.DrawRay(worldPos, Vector3.left * radius, Color.red,seconds);
        Debug.DrawRay(worldPos, Vector3.right * radius, Color.red,seconds);
        Debug.DrawRay(worldPos, Vector3.forward * radius, Color.red,seconds);
        Debug.DrawRay(worldPos, Vector3.back * radius, Color.red,seconds);
    }
}
