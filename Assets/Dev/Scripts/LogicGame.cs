using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;
using Water2D;

public enum StateGame
{
    Game,
    Pause
}

public class LogicGame : SingletonMono<LogicGame>
{
    public Vector2 SizeCamera = Vector2.zero;
    public Transform TrsObj = null;

    public Transform TrsObjUp = null;


    public float baseGravity = -9.81f; 
    public float maxGravity = -20.0f;

    private ClickWater _currentWater2dSpawn;

    [SerializeField] Camera _currentCamera;

    bool isDown = false;

    private void Start()
    {
      //  SizeCamera = HelperManager.GetSizeCamera();
    }

    private void Update()
    {
        if (!SystemInfo.supportsAccelerometer)
        {
            //Debug.Log("Accelerometer not supported on this device");
        }
        else
        {
            Vector3 acceleration = Input.acceleration;
            Physics2D.gravity = new Vector2(acceleration.x * 7.81f, acceleration.y * 7.81f);

            Debug.Log("Accelerometer supported on this device " + Physics2D.gravity);
        }

        /*if (Input.GetMouseButtonDown(0))
        {
            _currentWater2dSpawn?._Spawner?.StopSpawning();

            Ray ray = _currentCamera.ScreenPointToRay(Input.mousePosition);
            Vector3 mouseScreenPosition = Input.mousePosition;
            mouseScreenPosition.z = -_currentCamera.transform.position.z;
            Vector3 mouseWorldPosition = _currentCamera.ScreenToWorldPoint(mouseScreenPosition);
            RaycastHit2D[] hits = Physics2D.RaycastAll(mouseWorldPosition, Vector2.zero);

            foreach (var hit in hits)
            {
                if (hit.collider != null)
                {
                    var cpnEmptyHOle = hit.collider.GetComponent<ClickWater>();
                    if (cpnEmptyHOle != null)
                    {
                        _currentWater2dSpawn = cpnEmptyHOle;
                        break;
                    }
                }
            }

            isDown = true;

            _currentWater2dSpawn?._Spawner?.Spawn();
        }

        if (Input.GetMouseButtonUp(0))
        {
            isDown = false;
            
        }

        if(!isDown)
        {
            _currentWater2dSpawn?._Spawner?.StopSpawning();
            _currentWater2dSpawn = null;
        }    */
    }
}
