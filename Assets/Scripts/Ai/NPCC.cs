using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCC : NPC_Generic {
    private float a_seperation;
    private float a_rotation;
    private float max_speed;
    private AiDirector aid;

    public NPCC(float a_seperation, float a_rotation, float max_speed, AiDirector aid)
    {
        this.a_seperation = a_seperation;
        this.a_rotation = a_rotation;
        this.max_speed = max_speed;
        this.aid = aid;
    }

    public override void executeMovement(GameObject target_reference, GameObject active_npc)
    {
        float seperation = (target_reference.transform.position - active_npc.transform.position).magnitude;
        if (seperation < a_seperation)
        {
            active_npc.transform.position = new Vector3(
                active_npc.transform.position.x + a_seperation / 2,
                active_npc.transform.position.y,
                active_npc.transform.position.z);
        }
        else if (aid.fully_rotated == true)
        {
            Vector3 last_pos = active_npc.transform.position;
            active_npc.transform.position = AiMovement.kinematicFlee(
                target_reference,
                active_npc,
                1.0f, max_speed, 5.0f);
            active_npc.transform.eulerAngles = AiMovement.changeLookAngle_Flee(target_reference, active_npc);
            aid.last_recorded_velocity = (active_npc.transform.position - last_pos).magnitude;
        }
        else
        {
            Vector3 diff = -(target_reference.transform.position - active_npc.transform.position);
            Vector3 diff_norm = -(target_reference.transform.position - active_npc.transform.position).normalized;

            active_npc.transform.eulerAngles = AiMovement.changeLookAngle(target_reference, active_npc);

            float move_sens = 0.3f;
            if (
               Mathf.Abs(diff_norm.x - active_npc.transform.forward.x) < move_sens
                &&
                Mathf.Abs(diff_norm.z - active_npc.transform.forward.z) < move_sens
               )
            {
                aid.fully_rotated = true;
            }
        }
    }
}
