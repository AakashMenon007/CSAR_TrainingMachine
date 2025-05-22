using UnityEngine;
using Yarn.Unity;

public class YarnCommandControls : MonoBehaviour
{
    [YarnCommand("activate_object")]
    public void ActivateObject(string objectName)
    {
        GameObject obj = GameObject.Find(objectName);
        if (obj == null)
        {
            Debug.LogError($"GameObject '{objectName}' not found in the scene.");
            return;
        }

        Activatable activatable = obj.GetComponent<Activatable>();
        if (activatable == null)
        {
            Debug.LogError($"GameObject '{objectName}' does not have an Activatable component.");
            return;
        }

        activatable.Activate();
    }
}
