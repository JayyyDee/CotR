using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;


public abstract class Ring: NetworkBehaviour 
{
    public abstract void SetEquiped(Boolean boole);

    public abstract void Shoot();

    public abstract void Active();

    public abstract void Passive();

    public abstract void SetAttackSpeed(float speed);
  
    public abstract void SetFirePoint(GameObject point);

    public abstract void SetPlayer(GameObject player);

    public abstract Boolean GetCanFire();

    public abstract void Drop();

    public abstract int GetForm();

    public abstract float GetActiveCooldown();

}

