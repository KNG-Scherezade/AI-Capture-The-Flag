using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AiController : MonoBehaviour {

    public float a_speed = 0.5f;
    public float max_speed = 1.0f;
    public float a_seperation = 1.0f;
    public float a_rotation = 1.0f;

    public float max_perception = 45.0f;
    public float perception_constant = 1.0f;

    public string team_name;
    public bool retreat;
    public Material default_mat, froze_mat;

    private GameObject target_reference;
    private AiDirector aid;
    private NPCA NPCA;
    private NPCB NPCB;
    private NPCC NPCC;
    private NPCP NPCP;
    private NPCW NPCW;

    public GameObject getTargetReference()
    {
        return target_reference;
    }

    public void clearTargetReference()
    {
        target_reference = null;
    }

    public float getLastSpeed()
    {
        return aid.last_recorded_velocity;
    }

    public bool getSaving()
    {
        return aid.saving;
    }
    public void setSaving(bool save, GameObject target)
    {
         aid.saving = save;
        target_reference = target;
    }
    public bool getBeingSaved()
    {
        return aid.beingSaved;
    }
    public void setBeingSaved(bool save)
    {
        aid.beingSaved = save;
    }


    public void setHunted(bool hunt_state)
    {
        aid.hunted = hunt_state;
    }

    public bool getHunted()
    {
        return aid.hunted;
    }

    public void setHunting(bool hunt_state, GameObject target)
    {
        aid.hunting = hunt_state;
        target_reference = target;
    }

    public bool getHunting()
    {
        return aid.hunting;
    }

    public void setCapture(bool cap_state, GameObject target)
    {
        aid.capture = cap_state;
        target_reference = target;
    }

    public bool getCapture()
    {
        return aid.capture;
    }

    public bool getReturn()
    {
        return aid.returning;
    }

    public void setFreeze(bool fstate)
    {
        aid.freeze = fstate;
    }

    public bool getFreeze()
    {
       return aid.freeze ;
    }

    // Use this for initialization
    void Start () {
        aid = new AiDirector();
        NPCA = new NPCA(a_seperation, a_rotation, max_speed, aid);
        NPCB = new NPCB(a_seperation, a_rotation, max_speed, aid,
            max_perception, perception_constant);
        NPCC = new NPCC(a_seperation, a_rotation, max_speed, aid);
        NPCP = new NPCP(a_seperation, a_rotation, max_speed, aid);
        NPCW = new NPCW(a_seperation, a_rotation, max_speed, aid);
    }
    
    void FixedUpdate()
    {
        if (aid.freeze)
        {
            this.transform.Find("Cube").GetComponent<MeshRenderer>()
                .material = froze_mat;
            if (aid.returning)
            {
                Transform targ = this.transform.Find("Target(Clone)");

                targ.parent = GameObject.FindGameObjectWithTag("Respawn").transform;
                if(team_name == "B")
                {
                    targ.position = GameObject.FindGameObjectWithTag("DropY").transform.position;
                }
                else
                {
                    targ.position = GameObject.FindGameObjectWithTag("DropB").transform.position;
                }
                aid.returning = false;
            }
            return;
        }
        else
        {
            this.transform.Find("Cube").GetComponent<MeshRenderer>()
            .material = default_mat;
        }
        if(target_reference == null)
        {
            if(team_name == "B") {
                if (this.transform.position.z < 0 && !retreat) {
                    NPCW.executeMovement(null, this.gameObject);
                }
                else if (this.transform.position.z < -1)
                {
                    retreat = false;
                }
                else
                {
                    retreat = true;
                    NPCC.executeMovement(GameObject.FindGameObjectWithTag("DropY"), this.gameObject);
                }
            }
            else
            {
                if (this.transform.position.z > 0 && !retreat)
                {
                    NPCW.executeMovement(null, this.gameObject);
                }
                else if (this.transform.position.z > 1)
                {
                    retreat = false;
                }
                else
                {
                    retreat = true;
                    NPCC.executeMovement(GameObject.FindGameObjectWithTag("DropB"), this.gameObject);
                }
            }
        }
        if (aid.in_contact && aid.capture)
        {
            target_reference.transform.parent = this.gameObject.transform;
            if(target_reference.GetComponent<CapsuleCollider>() != null)
                target_reference.GetComponent<CapsuleCollider>().enabled = false;
            target_reference = GameObject.FindGameObjectWithTag("Drop" + team_name);
            aid.in_contact = false;
            aid.capture = false;
            aid.returning = true;
        }
        else if (aid.in_contact && aid.returning 
            && GameObject.FindGameObjectWithTag("uib").GetComponent<Text>().text.Length < 1)
        {
            aid.in_contact = false;
            aid.returning = false;
            if (team_name == "B")
            {
                GameObject.FindGameObjectWithTag("uib").GetComponent<Text>().text = "Team Blue Wins (R to reset)";
            }
            else
            {
                GameObject.FindGameObjectWithTag("uib").GetComponent<Text>().text = "Team Yellow Wins (R to reset)";
            }
        }
        else if (aid.hunting)
        {
            NPCP.executeMovement(target_reference, this.gameObject);
            if (aid.in_contact)
            {
                aid.in_contact = false;
                aid.hunting = false;
                target_reference.GetComponent<AiController>().setFreeze(true);
            }
        }
        if (aid.capture || aid.saving 
            || (aid.returning && team_name == "B" && this.transform.position.z < 0)
            || (aid.returning && team_name == "Y" && this.transform.position.z > 0))
        {
            if (aid.last_recorded_velocity < a_speed)
            {
                NPCA.executeMovement(target_reference, this.gameObject);
            }
            else if (aid.last_recorded_velocity >= a_speed)
            {
                NPCB.executeMovement(target_reference, this.gameObject);
            }
        }
        else if (aid.returning)
        {
            if (team_name == "B")
                NPCC.executeMovement(GameObject.FindGameObjectWithTag("DropY"), this.gameObject);
            else
                NPCC.executeMovement(GameObject.FindGameObjectWithTag("DropB"), this.gameObject);
        }

        if(aid.in_contact && aid.saving)
        {
            target_reference.GetComponent<AiController>().setFreeze(false);

            aid.in_contact = false;
            aid.saving = false;
        }
        //Debug.Log(aid.last_recorded_velocity);


        //NPCC.executeMovement(target_reference, this.gameObject);
    }
}
