using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class SignUp : MonoBehaviour
{

    public GameObject Reg_username;
    public GameObject Reg_password;
    public GameObject Reg_repasswod;
    public GameObject Reg_email;
    public GameObject signUp;

    public Button signUpBtn;

    private string Username;
    private string Password;
    private string Repassword;
    private string Email;



    // Use this for initialization
    void Start()
    {

        signUpBtn = signUp.GetComponent<Button>();
        signUpBtn.onClick.AddListener(CheckSignUp);

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void CheckSignUp()
    {

        Username = Reg_username.GetComponent<InputField>().text;
        Password = Reg_password.GetComponent<InputField>().text;
        Repassword = Reg_repasswod.GetComponent<InputField>().text;
        Email = Reg_email.GetComponent<InputField>().text;

        if (Username != "" && Password != "" && Repassword !="" && Email !="")
        {

            

            if (Password == Repassword)
            {
                StartCoroutine(CallSignUp(Username, Password, Email));
            }
            
            //else if (Username =="" || Password =="" || Repassword =="" || Email == "")
            //{
            //    SceneManager.LoadScene(32);


            //}
            else{

                SceneManager.LoadScene(3);
            }

        }
        else if (Username == "" ||Password == "" || Repassword == "" || Email == "")
        {
            SceneManager.LoadScene(32);
        }



    }

    public IEnumerator CallSignUp(string Username, string Password, string Email)
    {
        WWWForm form = new WWWForm();
        form.AddField("username", Username);
        form.AddField("password", Password);
        form.AddField("email", Email);

        UnityWebRequest www = UnityWebRequest.Post("http://f19-preview.awardspace.net//ikkcity.atwebpages.com/userInfo.php", form);

        yield return www.SendWebRequest(); ;

        if (www.error == null)
        {
            

            Debug.Log("Register Success" + www.downloadHandler.text);
            SceneManager.LoadScene(2);


        }

        else
        {

            Debug.Log("Error" + www.error);

        }


    }



}
