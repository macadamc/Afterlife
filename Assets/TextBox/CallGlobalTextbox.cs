using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class CallGlobalTextbox : MonoBehaviour
{
    TextBoxRef Textbox
    {
        get
        {
            if (_global == null)
                _global = GameObject.FindGameObjectWithTag("GlobalTextbox").GetComponent<TextBoxRef>();

            return _global;
        }
    }

    public string startNode;

    TextBoxRef _global;

    public void CallNode(string node)
    {
        Textbox.CallTextBox(node);
    }


}
