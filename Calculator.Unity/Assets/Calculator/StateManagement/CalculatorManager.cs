using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Calculator.StateManagement;

public class CalculatorManager : MonoBehaviour
{

    private CalculatorStateManager calculatorStateManager;

    private void Start()
    {
        this.calculatorStateManager = new CalculatorStateManager();
    }

    public void Click_Number(float number)
    {
        this.calculatorStateManager.SelectNumber(number);
    }

    public void Click_Operator(string operatorString)
    {
        this.calculatorStateManager.SelectOperator(operatorString);
    }

    public void Click_Equals()
    {
        this.calculatorStateManager.SelectEquals();
    }

}
