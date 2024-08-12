using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;
using System.Threading.Tasks;

public class EditUserScript : MonoBehaviour
{
    public GameObject availableImagesContent;
    public GameObject availableImageTemplate;

    public InputField userNameField;
    public Image userAvatar;
    
    public int currentAvatarIndex;

    private User userServices;
    private CustomNakamaConnection nakamaInstance;

    public GameObject controlScripts;
    public MainMenuScript mainMenuScript;

    // added
    public void Start()
    {
        userServices = new User();
        nakamaInstance = CustomNakamaConnection.Instance;
        currentAvatarIndex = 0;
    }

    // display username and profile avatar on UI
    public void displayUserProfile(int avatarIndex , string displayName)
    {
        userNameField.text = displayName;
        userAvatar.sprite = mainMenuScript.icons[avatarIndex];
    }

    // fetch user profile settings from remote server and display on UI
    // called when profile setting canvas is loaded
    public async void loadCurrentState()
    {
        try
        {
            var existingUser = await userServices.fetchUserAccount(nakamaInstance.nakamaSession.UserId);

            currentAvatarIndex = existingUser.avatarUrl;
            this.displayUserProfile(currentAvatarIndex , existingUser.displayName);
        }
        catch(Exception ex)
        {
            Debug.Log("Exception in loadcurrentState = " + ex);
        }
    }

    // load all of the available images in template to render.....
    // user will choose these images to set as profile avatar.....
    public void loadImagesInTemplate()
    {
        try
        {
            var icons = mainMenuScript.icons;

            for(int i = 0;i < icons.Length; i++)
            {
                var newImage = Instantiate(availableImageTemplate , availableImagesContent.transform);

                newImage.transform.GetComponent<Image>().sprite = icons[i];
                int currentIndex = i;
                newImage.transform.GetChild(0).GetComponent<Button>().onClick.AddListener(()=>{
                    setNewProfileAvatar(currentIndex);
                    });
            }
        }catch(Exception ex)
        {
            Debug.Log("Error in loading image template = " + ex);
        }
    }

    // save updated username and avatar on remote server.....
    public async void saveUserState()
    {
        try
        {
            string displayName = userNameField.text;
            await userServices.updateUser(nakamaInstance.nakamaSession.UserId, displayName, currentAvatarIndex.ToString());

        }catch(Exception ex)
        {
            Debug.Log("Exception in saveUserState = " + ex);
        }
    }

    // display new setting temporarily on screen.....
    public void setNewProfileAvatar(int index)
    {
        try
        {
            Debug.Log("Try to change profile avatar with index " + index);
            currentAvatarIndex = index;

            string displayName = userNameField.text;
            this.displayUserProfile(currentAvatarIndex , displayName);

        }catch(Exception ex)
        {
            Debug.Log("Error in changeProfile = " + ex);
        }
    }
}
