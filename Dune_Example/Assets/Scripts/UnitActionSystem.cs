using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UnitActionSystem : MonoBehaviour
{
    public static UnitActionSystem Instance { get; private set; }

    [SerializeField]
    private Units selectedUnit;

    [SerializeField]
    private LayerMask unitsLayerMask;

    internal event EventHandler OnSelectedUnitChanged;

    internal event EventHandler OnSelectedActionChanged;

    internal event EventHandler<bool> OnBusyChanged;

    internal event EventHandler OnActionStarted;

    private BaseAction selectedAction;

    private bool isBusy;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError($"There is more than one UnitActionSystem: {transform} - {Instance}");
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Start()
    {
        SetSelectedUnit(selectedUnit);
    }

    private void Update()
    {
        if (isBusy)
            return;

        if (!TurnSystem.Instance.IsPlayerTurn())
            return;

        if (EventSystem.current.IsPointerOverGameObject())
            return;

        if (TryHandleUnitSellection())
        {

            return;
        }

        HandleSelectedAction();
        //if (Input.GetKeyDown(KeyCode.Mouse0))
        #region Before action methods were not in Handle Selected Action
        //if (Input.GetMouseButtonDown(0))
        //{
        //    if (TryHandleUnitSellection()) return;
        //    GridPosition mouseGridPosition = LevelGrid.Instance.GetGridPosition(MouseWorld.GetPosition());
        //    if (selectedUnit.GetMoveAction().IsValidActionGridPosition(mouseGridPosition))
        //    {
        //        SetBusy(); 
        //        selectedUnit.GetMoveAction().Move(mouseGridPosition, ClearBusy);
        //    }
        //}

        //if (Input.GetMouseButtonDown(1))
        //{
        //    SetBusy();
        //    selectedUnit.GetSpinAction().Spin(ClearBusy);
        //    /* 
        //     * selectedUnit.GetSpinAction().Spin(Test);
        //     * Because of Test returns int but,
        //     * the delegate that we create in Spin Action
        //     * compatible with method returns void
        //    */
        //}
        #endregion
    }

    private void HandleSelectedAction()
    {
        if (Input.GetMouseButtonDown(0))
        {
            GridPosition mouseGridPosition = LevelGrid.Instance.GetGridPosition(MouseWorld.GetPosition());

            if (!selectedAction.IsValidActionGridPosition(mouseGridPosition))
                return;
            if (!selectedUnit.TrySpendActionPointsToTakeAction(selectedAction))
                return;

            SetBusy();
            selectedAction.TakeAction(mouseGridPosition, ClearBusy);

            #region Same code as above
            //if (selectedAction.IsValidActionGridPosition(mouseGridPosition))
            //{
            //    if (selectedUnit.TrySpendActionPointsYoTakeAction(selectedAction))
            //    {
            //        SetBusy();
            //        selectedAction.TakeAction(mouseGridPosition, ClearBusy);
            //    }
            //}
            #endregion

            #region Befor Move and Spin actions were not converted to Take Action in Base Action
            //switch (selectedAction)
            //{
            //    case MoveAction moveAction:
            //        if (moveAction.IsValidActionGridPosition(mouseGridPosition))
            //        {
            //            SetBusy();
            //            moveAction.Move(mouseGridPosition, ClearBusy);
            //        }
            //        break;
            //    case SpinAction spinAction:
            //        SetBusy();
            //        spinAction.Spin(ClearBusy);
            //        break;
            //}
            #endregion

            OnActionStarted?.Invoke(this, EventArgs.Empty);
        }
    }

    private bool TryHandleUnitSellection()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit raycastHit, float.MaxValue, unitsLayerMask))
            {
                if (raycastHit.transform.TryGetComponent<Units>(out Units unit))
                {
                    if (unit == selectedUnit)
                        return false;

                    if (unit.IsEnemy())
                        return false;
                    SetSelectedUnit(unit);
                    return true;
                }
            }
        }

        return false;
    }

    private void SetSelectedUnit(Units unit)
    {
        selectedUnit = unit;

        SetSelectedAction(unit.GetAction<MoveAction>());

        //if (OnSelectedUnitChanged != null)
        //    OnSelectedUnitChanged(this, EventArgs.Empty);
        OnSelectedUnitChanged?.Invoke(this, EventArgs.Empty);

    }

    public Units GetSelectedUnit()
    {
        return selectedUnit;
    }

    private void SetBusy()
    {
        isBusy = true;

        OnBusyChanged?.Invoke(this, isBusy);
    }

    private void ClearBusy()
    {
        isBusy = false;

        OnBusyChanged?.Invoke(this, isBusy);
    }

    //private int Test(bool b)
    //{
    //    return 0;
    //}

    public void SetSelectedAction(BaseAction baseAction)
    {
        selectedAction = baseAction;

        OnSelectedActionChanged?.Invoke(this, EventArgs.Empty);
    }

    public BaseAction GetSelectedAction()
    {
        return selectedAction;
    }
}
