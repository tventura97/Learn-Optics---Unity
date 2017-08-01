using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class FeedbackButtonScript : MonoBehaviour
{
    InputField NameIF, GradeIF, FeedbackIF;
    DependencyStatus dependencyStatus = DependencyStatus.UnavailableOther;
    DatabaseReference reference;
    void Start()
    {
        //Initialize Input Fields
        NameIF = GameObject.Find("NameIF").GetComponent<InputField>();
        GradeIF = GameObject.Find("GradeIF").GetComponent<InputField>();
        FeedbackIF = GameObject.Find("FeedbackIF").GetComponent<InputField>();

        //Check for Firebase required dependencies
        dependencyStatus = FirebaseApp.CheckDependencies();
        if (dependencyStatus != DependencyStatus.Available)
        {
            FirebaseApp.FixDependenciesAsync().ContinueWith(task => {
                dependencyStatus = FirebaseApp.CheckDependencies();
                if (dependencyStatus == DependencyStatus.Available)
                {
                    InitializeFirebase();
                }
                else
                {
                    Debug.LogError(
                        "Could not resolve all Firebase dependencies: " + dependencyStatus);
                }
            });
        }
        else
        {
            InitializeFirebase();
        }
    }

    //Change the url to some NJIT firebase database or something
    void InitializeFirebase()
    {
        FirebaseApp app = FirebaseApp.DefaultInstance;
        app.SetEditorDatabaseUrl("https://njit-optics-unity-app.firebaseio.com/");
        reference = FirebaseDatabase.DefaultInstance.RootReference;
    }

    //Send feedback to firebase database as JSON
    public void OnClick()
    {
        Feedback Feedback = new Feedback(GradeIF.text, FeedbackIF.text);
        string FeedbackJSONFile = JsonUtility.ToJson(Feedback);
        reference.Child("feedback").Child(NameIF.text).SetRawJsonValueAsync(FeedbackJSONFile);
    }

    public void SendFeedback(string Feedback)
    {
        if (string.IsNullOrEmpty(Feedback))
        {
            //Error handling
            return;
        }

     
    }
}

[System.Serializable]
public class Feedback
{
    public string UserGrade;
    public string UserFeedback;

    public Feedback(string grade, string feedback)
    {
        UserGrade = grade;
        UserFeedback = feedback;
    }
}