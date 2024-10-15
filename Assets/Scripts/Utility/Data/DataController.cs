using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Core.Unit.Player;

public class DataController : MonoBehaviour
{
    public Seth player;

    public GameObject Lamp1;
    public GameObject Lamp2;

    //�̱��� ����
    static GameObject _container;
    static GameObject Container
    {
        get
        {
            return _container;
        }
    }
    static DataController _instance;
    public static DataController Instance
    {
        get
        {
            if (!_instance)
            {
                _container = new GameObject();
                _container.name = "DataController";
                _instance = _container.AddComponent(typeof(DataController)) as DataController;
                DontDestroyOnLoad(_container);
            }
            return _instance;
        }
    }

    //���� ������ ���� �̸� ����
    public string GameDataFileName = "InsomniaData.json";

    //���ϴ� �̸�(����).json
    public GameData _gameData;
    public GameData gameData
    {
        get
        {
            //������ ���۵Ǹ� �ڵ����� ����
            if (_gameData == null)
            {
                LoadGameData();
                SaveGameData();
            }
            return _gameData;
        }
    }

    // Start is called before the first frame update
    public void Start()
    {
        LoadGameData();
        SaveGameData();
        //isClear1�� ���̸� �߰� �������� ����
        if (gameData.isClear1 == true)
        {
            player.transform.position = Lamp1.transform.position;
        }
    }

    // Update is called once per frame
    public void Update()
    {
        
    }

    //����� ���� �ҷ�����
    public void LoadGameData()
    {
        string filePath = Application.persistentDataPath + GameDataFileName;

        //����� ������ �ִٸ�
        if (File.Exists(filePath))
        {
            print("�ҷ����� ����");
            string FromJsonData = File.ReadAllText(filePath);
            _gameData = JsonUtility.FromJson<GameData>(FromJsonData);
        }

        //����� ������ ���ٸ�
        else
        {
            print("���ο� ���� ����");
            _gameData = new GameData();
        }
    }


    //���� �����ϱ�
    public void SaveGameData()
    {
        string ToJsonData = JsonUtility.ToJson(gameData);
        string filePath = Application.persistentDataPath + GameDataFileName;

        //�̹� ����� ������ �ִٸ� �����
        File.WriteAllText(filePath, ToJsonData);

        print("����Ϸ�");
        print("1�� " + gameData.isClear1);
        print("2�� " + gameData.isClear2);
    }

    //������ ����Ǹ� �ڵ�����ǵ���(�ӽ�)
    private void OnApplicationQuit()
    {
        SaveGameData();
    }
}
