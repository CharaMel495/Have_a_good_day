using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBallSpawner : MonoBehaviour
{
    [SerializeField] 
    private FireBall _ballPrefab;
    [SerializeField] 
    private Transform _player;
    [SerializeField]
    private Transform _target;
    [SerializeField] 
    private float _spawnRadius = 10f;

    [SerializeField]
    private float _spawnInterval = 2f;

    private Timer _timer;

    private void Start()
    {
        _timer = new();
        _timer.Initialize();

        SpawnBallFromDome();

        _timer.CreateTask(SpawnBallFromDome, _spawnInterval);
    }

    private void FixedUpdate()
    {
        _timer.Update();
    }

    public void SpawnBallFromDome()
    {
        // ドーム方向のランダムベクトル
        Vector3 dir = Random.onUnitSphere;
        if (dir.y < 0) dir.y *= -1f;

        // プレイヤーを中心にドーム上にスポーン位置を設定
        Vector3 spawnPos = _player.position + dir * _spawnRadius;

        // ボールを生成
        FireBall ball = Instantiate(_ballPrefab, spawnPos, Quaternion.identity);

        ball.Initialize(_player.transform);

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

        // オブジェクトを生成
        FireBall ball = Instantiate(_ballPrefab, spawnPos, Quaternion.identity);

        ball.Initialize(_target.transform);

        _timer.CreateTask(SpawnBallInFrontDome, _spawnInterval);
    }
}
