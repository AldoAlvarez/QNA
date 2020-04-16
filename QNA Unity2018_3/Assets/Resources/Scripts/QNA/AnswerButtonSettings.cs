using QNA.ButtonLabel;
using QNA.ButtonLabel.Options;
using UnityEngine;

namespace QNA
{
    [System.Serializable]
    public sealed class AnswerButtonSettings 
    {
        public AnswerButtonSettings() 
        {
            CreateSettings();
        }

        #region VARIABLES
        [SerializeField]
        private LabelColorSetting []LabelColorSettings;
        #endregion

        #region PUBLIC / INTERNAL METHODS
        internal Color GetSettingColor(Settings setting, int option) 
        {
            int i_setting = (int)setting;
            if (i_setting < 0 || i_setting >= (int)Settings.counter) return new Color(0, 0, 0, 0);
            if (option >= GetSettingSize(setting)) return new Color(0, 0, 0, 0);

            CreateSettings();

            return LabelColorSettings[i_setting].Colors[option];
        }

        internal void CreateSettings()
        {
            uint totalSettings = (uint)Settings.counter;
#if UNITY_EDITOR
            if (editorFoldoutPerSetting == null || editorFoldoutPerSetting.Length < totalSettings)
            {
                editorFoldoutPerSetting = new bool[totalSettings];
                for (int i = 0; i < totalSettings; ++i)
                    editorFoldoutPerSetting[i] = false;
            }
#endif
            if (LabelColorSettings != null && LabelColorSettings.Length == totalSettings) return;

            LabelColorSettings = new LabelColorSetting[totalSettings];
            for (int setting = 0; setting < totalSettings; ++setting)
            {
                LabelColorSettings[setting] = new LabelColorSetting(GetSettingSize((Settings)setting));
                uint totalColorInSetting = GetSettingSize((Settings)setting);

                for (uint color = 0; color < totalColorInSetting; ++color)
                    LabelColorSettings[setting].Colors[color] = Color.white;
            }

            InitializeLabelBackgroundColors();
            InitializeLabeTextColors();
        }
        #endregion

        #region PRIVATE METHODS
        private void InitializeLabelBackgroundColors() 
        {
            int backgroundSetting = (int)Settings.Background;
            LabelColorSettings[backgroundSetting].Colors[(int)Background.Neutral] = Color.gray;
            LabelColorSettings[backgroundSetting].Colors[(int)Background.Wrong_Answer] = Color.red;
            LabelColorSettings[backgroundSetting].Colors[(int)Background.Right_Answer] = Color.green;
        }

        private void InitializeLabeTextColors() 
        {
            int textSetting = (int)Settings.Text;
            LabelColorSettings[textSetting].Colors[(int)Text.Highlightened] = Color.gray * 0.80f;
            LabelColorSettings[textSetting].Colors[(int)Text.Normal] = Color.white;
        }

        private uint GetSettingSize(Settings setting) 
        {
            switch (setting)
            {
                case Settings.Background:return (int)Background.counter;
                case Settings.Text:return (int)Text.counter;
                default:return 0;
            }    
        }
        #endregion

#if UNITY_EDITOR
        [SerializeField]
        private bool[] editorFoldoutPerSetting;
        [SerializeField]
        private int lastSelectedIndex = -1;
        public static string GetColorSettingName(Settings setting, int option) 
        {
            switch (setting)
            {
                case Settings.Background: return ((Background)option).ToString();
                case Settings.Text: return ((Text)option).ToString();
                default: return "Unidentified Setting";
            }
        }
#endif
    }
}