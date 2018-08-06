using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace ShadyPixel.Screenshots
{
    public class Screenshot : MonoBehaviour
    {
        public string filename = "screenshot";
        public int suffix = 0;

        [Button("Take Screenshot")]
        public void TakeScreenshot()
        {
            ScreenCapture.CaptureScreenshot(filename+"_"+suffix+".png");
            suffix++;
        }
    }
}
