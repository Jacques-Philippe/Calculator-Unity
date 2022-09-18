using System.Collections;
using System.Collections.Generic;
using Calculator.UI.Scripts;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;
using UnityEngine.UI;
using System.Linq;

public class UITests
{
    private readonly string RESULT_DISPLAY_NAME = "ResultDisplay";
    private readonly string MANAGER_NAME = "Calculator";

    private readonly string ADD_BUTTON_NAME = "AddButton";
    private readonly string SUBTRACT_BUTTON_NAME = "SubtractButton"; 
    private readonly string MULTIPLY_BUTTON_NAME = "MultiplyButton"; 
    private readonly string DIVIDE_BUTTON_NAME = "DivideButton";


    private readonly string SCENE_NAME = "Calculator/Scenes/Main";

    private ResultDisplayManager displayManager;

    private CalculatorManager calculatorManager;


    private bool referencesSetup = false;
    private bool settingUpReferences = false;



    [OneTimeSetUp]
    public void OneTimeSetup()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        referencesSetup = false;
        Debug.Log($"Loaded scene {scene.name} in mode {mode.ToString()}");
        if (referencesSetup)
            return;
        this.SetUpReferences();
        referencesSetup = true;
    }

    void SetUpReferences()
    {
        if (!settingUpReferences)
        {
            settingUpReferences = true;

            var objects = Resources
                .FindObjectsOfTypeAll<Transform>()
                .Where(o => o.gameObject.activeInHierarchy);
            foreach (Transform t in objects)
            {
                if (t.name == MANAGER_NAME)
                {
                    Debug.Log("Initializing manager");
                    this.calculatorManager = t.GetComponent<CalculatorManager>();
                }
                else if (t.name == RESULT_DISPLAY_NAME)
                {
                    Debug.Log("Initializing display manager");
                    this.displayManager = t.GetComponent<ResultDisplayManager>();
                }
            }
            settingUpReferences = false;
        }
    }

    [UnityTest]
    [LoadScene("Assets/Calculator/Scenes/Main.unity")]
    public IEnumerator UIInitializesProperly()
    {
        yield return new WaitWhile(() => !referencesSetup);

        const string INITIAL_MESSAGE = "= 0";
        Assert.AreEqual(expected: INITIAL_MESSAGE, actual: this.displayManager.Message);
    }

    [UnityTest]
    [LoadScene("Assets/Calculator/Scenes/Main.unity")]
    public IEnumerator LHSUpdatesForNumberPress()
    {
        yield return new WaitWhile(() => !referencesSetup);
        const string INITIAL_MESSAGE = "= 0";
        Assert.AreEqual(expected: INITIAL_MESSAGE, actual: this.displayManager.Message);

        Click_Number(1);
        //yield return null;
        Assert.AreEqual(expected: "= 1", actual: this.displayManager.Message);

        Click_Number(2);
        Assert.AreEqual(expected: "= 12", actual: this.displayManager.Message);
    }

    [UnityTest]
    [LoadScene("Assets/Calculator/Scenes/Main.unity")]
    public IEnumerator Given_LHS_Operator_Functions()
    {
        yield return new WaitWhile(() => !referencesSetup);
        
        Click_Number(1);
        Click_Number(2);
        Assert.AreEqual(expected: "= 12", actual: this.displayManager.Message);

        Click_Operator(ADD_BUTTON_NAME);
        Assert.AreEqual(expected: "= 12 +", actual: this.displayManager.Message);

        Click_Operator(SUBTRACT_BUTTON_NAME);
        Assert.AreEqual(expected: "= 12 -", actual: this.displayManager.Message);

        Click_Operator(MULTIPLY_BUTTON_NAME);
        Assert.AreEqual(expected: "= 12 *", actual: this.displayManager.Message);

        Click_Operator(DIVIDE_BUTTON_NAME);
        Assert.AreEqual(expected: "= 12 /", actual: this.displayManager.Message);
    }

    [UnityTest]
    [LoadScene("Assets/Calculator/Scenes/Main.unity")]
    public IEnumerator Without_LHS_Operator_Functions()
    {
        yield return new WaitWhile(() => !referencesSetup);

        Assert.AreEqual(expected: "= 0", actual: this.displayManager.Message);

        Click_Operator(ADD_BUTTON_NAME);
        Assert.AreEqual(expected: "= 0 +", actual: this.displayManager.Message);

        Click_Operator(SUBTRACT_BUTTON_NAME);
        Assert.AreEqual(expected: "= 0 -", actual: this.displayManager.Message);

        Click_Operator(MULTIPLY_BUTTON_NAME);
        Assert.AreEqual(expected: "= 0 *", actual: this.displayManager.Message);

        Click_Operator(DIVIDE_BUTTON_NAME);
        Assert.AreEqual(expected: "= 0 /", actual: this.displayManager.Message);
    }

    [UnityTest]
    [LoadScene("Assets/Calculator/Scenes/Main.unity")]
    public IEnumerator Given_LHS_And_Operator_RHS_Functions()
    {
        yield return new WaitWhile(() => !referencesSetup);

        Assert.AreEqual(expected: "= 0", actual: this.displayManager.Message);

        Click_Operator(ADD_BUTTON_NAME);
        Assert.AreEqual(expected: "= 0 +", actual: this.displayManager.Message);

        Click_Number(1);
        Click_Number(2);
        Assert.AreEqual(expected: "= 0 + 12", actual: this.displayManager.Message);
        
        this.calculatorManager.Reset();
        Assert.AreEqual(expected: "= 0", actual: this.displayManager.Message);

        Click_Operator(SUBTRACT_BUTTON_NAME);
        Assert.AreEqual(expected: "= 0 -", actual: this.displayManager.Message);

        Click_Number(1);
        Click_Number(2);
        Assert.AreEqual(expected: "= 0 - 12", actual: this.displayManager.Message);

        this.calculatorManager.Reset();
        Assert.AreEqual(expected: "= 0", actual: this.displayManager.Message);

        Click_Operator(MULTIPLY_BUTTON_NAME);
        Assert.AreEqual(expected: "= 0 *", actual: this.displayManager.Message);

        Click_Number(1);
        Click_Number(2);
        Assert.AreEqual(expected: "= 0 * 12", actual: this.displayManager.Message);
        
        this.calculatorManager.Reset();
        Assert.AreEqual(expected: "= 0", actual: this.displayManager.Message);

        Click_Operator(DIVIDE_BUTTON_NAME);
        Assert.AreEqual(expected: "= 0 /", actual: this.displayManager.Message);

        Click_Number(1);
        Click_Number(2);
        Assert.AreEqual(expected: "= 0 / 12", actual: this.displayManager.Message);
    }

    [UnityTest]
    [LoadScene("Assets/Calculator/Scenes/Main.unity")]
    public IEnumerator Given_LHS_RHS_And_Operator_Equals_Functions()
    {
        yield return new WaitWhile(() => !referencesSetup);

        Assert.AreEqual(expected: "= 0", actual: this.displayManager.Message);

        Click_Operator(ADD_BUTTON_NAME);
        Assert.AreEqual(expected: "= 0 +", actual: this.displayManager.Message);

        Click_Number(1);
        Click_Number(2);
        Assert.AreEqual(expected: "= 0 + 12", actual: this.displayManager.Message);

        Click_Equals();
        Assert.AreEqual(expected: "= 0 + 12 = 12", actual: this.displayManager.Message);

        this.calculatorManager.Reset();
        Assert.AreEqual(expected: "= 0", actual: this.displayManager.Message);

        Click_Operator(SUBTRACT_BUTTON_NAME);
        Assert.AreEqual(expected: "= 0 -", actual: this.displayManager.Message);

        Click_Number(1);
        Click_Number(2);
        Assert.AreEqual(expected: "= 0 - 12", actual: this.displayManager.Message);

        Click_Equals();
        Assert.AreEqual(expected: "= 0 - 12 = -12", actual: this.displayManager.Message);

        this.calculatorManager.Reset();
        Assert.AreEqual(expected: "= 0", actual: this.displayManager.Message);

        Click_Operator(MULTIPLY_BUTTON_NAME);
        Assert.AreEqual(expected: "= 0 *", actual: this.displayManager.Message);

        Click_Number(1);
        Click_Number(2);
        Assert.AreEqual(expected: "= 0 * 12", actual: this.displayManager.Message);

        Click_Equals();
        Assert.AreEqual(expected: "= 0 * 12 = 0", actual: this.displayManager.Message);

        this.calculatorManager.Reset();
        Assert.AreEqual(expected: "= 0", actual: this.displayManager.Message);

        Click_Operator(DIVIDE_BUTTON_NAME);
        Assert.AreEqual(expected: "= 0 /", actual: this.displayManager.Message);

        Click_Number(1);
        Click_Number(2);
        Assert.AreEqual(expected: "= 0 / 12", actual: this.displayManager.Message);

        Click_Equals();
        Assert.AreEqual(expected: "= 0 / 12 = 0", actual: this.displayManager.Message);
    }

    [UnityTest]
    [LoadScene("Assets/Calculator/Scenes/Main.unity")]
    public IEnumerator Given_LHS_And_Operator_And_RHS_Clicking_Operator_Functions()
    {
        yield return new WaitWhile(() => !referencesSetup);

        Assert.AreEqual(expected: "= 0", actual: this.displayManager.Message);

        Click_Operator(ADD_BUTTON_NAME);
        Assert.AreEqual(expected: "= 0 +", actual: this.displayManager.Message);

        Click_Number(1);
        Click_Number(2);
        Assert.AreEqual(expected: "= 0 + 12", actual: this.displayManager.Message);

        Click_Operator(ADD_BUTTON_NAME);
        Assert.AreEqual(expected: "= 12 +", actual: this.displayManager.Message);

        this.calculatorManager.Reset();
        Assert.AreEqual(expected: "= 0", actual: this.displayManager.Message);

        Click_Operator(SUBTRACT_BUTTON_NAME);
        Assert.AreEqual(expected: "= 0 -", actual: this.displayManager.Message);

        Click_Number(1);
        Click_Number(2);
        Assert.AreEqual(expected: "= 0 - 12", actual: this.displayManager.Message);
        
        Click_Operator(SUBTRACT_BUTTON_NAME);
        Assert.AreEqual(expected: "= -12 -", actual: this.displayManager.Message);


        this.calculatorManager.Reset();
        Assert.AreEqual(expected: "= 0", actual: this.displayManager.Message);

        Click_Operator(MULTIPLY_BUTTON_NAME);
        Assert.AreEqual(expected: "= 0 *", actual: this.displayManager.Message);

        Click_Number(1);
        Click_Number(2);
        Assert.AreEqual(expected: "= 0 * 12", actual: this.displayManager.Message);

        Click_Operator(MULTIPLY_BUTTON_NAME);
        Assert.AreEqual(expected: "= 0 *", actual: this.displayManager.Message);


        this.calculatorManager.Reset();
        Assert.AreEqual(expected: "= 0", actual: this.displayManager.Message);

        Click_Operator(DIVIDE_BUTTON_NAME);
        Assert.AreEqual(expected: "= 0 /", actual: this.displayManager.Message);

        Click_Number(1);
        Click_Number(2);
        Assert.AreEqual(expected: "= 0 / 12", actual: this.displayManager.Message);

        Click_Operator(DIVIDE_BUTTON_NAME);
        Assert.AreEqual(expected: "= 0 /", actual: this.displayManager.Message);
    }

    /// <summary>
    /// Helper to retrieve a button gameobject from our scene
    /// </summary>
    /// <param name="number"></param>
    /// <returns></returns>
    private static GameObject GetNumberButton(int number)
    {
        if (number < 0 || number > 9)
        {
            Debug.LogError($"Expecting a value between 0 and 9 but received {number}");
            return null;
        }
        var gameObjectName = $"NumberButton{number}";
        var gameObject = FindGameObjectInActiveScene(gameObjectName);
        if (gameObject == null)
        {
            Debug.LogError($"Couldn't find gameobject {gameObjectName}");
            return null;
        }
        return gameObject;
    }

    /// <summary>
    /// Helper to click a button gameobject in our scene
    /// </summary>
    /// <param name="number"></param>
    private static void Click_Number(int number)
    {
        var button = GetNumberButton(number);
        Debug.Log($"Trying to click {button.name}");
        button.GetComponent<Button>().onClick.Invoke();
    }

    /// <summary>
    /// Helper to click a button gameobject in our scene
    /// </summary>
    /// <param name="number"></param>
    private static void Click_Operator(string op)
    {
        var button = FindGameObjectInActiveScene(op);
        Debug.Log($"Trying to click {button.name}");
        button.GetComponent<Button>().onClick.Invoke();
    }

    /// <summary>
    /// Helper to click a button gameobject in our scene
    /// </summary>
    private static void Click_Equals()
    {
        var button = FindGameObjectInActiveScene("EqualsButton");
        Debug.Log($"Trying to click {button.name}");
        button.GetComponent<Button>().onClick.Invoke();
    }


    private static GameObject FindGameObjectInActiveScene(string name)
    {
        var objects = Resources
            .FindObjectsOfTypeAll<Transform>()
            .Where(o => o.gameObject.activeInHierarchy);
        foreach (Transform t in objects)
        {
            if (t.name == name)
            {
                return t.gameObject;
            }
        }
        throw new System.Exception($"Gameobject {name} was not found in the active scene");
    }
}

public class LoadSceneAttribute : NUnitAttribute, IOuterUnityTestAction
{
    private string scene;

    public LoadSceneAttribute(string scene) => this.scene = scene;

    IEnumerator IOuterUnityTestAction.BeforeTest(ITest test)
    {
        Debug.Assert(scene.EndsWith(".unity"));
        yield return EditorSceneManager.LoadSceneAsyncInPlayMode(
            scene,
            new LoadSceneParameters(LoadSceneMode.Single)
        );
    }

    IEnumerator IOuterUnityTestAction.AfterTest(ITest test)
    {
        yield return null;
    }
}
