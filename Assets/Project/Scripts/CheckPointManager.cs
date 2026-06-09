using Unity.Cinemachine;
using UnityEngine;

public class CheckPointManager : MonoBehaviour
{
    public static CheckPointManager Instance { get; private set; }

    public Transform firstPoint;
    public CinemachineCamera firstCamera;

    private Vector3 lastCheckPoint;
    private string lastCameraName;
    private bool hasCheckPoint = false;
    private string lastSceneName;
    private int lastCheckPointID = -1;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void SetCheckPoint(int id, Vector3 position, CinemachineCamera cam, string sceneName)
    {
        lastCheckPointID = id;
        lastCheckPoint = position;
        lastCameraName = cam.name;
        lastSceneName = sceneName;
        hasCheckPoint = true;
        Debug.Log($"チェックポイント登録：id = {lastCheckPointID}, {lastCheckPoint}, cam = {cam.name}");
    }

    public Vector3 GetLastCheckPoint()
    {
        return hasCheckPoint ? lastCheckPoint : firstPoint.position;
    }

    public CinemachineCamera GetLastCamera()
    {
        var camObj = GameObject.Find(lastCameraName);
        return camObj != null ? camObj.GetComponent<CinemachineCamera>() : firstCamera;


        //if (!hasCheckPoint)
        //    return firstCamera;
//
        //// シーンが切り替わっていれば、名前からカメラをFindする
        //var foundCam = GameObject.Find(lastCameraName)?.GetComponent<CinemachineCamera>();
        //if (foundCam != null)
        //    return foundCam;
//
        ////これでも見つからない場合
        //Debug.Log($"カメラ{lastCameraName}が見つからない");
        //return firstCamera;
    }

    public string GetLastSceneName()
    {
        return lastSceneName;
    }

    public bool GetHasCheckPoint()
    {
        return hasCheckPoint;
    }

    public int GetLastCheckPointID()
    {
        return lastCheckPointID;
    }
}
