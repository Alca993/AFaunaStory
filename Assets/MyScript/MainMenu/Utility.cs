using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Globalization;

namespace Utility
{
    public class Utility : MonoBehaviour
    {
        public static Utility instance;

        private float MAX_RANGE;
        private float MAX_RATE;

        public static Utility Instance()
        {

            if (instance == null)
            {
                instance = new Utility();
            }
            return instance;

        }

        public float Max_Range
        {
            get
            {
                return MAX_RANGE;
            }
        }

        public float Max_Rate
        {
            get
            {
                return MAX_RATE;
            }
        }
        private Utility()
        {

            string filePath = "File/utilityFeatures";


            TextAsset data = Resources.Load<TextAsset>(filePath);
            string[] lines = data.text.Split('\n');

            for (int i = 0; i < lines.Length; i++)
            {
                string line = lines[i];
                string[] token = line.Split('=');

                switch (token[0])
                {
                    case "MAX_RANGE":
                        MAX_RANGE = float.Parse(token[1], CultureInfo.InvariantCulture);
                        break;
                    case "MAX_RATE":
                        MAX_RATE = float.Parse(token[1], CultureInfo.InvariantCulture);
                        Debug.Log("MAX_RATE: " + MAX_RATE);
                        break;
                    default:
                        break;
                }
            }
        }
    }
}
