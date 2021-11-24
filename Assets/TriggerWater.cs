using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerWater : MonoBehaviour {

    public Collider2D[] colWater;

    public void transformCollider(bool bol) {
        foreach (Collider2D col in colWater) col.isTrigger = bol;
    }

}
