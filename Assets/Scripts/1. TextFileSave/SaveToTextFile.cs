using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using TMPro;
using System;

public class SaveToTextFile : MonoBehaviour
{
    [Header("Folders")]
    public TMP_InputField folderNameInputField;
    public TMP_Text folderNameInputFeedback;
    public string customFolderName;

    [Header("Files")]
    public TMP_InputField fileNameInputField;
    public TMP_Text fileNameInputFeedback;
    public string customFileName;

    [Header("File Content")]
    public TMP_InputField fileContentInputField;
    public TMP_Text fileContentInputFeedback;
    public string contentForTextFile;

    public string directoryName;

    private void Awake()
    {
        Debug.Log(Application.persistentDataPath);
    }

    public void CheckForDirectory()
    {
        if (folderNameInputField.text == "")
        {
            folderNameInputFeedback.text = "Enter a folder name";
        }
        else
        {
            customFolderName = folderNameInputField.text;
            directoryName = Application.persistentDataPath + "/" + customFolderName + "/";

            if (Directory.Exists(directoryName))
            {
                folderNameInputFeedback.text = "There is a folder by that name in the directory";
            }
            else
            {
                folderNameInputFeedback.text = "Folder by that name does not exist";
            }
        }
    }

    public void CreateDirectory()
    {
        if (folderNameInputField.text == "")
        {
            folderNameInputFeedback.text = "Enter a folder name";
        }
        else
        {
            if (Directory.Exists(directoryName))
            {
                folderNameInputFeedback.text = "A directory of that name already exists";
            }
            else
            {
                Directory.CreateDirectory(directoryName);
                folderNameInputFeedback.text = "Your directory has now been created @ " + directoryName;
                Debug.Log("Your directory has now been created @ " + directoryName);
            }
        }
    }

    public void DeleteDirectory()
    {
        if (folderNameInputField.text == "")
        {
            folderNameInputFeedback.text = "Enter a folder name";
        }
        else
        {
            customFolderName = folderNameInputField.text;
            directoryName = Application.persistentDataPath + "/" + customFolderName + "/";

            if (Directory.Exists(directoryName))
            {
                Directory.Delete(directoryName);
                folderNameInputFeedback.text = "The folder has been found and deleted";
            }
            else
            {
                folderNameInputFeedback.text = "Folder by that name does not exist";
            }
        }
    }

    public void CheckForTextFile()
    {
        customFileName = fileNameInputField.text;

        if (directoryName == null || directoryName == "")
        {
            directoryName = Application.persistentDataPath + "/";
        }
        else
        {
            directoryName = Application.persistentDataPath + "/" + customFolderName + "/";
        }

        if (customFileName == null || customFileName == "")
        {
            fileNameInputFeedback.text = "Enter a file name";
        }
        else
        {
            string textDocumentPath = directoryName + customFileName + ".txt";

            if (File.Exists(textDocumentPath))
            {
                fileNameInputFeedback.text = "A file exists at " + textDocumentPath;
            }
            else
            {
                if (directoryName == null || directoryName == "")
                {
                    fileNameInputFeedback.text = "There is no file found called " + customFileName + " @ " + Application.persistentDataPath;
                }
                else
                {
                    fileNameInputFeedback.text = "There is no file found called " + customFileName + " @ " + directoryName;
                }
            }
        }
    }

    public void CreateTextFile()
    {
        customFileName = fileNameInputField.text;
        string textDocumentPath = directoryName + customFileName + ".txt";

        if (customFileName == null || customFileName == "")
        {
            fileNameInputFeedback.text = "Enter a file name";
        }
        else
        {
            if (directoryName == null || directoryName == "")
            {
                if (File.Exists(textDocumentPath))
                {
                    fileNameInputFeedback.text = "There is already a file named " + customFileName + " @ " + Application.persistentDataPath + " and a new one has NOT been created";
                    Debug.Log("There is already a file named " + customFileName + " @ " + Application.persistentDataPath + " and a new one has NOT been created");
                }
                else
                {
                    File.Create(textDocumentPath).Dispose();
                    fileNameInputFeedback.text = "This file " + customFileName + " has been saved to the persistent data path @ " + Application.persistentDataPath;
                    Debug.Log("This file " + customFileName + " has been saved to the persistent data path @ " + Application.persistentDataPath);
                }
            }
            else
            {
                if (File.Exists(textDocumentPath))
                {
                    fileNameInputFeedback.text = "There is already a file named " + customFileName + " @ " + directoryName + " and a new one has NOT been created";
                    Debug.Log("There is already a file named " + customFileName + " @ " + directoryName + " and a new one has NOT been created");
                }
                else
                {
                    File.Create(textDocumentPath).Dispose();
                    fileNameInputFeedback.text = "Your file " + customFileName + " has been saved to your custom folder @ " + textDocumentPath;
                    Debug.Log("Your file " + customFileName + " has been saved to your custom folder @ " + textDocumentPath);
                }
            }
        }
    }

    public void DeleteTextFile()
    {
        customFileName = fileNameInputField.text;
        string textDocumentPath = directoryName + customFileName + ".txt";

        if (customFileName == null || customFileName == "")
        {
            fileNameInputFeedback.text = "Enter a file name";
        }
        else
        {
            if (File.Exists(textDocumentPath))
            {
                File.Delete(textDocumentPath);
                fileNameInputFeedback.text = "The file " + customFileName + " has been found and deleted";
            }
            else
            {
                if (directoryName == null || directoryName == "")
                {
                    fileNameInputFeedback.text = "There is no file found called " + customFileName + " @ " + Application.persistentDataPath;
                }
                else
                {
                    fileNameInputFeedback.text = "There is no file found called " + customFileName + " @ " + directoryName;
                }
            }
        }
    }

    public void CreateOrAddToTextFile()
    {
        contentForTextFile = fileContentInputField.text;

        if (contentForTextFile == null || contentForTextFile == "")
        {
            fileContentInputFeedback.text = "Enter some content to be saved";
        }
        else
        {
            if (directoryName == null || directoryName == "")
            {
                if (customFileName == null || customFileName == "")
                {
                    fileContentInputFeedback.text = "Enter a file name to be able to save this content";
                }
                else
                {
                    string textDocumentPath = directoryName + customFileName + ".txt";
                    File.AppendAllText(textDocumentPath, contentForTextFile + "\n \n");
                    fileContentInputFeedback.text = "Your content has been added to the file " + customFileName + " saved at the persistent data path @ " + Application.persistentDataPath;
                    Debug.Log("Your content has been added to the file " + customFileName + " saved at the persistent data path @ " + Application.persistentDataPath);
                }
            }
            else
            {
                if (customFileName == null || customFileName == "")
                {
                    fileContentInputFeedback.text = "Enter a file name to be able to save this content";
                }
                else
                {
                    string textDocumentPath = directoryName + customFileName + ".txt";
                    File.AppendAllText(textDocumentPath, contentForTextFile + "\n \n");
                    fileContentInputFeedback.text = "Your content has been added to the file " + customFileName + " @ " + directoryName;
                    Debug.Log("Your content has been added to the file " + customFileName + " @ " + directoryName);
                }
            }
        }
    }

    public void OverwriteTextFile()
    {
        string textDocumentPath = directoryName + customFileName + ".txt";

        contentForTextFile = fileContentInputField.text;

        if (contentForTextFile == null || contentForTextFile == "")
        {
            fileContentInputFeedback.text = "Enter some content to be saved";
        }
        else
        {
            if (directoryName == null || directoryName == "")
            {
                if (customFileName == null || customFileName == "")
                {
                    fileContentInputFeedback.text = "Enter a file name to be able to save this content";
                }
                else
                {
                    File.WriteAllText(textDocumentPath, "");
                    File.AppendAllText(textDocumentPath, contentForTextFile + "\n \n");
                    fileContentInputFeedback.text = "Your content has been added to the file " + customFileName + " saved at the persistent data path @ " + Application.persistentDataPath;
                    Debug.Log("Your content has been added to the file " + customFileName + " saved at the persistent data path @ " + Application.persistentDataPath);
                }
            }
            else
            {
                if (customFileName == null || customFileName == "")
                {
                    fileContentInputFeedback.text = "Enter a file name to be able to save this content";
                }
                else
                {
                    File.WriteAllText(textDocumentPath, "");
                    File.AppendAllText(textDocumentPath, contentForTextFile + "\n \n");
                    fileContentInputFeedback.text = "Your content has been added to the file @ " + textDocumentPath;
                    Debug.Log("Your content has been added to the file @ " + textDocumentPath);
                }
            }
        }
    }

    public void ReadTextFile()
    {

    }
}
