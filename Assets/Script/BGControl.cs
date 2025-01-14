using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGControl : MonoBehaviour
{
    // Start is called before the first frame update
    public int changedTime = 2;
    Player player;
    public float speed = 6;
    private GameManager gameManager;

    void Start()
    {
        player = GameObject.Find("Player").GetComponent<Player>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

        //StartCoroutine("TimeSpeedChange");
    }
    // Update is called once per frame
    void Update()
    {
        if (transform.position.x >= -9.6)
            transform.Translate(Vector2.left * speed * Time.deltaTime);
        else
        {
            gameManager.GetCanCreate = true;
            Destroy(gameObject);
        }

 }
}
