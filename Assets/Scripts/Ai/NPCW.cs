using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCW : NPC_Generic
{
    private float a_seperation, a_rotation, max_speed;
    private AiDirector aid;

    private bool rando_turn;
    private float turn_f;

    private float t2t = 10.0f;

    public NPCW(float a_seperation, float a_rotation, float max_speed, AiDirector aid)
    {
        this.a_seperation = a_seperation;
        this.a_rotation = a_rotation;
        this.max_speed = max_speed;
        this.aid = aid;
    }

    public override void executeMovement(GameObject target_reference, GameObject active_npc)
    {
        Vector3 last_pos = active_npc.transform.position;
        active_npc.transform.position = active_npc.transform.forward* max_speed / 10 + active_npc.transform.position;
        if (Mathf.Repeat(Time.time, 0.1f) == 0)
        {
            rando_turn = true;
            turn_f = Random.Range(-1f, 2f);
        }

            active_npc.transform.eulerAngles = new Vector3(
                active_npc.transform.eulerAngles.x,
                active_npc.transform.eulerAngles.y + turn_f,
                active_npc.transform.eulerAngles.z);

        aid.last_recorded_velocity = (active_npc.transform.position - last_pos).magnitude;

    }
}
