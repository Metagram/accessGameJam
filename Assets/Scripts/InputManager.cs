/*
@Author - Craig
@Description - Handles user input (include controller) 
*/

using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    private float lastHorizontal = 0;
    private float lastVertical = 0;
    private float lastConfirm = 0;
    private float lastCancel = 0;
    
    private static float PRESS_THRESHOLD = 0.5f;

    private bool leftPressed = false;
    private bool rightPressed = false;
    private bool upPressed = false;
    private bool downPressed = false;
    private bool confirmPressed = false;
    private bool cancelPressed = false;
    
    private static Dictionary<string, KeyCode> defaultControls = new Dictionary<string, KeyCode>
    {
        { "Up", KeyCode.W },
        { "Down", KeyCode.S },
        { "Left", KeyCode.A },
        { "Right", KeyCode.D },
        { "Confirm", KeyCode.J },
        { "Cancel", KeyCode.L },
        { "Pause", KeyCode.Escape}
    };
    
    public Dictionary<string, KeyCode> controls = new Dictionary<string, KeyCode>();
    
    public static InputManager instance;

    public float Vertical()
    {
        int ans = 0;
        if (Input.GetKey(controls["Up"]))
        {
            ans++;
        }
        if (Input.GetKey(controls["Down"]))
        {
            ans--;
        }

        if (ans == 0)
        {
            return Input.GetAxis("Vertical");
        }
        else
        {
            return ans;
        }
    }
    
    public float Horizontal()
    {
        int ans = 0;
        if (Input.GetKey(controls["Left"]))
        {
            ans--;
        }
        if (Input.GetKey(controls["Right"]))
        {
            ans++;
        }

        if (ans == 0)
        {
            return Input.GetAxis("Horizontal");
        }
        else
        {
            return ans;
        }
    }

    public bool Up()
    {
        return upPressed || Input.GetKeyDown(controls["Up"]);
    }
    
    public bool Down()
    {
        return downPressed || Input.GetKeyDown(controls["Down"]);
    }
    
    public bool Left()
    {
        return leftPressed || Input.GetKeyDown(controls["Left"]);
    }
    
    public bool Right()
    {
        return rightPressed || Input.GetKeyDown(controls["Right"]);
    }

    public bool Confirm()
    {
        return confirmPressed || Input.GetKeyDown(controls["Confirm"]);
    }

    public bool Cancel()
    {
        return cancelPressed || Input.GetKeyDown(controls["Cancel"]);
    }

    public bool PausePressed()
    {
        return Input.GetKeyDown(controls["Pause"]);
    }

    protected virtual void Update()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        float confirm = Input.GetAxisRaw("Jump");
        float cancel = Input.GetAxisRaw("Fire3");
        if (horizontal > PRESS_THRESHOLD && lastHorizontal <= PRESS_THRESHOLD)
        {
            rightPressed = true;
        }
        else
        {
            rightPressed = false;
        }

        if (horizontal < -PRESS_THRESHOLD && lastHorizontal >= -PRESS_THRESHOLD)
        {
            leftPressed = true;
        }
        else
        {
            leftPressed = false;
        }

        if (vertical > PRESS_THRESHOLD && lastVertical <= PRESS_THRESHOLD)
        {
            upPressed = true;
        }
        else
        {
            upPressed = false;
        }

        if (vertical < -PRESS_THRESHOLD && lastVertical >= -PRESS_THRESHOLD)
        {
            downPressed = true;
        }
        else
        {
            downPressed = false;
        }

        if (confirm > PRESS_THRESHOLD && lastConfirm <= PRESS_THRESHOLD)
        {
            confirmPressed = true;
        }
        else
        {
            confirmPressed = false;
        }

        if (cancel > PRESS_THRESHOLD && lastCancel <= PRESS_THRESHOLD)
        {
            cancelPressed = true;
        }
        else
        {
            cancelPressed = false;
        }
        
        lastHorizontal = horizontal;
        lastVertical = vertical;
        lastConfirm = confirm;
        lastCancel = cancel;
    }
    
    public void UpdateControlsFile()
    {
        string output = "";
        foreach (KeyValuePair<string, KeyCode> pair in controls)
        {
            output += pair.Key + "=" + pair.Value + '\n';
        }
        System.IO.File.WriteAllText(ControlsFilePath(), output);
    }

    public void LoadControlsFile()
    {
        string[] lines = System.IO.File.ReadAllLines(ControlsFilePath());
        foreach (string line in lines)
        {
            string[] split = line.Split('=');
            controls[split[0]] = (KeyCode) System.Enum.Parse(typeof(KeyCode), split[1]);
        }
    }

    private string ControlsFilePath()
    {
        return Application.dataPath + "/controls.ini";
    }
    
    protected virtual void Start()
    {
        instance = this;
        if (System.IO.File.Exists(ControlsFilePath()))
        {
            LoadControlsFile();
        }
        else
        {
            controls = defaultControls;
        }
    }
}