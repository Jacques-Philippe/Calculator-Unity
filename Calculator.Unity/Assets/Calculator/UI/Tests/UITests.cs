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

    private readonly string SCENE_NAME = "Calculator/Scenes/Main";

    private ResultDisplayManager displayManager;

    private CalculatorManager calculatorManager;

    private GameObject FindGameObject(string name)
    {
        var gameobject = GameObject.Find(name);
        if (gameobject == null)
        {
            throw new System.Exception($"Unable to find gameobject with name {name}");
        }
        return gameobject;
    }

    private T GetComponentFromGameObjectWithName<T>(string name)
    {
        var obj = this.FindGameObject(name);
        var component = obj.GetComponent<T>();
        if (component == null)
        {
            throw new System.Exception($"Unable to find component of type {typeof(T)} on gameobject of name {name}");
        }
        return component;
    }


    //// A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
    //// `yield return null;` to skip a frame.
    //[UnityTest]
    //public IEnumerator UITestsWithEnumeratorPasses()
    //{
    //    this.calculatorManager.Reset();
    //    const string INITIAL_MESSAGE = "= 0";
    //    Assert.AreEqual(expected: INITIAL_MESSAGE, actual: this.displayManager.Message);

    //    // Use the Assert class to test conditions.
    //    // Use yield to skip a frame.
    //    yield return null;
    //}

    //[UnityTest]
    //public IEnumerator UIInitializesProperly()
    //{
    //    yield return null;
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
        var gameObject = GameObject.Find(gameObjectName);
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
        button.GetComponent<Button>().onClick.Invoke();
    }

    private bool referencesSetup = false;
    private bool settingUpReferences = false;

    [OneTimeSetUp]
    public void OneTimeSetup()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        //SceneManager.LoadScene(SCENE_NAME);
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log($"Loaded scene {scene.name} in mode {mode.ToString()}");
        if (referencesSetup) return;
        this.SetUpReferences();
        
    }

    void SetUpReferences()
    {
        if (!settingUpReferences)
        {
            settingUpReferences = true;

            var objects = Resources.FindObjectsOfTypeAll<Transform>().Where(o => o.gameObject.activeInHierarchy);
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

            referencesSetup = true;
        }
    }

    //[UnityTest]
    //public IEnumerator TestReferencesNotNullAfterLoad()
    //{
    //    //SceneManager.LoadScene(SCENE_NAME);
    //    //yield return null;
    //    this.SetUpReferences();
    //    yield return new WaitWhile(() => !referencesSetup);
    //    yield return null;
    //}

    //[UnityTest]
    //public IEnumerator TestReferencesNotNullAfterLoad()
    //{
    //    yield return new WaitWhile(() => !referencesSetup);
    //    Assert.IsNotNull(this.calculatorManager);
    //    Assert.IsNotNull(this.displayManager);
    //    //Add all other references as well for quick nullref testing
    //    yield return null;
    //}

    [UnityTest]
    [LoadScene("Assets/Calculator/Scenes/Main.unity")]
    public IEnumerator UIInitializesProperly()
    {
        yield return new WaitWhile(() => !referencesSetup);
        this.calculatorManager.Reset();
        yield return null;
        yield return null;
        const string INITIAL_MESSAGE = "= 0";
        Assert.AreEqual(expected: INITIAL_MESSAGE, actual: this.displayManager.Message);
        yield return null;
    }
}


public class LoadSceneAttribute : NUnitAttribute, IOuterUnityTestAction
{
    private string scene;

    public LoadSceneAttribute(string scene) => this.scene = scene;

    IEnumerator IOuterUnityTestAction.BeforeTest(ITest test)
    {
        Debug.Assert(scene.EndsWith(".unity"));
        yield return EditorSceneManager.LoadSceneAsyncInPlayMode(scene, new LoadSceneParameters(LoadSceneMode.Single));
    }

    IEnumerator IOuterUnityTestAction.AfterTest(ITest test)
    {
        yield return null;
    }
}