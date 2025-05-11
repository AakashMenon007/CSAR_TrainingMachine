using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class MyPlayerPrefsExample : MonoBehaviour
{
    [Header ("PlayerPrefsInteger")]
    public int intFromPlayerPrefs;
    public int currentIntValue;
    public TMP_InputField intInputFieldTMP;
    public TMP_Text intInputFieldInfoTMP;
    public TMP_Text intCurrentValueTMP;
    public TMP_Text intPlayerPrefValueTMP;

    [Header("PlayerPrefsFloat")]
    public float floatFromPlayerPrefs;
    public float currentFloatValue;
    public TMP_InputField floatInputFieldTMP;
    public TMP_Text floatInputFieldInfoTMP;
    public TMP_Text floatCurrentValueTMP;
    public TMP_Text floatPlayerPrefValueTMP;

    [Header("PlayerPrefsString")]
    public string stringFromPlayerPrefs;
    public string currentStringValue;
    public TMP_InputField stringInputFieldTMP;
    public TMP_Text stringInputFieldInfoTMP;
    public TMP_Text stringCurrentValueTMP;
    public TMP_Text stringPlayerPrefValueTMP;


    //function checks whether variable from the input field can be 'parsed' as int, if so set playerPref (if doesn't exsist, creates), if not return error message
    public void SaveInteger(string playerPrefsInt)
    {
        if (int.TryParse(intInputFieldTMP.text, out int value))
        {
            PlayerPrefs.SetInt(playerPrefsInt, currentIntValue);
            intInputFieldTMP.text = ("");
            intInputFieldInfoTMP.text = "Value saved";
        }
        else
        {
            intInputFieldTMP.text = ("");
            intInputFieldInfoTMP.text = "Try again with an integer";
        }
    }

    //checks if there is a playerPref 'key' with desired name, if so load variable (in this case int) and set to string for TMP, if not return error message
    public void LoadInteger(string playerPrefsInt)
    {
        if(PlayerPrefs.HasKey(playerPrefsInt))
        {
            intFromPlayerPrefs = PlayerPrefs.GetInt(playerPrefsInt);
            intPlayerPrefValueTMP.text = intFromPlayerPrefs.ToString();
            intInputFieldInfoTMP.text = "Value loaded";
        }
        else
        {
            intPlayerPrefValueTMP.text = "";
            intInputFieldInfoTMP.text = "No player pref to load";
        }
    }

    //checks if there is a playerPref 'key' with desired name, if so delete variable (in this case int), if not return error message
    public void DeleteIntPlayerPref(string playerPrefsInt)
    {
        if (PlayerPrefs.HasKey(playerPrefsInt))
        {
            PlayerPrefs.DeleteKey(playerPrefsInt);
            intInputFieldInfoTMP.text = "Int player pref deleted";
        }
        else
        {
            intPlayerPrefValueTMP.text = "";
            intInputFieldInfoTMP.text = "No player pref to delete";
        }
    }

    //function checks whether variable from the input field can be 'parsed' as int, if so update currentValue TMP, if not return error message
    public void IntUpdateValue(string playerPrefsInt)
    {
        if (int.TryParse(intInputFieldTMP.text, out int value))
        {
            currentIntValue = value;
            intCurrentValueTMP.text = currentIntValue.ToString();
        }
        else
        {
            intInputFieldTMP.text = ("");
            intInputFieldInfoTMP.text = "Try again with an integer";
        }
    }

    //function checks whether variable can be 'parsed' as float, if so set playerPref (if doesn't exsist, creates), if not return error message
    public void SaveFloat()
    {
        if (float.TryParse(floatInputFieldTMP.text, out float value))
        {
            PlayerPrefs.SetFloat("playerPrefsFloat", currentFloatValue);
            floatInputFieldTMP.text = ("");
            floatInputFieldInfoTMP.text = "Value saved";
        }
        else
        {
            floatInputFieldTMP.text = ("");
            floatInputFieldInfoTMP.text = "Try again with a float";
        }
    }

    //checks if there is a playerPref 'key' with desired name, if so load variable (in this case float) and set to string for TMP, if not return error message
    public void LoadFloat()
    {
        if(PlayerPrefs.HasKey("playerPrefsFloat"))
        {
            floatFromPlayerPrefs = PlayerPrefs.GetFloat("playerPrefsFloat");
            floatPlayerPrefValueTMP.text = floatFromPlayerPrefs.ToString();
            floatInputFieldInfoTMP.text = "Value loaded";
        }
        else
        {
            floatPlayerPrefValueTMP.text = "";
            floatInputFieldInfoTMP.text = "No player pref to load";
        }
    }

    //checks if there is a playerPref 'key' with desired name, if so delete variable (in this case float), if not return error message
    public void DeleteFloatPlayerPref()
    {
        if (PlayerPrefs.HasKey("playerPrefsFloat"))
        {
            PlayerPrefs.DeleteKey("playerPrefsFloat");
            floatInputFieldInfoTMP.text = "Float player pref deleted";
        }
        else
        {
            floatPlayerPrefValueTMP.text = "";
            floatInputFieldInfoTMP.text = "No player pref to delete";
        }
    }

    //function checks whether variable from the input field can be 'parsed' as float, if so update currentValue TMP, if not return error message
    public void FloatUpdateValue()
    {
        if (float.TryParse(floatInputFieldTMP.text, out float value))
        {
            currentFloatValue = value;
            floatCurrentValueTMP.text = currentFloatValue.ToString();
        }
        else
        {
            floatInputFieldTMP.text = ("");
            floatInputFieldInfoTMP.text = "Try again with a float";
        }
    }

    //function sets string playerPref to desired name (if doesn't exsist, creates)
    public void SaveString()
    {
        PlayerPrefs.SetString("playerPrefsString", currentStringValue);
        stringInputFieldTMP.text = ("");
        stringInputFieldInfoTMP.text = "Value saved";
    }

    //checks if there is a playerPref 'key' with desired name, if so load variable (in this case string), if not return error message
    public void LoadString()
    {
        if(PlayerPrefs.HasKey("playerPrefsString"))
        {
            stringFromPlayerPrefs = PlayerPrefs.GetString("playerPrefsString");
            stringPlayerPrefValueTMP.text = stringFromPlayerPrefs;
            stringInputFieldInfoTMP.text = "Value loaded";
        }
        else
        {
            stringPlayerPrefValueTMP.text = "";
            stringInputFieldInfoTMP.text = "No value to load";
        }
    }

    //checks if there is a playerPref 'key' with desired name, if so delete variable (in this case string), if not return error message
    public void DeleteStringPlayerPref()
    {
        if (PlayerPrefs.HasKey("playerPrefsString"))
        {
            PlayerPrefs.DeleteKey("playerPrefsString");
            stringInputFieldInfoTMP.text = "String player pref deleted";
        }
        else
        {
            stringPlayerPrefValueTMP.text = "";
            stringInputFieldInfoTMP.text = "No value to delete";
        }

    }

    //sets curent value string feedback TMP to current string input field
    public void StringUpdateValue()
    {
        currentStringValue = stringInputFieldTMP.text;
        stringCurrentValueTMP.text = currentStringValue;
    }

    //deletes all playerPref 'keys'
    public void DeleteAllPlayerPrefs()
    {
        PlayerPrefs.DeleteAll();
        intInputFieldInfoTMP.text = "All player prefs deleted";
        floatInputFieldInfoTMP.text = "All player prefs deleted";
        stringInputFieldInfoTMP.text = "All player prefs deleted";
    }

    //gets active scene name and compares - loads next scene dependant on current scene name
    public void ChangeScene()
    {
        Scene scene = SceneManager.GetActiveScene();
        if(scene.name == "PlayerPrefsExamplePage1")
        {
            SceneManager.LoadScene("PlayerPrefsExamplePage2");
        }
        else
        {
            SceneManager.LoadScene("PlayerPrefsExamplePage1");
        }
    }
}
