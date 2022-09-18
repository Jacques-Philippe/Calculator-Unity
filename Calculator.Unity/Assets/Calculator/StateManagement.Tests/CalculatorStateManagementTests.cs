using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using Calculator.StateManagement;

public class CalculatorStateManagementTests
{
    /// <summary>
    /// Ensure the SelectOperatorTransition initializes properly given the provided string
    /// </summary>
    [Test]
    public void OperatorSelectionInitializesProperly()
    {
        var _operator = new SelectOperatorTransition("+");
        Assert.AreEqual(expected: OPERATOR.ADDITION, actual: _operator.Operator);

        _operator = new SelectOperatorTransition("-");
        Assert.AreEqual(expected: OPERATOR.SUBTRACTION, actual: _operator.Operator);

        _operator = new SelectOperatorTransition("*");
        Assert.AreEqual(expected: OPERATOR.MULTIPLICATION, actual: _operator.Operator);

        _operator = new SelectOperatorTransition("/");
        Assert.AreEqual(expected: OPERATOR.DIVISION, actual: _operator.Operator);
    }

    /// <summary>
    /// Ensure the SelectOperatorTransition initializes properly given the provided string
    /// </summary>
    [Test]
    public void LHSBuildsProperly()
    {
        var calculator = new CalculatorStateManager();
        calculator.SelectEquals();

        Assert.AreEqual(expected: 0, actual: calculator.Result);

        calculator.SelectNumber(1);
        calculator.SelectEquals();

        Assert.AreEqual(expected: 1, actual: calculator.Result);

        calculator.SelectNumber(1);
        calculator.SelectNumber(1);
        calculator.SelectEquals();

        Assert.AreEqual(expected: 11, actual: calculator.Result);
    }

    /// <summary>
    /// Ensure addition works properly, and that given num1 and num2 and operator addition, the result is equal to the sum of num1 and num2
    /// </summary>
    [Test]
    public void AdditionWorksProperly()
    {
        var calculator = new CalculatorStateManager();
        calculator.SelectEquals();

        Assert.AreEqual(expected: calculator.Result, actual: 0);

        var num1 = RandomIntInRange0to9();
        var num2 = (RandomIntInRange0to9() + num1) % 10;

        calculator.SelectNumber(num1);
        calculator.SelectOperator("+");
        calculator.SelectNumber(num2);

        calculator.SelectEquals();

        Assert.AreEqual(expected: num1 + num2, actual: calculator.Result);
    }

    /// <summary>
    /// Ensure multiplication works properly, and that given num1 and num2 and operator multiplication, the result is equal to the product of num1 and num2
    /// </summary>
    [Test]
    public void MultiplicationWorksProperly()
    {
        var calculator = new CalculatorStateManager();
        calculator.SelectEquals();

        Assert.That(calculator.Result == 0);

        var num1 = RandomIntInRange0to9();
        var num2 = (RandomIntInRange0to9() + num1) % 10;

        calculator.SelectNumber(num1);
        calculator.SelectOperator("*");
        calculator.SelectNumber(num2);

        calculator.SelectEquals();

        Assert.AreEqual(expected: num1 * num2, actual: calculator.Result);
    }

    /// <summary>
    /// Ensure subtraction works properly, and that given num1 and num2 and operator subtraction, the result is equal to the difference of num1 and num2
    /// </summary>
    [Test]
    public void SubtractionWorksProperly()
    {
        var calculator = new CalculatorStateManager();
        calculator.SelectEquals();

        Assert.That(calculator.Result == 0);

        var num1 = RandomIntInRange0to9();
        var num2 = (RandomIntInRange0to9() + num1) % 10;

        calculator.SelectNumber(num1);
        calculator.SelectOperator("-");
        calculator.SelectNumber(num2);

        calculator.SelectEquals();

        Assert.AreEqual(expected: num1 - num2, actual: calculator.Result);
    }

    /// <summary>
    /// Ensure division works properly, and that given num1 and num2 and operator vision, the result is equal to the quotient of num1 and num2
    /// </summary>
    [Test]
    public void DivisionWorksProperly()
    {
        var calculator = new CalculatorStateManager();
        calculator.SelectEquals();

        Assert.That(calculator.Result == 0);

        var num1 = RandomIntInRange0to9();
        var num2 = (RandomIntInRange0to9() + num1) % 10;

        calculator.SelectNumber(num1);
        calculator.SelectOperator("/");
        calculator.SelectNumber(num2);

        calculator.SelectEquals();

        Assert.AreEqual(expected: (float)num1 / num2, actual: calculator.Result, delta: 0.01);
    }

    private int RandomIntInRange0to9()
    {
        var r = new System.Random();
        int rInt = r.Next(0, 9);
        return rInt;
    }

    //// A Test behaves as an ordinary method
    //[Test]
    //public void CalculatorStateManagementTestsSimplePasses()
    //{
    //    // Use the Assert class to test conditions
    //}

    //// A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
    //// `yield return null;` to skip a frame.
    //[UnityTest]
    //public IEnumerator CalculatorStateManagementTestsWithEnumeratorPasses()
    //{
    //    // Use the Assert class to test conditions.
    //    // Use yield to skip a frame.
    //    yield return null;
    //}
}
