﻿using System;
using FSM;
using UnityEngine;
//IA2-P2
public class MeleeAttackState : MonoBaseState {

    public bool targetKilled = false;
    
    public override void UpdateLoop() {
        if (!targetKilled) {
            Debug.Log("Ataco");
			FindObjectOfType<Player>().SetHealth(0);
			targetKilled = true;
        }
    }

    public override IState ProcessInput() {
        return this;
    }
}