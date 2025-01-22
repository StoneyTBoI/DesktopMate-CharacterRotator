using MelonLoader;
using UnityEngine;

[assembly: MelonInfo(typeof(DM_UnlockedRotation.Core), "DM_UnlockedRotation", "1.0.0", "Stoney", null)]
[assembly: MelonGame("infiniteloop", "DesktopMate")]

// TODO: Comment, Fix Cursor Grab Point, Split into seperate files, GUI for Custom Keybinds & More Keybinds
namespace DM_UnlockedRotation
{
    public class Core : MelonMod
    {
        private GameObject character;
        private bool rot_Enabled = false;
        private float last_Rot_Time = 0f;
        private float rot_Delay = 0.01f;

        private MelonPreferences_Category CharacterRotator;
        private MelonPreferences_Entry<KeyCode> rotateLeftKey;
        private MelonPreferences_Entry<KeyCode> rotateRightKey;
        private MelonPreferences_Entry<KeyCode> rotateUpKey;
        private MelonPreferences_Entry<KeyCode> rotateDownKey;
        private MelonPreferences_Entry<KeyCode> rotatePageUpKey;
        private MelonPreferences_Entry<KeyCode> rotatePageDownKey;
        private MelonPreferences_Entry<KeyCode> toggleRotationKey;
        private MelonPreferences_Entry<bool> debugEnabled;

        public override void OnInitializeMelon()
        {
            LoggerInstance.Msg("Desktop Mate Character Rotator Initialized");

            CharacterRotator = MelonPreferences.CreateCategory("CharacterRotator");
            rotateLeftKey = CharacterRotator.CreateEntry("RollPositive", KeyCode.LeftArrow);
            rotateRightKey = CharacterRotator.CreateEntry("RollNegative", KeyCode.RightArrow);
            rotateUpKey = CharacterRotator.CreateEntry("PitchPositive", KeyCode.UpArrow);
            rotateDownKey = CharacterRotator.CreateEntry("PitchNegative", KeyCode.DownArrow);
            rotatePageUpKey = CharacterRotator.CreateEntry("YawPositive", KeyCode.PageUp);
            rotatePageDownKey = CharacterRotator.CreateEntry("YawNegative", KeyCode.PageDown);
            toggleRotationKey = CharacterRotator.CreateEntry("ToggleRotation", KeyCode.F8);
            debugEnabled = CharacterRotator.CreateEntry("DebugEnabled", false);
        }

        public override void OnUpdate()
        {
            if (character == null)
            {
                find_Char_By_Root();
                return;
            }

            if (Input.GetKeyDown(toggleRotationKey.Value))
            {
                rot_Enabled = !rot_Enabled;
                string status = rot_Enabled ? "enabled" : "disabled";
                MelonLogger.Msg($"[DM_UnlockedRotation] Rotation {status}.");
            }

            if (rot_Enabled && Time.time - last_Rot_Time >= rot_Delay)
            {
                key_to_rot();
            }
        }

        private void find_Char_By_Root()
        {
            GameObject[] allObjects = GameObject.FindObjectsOfType<GameObject>();

            foreach (GameObject obj in allObjects)
            {
                if (obj.name == "CharactersRoot")
                {
                    character = obj;
                    if (debugEnabled.Value)
                    {
                        MelonLogger.Msg($"[DM_UnlockedRotation] Character found: {obj.name}");
                        return;
                    }
                }
            }
            if (debugEnabled.Value)
            {
                MelonLogger.Warning("[DM_UnlockedRotation] No character with a 'CharactersRoot' GameObject found.");
            }
        }

        private void key_to_rot()
        {
            if (Input.GetKey(rotateLeftKey.Value))
            {
                rot_Char(Vector3.forward, 1f);
            }

            if (Input.GetKey(rotateRightKey.Value))
            {
                rot_Char(Vector3.forward, -1f);
            }

            if (Input.GetKey(rotateUpKey.Value))
            {
                rot_Char(Vector3.right, 1f);
            }

            if (Input.GetKey(rotateDownKey.Value))
            {
                rot_Char(Vector3.right, -1f);
            }

            if (Input.GetKey(rotatePageUpKey.Value))
            {
                rot_Char(Vector3.up, 1f);
            }

            if (Input.GetKey(rotatePageDownKey.Value))
            {
                rot_Char(Vector3.up, -1f);
            }
        }

        private void rot_Char(Vector3 axis, float angle)
        {
            if (character != null)
            {
                character.transform.Rotate(axis, angle, Space.World);

                last_Rot_Time = Time.time;

                Vector3 currentRotation = character.transform.eulerAngles;
                if (debugEnabled.Value)
                {
                    MelonLogger.Msg($"[DM_UnlockedRotation] Rotated {character.name} by {angle} degrees on axis {axis}. ");
                }
            }
        }
        public override void OnApplicationQuit()
        {
            MelonPreferences.Save();
        }
    }

    //                      ░██                                                     
    //                         ███                                                    
    //                           ██                                                   
    //                           ██                                                   
    //                           ░██                                                  
    //                         ▓█▒░░▒▓███                                             
    //                       ░▒░  ▒░░░▒▒▓██                                           
    //                       ▒▒   ░░░░░▒░▒█▓                                          
    //                      ░▒░  ░░░░░▒▓▓▓▓█                                          
    //                      ▒░░░░░▒▒▓▓▒▓▓▒▒██                                         
    //                     ░░░░ ░░░░░▒▒▒▒▒▓▓█                                         
    //                     ░░░░▒▒▒░▓▓▒▒▒▓▒▒▓█░                                        
    //                    ░░░▒▒▒▒▒▒▒▓▒▒▒▒▒▒▓▓█░                                       
    //                   ░░░▒▒▒▒▒▒▒▒▒▒▓▒▓▓▓▒▓██░                                      
    //                ░▒████▓▓▒▒▓▓█▒▒▒▒░▒▒▓▒▒▒▓█                                      
    //               ██▒  ▓█▓▒▒▓▒█░    ▒░▒▒▒▒▓▓▓▓                                     
    //              ▓█▓██░  ████▓▒  ████▒░▒▒▒▒▒▓▓▓                                    
    //              ██▓ ░██  ▓███ ░█▒ ███▒▓▒▒▓▒█▓▓▓                                   
    //              ██ ████▒   ██  ▓███████▒▒▒▒▒▒▒▓                                   
    //              █   ▒█      ▒    ██▓  ▒█▓▒▒▒█▓▒░                                  
    //              █▓       ▒░           ░▒▒▓▓▓▒▒▒                                   
    //               █ ░▒▒▒▒░▒░   █       ▒▒▒▒▒░▒▒▒▒                                   
    //               ░▒  ░░░██████▓     ░▓▓▓▓░░▒▓█                                    
    //                ▒██░     ░      ▒█▒▒▒░░░▒▒█                                     
    //                  ▓██▓      ░ ░▒▒░░░▒▒▒▒▓█  ░▒░░░                               
    //                   ░▓▓███▓▒▒░░▒░░░▒░░░▒█▒ ░▒▒▒▒░░▒▒▒▒▒░░░                       
    //                      ░░░░░░░░░░░░░░▒▓▒░▒▒▒▒▒▒▒▒░░░░░░░░░░░░░░░░░░░             
    //                       ███████████████▓▓▒▒▒▒▒▒▒▒▒░░░░░░░░░░░ ░ ░░░░░░           
    //                        ▒███████████▓▓▓▓▓▓▒▒▒▒▒▒░░░░░░░ ░░  ░░░░░░              
    //                             ▒▒▒▓▒▒▒▒▒▒▒▒▒▒▒░░░░░░░░ ░░░░░░░░░

}