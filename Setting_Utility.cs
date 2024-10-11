using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using System.Collections.Generic;
using CorebeastInteractive.RoH.Corebeast;
using System.Linq;

#pragma warning disable CS4014
#pragma warning disable UNT0013
#pragma warning disable IDE0090
namespace CorebeastInteractive.RoH.Menu
{
    public class Setting_Utility : MonoBehaviour
    { 
        [Space(10)]
        [Header("Local Settings")]
        [Space(5)]
        public Value choosenValue = new Value();
        [SerializeField] public TMPro.TMP_Text nameHeader;
        [SerializeField] public TMPro.TMP_Text statusHeader;
        [SerializeField] public Image Left_Cross;
        [SerializeField] public Image Right_Cross;
        [SerializeField] public float decreaseValuePerClick = 0.05f;
        [SerializeField] public float addValuePerClick = 0.05f;
        [SerializeField] public AudioSource Error_AudioSource;
        [SerializeField] public AudioSource Sucess_AudioSource;
        public static float successVolume = 0.5f;
        public static float Error_Volume = 1f;
        [SerializeField] public AudioClip errorAudioClip;
        [SerializeField] public AudioClip successAudioClip;

        [Header("Game Data")]
        [SerializeField] public TMPro.TMP_Text SaveGame01_Status;
        [SerializeField] public TMPro.TMP_Text SaveGame02_Status;
        [SerializeField] public TMPro.TMP_Text SaveGame03_Status;
        [SerializeField] public Image SaveGame_Delete_Button;
        [SerializeField] private string[] savegameStatus;
        [SerializeField] private bool Savegame01_Availible;
        [SerializeField] private bool Savegame02_Availible;
        [SerializeField] private bool Savegame03_Availbile;

        [Space(10)]
        [Header("SaveFile Values")]
        [Space(5)]
        //Comfort
        public static float MotionBlurLevel;
        public static float DepthLevel;
        public static float ShakeLevel;
        public static float FOVLevel;
        //Graphics
        public static float GraphicsLevel;
        public static float ScreenLevel;
        public static float FramerateLevel;
        public static float ShadowLevel;
        public static float AnisotropicLevel;
        public static float VsyncLevel;
        //Sound
        public static float MasterAudioLevel;
        public static float MusicAudioLevel;
        public static float SoundAudioLevel;
        public static float VoiceAudioLevel;
        public static float EnemyAudioLevel;
        public static float VFXAudioLevel;

        [Space(10)]
        [Header("Savegame Stuff")]
        [Space(5)]
        [SerializeField] public AudioMixer mainMixer;

        private static List<Setting_Utility> allInstances = new List<Setting_Utility>();
        public static IEnumerable<Setting_Utility> AllInstances => allInstances;

        void OnEnable()
        {
            // Add to the list when enabled
            if (!allInstances.Contains(this))
            {
                allInstances.Add(this);
            }
        }


        async void Awake()
        {
            await GetValues();
            await UpdateValues();
            await ExecuteValues();
        }
        async void Start()
        {
            await GetValues();
            await UpdateValues();
            await ExecuteValues();
        }

        public async Task GetValues()
        {
            try
            {
                foreach (var item in Corebeast.SaveGameManager.Options_File)
                {
                    switch (item.Key)
                    {
                        case "MotionBlurLevel": MotionBlurLevel = item.Value; break;
                        case "DepthLevel": DepthLevel = item.Value; break;
                        case "GraphicsLevel": GraphicsLevel = item.Value; break;
                        case "ScreenLevel": ScreenLevel = item.Value; break;
                        case "FramerateLevel": FramerateLevel = item.Value; break;
                        case "ShadowLevel": ShadowLevel = item.Value; break;
                        case "AnisotropicLevel": AnisotropicLevel = item.Value; break;
                        case "MasterAudioLevel": MasterAudioLevel = item.Value; break;
                        case "MusicAudioLevel": MusicAudioLevel = item.Value; break;
                        case "SoundAudioLevel": SoundAudioLevel = item.Value; break;
                        case "VoiceAudioLevel": VoiceAudioLevel = item.Value; break;
                        case "EnemyAudioLevel": EnemyAudioLevel = item.Value; break;
                        case "VFXAudioLevel": VFXAudioLevel = item.Value; break;
                        case "VsyncLevel": VsyncLevel = item.Value; break;
                        case "ShakeLevel": ShakeLevel = item.Value; break;
                        case "FOVLevel": FOVLevel = item.Value; break;
                    }
                }
                Corebeast.SaveGameManager.Check_For_SavegameData_Public(out savegameStatus);
                if (savegameStatus[0] == "Unavailible") {
                    Savegame01_Availible = false;
                    if (choosenValue == Value.GameData_SaveGame_01) {
                        SaveGame_Delete_Button.color = new Color32(114, 114, 114, 200);
                    }
                } else {
                    Savegame01_Availible = true;
                    if (choosenValue == Value.GameData_SaveGame_01) {
                        SaveGame_Delete_Button.color = new Color32(255, 255, 255, 255);
                    }
                }
                if (savegameStatus[1] == "Unavailible")
                {
                    Savegame02_Availible = false;
                    if (choosenValue == Value.GameData_SaveGame_02) {
                        SaveGame_Delete_Button.color = new Color32(114, 114, 114, 200);
                    }
                } else {
                    Savegame02_Availible = true;
                    if (choosenValue == Value.GameData_SaveGame_02) {
                        SaveGame_Delete_Button.color = new Color32(255, 255, 255, 255);
                    }
                }
                if (savegameStatus[2] == "Unavailible") {
                    Savegame03_Availbile = false;
                    if (choosenValue == Value.GameData_SaveGame_03) {
                        SaveGame_Delete_Button.color = new Color32(114, 114, 114, 200);
                    }
                } else {
                    Savegame03_Availbile = true;
                    if (choosenValue == Value.GameData_SaveGame_03) {
                        SaveGame_Delete_Button.color = new Color32(255, 255, 255, 255);
                    }
                }
            }
            catch (Exception ex)
            {
                string problem = $"An Error Occured in Setting_Utility:   Error:  {ex.Message}   At: {gameObject}";
                RoH.Corebeast.LogManager.Log(problem, LogLevel.Error);
                Debug.LogError(problem);
                statusHeader.text = "Error";
                this.enabled = false;
                await Task.CompletedTask;
                return;
            }
            await Task.CompletedTask;
            return;
        }

        public async Task UpdateValues()
        {
            switch (choosenValue)
            {
                case Value.Comfort_MotionBlurLevel:
                    string motionBlurStatus = MotionBlurLevel == 1 ? "ON" : "OFF";
                    nameHeader.text = "Motion Blur";
                    statusHeader.text = motionBlurStatus;
                    break;

                case Value.Comfort_DepthLevel:
                    string depthStatus = DepthLevel == 1 ? "ON" : "OFF";
                    nameHeader.text = "Depth";
                    statusHeader.text = depthStatus;
                    break;

                case Value.Comfort_CameraShakeLevel:
                    string shakeStatus = ShakeLevel == 1 ? "ON" : "OFF";
                    nameHeader.text = "Camera Shake";
                    statusHeader.text = shakeStatus;
                    break;

                case Value.Comfort_FOVLevel:
                    nameHeader.text = "FOV";
                    statusHeader.text = FOVLevel.ToString();
                    break;

                case Value.Graphics_VsyncLevel:
                    string vsyncStatus = VsyncLevel == 1 ? "ON" : "OFF";
                    nameHeader.text = "Vsync";
                    statusHeader.text = vsyncStatus;
                    break;

                case Value.Graphics_GraphicsLevel:
                    string graphicsStatus = "Error";
                    switch (GraphicsLevel) {
                        case 1: graphicsStatus = "Low"; break;
                        case 2: graphicsStatus = "Medium"; break;
                        case 3: graphicsStatus = "Ultra"; break;  }
                    nameHeader.text = "Graphics Presets";
                    statusHeader.text = graphicsStatus;
                    break;

                case Value.Graphics_ScreenLevel:
                    string screenStatus = "Error";
                    switch (ScreenLevel) {
                        case 1: screenStatus = "Windowed"; break;
                        case 2: screenStatus = "Fullscreen"; break;
                        case 3: screenStatus = "Bord. Fullscreen"; break; }
                    nameHeader.text = "Display Mode";
                    statusHeader.text = screenStatus;
                    break;

                case Value.Graphics_FramerateLevel:
                    nameHeader.text = "Framerate";
                    statusHeader.text = FramerateLevel.ToString();
                    break;

                case Value.Graphics_ShadowLevel:
                    string shadowStatus = "Error";
                    switch (ShadowLevel) {
                        case 1: shadowStatus = "Low"; break;
                        case 2: shadowStatus = "Medium"; break;
                        case 3: shadowStatus = "High"; break; }
                    nameHeader.text = "Shadown Quality";
                    statusHeader.text = shadowStatus;
                    break;

                case Value.Graphics_AnisotropicLevel:
                    string anisotropicStatus = "Error";
                    switch (AnisotropicLevel) {
                        case 2: anisotropicStatus = "2x Low"; break;
                        case 4: anisotropicStatus = "4x Medium"; break;
                        case 8: anisotropicStatus = "8x High"; break;
                        case 16: anisotropicStatus = "16x Ultra"; break; }
                    nameHeader.text = "Anisotropic Filtering";
                    statusHeader.text = anisotropicStatus;
                    break;

                case Value.Audio_MasterAudioLevel:
                    nameHeader.text = "Master Volume";
                    statusHeader.text = Math.Round(MasterAudioLevel,2).ToString();
                    break;

                case Value.Audio_MusicAudioLevel:
                    nameHeader.text = "Music Volume";
                    statusHeader.text = Math.Round(MusicAudioLevel,2).ToString();
                    break;

                case Value.Audio_SoundAudioLevel:
                    nameHeader.text = "Sound Volume";
                    statusHeader.text = Math.Round(SoundAudioLevel,2).ToString();
                    break;

                case Value.Audio_VoiceAudioLevel:
                    nameHeader.text = "Voice Volume";
                    statusHeader.text = Math.Round(VoiceAudioLevel,2).ToString();
                    break;

                case Value.Audio_EnemyAudioLevel:
                    nameHeader.text = "Enemy Volume";
                    statusHeader.text = Math.Round(EnemyAudioLevel,2).ToString();
                    break;

                case Value.Audio_VFXAudioLevel:
                    nameHeader.text = "VFX Volume";
                    statusHeader.text = Math.Round(VFXAudioLevel,2).ToString();
                    break;

                case Value.GameData_SaveGame_01:
                    SaveGameManager.Return_Difficulty_From_Savefile_Public(SaveGameType.Savegame01, out string save01Difficulty);
                    SaveGameManager.Return_Location_From_Savefile_Public(SaveGameType.Savegame01, out string save01Location);
                    SaveGame01_Status.text = Savegame01_Availible == true ? $"{save01Difficulty}:  {save01Location}" : "No Save Found";  
                    break;

                case Value.GameData_SaveGame_02:
                    SaveGameManager.Return_Difficulty_From_Savefile_Public(SaveGameType.Savegame02, out string save02Difficulty);
                    SaveGameManager.Return_Location_From_Savefile_Public(SaveGameType.Savegame02, out string save02Location);
                    SaveGame02_Status.text = Savegame02_Availible == true ? $"{save02Difficulty}:  {save02Location}" : "No Save Found";
                    break;

                case Value.GameData_SaveGame_03:
                    SaveGameManager.Return_Difficulty_From_Savefile_Public(SaveGameType.Savegame03, out string save03Difficulty);
                    SaveGameManager.Return_Location_From_Savefile_Public(SaveGameType.Savegame03, out string save03Location);
                    SaveGame03_Status.text = Savegame03_Availbile == true ? $"{save03Difficulty}:  {save03Location}" : "No Save Found";
                    break;
                    
            }
            await ExecuteValues();
            await Task.CompletedTask;
            return;
        }
        public async Task ExecuteValues()
        {
            try
            {
                // Comfort
                switch (MotionBlurLevel)
                {
                    case 0:
                        break;
                    case 1:
                        break;
                }
                switch (DepthLevel)
                {
                    case 0:
                        break;

                    case 1:
                        break;
                }
                switch (ShakeLevel)
                {
                    case 0:
                        break;
                    case 1:
                        break;
                }
                //Camera.main.Fov = FOVLevel;



                // Graphics

                //-Graphics Level
                int qualityIndex = 0;
                switch (GraphicsLevel)
                {
                    case 1: qualityIndex = 0; break;
                    case 2: qualityIndex = 1; break;
                    case 3: qualityIndex = 2; break;
                }
                QualitySettings.SetQualityLevel(qualityIndex);

                //-Screen Level
                switch (ScreenLevel)
                {
                    case 1: Screen.fullScreenMode = FullScreenMode.Windowed; break;
                    case 2: Screen.fullScreenMode = FullScreenMode.ExclusiveFullScreen; break;
                    case 3: Screen.fullScreenMode = FullScreenMode.FullScreenWindow; break;
                }

                //-FramerateLevel
                Application.targetFrameRate = (int)FramerateLevel;

                //-Shadow Settings
                switch (ShadowLevel)
                {
                    case 1:
                        QualitySettings.shadowDistance = 10f;
                        QualitySettings.shadowResolution = ShadowResolution.Low;
                        QualitySettings.shadowCascades = 0;
                        QualitySettings.shadowProjection = ShadowProjection.CloseFit;
                        break;

                    case 2:
                        QualitySettings.shadowDistance = 30f;
                        QualitySettings.shadowResolution = ShadowResolution.Medium;
                        QualitySettings.shadowCascades = 2;
                        QualitySettings.shadowProjection = ShadowProjection.StableFit;
                        break;

                    case 3:
                        QualitySettings.shadowDistance = 50f;
                        QualitySettings.shadowResolution = ShadowResolution.VeryHigh;
                        QualitySettings.shadowCascades = 4;
                        QualitySettings.shadowProjection = ShadowProjection.StableFit;
                        break;
                }

                //-AnisotropicLevel
                switch (AnisotropicLevel)
                {
                    case 2:
                        QualitySettings.anisotropicFiltering = AnisotropicFiltering.Disable;
                        QualitySettings.antiAliasing = 2;
                        break;

                    case 4:
                        QualitySettings.anisotropicFiltering = AnisotropicFiltering.Enable;
                        QualitySettings.antiAliasing = 4;
                        break;

                    case 8:
                        QualitySettings.anisotropicFiltering = AnisotropicFiltering.Enable;
                        QualitySettings.antiAliasing = 8;
                        break;

                    case 16:
                        QualitySettings.anisotropicFiltering = AnisotropicFiltering.ForceEnable;
                        QualitySettings.antiAliasing = 16;
                        break;
                }

                //-Vsync Level
                QualitySettings.vSyncCount = (int)VsyncLevel;

                // Sound
                MasterAudioLevel = Mathf.Clamp(MasterAudioLevel, 0.0001f, 1.0f);
                MusicAudioLevel = Mathf.Clamp(MusicAudioLevel, 0.0001f, 1.0f);
                SoundAudioLevel = Mathf.Clamp(SoundAudioLevel, 0.0001f, 1.0f);
                VoiceAudioLevel = Mathf.Clamp(VoiceAudioLevel, 0.0001f, 1.0f);
                EnemyAudioLevel = Mathf.Clamp(EnemyAudioLevel, 0.0001f, 1.0f);
                VFXAudioLevel = Mathf.Clamp(VFXAudioLevel, 0.0001f, 1.0f);
                mainMixer.SetFloat("MasterVolume", Mathf.Log10(MasterAudioLevel) * 50);
                mainMixer.SetFloat("MusicVolume", Mathf.Log10(MusicAudioLevel) * 50);
                mainMixer.SetFloat("SoundVolume", Mathf.Log10(SoundAudioLevel) * 50);
                mainMixer.SetFloat("VoiceVolume", Mathf.Log10(VoiceAudioLevel) * 50);
                mainMixer.SetFloat("EnemyVolume", Mathf.Log10(EnemyAudioLevel) * 50);
                mainMixer.SetFloat("VFXVolume", Mathf.Log10(VFXAudioLevel) * 50);
                await Task.CompletedTask;
                return;
            }
            catch (Exception ex)
            {
                string problem = $"An Error Occured Executing the Save Values Given:  {ex.Message}";
                Debug.LogError(problem);
                RoH.Corebeast.LogManager.Log(problem, LogLevel.Error);
            }
            await Task.CompletedTask;
            return;
        }



        public async void Clicked_Right_Cross()
        {
            // Right Click = + (Plus)
            switch (choosenValue)
            {
                case Value.Comfort_MotionBlurLevel:
                    if (MotionBlurLevel != 1) {
                        MotionBlurLevel = 1f;
                        Corebeast.SaveGameManager.Options_File["MotionBlurLevel"] = 1f;
                        Play_Sound(Sucess_AudioSource, successAudioClip, successVolume, false);
                    } else {
                        Play_Sound(Error_AudioSource, errorAudioClip, Error_Volume, false);
                    }
                    break;

                case Value.Comfort_DepthLevel:
                    if (DepthLevel != 1) {
                        DepthLevel = 1f;
                        Corebeast.SaveGameManager.Options_File["DepthLevel"] = 1f;
                        Play_Sound(Sucess_AudioSource, successAudioClip, successVolume, false);
                    } else {
                        Play_Sound(Error_AudioSource, errorAudioClip, Error_Volume, false);
                    }
                    break;

                case Value.Comfort_CameraShakeLevel:
                    if (ShakeLevel !=  1) {
                        ShakeLevel = 1f;
                        Corebeast.SaveGameManager.Options_File["ShakeLevel"] = 1f;
                        Play_Sound(Sucess_AudioSource,successAudioClip, successVolume, false);
                    } else {
                        Play_Sound(Error_AudioSource,errorAudioClip, Error_Volume, false);
                    }
                    break;

                case Value.Comfort_FOVLevel:
                    if (FOVLevel < 90) {
                        FOVLevel += 1;
                        Corebeast.SaveGameManager.Options_File["FOVLevel"] += 1;
                        Play_Sound(Sucess_AudioSource, successAudioClip, successVolume, false);
                    } else {
                        Play_Sound(Error_AudioSource,errorAudioClip,Error_Volume, false);
                    }
                    break;

                case Value.Graphics_VsyncLevel:
                    if (VsyncLevel != 1) {
                        VsyncLevel = 1f;
                        Corebeast.SaveGameManager.Options_File["VsyncLevel"] = 1f;
                        Play_Sound(Sucess_AudioSource, successAudioClip, successVolume, false);
                    }
                    else {
                        Play_Sound(Error_AudioSource,errorAudioClip, Error_Volume, false);
                    }
                    break;

                case Value.Graphics_GraphicsLevel:
                    if (GraphicsLevel < 3) {
                        GraphicsLevel += 1f;
                        Corebeast.SaveGameManager.Options_File["GraphicsLevel"] += 1f;
                        Play_Sound(Sucess_AudioSource, successAudioClip, successVolume, false);
                    } else {
                        Play_Sound(Error_AudioSource, errorAudioClip, Error_Volume, false);
                    }
                    break;

                case Value.Graphics_ScreenLevel:
                    if (ScreenLevel < 3) {
                        ScreenLevel += 1f;
                        Corebeast.SaveGameManager.Options_File["ScreenLevel"] += 1f;
                        Play_Sound(Sucess_AudioSource, successAudioClip, successVolume, false);
                    } else {
                        Play_Sound(Error_AudioSource, errorAudioClip, Error_Volume, false);
                    }
                    break;

                case Value.Graphics_FramerateLevel:
                    if (FramerateLevel >= 30)
                    {
                        FramerateLevel += 1f;
                        Corebeast.SaveGameManager.Options_File["FramerateLevel"] += 1f;
                        Play_Sound(Sucess_AudioSource, successAudioClip, successVolume, false);
                    } else {
                        Play_Sound(Error_AudioSource, errorAudioClip, Error_Volume, false);
                    }
                    break;

                case Value.Graphics_ShadowLevel:
                    if (ShadowLevel < 3) {
                        ShadowLevel += 1f;
                        Corebeast.SaveGameManager.Options_File["ShadowLevel"] += 1f;
                        Play_Sound(Sucess_AudioSource, successAudioClip, successVolume, false);
                    } else {
                        Play_Sound(Error_AudioSource, errorAudioClip, Error_Volume, false);
                    }
                    break;

                case Value.Graphics_AnisotropicLevel:
                    switch (AnisotropicLevel) {
                        case 2:
                            AnisotropicLevel = 4f;
                            Corebeast.SaveGameManager.Options_File["AnisotropicLevel"] = 4f;
                            Play_Sound(Sucess_AudioSource, successAudioClip, successVolume, false);
                            break;
                        case 4: 
                            AnisotropicLevel = 8f;
                            Corebeast.SaveGameManager.Options_File["AnisotropicLevel"] = 8f;
                            Play_Sound(Sucess_AudioSource, successAudioClip, successVolume, false);
                            break;
                        case 8:
                            AnisotropicLevel = 16f;
                            Corebeast.SaveGameManager.Options_File["AnisotropicLevel"] = 16f;
                            Play_Sound(Sucess_AudioSource, successAudioClip, successVolume, false);
                            break;
                    }
                    if (AnisotropicLevel == 16) {
                        Play_Sound(Error_AudioSource, errorAudioClip, Error_Volume, false);
                    }
                    break;

                case Value.Audio_MasterAudioLevel:
                    if (MasterAudioLevel < 1f) {
                        MasterAudioLevel += addValuePerClick;
                        Corebeast.SaveGameManager.Options_File["MasterAudioLevel"] += addValuePerClick;
                        Play_Sound(Sucess_AudioSource, successAudioClip, successVolume, false);
                    } 
                    else {
                        Play_Sound(Error_AudioSource, errorAudioClip, Error_Volume, false);
                    }
                    break;

                case Value.Audio_MusicAudioLevel:
                    if (MusicAudioLevel < 1f)
                    {
                        MusicAudioLevel += addValuePerClick;
                        Corebeast.SaveGameManager.Options_File["MusicAudioLevel"] += addValuePerClick;
                        Play_Sound(Sucess_AudioSource, successAudioClip, successVolume, false);
                    } else {
                        Play_Sound(Error_AudioSource, errorAudioClip, Error_Volume, false);
                    }
                    break;

                case Value.Audio_SoundAudioLevel:
                    if (SoundAudioLevel < 1f)
                    {
                        SoundAudioLevel += addValuePerClick;
                        Corebeast.SaveGameManager.Options_File["SoundAudioLevel"] += addValuePerClick;
                        Play_Sound(Sucess_AudioSource, successAudioClip, successVolume, false);
                    } else {
                        Play_Sound(Error_AudioSource, errorAudioClip, Error_Volume, false);
                    }
                    break;

                case Value.Audio_VoiceAudioLevel:
                    if (VoiceAudioLevel < 1f)
                    {
                        VoiceAudioLevel += addValuePerClick;
                        Corebeast.SaveGameManager.Options_File["VoiceAudioLevel"] += addValuePerClick;
                        Play_Sound(Sucess_AudioSource, successAudioClip, successVolume, false);
                    } else {
                        Play_Sound(Error_AudioSource, errorAudioClip, Error_Volume, false);
                    }
                    break;

                case Value.Audio_EnemyAudioLevel:
                    if (EnemyAudioLevel < 1f)
                    {
                        EnemyAudioLevel += addValuePerClick;
                        Corebeast.SaveGameManager.Options_File["EnemyAudioLevel"] += addValuePerClick;
                        Play_Sound(Sucess_AudioSource, successAudioClip, successVolume, false);
                    } else {
                        Play_Sound(Error_AudioSource, errorAudioClip, Error_Volume, false);
                    }
                    break;

                case Value.Audio_VFXAudioLevel:
                    if (VFXAudioLevel < 1f)
                    {
                        VFXAudioLevel += addValuePerClick;
                        Corebeast.SaveGameManager.Options_File["VFXAudioLevel"] += addValuePerClick;
                        Play_Sound(Sucess_AudioSource, successAudioClip, successVolume, false);
                    } else {
                        Play_Sound(Error_AudioSource, errorAudioClip, Error_Volume, false);
                    }
                    break;

                case Value.GameData_SaveGame_01:
                    if (Savegame01_Availible) {
                        Corebeast.SaveGameManager.Delete_Savegame_Data_Public(SaveGameType.Savegame01, out string savegame01Result);
                        if (savegame01Result == "Success") {
                            Corebeast.LogManager.Log($"Success while Deleting SaveGame01 (User Initiated)", LogLevel.Info);
                            Play_Sound(Sucess_AudioSource, successAudioClip, successVolume, false);
                        } else {
                            Corebeast.LogManager.Log($"A Problem occured while Deleting SaveGame01 (User Initiated)", LogLevel.Error);
                            Play_Sound(Error_AudioSource, errorAudioClip, Error_Volume, false);
                        }
                    }
                    break;

                case Value.GameData_SaveGame_02:
                    if (Savegame02_Availible) {
                        Corebeast.SaveGameManager.Delete_Savegame_Data_Public(SaveGameType.Savegame02, out string savegame02Result);
                        if (savegame02Result == "Success") {
                            Corebeast.LogManager.Log($"Success while Deleting SaveGame02 (User Initiated)", LogLevel.Info);
                            Play_Sound(Sucess_AudioSource, successAudioClip, successVolume, false);
                        } else {
                            Corebeast.LogManager.Log($"A Problem occured while Deleting SaveGame02 (User Initiated)", LogLevel.Error);
                            Play_Sound(Error_AudioSource, errorAudioClip, Error_Volume, false);
                        }
                    }
                    break;

                case Value.GameData_SaveGame_03:
                    if (Savegame03_Availbile) {
                        Corebeast.SaveGameManager.Delete_Savegame_Data_Public(SaveGameType.Savegame03, out string savegame03Result);
                        if (savegame03Result == "Success") {
                            Corebeast.LogManager.Log($"Success while Deleting SaveGame03 (User Initiated)", LogLevel.Info);
                            Play_Sound(Sucess_AudioSource, successAudioClip, successVolume, false);   
                        } else {
                            Corebeast.LogManager.Log($"A Problem occured while Deleting SaveGame03 (User Initiated)", LogLevel.Error);
                            Play_Sound(Error_AudioSource, errorAudioClip, Error_Volume, false);
                        }
                    }
                    break;

            }
            Setting_Utility[] scripts = Setting_Utility.AllInstances.ToArray();
            foreach (var script in scripts)
            {
                await script.GetValues();
                await script.UpdateValues();
                await script.ExecuteValues();
            }
            UpdateValues();
        }
        public void Pointer_Enter_Right_Cross()
        {
            Right_Cross.color = new Color32(255, 138, 138, 255);
            switch (choosenValue)
            {
                case Value.GameData_SaveGame_01:
                    if (Savegame01_Availible) {
                        SaveGame_Delete_Button.color = new Color32(255, 138, 138, 255);
                    } else { 
                        SaveGame_Delete_Button.color = new Color32(114, 114, 114, 200); 
                    }
                    break;

                case Value.GameData_SaveGame_02:
                    if (Savegame02_Availible) {
                        SaveGame_Delete_Button.color = new Color32(255, 138, 138, 255);
                    } else {
                        SaveGame_Delete_Button.color = new Color32(114, 114, 114, 200);
                    }
                    break;

                case Value.GameData_SaveGame_03:
                    if (Savegame03_Availbile) {
                        SaveGame_Delete_Button.color = new Color32(255, 138, 138, 255);
                    } else {
                        SaveGame_Delete_Button.color = new Color32(114, 114, 114, 200);
                    }
                    break;
            }

        }
        public void Pointer_Exit_Right_Cross()
        {
            Right_Cross.color = new Color32(255, 255, 255, 255);
            switch (choosenValue)
            {
                case Value.GameData_SaveGame_01:
                    if (Savegame01_Availible) {
                        SaveGame_Delete_Button.color = new Color32(255, 255, 255, 255);
                    } else {
                        SaveGame_Delete_Button.color = new Color32(114, 114, 114, 200);
                    }
                    break;

                case Value.GameData_SaveGame_02:
                    if (Savegame02_Availible) {
                        SaveGame_Delete_Button.color = new Color32(255, 255, 255, 255);
                    } else {
                        SaveGame_Delete_Button.color = new Color32(114, 114, 114, 200);
                    }
                    break;

                case Value.GameData_SaveGame_03:
                    if (Savegame03_Availbile) {
                        SaveGame_Delete_Button.color = new Color32(255, 255, 255, 255);
                    } else {
                        SaveGame_Delete_Button.color = new Color32(114, 114, 114, 200);
                    }
                    break;
            }
        }


        public void Clicked_Left_Cross()
        {
            // Left Click = - (Minus)
            switch (choosenValue)
            {
                case Value.Comfort_MotionBlurLevel:
                    if (MotionBlurLevel != 0){
                        MotionBlurLevel = 0f;
                        Corebeast.SaveGameManager.Options_File["MotionBlurLevel"] = 0f;
                        Play_Sound(Sucess_AudioSource, successAudioClip, successVolume, false);
                    } else {
                        Play_Sound(Error_AudioSource, errorAudioClip, Error_Volume, false);
                    }
                    break;

                case Value.Comfort_DepthLevel:
                    if (DepthLevel != 0)
                    {
                        DepthLevel = 0f;
                        Corebeast.SaveGameManager.Options_File["DepthLevel"] = 0f;
                        Play_Sound(Sucess_AudioSource, successAudioClip, successVolume, false);
                    } else {
                        Play_Sound(Error_AudioSource, errorAudioClip, Error_Volume, false);
                    }
                    break;

                case Value.Comfort_CameraShakeLevel:
                    if (ShakeLevel != 0) {
                        ShakeLevel = 0f;
                        Corebeast.SaveGameManager.Options_File["ShakeLevel"] = 0f;
                        Play_Sound(Sucess_AudioSource,successAudioClip, successVolume, false);
                    } else {
                        Play_Sound(Error_AudioSource,errorAudioClip, Error_Volume, false);
                    }
                    break;

                case Value.Comfort_FOVLevel:
                    if (FOVLevel > 70) {
                        FOVLevel -= 1;
                        Corebeast.SaveGameManager.Options_File["FOVLevel"] -= 1;
                        Play_Sound(Sucess_AudioSource, successAudioClip, successVolume, false);
                    } else {
                        Play_Sound(Error_AudioSource,errorAudioClip,Error_Volume, false);
                    }
                    break;

                case Value.Graphics_VsyncLevel:
                    if (VsyncLevel != 0) {
                        VsyncLevel = 0f;
                        Corebeast.SaveGameManager.Options_File["VsyncLevel"] = 0f;
                        Play_Sound(Sucess_AudioSource, successAudioClip, successVolume, false);
                    }
                    else {
                        Play_Sound(Error_AudioSource,errorAudioClip, Error_Volume, false);
                    }
                    break;

                case Value.Graphics_GraphicsLevel:
                    if (GraphicsLevel > 1)
                    {
                        GraphicsLevel -= 1f;
                        Corebeast.SaveGameManager.Options_File["GraphicsLevel"] -= 1f;
                        Play_Sound(Sucess_AudioSource, successAudioClip, successVolume, false);
                    } else {
                        Play_Sound(Error_AudioSource, errorAudioClip, Error_Volume, false);
                    }
                    break;

                case Value.Graphics_ScreenLevel:
                    if (ScreenLevel > 1)
                    {
                        ScreenLevel -= 1f;
                        Corebeast.SaveGameManager.Options_File["ScreenLevel"] -= 1f;
                        Play_Sound(Sucess_AudioSource, successAudioClip, successVolume, false);
                    } else {
                        Play_Sound(Error_AudioSource, errorAudioClip, Error_Volume, false);
                    }
                    break;

                case Value.Graphics_FramerateLevel:
                    if (FramerateLevel > 30) {
                        FramerateLevel -= 1f;
                        Corebeast.SaveGameManager.Options_File["FramerateLevel"] -= 1f;
                        Play_Sound(Sucess_AudioSource, successAudioClip, successVolume, false);
                    } else {
                        Play_Sound(Error_AudioSource, errorAudioClip, Error_Volume, false);
                    }
                    break;

                case Value.Graphics_ShadowLevel:
                    if (ShadowLevel > 1)
                    {
                        ShadowLevel -= 1f;
                        Corebeast.SaveGameManager.Options_File["ShadowLevel"] -= 1f;
                        Play_Sound(Sucess_AudioSource, successAudioClip, successVolume, false);
                    } else {
                        Play_Sound(Error_AudioSource, errorAudioClip, Error_Volume, false);
                    }
                    break;

                case Value.Graphics_AnisotropicLevel:
                    switch (AnisotropicLevel)
                    {
                        case 16:
                            AnisotropicLevel = 8f;
                            Corebeast.SaveGameManager.Options_File["AnisotropicLevel"] = 8f;
                            Play_Sound(Sucess_AudioSource, successAudioClip, successVolume, false);
                            break;
                        case 8:
                            AnisotropicLevel = 4f;
                            Corebeast.SaveGameManager.Options_File["AnisotropicLevel"] = 4f;
                            Play_Sound(Sucess_AudioSource, successAudioClip, successVolume, false);
                            break;
                        case 4:
                            AnisotropicLevel = 2f;
                            Corebeast.SaveGameManager.Options_File["AnisotropicLevel"] = 2f;
                            Play_Sound(Sucess_AudioSource, successAudioClip, successVolume, false);
                            break;
                    }
                    if (AnisotropicLevel == 2) {
                        Play_Sound(Error_AudioSource, errorAudioClip, Error_Volume, false);
                    }
                    break;

                case Value.Audio_MasterAudioLevel:
                    if (MasterAudioLevel > 0.02f)
                    {
                        MasterAudioLevel -= decreaseValuePerClick;
                        Corebeast.SaveGameManager.Options_File["MasterAudioLevel"] -= decreaseValuePerClick;
                        Play_Sound(Sucess_AudioSource, successAudioClip, successVolume, false);
                    } else {
                        Play_Sound(Error_AudioSource, errorAudioClip, Error_Volume, false);
                    }
                    break;

                case Value.Audio_MusicAudioLevel:
                    if (MusicAudioLevel > 0.02f)
                    {
                        MusicAudioLevel -= decreaseValuePerClick;
                        Corebeast.SaveGameManager.Options_File["MusicAudioLevel"] -= decreaseValuePerClick;
                        Play_Sound(Sucess_AudioSource, successAudioClip, successVolume, false);
                    } else {
                        Play_Sound(Error_AudioSource, errorAudioClip, Error_Volume, false);
                    }
                    break;

                case Value.Audio_SoundAudioLevel:
                    if (SoundAudioLevel > 0.02f)
                    {
                        SoundAudioLevel -= decreaseValuePerClick;
                        Corebeast.SaveGameManager.Options_File["SoundAudioLevel"] -= decreaseValuePerClick;
                        Play_Sound(Sucess_AudioSource, successAudioClip, successVolume, false);
                    } else {
                        Play_Sound(Error_AudioSource, errorAudioClip, Error_Volume, false);
                    }
                    break;

                case Value.Audio_VoiceAudioLevel:
                    if (VoiceAudioLevel > 0.02f)
                    {
                        VoiceAudioLevel -= decreaseValuePerClick;
                        Corebeast.SaveGameManager.Options_File["VoiceAudioLevel"] -= decreaseValuePerClick;
                        Play_Sound(Sucess_AudioSource, successAudioClip, successVolume, false);
                    } else {
                        Play_Sound(Error_AudioSource, errorAudioClip, Error_Volume, false);
                    }
                    break;

                case Value.Audio_EnemyAudioLevel:
                    if (EnemyAudioLevel > 0.02f)
                    {
                        EnemyAudioLevel -= decreaseValuePerClick;
                        Corebeast.SaveGameManager.Options_File["EnemyAudioLevel"] -= decreaseValuePerClick;
                        Play_Sound(Sucess_AudioSource, successAudioClip, successVolume, false);
                    } else {
                        Play_Sound(Error_AudioSource, errorAudioClip, Error_Volume, false);
                    }
                    break;

                case Value.Audio_VFXAudioLevel:
                    if (VFXAudioLevel > 0.02f)
                    {
                        VFXAudioLevel -= decreaseValuePerClick;
                        Corebeast.SaveGameManager.Options_File["VFXAudioLevel"] -= decreaseValuePerClick;
                        Play_Sound(Sucess_AudioSource, successAudioClip, successVolume, false);
                    } else {
                        Play_Sound(Error_AudioSource, errorAudioClip, Error_Volume, false);
                    }
                    break;
            }
            UpdateValues();
        }
        public void Pointer_Enter_Left_Cross()
        {
            Left_Cross.color = new Color32(255, 138, 138, 255);
        }
        public void Pointer_Exit_Left_Cross()
        {
            Left_Cross.color = new Color32(255, 255, 255, 255);
        }

        public void Play_Sound(AudioSource audioSource, AudioClip audioClip, float volume, bool loopProtection)
        {
            if (loopProtection) {
                if (!audioSource.isPlaying) {
                    audioSource.PlayOneShot(audioClip, volume);
                }
            }
            else {
                audioSource.PlayOneShot(audioClip, volume);
            }
        }
    }
}
public enum Value
{
    Comfort_MotionBlurLevel,
    Comfort_DepthLevel,
    Comfort_CameraShakeLevel,
    Comfort_FOVLevel,
    Graphics_GraphicsLevel,
    Graphics_ScreenLevel,
    Graphics_FramerateLevel,
    Graphics_ShadowLevel,
    Graphics_AnisotropicLevel,
    Graphics_VsyncLevel,
    Audio_MasterAudioLevel,
    Audio_MusicAudioLevel,
    Audio_SoundAudioLevel,
    Audio_VoiceAudioLevel,
    Audio_EnemyAudioLevel,
    Audio_VFXAudioLevel,
    GameData_SaveGame_01,
    GameData_SaveGame_02,
    GameData_SaveGame_03
}
