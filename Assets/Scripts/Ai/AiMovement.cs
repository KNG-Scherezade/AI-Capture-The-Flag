using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiMovement {

    private static float rot_start_time = -1;

    public static Vector3 kinematicArrive(GameObject target_reference, GameObject active_npc, float time_unit, float max_vel, float t2t)
    {
        float v = Mathf.Min(max_vel, (target_reference.transform.position - active_npc.transform.position).magnitude / t2t);
        return active_npc.transform.position + v * (active_npc.transform.forward).normalized * time_unit;
    }

    public static Vector3 kinematicSeek(GameObject target_reference, GameObject active_npc, float time_unit, float max_vel)
    {

        return active_npc.transform.position + max_vel * (active_npc.transform.forward).normalized * time_unit;
    }

    public static Vector3 changeLookAngle(GameObject target_reference, GameObject active_npc)
    {
        Vector3 diff = (target_reference.transform.position - active_npc.transform.position);
        Vector3 diff_norm = (target_reference.transform.position - active_npc.transform.position).normalized;

        float rot_ang = Vector3.SignedAngle(
            active_npc.transform.forward,
            diff, Vector3.up);

        return new Vector3(
            active_npc.transform.eulerAngles.x,
            Vector3.Lerp(
                active_npc.transform.eulerAngles,
                new Vector3(
                    active_npc.transform.eulerAngles.x,
                    active_npc.transform.eulerAngles.y + rot_ang,
                    active_npc.transform.eulerAngles.z),
                0.1f
                ).y,
            active_npc.transform.eulerAngles.z);
    }

    public static Vector3 changeLookAngle_Flee(GameObject target_reference, GameObject active_npc)
    {
        Vector3 diff = -(target_reference.transform.position - active_npc.transform.position);
        Vector3 diff_norm = -(target_reference.transform.position - active_npc.transform.position).normalized;

        float rot_ang = Vector3.SignedAngle(
            active_npc.transform.forward,
            diff, Vector3.up);

        return new Vector3(
            active_npc.transform.eulerAngles.x,
            Vector3.Lerp(
                active_npc.transform.eulerAngles,
                new Vector3(
                    active_npc.transform.eulerAngles.x,
                    active_npc.transform.eulerAngles.y + rot_ang,
                    active_npc.transform.eulerAngles.z),
                0.1f
                ).y,
            active_npc.transform.eulerAngles.z);
    }

    public static Vector3 kinematicFlee(GameObject target_reference, GameObject active_npc, float time_unit, float max_vel, float t2t)
    {
        float v = max_vel;
        return active_npc.transform.position + v * (active_npc.transform.forward).normalized * time_unit;
    }
}
