using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Save_Manager : MonoBehaviour
{
    private static Save_Manager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); 
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public static Save_Manager Instance
    {
        get { return instance; }
    }
    public static void SaveGame(PlayerData playerData, EnemyData enemyData)
    {
        PlayerPrefs.SetFloat("PlayerPosX", playerData.Position.x);
        PlayerPrefs.SetFloat("PlayerPosY", playerData.Position.y);
        PlayerPrefs.SetFloat("PlayerPosZ", playerData.Position.z);

        PlayerPrefs.SetFloat("PlayerRotationX", playerData.Rotation.x);
        PlayerPrefs.SetFloat("PlayerRotationY", playerData.Rotation.y);
        PlayerPrefs.SetFloat("PlayerRotationZ", playerData.Rotation.z);

        PlayerPrefs.SetInt("PlayerHealth", playerData.Health);

        PlayerPrefs.SetInt("EnemyHealth", enemyData.Health);

        PlayerPrefs.Save();
    }

    public static void LoadGame(ref PlayerData playerData, ref EnemyData enemyData)
    {
        if (PlayerPrefs.HasKey("PlayerPosX"))
        {
            // Load player position
            float posX = PlayerPrefs.GetFloat("PlayerPosX");
            float posY = PlayerPrefs.GetFloat("PlayerPosY");
            float posZ = PlayerPrefs.GetFloat("PlayerPosZ");

            playerData.Position = new Vector3(posX, posY, posZ);

            // Load player rotation
            float rotX = PlayerPrefs.GetFloat("PlayerRotationX");
            float rotY = PlayerPrefs.GetFloat("PlayerRotationY");
            float rotZ = PlayerPrefs.GetFloat("PlayerRotationZ");

            playerData.Rotation = new Vector3(rotX, rotY, rotZ);

            // Load player health
            playerData.Health = PlayerPrefs.GetInt("PlayerHealth", 100);

            // Load enemy health
            enemyData.Health = PlayerPrefs.GetInt("EnemyHealth", 100);
        }
    }
    public class PlayerData
    {
        public Vector3 Position;
        public Vector3 Rotation;
        public int Health;
    }

    public class EnemyData
    {
        public int Health;
    }
}
