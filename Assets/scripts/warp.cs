using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ワープの処理をするクラス
/// </summary>
public class warp : MonoBehaviour {

    /// <summary>
    /// ワープ先の<see cref="warp"/>を持つゲームオブジェクト
    /// </summary>
    [SerializeField] private GameObject WarpTarget;

    /// <summary>
    /// 次にワープできるようになるまでの時間
    /// </summary>
    [SerializeField] private float nextWarpDelay=5;

    /// <summary>
    /// ワープして移動してくるときの高さ
    /// </summary>
    [SerializeField] private float warpHigh = 20;

    /// <summary>
    /// 時間カウント用変数
    /// </summary>
    private float count = 0.0f;

    /// <summary>
    /// <see cref="nextWarpDelay"/>のカウントをするためのフラグ
    /// </summary>
    private bool cameON = false;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (cameON)
        {
            count += Time.deltaTime;
            if (count > nextWarpDelay)
            {
                count = 0.0f;
                cameON = false;
            }
        }
	}

    void OnTriggerEnter(Collider other)
    {
        if (!cameON&&other.gameObject.tag=="Character")
        {
            WarpTarget.GetComponent<warp>().warpHere(other.gameObject);
        }
    }

    /// <summary>
    /// <paramref name="gameObject"/>をここにワープさせるメソッド。
    /// used in <see cref="warp.OnTriggerEnter(Collider)"/>
    /// </summary>
    /// <param name="gameObject"></param>
    void warpHere(GameObject gameObject)
    {
        Vector3 position = transform.position;
        position.y += warpHigh;
        gameObject.transform.position = position;
        cameON = true;
    }
}
