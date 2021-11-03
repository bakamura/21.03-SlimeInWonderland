using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaAtkBoss : MonoBehaviour {

    public float areaForce;
    public float areaDamage;

    private void Start() {
        GetComponent<Rigidbody2D>().AddRelativeForce(new Vector2(3, 3));
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        
    }



}
