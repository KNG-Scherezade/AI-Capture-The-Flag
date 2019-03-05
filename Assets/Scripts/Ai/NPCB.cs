using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCB : NPC_Generic {
    private float a_seperation;
    private float a_rotation;
    private float a_speed;
    private AiDirector aid;
    private float max_perception;
    private float perception_constant;

    private float t2t = 10.0f;

    public NPCB(float a_seperation, float a_rotation, float a_speed, AiDirector aid, float max_perception, float perception_constant) 
    {
        this.a_seperation = a_seperation;
        this.a_rotation = a_rotation;
        this.a_speed = a_speed;
        this.aid = aid;
        this.max_perception = max_perception;
        this.perception_constant = perception_constant;
    }

    public override void executeMovement(GameObject target_reference, GameObject active_npc)
    {
        if (withinPerception(target_reference, active_npc))
        {
            Debug.Log("omw");
            Vector3 last_pos = active_npc.transform.position;
            active_npc.transform.position = AiMovement.kinematicArrive(
                            target_reference,
                            active_npc,
                            1.0f, a_speed, t2t);
            active_npc.transform.eulerAngles = AiMovement.changeLookAngle(target_reference, active_npc);
            aid.last_recorded_velocity = (active_npc.transform.position - last_pos).magnitude;
        }
        else
        {
            Debug.Log("--------out");
            aid.last_recorded_velocity = 0;
            aid.fully_rotated = false;
        }
    }

    public bool withinPerception(GameObject target_reference, GameObject active_npc)
    {
        float perception_arc = (max_perception - perception_constant * active_npc.GetComponent<Rigidbody>().velocity.magnitude);
        perception_arc = perception_arc < 0 ? 10 : perception_arc;
        //Debug.Log("PA" + perception_arc);

        Vector3 diff = (target_reference.transform.position - active_npc.transform.position);
        float angle = Vector3.Angle(active_npc.transform.forward, diff);

       // Debug.Log("A" + angle);
        return perception_arc / 2 >= angle;
    }
}
