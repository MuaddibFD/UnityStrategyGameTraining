using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveAction : BaseAction
{

    private Vector3 targetPosition;

    [SerializeField]
    private float speed;

    [SerializeField]
    private float rotateSpeed;

    [SerializeField]
    private int maxMoveDistance;

    #region Befor we did not reffer character animator to Unit Animator
    //[SerializeField]
    //private Animator unitAnimator;
    #endregion

    public event EventHandler OnStartMoving;
    public event EventHandler OnStopMoving;
    protected override void Awake()
    {
        base.Awake();
        targetPosition = transform.position;
    }

    private void Update()
    {
        if (!isActive)
            return;

        float ignoreDistance = 0.2f;
        float distance = Vector3.Distance(transform.position, targetPosition);

        Vector3 moveDirection = (targetPosition - transform.position).normalized;


        if (distance > ignoreDistance)
        {
            //unitAnimator.SetBool("IsWalking", true);

            transform.position += moveDirection * speed * Time.deltaTime;
        }
        else
        {
            //unitAnimator.SetBool("IsWalking", false);
            //targetPosition = transform.position;

            OnStopMoving?.Invoke(this, EventArgs.Empty);

            ActionCompleted();
        }

        transform.forward = Vector3.Lerp(transform.forward, moveDirection, Time.deltaTime * rotateSpeed);
    }

    public override void TakeAction(GridPosition gridPosition, Action onActionComplete)
    {
        this.targetPosition = LevelGrid.Instance.GetWorldPosition(gridPosition);

        OnStartMoving?.Invoke(this, EventArgs.Empty);

        ActionStart(onActionComplete);
    }
    #region Before This method is not in Base Action to be common action
    //public bool IsValidActionGridPosition(GridPosition gridPosition)
    //{
    //    List<GridPosition> validGridPositionList = GetValidActionGridPositionList();
    //    return validGridPositionList.Contains(gridPosition);

    //}
    #endregion
    public override List<GridPosition> GetValidActionGridPositionList()
    {
        GridPosition unitGridPosition = unit.GetGridPosition();
        List<GridPosition> validGridPositionList = new List<GridPosition>();
        for (int x = -maxMoveDistance; x <= maxMoveDistance; x++)
        {
            for (int z = -maxMoveDistance; z <= maxMoveDistance; z++)
            {
                GridPosition offsetGridPosition = new GridPosition(x, z);
                GridPosition testGridPosition = unitGridPosition + offsetGridPosition;
                if (!LevelGrid.Instance.IsValidGridPosition(testGridPosition))
                    continue;
                if (unitGridPosition == testGridPosition)
                    continue;//Same grid position where the unit is already at
                if (LevelGrid.Instance.HasAnyUnitOnGridPosition(testGridPosition))
                    continue;//Grid position already occupied with another unit
                validGridPositionList.Add(testGridPosition);
            }
        }

        return validGridPositionList;
    }

    public override string GetActionName()
    {
        return "Move";
    }

    public override EnemyAIAction GetEnemyAIAction(GridPosition gridPosition)
    {
        int targetCountAtGridPosition = unit.GetAction<ShootAction>().GetTargetCountAtPosition(gridPosition);

        return new EnemyAIAction
        {
            gridPosition = gridPosition,
            actionValue = targetCountAtGridPosition * 10
        };
    }
}
