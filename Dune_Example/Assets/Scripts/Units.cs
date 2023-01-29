using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Units : MonoBehaviour
{
    public static event EventHandler OnAnyActionPointsChanged; // If start method on Unit Action System UI is run before start method on Units, may be we encounter problems. So we use this one

    public static event EventHandler OnAnyUnitSpawned;

    public static event EventHandler OnAnyUnitDead;

    //public event EventHandler OnActionPointsChanged;

    private GridPosition gridPosition;

    [SerializeField]
    private bool isEnemy;

    #region Before we use GetAction generic method

    //private MoveAction moveAction;
    //private SpinAction spinAction;
    //private ShootAction shootAction;

    #endregion

    private BaseAction[] baseActionArray;
    private HealthSystem healthSystem;

    [SerializeField]
    private int actionPoint;

    private int resetActionPoints;

    private void Awake()
    {
        resetActionPoints = actionPoint;

        #region Befor we use GetAction generic method
        //moveAction = GetComponent<MoveAction>();
        //spinAction = GetComponent<SpinAction>();
        //shootAction = GetComponent<ShootAction>();
        #endregion

        baseActionArray = GetComponents<BaseAction>();
        healthSystem = GetComponent<HealthSystem>();
    }
    private void Start()
    {
        gridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        LevelGrid.Instance.AddUnitAtGridPosition(gridPosition, this);

        TurnSystem.Instance.OnTurnChanged += TurnSystem_OnTurnChanged;

        healthSystem.OnDead += HealthSystem_OnDead;

        OnAnyUnitSpawned?.Invoke(this, EventArgs.Empty);
    }

    private void Update()
    {

        GridPosition newGridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        if (newGridPosition != gridPosition)
        {
            GridPosition oldGridPosition = gridPosition;
            gridPosition = newGridPosition;
            LevelGrid.Instance.UnitMoveGridPosition(this, oldGridPosition, newGridPosition);
        }
    }

    public T GetAction<T>() where T: BaseAction
    {
        foreach (BaseAction baseAction in baseActionArray)
        {
            if (baseAction is T)
                return (T)baseAction;
        }
        return null;
    }

    #region Befor we use GetAction generic method
    //public MoveAction GetMoveAction()
    //{
    //    return moveAction;
    //}

    //public SpinAction GetSpinAction()
    //{
    //    return spinAction;
    //}

    //public ShootAction GetShootAction()
    //{
    //    return shootAction;
    //}
    #endregion

    public GridPosition GetGridPosition()
    {
        return gridPosition;
    }
    
    public Vector3 GetWorldPosition()
    {
        return transform.position;
    }

    public BaseAction[] GetBaseActionArray()
    {
        return baseActionArray;
    }

    public bool TrySpendActionPointsToTakeAction(BaseAction baseAction)
    {
        if (CanSpendActionPointsToTakeAction(baseAction))
        {
            SpendActionPoints(baseAction.GetActionPointCost());
            return true;
        }
        else
            return false;
    }

    public bool CanSpendActionPointsToTakeAction(BaseAction baseAction)
    {
        return actionPoint >= baseAction.GetActionPointCost();
    }

    private void SpendActionPoints(int amount)
    {
        actionPoint -= amount;

        OnAnyActionPointsChanged?.Invoke(this, EventArgs.Empty);
    }

    public int GetActionPoints()
    {
        return actionPoint;
    }

    private void TurnSystem_OnTurnChanged(object sender, EventArgs e)
    {
        if ( (IsEnemy() && !TurnSystem.Instance.IsPlayerTurn()) ||
            (!IsEnemy() && TurnSystem.Instance.IsPlayerTurn()))
        {
            actionPoint = resetActionPoints;

            OnAnyActionPointsChanged?.Invoke(this, EventArgs.Empty);
        }
    }

    public bool IsEnemy()
    {
        return isEnemy;
    }

    public void Damage(int damageAmount)
    {
        healthSystem.Damage(damageAmount);
    }

    private void HealthSystem_OnDead(object sender, EventArgs e)
    {
        LevelGrid.Instance.RemoveUnitAtGridPosition(gridPosition, this);

        Destroy(gameObject);

        OnAnyUnitDead?.Invoke(this, EventArgs.Empty);
    }

    public float GetHealthNormalized()
    {
        return healthSystem.GetHealthNormalized();
    }
}
