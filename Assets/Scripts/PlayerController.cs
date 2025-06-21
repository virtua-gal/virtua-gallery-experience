using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Rigidbody player;

    public float jumpPower = 300f;

    public float speed = 5f;


    // Start is called before the first frame update

    void Start()
    {
        player = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space) && player.transform.position.y <= 0.51f)
       {
           Vector3 jump = new Vector3(0f, jumpPower, 0f);
           player.AddForce(jump);
       }
    
    }

    // Update is called once per frame
    void FixedUpdate()
    {
       float moveHorizontal = Input.GetAxis("Horizontal");
       float moveVertical = Input.GetAxis("Vertical");

       Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);

       player.AddForce(movement * speed); 

    }

}
