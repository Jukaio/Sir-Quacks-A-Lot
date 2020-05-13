using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml.Serialization;
using System.IO;

//Control layout for playstation 
namespace Game_Input
{
    namespace Devices
    {
        public class Keyboard
        {
            static public bool Get_Button(KeyCode button)
            {
                if (button == KeyCode.None)
                    return false;
                return Input.GetKey(button);
            }
        }
    }
}
