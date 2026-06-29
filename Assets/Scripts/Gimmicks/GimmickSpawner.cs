using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Gimmicks
{
    ヘビ,
    ネコ,
    カラス,
    火球,
    雷
}

public class GimmickSpawner : MonoBehaviour
{
    [SerializeField] 
    private GimmickBase[] _gmmickPrefabs;
    [SerializeField]
    private Gimmicks _gimmick;
    [SerializeField] 
    private Transform _player;
    [SerializeField]
    private Transform _target;
    [SerializeField] 
    private float _spawnRadius = 10f;

    [SerializeField]
    private float _spawnInterval = 2f;

    [SerializeField]
    private float _highTime = 60.0f;
    private float _currentTime;

    private Timer _timer;

    private bool _isRain;
    private bool _hoge;

    private float _minSpawnTime;

    private void Start()
    {
        _timer = new();
        _timer.Initialize();

        _timer.CreateTask(SpawnGimmick, _spawnInterval);

        _isRain = false;
        _hoge = true;

        _minSpawnTime = _spawnInterval * 0.5f;
    }

    private void FixedUpdate()
    {
        _timer.Update();
        _currentTime += Time.fixedDeltaTime;
    }

    public void SpawnGimmick()
    {
        // ギミックを生成
        var gimmickPrefab = _isRain ? _gmmickPrefabs[(int)Gimmicks.雷] : _gmmickPrefabs[(int)_gimmick];
        var pos = this.transform.position;
        var rand = Random.Range(-2.0f, 2.0f);
        pos += rand * this.transform.right;
        GimmickBase gimmick = Instantiate(gimmickPrefab, pos, Quaternion.identity);

        gimmick.Initialize(_player.transform);

        EventDispatcher.Instance.Dispatch("SpawnEnemy", gimmick);

        var t = Mathf.InverseLerp(0, _highTime, _currentTime);
        var interval = Mathf.Lerp(_spawnInterval, _minSpawnTime, t);

        _timer.CreateTask(SpawnGimmick, interval);
        if (_hoge)
            EventDispatcher.Instance.Bind(this);
        _hoge = false;
    }

    public void SpawnBallFromDome()
    {
        // ドーム方向のランダムベクトル
        Vector3 dir = Random.onUnitSphere;
        if (dir.y < 0) dir.y *= -1f;

        // プレイヤーを中心にドーム上にスポーン位置を設定
        Vector3 spawnPos = _player.position + dir * _spawnRadius;

        // ギミックを生成
        GimmickBase gimmick = Instantiate(_gmmickPrefabs[(int)_gimmick], spawnPos, Quaternion.identity);

        gimmick.Initialize(_player.transform);

        _timer.CreateTask(SpawnBallFromDome, _spawnInterval);
    }

    public void SpawnBallInFrontDome()
    {
        // まずは上向き半球のランダムな方向を得る
        Vector3 randomDir = Random.onUnitSphere;
        if (randomDir.z < 0) randomDir.z *= -1f; // Z正面限定（前向き）

        // 正面方向にドームを回転させる（プレイヤーの forward を中心に）
        Quaternion lookRot = Quaternion.LookRotation(-_player.forward, Vector3.up);
        Vector3 domeDir = lookRot * randomDir;

        // スポーン位置を決定（プレイヤー中心 + 指定距離）
        Vector3 spawnPos = _player.position + domeDir * _spawnRadius;

        // ギミックを生成
        GimmickBase gimmick = Instantiate(_gmmickPrefabs[(int)_gimmick], spawnPos, Quaternion.identity);

        gimmick.Initialize(_player.transform);

        _timer.CreateTask(SpawnBallFromDome, _spawnInterval);
    }

    [CallableEvent("StartRain")]
    public void StartRain(object data)
    {
        _isRain = true;
    }

    [CallableEvent("EndRain")]
    public void EndRain(object data)
    {
        _isRain = false;
    }
}
