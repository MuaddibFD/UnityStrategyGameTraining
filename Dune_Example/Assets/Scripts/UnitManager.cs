using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitManager : MonoBehaviour
{
    public static UnitManager Instance { get; private set; }

    private List<Units> unitList;
    private List<Units> friendlyUnitList;
    private List<Units> enemyUnitList;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.Log($"There is more than one UnitManager! {transform} - {gameObject}");
            Destroy(gameObject);
            return;
        }
        Instance = this;

        unitList = new List<Units>();
        friendlyUnitList = new List<Units>();
        enemyUnitList = new List<Units>();
    }

    private void Start()
    {
        Units.OnAnyUnitSpawned += Units_OnAnyUnitSpawned;

        Units.OnAnyUnitDead += Units_OnAnyUnitDead;
    }


    private void Units_OnAnyUnitSpawned(object sender, EventArgs e)
    {
        Units unit = sender as Units;

        unitList.Add(unit);

        if (unit.IsEnemy())
        {
            enemyUnitList.Add(unit);
        }
        else
        {
            friendlyUnitList.Add(unit);
        }
    }
    
    private void Units_OnAnyUnitDead(object sender, EventArgs e)
    {
        Units unit = sender as Units;

        unitList.Remove(unit);

        if (unit.IsEnemy())
        {
            enemyUnitList.Remove(unit);
        }
        else
        {
            friendlyUnitList.Remove(unit);
        }
    }


    public List<Units> GetUnitList()
    {
        return unitList;
    }

    public List<Units> GetFriendlyUnitList()
    {
        return friendlyUnitList;
    }

    public List<Units> GetEnemyUnitList()
    {
        return enemyUnitList;
    }



}
