using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharacterInputController : CharacterClass
{

    private Rigidbody rb;

    private float screenWidth;
    private int numberOfred = 0;
    [SerializeField] private float percentage = 0;
    [SerializeField] private float percentageToWin = 90;

    [SerializeField] private float swerveSpeed;
    [SerializeField] private float moveSpeed;

    [SerializeField] private GameObject wallObject;
    [SerializeField] private GameObject percantageUI;
    [SerializeField] private GameObject gameOverUI;

    [SerializeField] private BoolType isGameOver;

    private bool isMovingSideWays;

    private GameObject percentageUIText;

    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        screenWidth = Screen.width;
        percentageUIText = percantageUI.transform.GetChild(0).gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        CheckIfGameOver();
        HandleInput();
    }

    private void CheckIfGameOver()
    {
        if (isGameOver.value)
        {
            currentPlayerPhase = PlayerPhase.gameOver;
        }
    }

    private void HandleInput()
    {
        switch (currentPlayerPhase)
        {
            case PlayerPhase.platforming:
                Move();
                MoveCharacterLeftOrRight();
                break;
            case PlayerPhase.drawing:
                PaintWall();
                UpdatePercantageUI();
                break;
            default:
                break;
        }
    }

    private void UpdatePercantageUI()
    {
        percentageUIText.GetComponent<Text>().text = percentage.ToString();
    }

    private void PaintWall()
    {
#if UNITY_ANDROID
                if (Input.touchCount == 1)
                {
                    float inputPositionX = Input.GetTouch(0).position.x;
                    if (inputPositionX > screenWidth / 2)
                    {
                        //move right
                        MoveCharacter(1.0f * (inputPositionX - screenWidth / 2));
                    }
                    if (inputPositionX < screenWidth / 2)
                    {
                        //move left
                        MoveCharacter(-1.0f * (screenWidth / 2 - inputPositionX));
                    }
                }
#else
        if (percentage > percentageToWin)
        {
            HandleGameOver();

        }
        if (Input.GetKey(KeyCode.Mouse0))
        {
            Texture2D currentTexture;
            Vector3 mouseClickPosition;

            Vector3 relativeClickPosition = GetRelativeMousePositionToObjectTexture(out currentTexture, out mouseClickPosition);

            PaintSelectedPixels(currentTexture, relativeClickPosition);
        }
#endif
    }

    private void HandleGameOver()
    {
        currentPlayerPhase = PlayerPhase.gameOver;
        gameOverUI.SetActive(true);
        percantageUI.SetActive(false);
        wallObject.SetActive(false);
        FreezeCharacterRigidbody();
    }

    private Vector3 GetRelativeMousePositionToObjectTexture(out Texture2D texture, out Vector3 mouseClickPosition)
    {
        //Texture to texture2D
        Texture mainTexture = wallObject.GetComponent<Renderer>().material.GetTexture("_MainTex");

        texture = new Texture2D(mainTexture.width, mainTexture.height, TextureFormat.RGBA32, false);

        RenderTexture currentRT = RenderTexture.active;

        RenderTexture renderTexture = new RenderTexture(mainTexture.width, mainTexture.height, 32);
        Graphics.Blit(mainTexture, renderTexture);

        RenderTexture.active = renderTexture;
        texture.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
        texture.Apply();

        Color[] pixels = texture.GetPixels();

        RenderTexture.active = currentRT;


        //Converting mouseposition to pixel from the texture
        mouseClickPosition = Input.mousePosition;

        //Get the screen size
        Vector3 screenSize = new Vector3(Screen.width, Screen.height, 0);
        //Get the texture size
        Vector3 textureSize = new Vector3(texture.width, texture.height, 0);
        //Get the screen position of the texture (This will be the center of the image)
        Vector3 textureScreenPosition = Camera.main.WorldToScreenPoint(wallObject.transform.position);
        //Get the 0,0 position of the texture:
        Vector3 textureStartPosition = textureScreenPosition - textureSize / 2;
        //Subtract the 0,0 position of the texture from the mouse click position.
        Vector3 relativeClickPosition = mouseClickPosition - textureStartPosition;

        return relativeClickPosition;
    }

    private void PaintSelectedPixels(Texture2D currentTexture, Vector3 relativeClickPosition)
    {
        Texture2D texture = currentTexture;
        wallObject.GetComponent<Renderer>().material.SetTexture("_MainTex", texture);

        // RGBA32 texture format data layout exactly matches Color32 struct
        var data = texture.GetRawTextureData<Color32>();

        // fill texture data with a simple pattern
        Color32 red = new Color32(255, 0, 0, 255);
        int index = 0;
        for (int y = 0; y < texture.height; y++)
        {
            for (int x = 0; x < texture.width; x++)
            {
                if (x < relativeClickPosition.x + 25  && x > relativeClickPosition.x - 25 &&
                    y < relativeClickPosition.y + 25 && y > relativeClickPosition.y - 25)
                {
                    if (!CompareTwoColors(data[index],red))
                    {
                        data[index] = red;
                        numberOfred += 1;
                    }
                }
                index++;
            }
        }
        // upload to the GPU
        texture.Apply();

        percentage = (numberOfred * 100) / (texture.height * texture.width);
    }

    private void MoveCharacterLeftOrRight()
    {
        isMovingSideWays = false;
#if UNITY_ANDROID
                if (Input.touchCount == 1)
                {
                    float inputPositionX = Input.GetTouch(0).position.x;
                    if (inputPositionX > screenWidth / 2)
                    {
                        //move right
                        MoveCharacter(1.0f * (inputPositionX - screenWidth / 2));
                    }
                    if (inputPositionX < screenWidth / 2)
                    {
                        //move left
                        MoveCharacter(-1.0f * (screenWidth / 2 - inputPositionX));
                    }
                }
#else
        if (Input.GetKey(KeyCode.Mouse0))
        {
            float inputPositionX = Input.mousePosition.x;
            if (inputPositionX > screenWidth / 2)
            {
                //move right
                AddForceLeftOrRight(1.0f * (inputPositionX - screenWidth / 2));
            }
            if (inputPositionX < screenWidth / 2)
            {
                //move left
                AddForceLeftOrRight(-1.0f * (screenWidth / 2 - inputPositionX));
            }
        }
#endif
    }

    private void Move()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + (moveSpeed * Time.deltaTime / (isMovingSideWays ? 3 : 2)));
    }

    private void AddForceLeftOrRight(float horizontalInput)
    {
        isMovingSideWays = true;
        rb.AddForce(new Vector3(horizontalInput * swerveSpeed * Time.deltaTime, 0));
    }

    private bool CompareTwoColors(Color32 color1, Color32 color2)
    {
        if (color1.r == color2.r &&
            color1.g == color2.g &&
            color1.b == color2.b &&
            color1.a == color2.a)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public override void HandleFinishLine()
    {
        currentPlayerPhase = PlayerPhase.drawing;
        wallObject.SetActive(true);
        percantageUI.SetActive(true);
        FreezeCharacterRigidbody();
    }
}
