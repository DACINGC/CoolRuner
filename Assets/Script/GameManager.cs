using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    Player player;
    public Transform bgParent;
    private GameObject[] bgArray;
    private GameObject[] awardArray;
    private bool canCreate;

    [HideInInspector]
    public int createCount = 0;

    public bool GetCanCreate
    {
        set { canCreate = value; }
        get { return canCreate; }
    }

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player").GetComponent<Player>();
        canCreate = true;
        bgArray = Resources.LoadAll<GameObject>("BG");
        awardArray = Resources.LoadAll<GameObject>("Award");
    }

    // Update is called once per frame
    void Update()
    {
        if (canCreate)
        {
            createCount++;

            GameObject bg;

            if (player.isAwarding)
            {
                bg = Instantiate(awardArray[Random.Range(0, awardArray.Length)], new Vector2(22f, -3.43f), Quaternion.identity);
                bg.transform.parent = bgParent;
            }
            else 
            { 
                bg = Instantiate(bgArray[Random.Range(0, bgArray.Length)], new Vector2(22f, -3.43f), Quaternion.identity);
                bg.transform.SetParent(bgParent);
            }

            if (bg.GetComponent<BGControl>() == null)
                bg.AddComponent<BGControl>();

            canCreate = false;
        }
  
    }

}
