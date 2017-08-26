using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public uint Id;
    public string Name;
    public string Description;
    
    public int Attack;
    public int Defend;
    public int Agility;
    public int Intelligence;

    public int AdditiveDebuff;

    public bool[,] ReachRange;
    public Point ReachCenter;
    public bool[,] AttackRange;
    public Point AttackCenter;
    public float[,] AttackPowerRate;
    public float[,] AttackHitRate;
}
