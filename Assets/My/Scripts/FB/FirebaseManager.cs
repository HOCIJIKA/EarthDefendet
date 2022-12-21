using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using Firebase;
using Firebase.Auth;
using Firebase.Database;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms;
using UnityEngine.UI;

public class FirebaseManager : MonoBehaviour
{
    public static FirebaseManager Instance => _instance;
    [SerializeField] private GameObject _loginPanel;
    [SerializeField] private GameObject _registrationPanel;
    
    [SerializeField] private InputField _emailField;
    [SerializeField] private InputField _passwordField;
    [SerializeField] private Button _logonButton;
    [SerializeField] private Toggle _rememberLoginToggle;
    [Space]
    [SerializeField] private Button _registrationButton;
    [SerializeField] private InputField _emailRegistrationField;
    [SerializeField] private InputField _userNameField;
    [SerializeField] private InputField _passwordRegistrationField;
    [SerializeField] private InputField _passwordConfirmField;
    
    [SerializeField] private Text _loginResultText;

    private FirebaseAuth _auth;
    private DependencyStatus _dependencyStatus;
    private FirebaseUser _user;

    private string _lastUserEmail;
    private string _lastUserPassword;
    private bool _isUserLoggedIn;

    private DatabaseReference _databaseReference;
    
    private static FirebaseManager _instance;
    
    private List<LoadedUser> _users = new List<LoadedUser>();
    private LoadedUser _myData;
    private bool _isUsersDataLoaded = false;
    public bool IsUsersDataLoaded => _isUsersDataLoaded;
    public List<LoadedUser> Users => _users;
    public LoadedUser MyData => _myData;

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);
        
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
        {
            _dependencyStatus = task.Result;
            if (_dependencyStatus == DependencyStatus.Available)
            {
                InitialFirebase();
            }
            else
            {
                Debug.LogError($"Firebase initialisation failed: {_dependencyStatus}");
            }
        });
    }

    private void Start()
    {
        if (PlayerPrefs.GetInt("LoadLoginData") == 1)
        {
            try
            {
                _lastUserEmail = PlayerPrefs.GetString("LastUserEmail");
                _lastUserPassword = PlayerPrefs.GetString("LastUserPassword");
                
                _emailField.text = _lastUserEmail.ToString();
                _passwordField.text = _lastUserPassword.ToString();
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
        }
        _rememberLoginToggle.isOn = PlayerPrefs.GetInt("LoadLoginData") == 1;

        if (_rememberLoginToggle.isOn)
        {
            StartCoroutine(Login(_lastUserEmail, _lastUserPassword));
        }
    }
    
    public  void SaveUserMaxPoints(int maxPoints)
    {
        StartCoroutine(UpdatePointsDatabase(maxPoints));

    }
    public  void SaveUserMaxLevel(int numberOfLevel)
    {
        StartCoroutine(UpdateLevelDatabase(numberOfLevel));
    }
    public void SaveUserName(string username)
    {
        StartCoroutine(UpdateNameDatabase(username));
    }

    public void LoadedUsersData()
    {
        StartCoroutine(LoadUsersData());
    }
    
    private void InitialFirebase()
    {
        Debug.LogWarning($"Firebase initialisation {_dependencyStatus}");
        _auth = FirebaseAuth.DefaultInstance;
        _databaseReference = FirebaseDatabase.DefaultInstance.RootReference;
    }

    #region Registration/Login
    
    public void OnRegistrationConfirmFieldChanged()
    {
        _registrationButton.interactable = _passwordRegistrationField.text == _passwordConfirmField.text;
    }

    public void OnToggleLoginChanged()
    {
        PlayerPrefs.SetInt("LoadLoginData", _rememberLoginToggle.isOn ? 1 : 0);
    }

    public void ShowRegistrationPanel()
    {
        _registrationPanel.SetActive(true);
        _loginPanel.SetActive(false);
    }

    public void ShowLoginPanel()
    {        
        _loginPanel.SetActive(true);
        _registrationPanel.SetActive(false);
    }

    public void OnLoginButton()
    {
        StartCoroutine(Login(_emailField.text, _passwordField.text));

        _lastUserEmail = _emailField.text;
        _lastUserPassword = _passwordField.text;
        PlayerPrefs.SetString("LastUserEmail", _lastUserEmail);
        PlayerPrefs.SetString("LastUserPassword", _lastUserPassword);
        PlayerPrefs.Save();
    }
    
    public void OnRegistrationButton()
    {
        StartCoroutine(Registration(_emailRegistrationField.text, _passwordRegistrationField.text, _userNameField.text));
    }

    private IEnumerator Login(string email, string password)
    {
        _loginResultText.color = Color.red;

        var loginTask = _auth.SignInWithEmailAndPasswordAsync(email, password);
        yield return new WaitUntil(predicate: () => loginTask.IsCompleted);

        if (loginTask.Exception != null)
        {
            
            FirebaseException firebaseException = loginTask.Exception.GetBaseException() as FirebaseException;
            AuthError authError = (AuthError) firebaseException.ErrorCode;

            string message = "Login Failed";
            
            switch (authError)
            {
                case AuthError.MissingEmail:
                    message = "Login Failed";
                    break;
                
                case AuthError.MissingPassword:
                    message = "Missing Password";
                    break;
                
                case AuthError.WrongPassword:
                    message = "Wrong Password";
                    break;
                
                case AuthError.InvalidEmail:
                    message = "Invalid Email";
                    break;
                
                case AuthError.UserNotFound:
                    message = "User Not Found";
                    break;
            }

            _loginResultText.text = message;
        }
        else
        {
            _user = loginTask.Result;
            Debug.Log($" User signin successful ");
            
            _loginResultText.color = Color.green;
            _loginResultText.text = "Logged In";
            _isUserLoggedIn = true;
            SaveUserName(_user.DisplayName);
            StartGame();
        }
    }

    private IEnumerator Registration(string email, string password, string userName)
    {
        _loginResultText.color = Color.red;

        if (userName == null)
        {
            _loginResultText.text = "Missing Username";

        }
        else if (_passwordRegistrationField.text != _passwordConfirmField.text)
        {
            _loginResultText.text = "Password does not match";
        }
        else
        {
            var registerTask = _auth.CreateUserWithEmailAndPasswordAsync(email, password);

            yield return new WaitUntil(predicate: () => registerTask.IsCompleted);
            
            if (registerTask.Exception != null)
            {
                
                FirebaseException firebaseException = registerTask.Exception.GetBaseException() as FirebaseException;
                AuthError authError = (AuthError) firebaseException.ErrorCode;
                
                string message = "Registration Failed";

                switch (authError)
                {
                    case AuthError.MissingEmail:
                        message = "Missing Email";
                        break;

                    case AuthError.MissingPassword:
                        message = "Missing Password";
                        break;

                    case AuthError.WeakPassword:
                        message = "Weak Password";
                        break;

                    case AuthError.EmailAlreadyInUse:
                        message = "Email already in use";
                        break;
                    
                    case AuthError.Failure:
                        message = "Failure";
                        break;
                }
                
                _loginResultText.text = message;
            }
            else
            {
                _user = registerTask.Result;

                if (_user != null)
                {
                    UserProfile userProfile = new UserProfile {DisplayName = userName};

                    var profileTask = _user.UpdateUserProfileAsync(userProfile);
                    
                    yield return new WaitUntil(predicate: () => profileTask.IsCompleted);

                    if (profileTask.Exception != null)
                    {
                        _loginResultText.text = "Failed to register user";
                        _loginResultText.color = Color.red;

                        FirebaseException firebaseException = profileTask.Exception.GetBaseException() as FirebaseException;
                        AuthError authError = (AuthError) firebaseException.ErrorCode;
                        Debug.LogError(authError);
                    }
                    
                    Debug.Log("Registration successful");

                    _loginResultText.text = "successful registered";
                    _loginResultText.color = Color.green;
                    SuccessfulLoggedIn();
                }
                else
                {
                }
            }
        }
    }

    private void StartGame()
    {
        if (_isUserLoggedIn)
        {
            SceneManager.LoadScene("PlayScene");
        }
    }
    
    private void SuccessfulLoggedIn()
    {
        _loginResultText.text = "Ready to start!";
        _emailField.text = _emailRegistrationField.text;
        _passwordField.text = _passwordRegistrationField.text;
        
        _userNameField.text = string.Empty;
        _passwordRegistrationField.text = string.Empty;
        _passwordConfirmField.text = string.Empty;
        _emailRegistrationField.text = string.Empty;
        ShowLoginPanel();
    }
    #endregion

    #region FBDB
    
    private IEnumerator UpdateNameDatabase(string username)
    {
        var DBTaskName = _databaseReference.Child("users")
            .Child(_user.UserId)
            .Child("username")
            .SetValueAsync(_user.DisplayName);
        
        yield return new WaitUntil(predicate: () => DBTaskName.IsCompleted);

        if (DBTaskName.Exception != null)
        {
            Debug.LogError($"Fail to register Task with: {DBTaskName.Exception}");
        }
    }
    
    private IEnumerator UpdatePointsDatabase(int maxPoints)
    {
        var DBTaskPoins = _databaseReference.Child("users")
            .Child(_user.UserId)
            .Child("maxPoints")
            .SetValueAsync(maxPoints);
        
        yield return new WaitUntil(predicate: () => DBTaskPoins.IsCompleted);

        if (DBTaskPoins.Exception != null)
        {
            Debug.LogError($"Fail to register Task with: {DBTaskPoins.Exception}");
        }
    }

    private IEnumerator UpdateLevelDatabase(int numberOfLevel)
    {
        yield return new WaitForFixedUpdate();
        var DBTaskLevel = _databaseReference.Child("users")
            .Child(_user.UserId)
            .Child("maxLevel")
            .SetValueAsync(numberOfLevel);

        yield return new WaitUntil(predicate: () => DBTaskLevel.IsCompleted);

        if (DBTaskLevel.Exception != null)
        {
            Debug.LogError($"Fail to register Task with: {DBTaskLevel.Exception}");
        }
    }

    private IEnumerator LoadUsersData()
    {
        var DBTask = _databaseReference.Child("users").OrderByChild("maxPoints").GetValueAsync();
        
        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);
        
        if (DBTask.Exception != null)
        {
            Debug.LogError($"Fail to register Task with: {DBTask.Exception}");
        }
        else
        {
            DataSnapshot dataSnapshot = DBTask.Result;

            int i = 0;
            foreach (DataSnapshot child in dataSnapshot.Children.Reverse())
            {
                i++;
                _users.Add(new LoadedUser(
                    i,
                    child.Child("username").Value.ToString(), 
                    child.Child("maxPoints").Value.ToString(),
                    child.Child("maxLevel").Value.ToString()));

                if ((string) child.Child("username").Value == _user.DisplayName)
                {
                    _myData = new LoadedUser(
                        i,
                        child.Child("username").Value.ToString(), 
                        child.Child("maxPoints").Value.ToString(),
                        child.Child("maxLevel").Value.ToString());
                }
            }
        }

        _isUsersDataLoaded = true;
    }
    


    #endregion
}

public class LoadedUser
{
    public LoadedUser(int number, string username, string maxPoints,string maxLevel)
    {
        Number = number;
        Username = username;
        MaxPoints = maxPoints;
        MaxLevel = maxLevel;
    }

    public int Number;
    public string Username;
    public string MaxPoints;
    public string MaxLevel;
}
