
using System.Collections;
using System.Collections.Generic;
using Calculator.UI.Scripts;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.UI;

public class UITests
{
    private readonly string RESULT_DISPLAY_NAME = "ResultDisplay";
    private readonly string MANAGER_NAME = "Manager";


    private ResultDisplayManager displayManager;

    private CalculatorManager calculatorManager;

    //private void SetUp()
    //{

    //    this.displayManager = this.FindGameObject("Main Camera").transform.GetComponentInChildren<ResultDisplayManager>();
    //    this.calculatorManager = GameObject.Find(MANAGER_NAME).GetComponent<CalculatorManager>();

    //    //Reset calculator state
    //    this.calculatorManager.Reset();

    //}

    //private GameObject FindGameObject(string name)
    //{
    //    var gameobject = GameObject.Find(name);
    //    if (gameobject == null)
    //    {
    //        throw new System.Exception($"Unable to find gameobject with name {name}");
    //    }
    //    return gameobject;
    //}

    //private T GetComponentFromGameObjectWithName<T>(string name)
    //{
    //    var obj = this.FindGameObject(name);
    //    var component = obj.GetComponent<T>();
    //    if (component == null)
    //    {
    //        throw new System.Exception($"Unable to find component of type {typeof(T)} on gameobject of name {name}");
    //    }
    //    return component;
    //}

    //// A Test behaves as an ordinary method
    //[Test]
    //public void UITestsSimplePasses()
    //{
    //    // Use the Assert class to test conditions
    //}

    ////// A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
    ////// `yield return null;` to skip a frame.
    ////[UnityTest]
    ////public IEnumerator UITestsWithEnumeratorPasses()
    ////{
    ////    this.calculatorManager.Reset();
    ////    const string INITIAL_MESSAGE = "= 0";
    ////    Assert.AreEqual(expected: INITIAL_MESSAGE, actual: this.displayManager.Message);

    ////    // Use the Assert class to test conditions.
    ////    // Use yield to skip a frame.
    ////    yield return null;
    ////}

    //[UnityTest]
    //public IEnumerator UIInitializesProperly()
    //{
    //    yield return null;
    //    this.SetUp();
    //    this.calculatorManager.Reset();
    //    const string INITIAL_MESSAGE = "= 0";
    //    Assert.AreEqual(expected: INITIAL_MESSAGE, actual: this.displayManager.Message);

    //    // Use the Assert class to test conditions.
    //    // Use yield to skip a frame.
    //    yield return null;
    //}

    //[UnityTest]
    //public IEnumerator LHSUpdatesForNumberPress()
    //{
    //    this.calculatorManager.Reset();
    //    const string INITIAL_MESSAGE = "= 0";
    //    Assert.AreEqual(expected: INITIAL_MESSAGE, actual: this.displayManager.Message);

    //    Click_Number(1);
    //    Assert.AreEqual(expected: "= 1", actual: this.displayManager.Message);

    //    Click_Number(2);
    //    Assert.AreEqual(expected: "= 12", actual: this.displayManager.Message);

    //    // Use the Assert class to test conditions.
    //    // Use yield to skip a frame.
    //    yield return null;
    //}

    ///// <summary>
    ///// Helper to retrieve a button gameobject from our scene
    ///// </summary>
    ///// <param name="number"></param>
    ///// <returns></returns>
    //private static GameObject GetNumberButton(int number)
    //{
    //    if (number < 0 || number > 9)
    //    {
    //        Debug.LogError($"Expecting a value between 0 and 9 but received {number}");
    //        return null;
    //    }
    //    var gameObjectName = $"NumberButton{number}";
    //    var gameObject = GameObject.Find(gameObjectName);
    //    if (gameObject == null)
    //    {
    //        Debug.LogError($"Couldn't find gameobject {gameObjectName}");
    //        return null;
    //    }
    //    return gameObject;
    //}

    ///// <summary>
    ///// Helper to click a button gameobject in our scene
    ///// </summary>
    ///// <param name="number"></param>
    //private static void Click_Number(int number)
    //{
    //    var button = GetNumberButton(number);
    //    button.GetComponent<Button>().onClick.Invoke();
    //}
}
