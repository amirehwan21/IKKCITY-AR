using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class LoginScript : MonoBehaviour
{

    public GameObject Log_username;
    public GameObject Log_password;
    public GameObject Login;

    public Button LoginBtn;

    private string Username;
    private string Password;



    // Use this for initialization
    void Start()
    {

        LoginBtn = Login.GetComponent<Button>();
        LoginBtn.onClick.AddListener(ValidateLogin);

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void ValidateLogin()
    {

        Username = Log_username.GetComponent<InputField>().text;
        Password = Log_password.GetComponent<InputField>().text;

        if (Username != "" && Password != "")
        {


            StartCoroutine(CallLogin(Username, Password));

        }
        else
        {

        }



    }

    public IEnumerator CallLogin(string Username, string Password)
    {
        WWWForm form = new WWWForm();
        form.AddField("username", Username);
        form.AddField("password", Password);

        UnityWebRequest www = UnityWebRequest.Post("http://f19-preview.awardspace.net/ikkcity.atwebpages.com/login.php", form);

        yield return www.SendWebRequest();

        if (www.error == null)
        {


            Debug.Log(www.downloadHandler.text);
            SceneManager.LoadScene(2);


        }

        else
        {

            Debug.Log("Error" + www.error);
            //SceneManager.LoadScene(1);


        }


    }



}
