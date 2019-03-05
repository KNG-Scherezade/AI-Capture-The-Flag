using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TeamManager : MonoBehaviour {
    public string team_name;
    public string enemy_name;

    private GameObject captureObjective;

    private List<GameObject> free_subordinates;
    private List<GameObject> enemy_subordinates;
    private List<GameObject> working_subordinates = new List<GameObject>();
    private List<GameObject> hunting_subordinates = new List<GameObject>();
    private List<GameObject> saving_subordinates = new List<GameObject>();
    private List<GameObject> frozen_subordinates = new List<GameObject>();

    private bool sending_guy;

    void Start()
    {
        captureObjective = GameObject.FindGameObjectWithTag("Target" + team_name);
        free_subordinates = GameObject.FindGameObjectsWithTag("NPC" + team_name).ToList();
        enemy_subordinates = GameObject.FindGameObjectsWithTag("NPC" + enemy_name).ToList();
    }

    void Update()
    {
        /*
        //search through enemies(to hunt)
        Debug.Log(team_name + enemy_subordinates.Count + "ES");
        Debug.Log(team_name + free_subordinates.Count + "FS");
        Debug.Log(team_name + saving_subordinates.Count + "SS");
        Debug.Log(team_name + hunting_subordinates.Count + "HS");
        Debug.Log(team_name + working_subordinates.Count + "WS");
        Debug.Log(team_name + frozen_subordinates.Count + "DS");
        */
        for (int j = 0; j < enemy_subordinates.Count; j++)
        {
            if (!enemy_subordinates[j].GetComponent<AiController>().getFreeze()
                && !enemy_subordinates[j].GetComponent<AiController>().getHunted())
            {
                if ((enemy_name == "B" && enemy_subordinates[j].transform.position.z > 0) ||
                    enemy_name == "Y" && enemy_subordinates[j].transform.position.z < 0)
                {
                    int closest_index = -1;
                    float previous_record = 1000;
                    for (int k = 0; k < free_subordinates.Count; k++)
                    {
                        if ((free_subordinates[k].transform.position - enemy_subordinates[j].transform.position
                            ).magnitude < previous_record)
                        {
                            previous_record = (free_subordinates[k].transform.position - enemy_subordinates[j].transform.position
                            ).magnitude;
                            closest_index = k;
                        }
                    }

                    if(closest_index != -1)
                    {
                        free_subordinates[closest_index].GetComponent<AiController>().setHunting(true, enemy_subordinates[j]);
                        enemy_subordinates[j].GetComponent<AiController>().setHunted(true);
                        hunting_subordinates.Add(free_subordinates[closest_index]);
                        free_subordinates.Remove(free_subordinates[closest_index]);
                    }
                }
            }
        }
        for (int i = 0; i < free_subordinates.Count; i++)
        {
            free_subordinates[i].GetComponent<AiController>().clearTargetReference();
            if (free_subordinates[i].GetComponent<AiController>().getFreeze())
            {
                frozen_subordinates.Add(free_subordinates[i]);
                free_subordinates.Remove(free_subordinates[i]);
                continue;
            }
            if (free_subordinates.Count > 0 && !sending_guy)
            {
                free_subordinates[i].GetComponent<AiController>().setCapture(true, captureObjective);
                sending_guy = true;
                working_subordinates.Add(free_subordinates[i]);
                free_subordinates.Remove(free_subordinates[i]);
            }
        }

        //search through frozen(to free)
        for (int j = 0; j < frozen_subordinates.Count; j++)
        {
            if (!frozen_subordinates[j].GetComponent<AiController>().getFreeze())
            {
                frozen_subordinates[j].GetComponent<AiController>().setBeingSaved(false);
                frozen_subordinates[j].GetComponent<AiController>().setCapture(false, null);
                frozen_subordinates[j].GetComponent<AiController>().setSaving(false, null);
                frozen_subordinates[j].GetComponent<AiController>().clearTargetReference();
                free_subordinates.Add(frozen_subordinates[j]);
                frozen_subordinates.Remove(frozen_subordinates[j]);
            }
            else if (!frozen_subordinates[j].GetComponent<AiController>().getBeingSaved()) {
                int closest_index = -1;
                float previous_record = 1000;
                for (int k = 0; k < free_subordinates.Count; k++)
                {
                  
                    if ((free_subordinates[k].transform.position - frozen_subordinates[j].transform.position
                        ).magnitude < previous_record)
                    {
                        previous_record = (free_subordinates[k].transform.position - frozen_subordinates[j].transform.position
                        ).magnitude;
                        closest_index = k;
                    }
                }

                if (closest_index != -1) { 
                    free_subordinates[closest_index].GetComponent<AiController>()
                        .setSaving(true, frozen_subordinates[j]);
                    frozen_subordinates[j].GetComponent<AiController>().setBeingSaved(true);
                    saving_subordinates.Add(free_subordinates[closest_index]);
                    free_subordinates.Remove(free_subordinates[closest_index]);
                }

            }
        }

        for (int j = 0; j < saving_subordinates.Count; j++)
        {
            if (!saving_subordinates[j].GetComponent<AiController>().getSaving())
            {
                free_subordinates.Add(saving_subordinates[j]);
                saving_subordinates.Remove(saving_subordinates[j]);
            }
            else if (saving_subordinates[j].GetComponent<AiController>().getFreeze())
            {
                frozen_subordinates.Add(saving_subordinates[j]);
                saving_subordinates.Remove(saving_subordinates[j]);
            }
        }

            //remove escaped/frozen hunted targets
            for (int j = 0; j < hunting_subordinates.Count; j++)
        {
            if (hunting_subordinates[j].GetComponent<AiController>().getFreeze())
            {
                frozen_subordinates.Add(hunting_subordinates[j]);          
                hunting_subordinates[j].GetComponent<AiController>()
                    .getTargetReference().GetComponent<AiController>().setHunted(false);
                hunting_subordinates[j].GetComponent<AiController>().setHunting(false, null);
                hunting_subordinates.Remove(hunting_subordinates[j]);
                continue;
            }
            if (enemy_name == "B" && 
                (hunting_subordinates[j].GetComponent<AiController>()
                .getTargetReference().transform.position.z <= 0 
                || hunting_subordinates[j].GetComponent<AiController>()
                .getTargetReference().GetComponent<AiController>().getFreeze()))
            {
                hunting_subordinates[j].GetComponent<AiController>()
                    .getTargetReference().GetComponent<AiController>().setHunted(false);
                hunting_subordinates[j].GetComponent<AiController>().setHunting(false, null);
                free_subordinates.Add(hunting_subordinates[j]);
                hunting_subordinates.Remove(hunting_subordinates[j]);
                    
            }
            else if (enemy_name == "Y" &&
                (hunting_subordinates[j].GetComponent<AiController>()
                .getTargetReference().transform.position.z >= 0 
                || hunting_subordinates[j].GetComponent<AiController>()
                .getTargetReference().GetComponent<AiController>().getFreeze()))
            {
                hunting_subordinates[j].GetComponent<AiController>()
                 .getTargetReference().GetComponent<AiController>().setHunted(false);
                hunting_subordinates[j].GetComponent<AiController>().setHunting(false, null);
                free_subordinates.Add(hunting_subordinates[j]);
                hunting_subordinates.Remove(hunting_subordinates[j]);
            }
        }
        //search through working(for flag)
        for (int j = 0; j < working_subordinates.Count; j++)
        {
            if (working_subordinates[j].GetComponent<AiController>().getFreeze())
            {
                working_subordinates[j].GetComponent<AiController>().clearTargetReference();
                frozen_subordinates.Add(working_subordinates[j]);
                working_subordinates.Remove(working_subordinates[j]);
                
                sending_guy = false; 
            }
            else if (!working_subordinates[j].GetComponent<AiController>().getCapture()
                 && !working_subordinates[j].GetComponent<AiController>().getReturn())
            {
                working_subordinates[j].GetComponent<AiController>().clearTargetReference();
                free_subordinates.Add(working_subordinates[j]);
                working_subordinates.Remove(working_subordinates[j]);

                sending_guy = false;
            }
            else if (working_subordinates[j].GetComponent<AiController>().getCapture())
            {
                sending_guy = true;
            }
        }
    }
}
