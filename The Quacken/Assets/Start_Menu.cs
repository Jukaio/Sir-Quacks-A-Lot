using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Start_Menu : MonoBehaviour
{
    private int m_index = -1;

    private void Start()
    {
        //Scene_Manager.Load_Level(1); // <- Loads a certain scene from the build settings
                                       // 0 = Game_Managment; 1 = Start_Menu; 2 = Level_One

        /*
        Interactive Start Screen:
        1. Movement - Duck moves already, so this part is done
        2. If the duck is within a certain rectangle, the index value changes 
        3. If the duck is not within any rectangle, the index is -1 
        4. If the player presses "a certain button" (Let's call it the action button) a certain action happens


        */
    }
}
