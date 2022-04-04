using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

#if UNITY_EDITOR
public class Screenshots : MonoBehaviour {

	// Update is called once per frame
	void Update () {

		if(Input.GetKeyDown(KeyCode.F10))
        {
            TakeScreenshot();
        }
	}

    [ContextMenu("Screenshot")]
    public void TakeScreenshot()
    {
        int i= 0;

        while(File.Exists("Screenshot_"+i.ToString("00")+".png"))
        {
            i++;
        }

        ScreenCapture.CaptureScreenshot("Screenshot_" + i.ToString("00") + ".png");
    }
}

#endif