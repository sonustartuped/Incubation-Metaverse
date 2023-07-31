using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        for (int i = 0; i < ActivityHandler.instance.officeTriggers.Length; i++)
        {
            if (other.gameObject.name.Equals(ActivityHandler.instance.officeTriggers[i].enterInOffice.name))
            {
                ActivityHandler.instance.OnTrigger_EnterInOffice(true, i);
                break;
            }
            else if (other.gameObject.name.Equals(ActivityHandler.instance.officeTriggers[i].exitFromOffice.name))
            {
                ActivityHandler.instance.OnTrigger_ExitFromOffice(true, i);
                break;
            }
        }

       
    }

    private void OnTriggerExit(Collider other)
    {
        for (int i = 0; i < ActivityHandler.instance.officeTriggers.Length; i++)
        {
            if (other.gameObject.name.Equals(ActivityHandler.instance.officeTriggers[i].enterInOffice.name))
            {
                ActivityHandler.instance.OnTrigger_EnterInOffice(false, i);
                break;
            }
            else if (other.gameObject.name.Equals(ActivityHandler.instance.officeTriggers[i].exitFromOffice.name))
            {
                ActivityHandler.instance.OnTrigger_ExitFromOffice(false, i);
                break;
            }
        }
    }
}
