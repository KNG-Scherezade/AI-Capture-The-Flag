using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Spawner : MonoBehaviour {

    public GameObject NPC_Prefab_y, NPC_Prefab_b;
    public GameObject director_prefab_y, director_prefab_b;
    public GameObject target_prefab_y, target_prefab_b;

    public Text text;

    private GameObject manager_y, manager_b;
    private GameObject target_y, target_b;
    private List<GameObject> NPCs = new List<GameObject>();

    public float padding = 0.1f;
    public int no_of_npc_y = 4;
    public int no_of_npc_b = 4;
    // Use this for initialization
    void Start () {
        BoxCollider boundry = this.GetComponent<BoxCollider>();

        target_b = Instantiate(target_prefab_b, GameObject.FindGameObjectWithTag("DropY").transform.position
            , Quaternion.identity);
        target_y = Instantiate(target_prefab_y, GameObject.FindGameObjectWithTag("DropB").transform.position
            , Quaternion.identity);

        for (int i = 0; i < no_of_npc_b; i++)
        {
            NPCs.Add(Instantiate(NPC_Prefab_b, new Vector3(
                Random.Range(-boundry.bounds.size.x / 2 + padding, boundry.bounds.size.x / 2 - padding),
                1.0f,
                Random.Range(-boundry.bounds.size.z / 2 + padding, 0 - padding)),
                new Quaternion(0.0f, Random.rotation.y, 0.0f, 1.0f)));
        }
        for (int i = 0; i < no_of_npc_y; i++)
        {
            NPCs.Add(Instantiate(NPC_Prefab_y, new Vector3(
                Random.Range(-boundry.bounds.size.x / 2 + padding, boundry.bounds.size.x / 2 - padding),
                1.0f,
                Random.Range(0 + padding, boundry.bounds.size.z / 2 - padding)),
                new Quaternion(0.0f, Random.rotation.y, 0.0f, 1.0f)));
        }
        manager_b = Instantiate(director_prefab_b, Vector3.zero, Quaternion.identity);
        manager_y = Instantiate(director_prefab_y, Vector3.zero, Quaternion.identity);

    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown("r"))
        {
            SceneManager.LoadScene("SampleScene");
        }
	}
}
