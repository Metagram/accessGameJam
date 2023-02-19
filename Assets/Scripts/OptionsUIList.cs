/*
@Author - Craig
@Description - 
*/

using Unity.VisualScripting;

public class OptionsUIList : UIList
{
    protected override void Start()
    {
        base.Start();
        gameObject.SetActive(false);
    }
}