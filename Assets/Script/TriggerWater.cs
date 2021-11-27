using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerWater : MonoBehaviour {

    public CompositeCollider2D[] colWater;

    public void transformCollider(bool bol) {
        foreach(CompositeCollider2D col in colWater) col.isTrigger = bol;
    }

}
