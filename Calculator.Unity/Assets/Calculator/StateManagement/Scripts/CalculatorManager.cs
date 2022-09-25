using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Calculator.StateManagement;
using Calculator.UI.Scripts;

public class CalculatorManager : MonoBehaviour
{
    [SerializeField]
    private ResultDisplayManager calculatorUIManager;

    [SerializeField]
    private CalculatorSoundManager calculatorSoundManager;

    private CalculatorStateManager calculatorStateManager = new CalculatorStateManager();

    private void Start()
    {
        //Attach delegate to be invoked on state update, in order to update the UI on state change
        this.calculatorStateManager.onCalculatorMessageChanged += this.UpdateUI;

        //Initialize the UI manager from the state manager
        this.calculatorUIManager.Message = this.calculatorStateManager.CalculatorMessage;
    }

    private void OnDestroy()
    {
        this.calculatorStateManager.onCalculatorMessageChanged -= this.UpdateUI;
    }

    /// <summary>
    /// A function to be invoked on calculator state change, to update the UI from the updated state
    /// </summary>
    /// <param name="newMessage"></param>
    private void UpdateUI(string newMessage)
    {
        this.calculatorUIManager.Message = newMessage;
    }

    /// <summary>
    /// A function to be invoked on user number selection
    /// </summary>
    /// <param name="number"></param>
    public void Click_Number(float number)
    {
        this.calculatorSoundManager.PlaySound("Button Press");
        this.calculatorStateManager.SelectNumber(number);
    }

    /// <summary>
    /// A function to be invoked on user operator selection
    /// </summary>
    /// <param name="operatorString"></param>
    public void Click_Operator(string operatorString)
    {
        this.calculatorSoundManager.PlaySound("Button Press");
        this.calculatorStateManager.SelectOperator(operatorString);
    }

    /// <summary>
    /// A function to be invoked on user equals select
    /// </summary>
    public void Click_Equals()
    {
        this.calculatorSoundManager.PlaySound("Button Press");
        this.calculatorStateManager.SelectEquals();
    }

    public void Reset()
    {
        this.calculatorStateManager.Reset();
    }
}
