using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridObject
{
    public GridSystem<GridObject> gridSystem;
    public GridPosition gridPosition;
    private List<Units> unitList;

    public GridObject(GridSystem<GridObject> gridSystem, GridPosition gridPosition)
    {
        this.gridSystem = gridSystem;
        this.gridPosition = gridPosition;
        unitList = new List<Units>();
    }

    public override string ToString()
    {
        string unitString = "";
        foreach (Units unit in unitList)
        {
            unitString += unit + "\n";
        }
        return gridPosition.ToString() + "\n" + unitString;
    }

    public void AddUnit(Units unit)
    {
        this.unitList.Add(unit);
    }

    public List<Units> GetUnitList()
    {
        return unitList;
    }

    public void RemoveUnit(Units unit)
    {
        unitList.Remove(unit);
    }

    public bool HasAnyUnit()
    {
        return unitList.Count > 0;
    }

    public Units GetUnit()
    {
        if (HasAnyUnit())
        {
            return unitList[0];
        }
        return null;
    }
}
