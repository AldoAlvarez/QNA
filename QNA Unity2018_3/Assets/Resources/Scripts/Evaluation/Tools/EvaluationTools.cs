using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AGAC.Evaluation.Tools;
using AGAC.Evaluation.Base;

namespace AGAC.Evaluation {
    public static class EvaluationTools
    {
        public static TestsContainer LoadTests()
        {
            TestsContainer Container = new TestsContainer();
            TestLoader.LoadTest(out Container);
            return Container;
        }

        public static void CreateSampleTest() 
        {
            TestCreator.Instance.CreateSampleTest();
        }
    }
}