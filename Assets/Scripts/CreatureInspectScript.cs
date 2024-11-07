using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class CreatureInspectScript : MonoBehaviour
{
    // References to the text fields and UI elements
    public Text nameText;
    public Text headText;
    public Text bodyText;
    public Text legsText;
    public GameObject panelInspect;
    public GameObject panelImage;
    public GameObject loadingSpinner;  // Loading animation
    public Button backButton;
    public Button deleteButton;
    public Button showImageButton;
    public Button xButton;
    public Image creatureImage;

    private Creature currentCreature;
    private Text alertText; // For displaying alerts
    // Set creature data in the UI
    public void SetCreature(Creature creature)
    {
        currentCreature = creature;
        nameText.text = currentCreature.name;
        headText.text = currentCreature.head;
        bodyText.text = currentCreature.body;
        legsText.text = currentCreature.legs;
    }

    void Start()
    {
        backButton.onClick.AddListener(GoBack);
        deleteButton.onClick.AddListener(DeleteCreature);
        showImageButton.onClick.AddListener(ShowImage);
        xButton.onClick.AddListener(CloseImage);
    }

    // Method to check for internet connectivity
    bool IsConnectedToInternet()
    {
        return Application.internetReachability != NetworkReachability.NotReachable;
    }

    // Show image method
    public void ShowImage()
    {
        // Check network connectivity
        if (!IsConnectedToInternet())
        {
            ShowAlert("No internet connection. Cannot show the image.");
            return;
        }

        // Start loading animation
        loadingSpinner.SetActive(true);

        // Construct the prompt from the creature's attributes
        string prompt = currentCreature.getPrompt();
        string url = $"https://image.pollinations.ai/prompt/{prompt}";

        // Start the coroutine to fetch the image
        StartCoroutine(DownloadImage(url));
    }

    // Coroutine to download image
    IEnumerator DownloadImage(string url)
    {
        UnityWebRequest request = UnityWebRequestTexture.GetTexture(url);

        // Send the request and wait for the response
        yield return request.SendWebRequest();

        // Stop loading animation
        loadingSpinner.SetActive(false);

        if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
        {
            // Handle errors
            ShowAlert("Failed to download the image. Please try again.");
        }
        else
        {
            // Successfully downloaded the image
            Texture2D downloadedTexture = ((DownloadHandlerTexture)request.downloadHandler).texture;
            creatureImage.sprite = Sprite.Create(downloadedTexture, new Rect(0, 0, downloadedTexture.width, downloadedTexture.height), new Vector2(0.5f, 0.5f));

            // Show the panel with the image
            panelImage.SetActive(true);
        }
    }

    // Show alert method
    void ShowAlert(string message)
    {
        alertText.text = message;
        alertText.gameObject.SetActive(true);
        StartCoroutine(HideAlertAfterDelay(7f));  // Hide after 3 seconds
    }

    // Coroutine to hide alert after a delay
    IEnumerator HideAlertAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        alertText.gameObject.SetActive(false);
    }

    public void CloseImage()
    {
        panelImage.SetActive(false);
    }

    public void DeleteCreature()
    {
        CreatureManager.RemoveCreature(currentCreature);
        panelInspect.SetActive(false);
    }

    public void GoBack()
    {
        panelInspect.SetActive(false);
    }
}
