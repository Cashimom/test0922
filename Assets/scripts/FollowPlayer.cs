using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// カメラがプレイヤーを追いかけられるようにするクラス
/// </summary>
public class FollowPlayer : MonoBehaviour {
    //[SerializeField] private float turnSpeed = 10.0f;   // 回転速度
    //[SerializeField] private Transform player;          // 注視対象プレイヤー

    //[SerializeField] private float distance = 15.0f;    // 注視対象プレイヤーからカメラを離す距離
    //[SerializeField] private Quaternion vRotation;      // カメラの垂直回転(見下ろし回転)
    //[SerializeField] public Quaternion hRotation;      // カメラの水平回転

    //void Start()
    //{
    //    // 回転の初期化
    //    vRotation = Quaternion.Euler(30, 0, 0);         // 垂直回転(X軸を軸とする回転)は、30度見下ろす回転
    //    hRotation = Quaternion.identity;                // 水平回転(Y軸を軸とする回転)は、無回転
    //    transform.rotation = hRotation * vRotation;     // 最終的なカメラの回転は、垂直回転してから水平回転する合成回転

    //    // 位置の初期化
    //    // player位置から距離distanceだけ手前に引いた位置を設定します
    //    transform.position = player.position - transform.rotation * Vector3.forward * distance;
    //}

    //void LateUpdate()
    //{
    //    // 水平回転の更新
    //    if (Input.GetMouseButton(0))
    //        hRotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * turnSpeed, 0);

    //    // カメラの回転(transform.rotation)の更新
    //    // 方法1 : 垂直回転してから水平回転する合成回転とします
    //    transform.rotation = hRotation * vRotation;

    //    // カメラの位置(transform.position)の更新
    //    // player位置から距離distanceだけ手前に引いた位置を設定します(位置補正版)
    //    transform.position = player.position + new Vector3(0, 3, 0) - transform.rotation * Vector3.forward * distance;
    //}

    /// <summary>
    /// 追いかけるTransform
    /// </summary>
    [SerializeField] private Transform Target;

    /// <summary>
    /// カメラとプレイヤーとの距離[m]
    /// </summary>
    [SerializeField] private float DistanceToPlayerM = 2f;

    /// <summary>
    /// カメラを横にスライドさせる；プラスの時右へ，マイナスの時左へ[m]
    /// </summary>
    [SerializeField] private float SlideDistanceM = 0f;

    /// <summary>
    /// 注視点の高さ[m]
    /// </summary>
    [SerializeField] private float HeightM = 1.2f;

    /// <summary>
    /// カメラを回転させる用。
    /// using <see cref="PlayerController.vector"/>
    /// </summary>
    [SerializeField] private PlayerController playerController;

    /// <summary>
    /// trueでTPSカメラ、falseでFPSカメラ
    /// </summary>
    [SerializeField] public bool isTPS = true;

    void Start()
    {
        if (Target == null)
        {
            Debug.LogError("ターゲットが設定されていない");
            Application.Quit();
        }
        if (playerController == null)
        {
            playerController = Target.GetComponent<PlayerController>();
        }
    }

    void LateUpdate()
    {

        //var rotX = playerController.rotX;//Input.GetAxis("Mouse X") * Time.deltaTime * RotationSensitivity;
        //var rotY = playerController.rotY;// -Input.GetAxis("Mouse Y") * Time.deltaTime * RotationSensitivity;
        //playerController.rotX = 0.0f;
        //playerController.rotY = 0.0f;
        if (Input.GetKeyDown(KeyCode.F5))
        {
            isTPS = !isTPS;
        }

        if (isTPS)
        {

            // 回転
            //transform.RotateAround(lookAt, Vector3.up, 0);
            //transform.RotateAround(lookAt, transform.right, 0);
            var angle = playerController.vector;
            angle.x += 10.0f;
            if (angle.x >= 90 && angle.x <= 180)
            {
                angle.x = 89;
            }
            transform.eulerAngles = angle;


            // カメラとプレイヤーとの間の距離を調整

            var lookAt = Target.position + Vector3.up * HeightM;
            transform.position = lookAt - transform.forward * DistanceToPlayerM;

            //Target -> This(Camera)にRayを飛ばしてHitしたらカメラの位置を調整
            RaycastHit hit;
            Vector3 direction = transform.position - Target.transform.position;
            if (Physics.Raycast(Target.transform.position, direction.normalized ,out hit,direction.magnitude))
            {
                Vector3 localHit = hit.point - Target.transform.position;
                lookAt = Target.position + Vector3.up * (HeightM*(localHit.magnitude / direction.magnitude));
                transform.position = Target.transform.position + (localHit)*0.99f;
                transform.LookAt(lookAt);
            }
            else
            {
                // カメラを横にずらして中央を開ける
                transform.position = transform.position + transform.right * SlideDistanceM;
                // 注視点の設定
                transform.LookAt(lookAt);
            }
            


        }
        else
        {
            transform.position = Target.position;
            transform.rotation = Target.transform.rotation;
        }
    }
}
