using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using Players;
using UniRx;
using UniRx.Triggers;
using Cysharp.Threading.Tasks;
using CodeHelper;
using UnityEngine.Serialization;

public class PlayerMover : PlayerBehaviour
{

    [SerializeField] Rigidbody playerRigidbody;

    [SerializeField] GameObject playerHead;

    //ReactiveProperty<Quaternion> playerDirection;

    BoolReactiveProperty isOnGround = new BoolReactiveProperty(false);

    // マウス感度
    [SerializeField] public float mouseSensitivity = 500f;

    // 基準の重さ(playerStatus.weightがこの値の時デフォルトの動作をするように)
    private float basicWeight = 100;

    //ブーストの消費エネルギー
    [SerializeField] float boostEnergy = 5;

    // ブーストの力
    [SerializeField] float boostForce = 100;

    // ブースト中かどうか
    private bool boostFlg = false;

    // ブースト時に加速する時間
    [SerializeField] float boostTime = 1;

    // ブーストの中断用
    CancellationTokenSource boostCancellationToken;

    // ジャンプする強さ
    [SerializeField] private float jumpForce = 50;

    // 床までの距離
    public float OnFloorHeight = 1.2f;

    // 空中ジャンプをしたかどうか
    bool secondJumpFlg = false;

    // 上昇する時の消費エネルギー(/s)
    [SerializeField] float riseEnergy = 3;

    // 上昇する時の力
    [SerializeField] float riseForce = 100;

    // 上昇の最高速度
    [SerializeField] float riseMaxSpeed = 64;


    // Start is called before the first frame update
    void Start()
    {
        //playerDirection = new ReactiveProperty<Quaternion>(transform.localRotation);
        boostCancellationToken = new CancellationTokenSource();

        // 移動
        this.UpdateAsObservable().Subscribe(x => Move(playerInput.moveDirection.Value));

        // ブースト
        playerInput.boostDirection.Subscribe(x => BoostMove(x)).AddTo(this);

        //ジャンプ
        playerInput.isJump.Subscribe(x => Jump());
        isOnGround.Where(x => x).Subscribe(x => secondJumpFlg = false);

        // 上昇
        this.UpdateAsObservable().Where(_ => playerInput.isRise.Value).Subscribe(x => Rise());

        // 回転
        this.UpdateAsObservable().Subscribe(x => Rotation(playerInput.rotationDirection.Value));

    }

    // Update is called once per frame
    void Update()
    {

    }

    RaycastHit objectHit;
    private void FixedUpdate()
    {
        isOnGround.Value = Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), out objectHit, OnFloorHeight);
    }

    public void Move(Vector3 vector3)
    {
        var v3 = vector3.normalized;

        float maxMagnitude = 20;
        var rbVec2 = new Vector2(playerRigidbody.velocity.x, playerRigidbody.velocity.z);

        var rbVel = (transform.rotation * v3) * (basicWeight / playerStatus.weight) * maxMagnitude;
        if (rbVec2.magnitude < maxMagnitude * 1.1 && !boostFlg)
        {
            //rb.velocity += rbVel+vecVel;

            //FPSによって移動速度が変わらないようにする
            var ratio = 1f - Mathf.Pow(1f - 0.2f, 60f * Time.deltaTime);
            playerRigidbody.velocity = vec.lerp(playerRigidbody.velocity, vec.vec3(rbVel.x, playerRigidbody.velocity.y, rbVel.z), ratio);
        }

        if (playerRigidbody.velocity.y < -1)
        {
            playerRigidbody.velocity = playerRigidbody.velocity + vec.vec3(0, -20*Time.deltaTime, 0);
        }
    }

    // ブーストの開始
    public async void BoostMove(Vector3 direction)
    {
        if (direction.magnitude <= 0.1 || !playerStatus.Consume(5))
        {
            return;
        }
        boostCancellationToken.Cancel();
        boostCancellationToken = new CancellationTokenSource();

        Debug.Log($"direction : {direction}");
        await Boosting(playerHead.transform.rotation* direction, boostCancellationToken);
    }

    // ブースト中
    private async UniTask Boosting(Vector3 direction, CancellationTokenSource cancellationToken)
    {
        boostFlg = true;
        var v3 = direction * boostForce;
        for (float i = Time.time; Time.time - i < boostTime;)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                return;
            }
            var t = (1f - (Time.time - i) / boostTime);
            playerRigidbody.velocity += (v3 * t*t);
            await UniTask.DelayFrame(1, PlayerLoopTiming.FixedUpdate);
        }

        //一定時間たったら減速
        playerRigidbody.drag *= 5;
        boostFlg = false;
        await UniTask.Delay(500);
        playerRigidbody.drag /= 5;

        boostFlg = false;
    }


    public void Jump()
    {
        RaycastHit objectHit;

        // 地上のジャンプ
        if (isOnGround.Value)
        {
            playerRigidbody.AddForce(0, jumpForce, 0, ForceMode.Impulse);
            secondJumpFlg = false;
        }

        // 空中ジャンプ
        else if (!secondJumpFlg)
        {
            playerRigidbody.velocity = new Vector3(playerRigidbody.velocity.x, 0, playerRigidbody.velocity.z);
            playerRigidbody.AddForce(0, jumpForce*2, 0, ForceMode.Impulse);
            secondJumpFlg = true;
        }
    }

    public void Rise()
    {

        if (playerRigidbody.velocity.y < riseMaxSpeed * (basicWeight / playerStatus.weight) && playerStatus.Consume(3 * Time.deltaTime))
        {
            playerRigidbody.AddForce(playerRigidbody.transform.up * riseForce, ForceMode.Acceleration);
        }
    }

    public void Rotation(Vector2 rotate)
    {
        playerHead.transform.Rotate(rotate.y * mouseSensitivity, 0, 0);
        transform.Rotate(0, rotate.x * mouseSensitivity, 0);
    }
}
