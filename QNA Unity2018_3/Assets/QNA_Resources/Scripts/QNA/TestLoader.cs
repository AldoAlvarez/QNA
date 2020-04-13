using UnityEngine;

namespace QNA
{
    public class TestLoader
    {
        public static void LoadTest(out TestsContainer test_man)
        {
            try
            {
                TextAsset file = Resources.Load<TextAsset>(TestCreator.AssetsPath + TestCreator.TestsFileName);
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