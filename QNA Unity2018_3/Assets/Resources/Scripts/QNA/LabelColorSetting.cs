using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace QNA
{
    [System.Serializable]
    sealed class LabelColorSetting
    {
        public LabelColorSetting(uint setting_size)
        {
            Colors = new Color[setting_size];
            for (uint i = 0; i < setting_size; ++i)
                Colors[i] = Color.white;
        }

        public Color[] Colors;
    }
}