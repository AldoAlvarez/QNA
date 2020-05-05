using UnityEngine;
using AGAC.Evaluation.Base;
using System.IO;

namespace AGAC.Evaluation.Tools
{
    public sealed class TestLoader
    {
        public static void LoadTest(out TestsContainer test_man)
        {
            try
            {
                string resources_path = TestCreator.Instance.AssetsPath + TestCreator.Instance.TestsFileName;
                TextAsset file = Resources.Load<TextAsset>(resources_path);
                string json_contents = System.Text.Encoding.UTF7.GetString(file.bytes);
                test_man = JsonUtility.FromJson<TestsContainer>(json_contents);
            }
            catch (System.Exception)
            {
                test_man = null;
                Debug.LogError("Critical error loading the tests file.");
            }
        }
    }
}