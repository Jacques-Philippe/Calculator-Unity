using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.TestTools.TestRunner.Api;
using UnityEngine;

namespace Calculator.AppManagement.Editor
{
    public class TestScripts : MonoBehaviour
    {
        [MenuItem("CICD/Run Tests")]
        public static void RunTests()
        {
            Debug.Log("Hello?");
            var testRunnerApi = ScriptableObject.CreateInstance<TestRunnerApi>();
            var filter = new Filter() { testMode = TestMode.PlayMode };
            testRunnerApi.Execute(new ExecutionSettings(filter));
        }
    }
}
