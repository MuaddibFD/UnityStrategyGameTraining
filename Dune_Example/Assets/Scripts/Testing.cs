using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Testing : MonoBehaviour
{
    [SerializeField]
    private Units unit;
    private void Start()
    {
    }

    //private void Update()
    //{
    //    if (Input.GetKey(KeyCode.T))
    //    {
    //        GridSystemVisual.Instance.HideAllGridPosition();
    //        GridSystemVisual.Instance.ShowGridPositionList(
    //            unit.GetMoveAction().GetValidActionGridPositionList());
    //    }
    //}

    private void Update()
    {
        if (Input.GetKey(KeyCode.T))
        {
            new MyGenericClass<string>("test");

            new MyGenericClass<int>(2);

            MyGenericClass<int> testing = new MyGenericClass<int>(6);
            testing.Testing<string>("testing for method");
        }
    }
}

public class MyGenericClass<T>
{
    private T input;

    public MyGenericClass(T input)
    {
        this.input = input;
        Debug.Log(input);
    }

    public void Testing<T>(T t)
    {
        Debug.Log(t);
    }
}
