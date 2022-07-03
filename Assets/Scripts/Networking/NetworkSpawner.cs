using Fusion;
using Fusion.Sockets;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Fusion;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public struct NetworkInputData : INetworkInput
{
    public NetworkBool BUTTON_FORWARD;
    public NetworkBool BUTTON_BACKWARD;
    public NetworkBool BUTTON_LEFT_STRAFE;
    public NetworkBool BUTTON_RIGHT_STRAFE;
    public NetworkBool BUTTON_JUMP;
    public int VERTICAL;
    public int HORIZONTAL;
}

public class NetworkSpawner : MonoBehaviour, INetworkRunnerCallbacks
{

    private NetworkRunner _runner;

    [SerializeField] private NetworkPrefabRef _playerPrefab;
    [SerializeField] private NetworkPrefabRef _ballPrefab;
    [SerializeField] private GameObject clientSingleton;
    private Dictionary<PlayerRef, NetworkObject> _spawnedCharacters = new Dictionary<PlayerRef, NetworkObject>();

    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
    {
        // Create a unique position for the player
        Vector3 spawnPosition = new Vector3((player.RawEncoded % runner.Config.Simulation.DefaultPlayers) * 3, 1, 0);
        runner.Spawn(_ballPrefab, new Vector3(0, 0, 0), Quaternion.identity, player);
        NetworkObject networkPlayerObject = runner.Spawn(_playerPrefab, spawnPosition, Quaternion.identity, player);
        // Keep track of the player avatars so we can remove it when they disconnect
        _spawnedCharacters.Add(player, networkPlayerObject);
    }

    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
    {
        // Find and remove the players avatar
        if (_spawnedCharacters.TryGetValue(player, out NetworkObject networkObject))
        {
            runner.Despawn(networkObject);
            _spawnedCharacters.Remove(player);
        }
    }
    public void OnInput(NetworkRunner runner, NetworkInput input)
    {
        var data = new NetworkInputData();

        if (Input.GetKey(KeyCode.Space))
            data.BUTTON_JUMP = true;

        if (Input.GetKey(KeyCode.W))
            data.BUTTON_FORWARD = true;

        if (Input.GetKey(KeyCode.S))
            data.BUTTON_BACKWARD = true;

        if (Input.GetKey(KeyCode.A))
            data.BUTTON_LEFT_STRAFE = true;

        if (Input.GetKey(KeyCode.D))
            data.BUTTON_RIGHT_STRAFE = true;

        data.VERTICAL = (int)(CrossPlatformInputManager.GetAxis("Vertical") * 100);
        data.HORIZONTAL = (int)(CrossPlatformInputManager.GetAxis("Horizontal") * 100);
        //Debug.Log("Sending: " + data.VERTICAL);
        input.Set(data);
    }
    public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input) { }
    public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason) { }
    public void OnConnectedToServer(NetworkRunner runner) { }
    public void OnDisconnectedFromServer(NetworkRunner runner) { }
    public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token) { }
    public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason) { }
    public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message) { }
    public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList) { }
    public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data) { }
    public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken) { }
    public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ArraySegment<byte> data) { }
    public void OnSceneLoadDone(NetworkRunner runner) {
        Instantiate(clientSingleton, Vector3.zero, Quaternion.identity);
    }
    public void OnSceneLoadStart(NetworkRunner runner) { }

    async void StartGame(GameMode mode)
    {
        // Create the Fusion runner and let it know that we will be providing user input
        _runner = gameObject.AddComponent<NetworkRunner>();
        _runner.ProvideInput = true;

        // Start or join (depends on gamemode) a session with a specific name
        await _runner.StartGame(new StartGameArgs()
        {
            GameMode = mode,
            SessionName = "TestRoom",
            Scene = SceneManager.GetActiveScene().buildIndex,
            SceneObjectProvider = gameObject.AddComponent<NetworkSceneManagerDefault>()
        });
    }

    private void OnGUI()
    {
        if (_runner == null)
        {
            if (GUI.Button(new Rect(0, 0, 200, 40), "Host"))
            {
                StartGame(GameMode.Host);
            }
            if (GUI.Button(new Rect(0, 40, 200, 40), "Join"))
            {
                StartGame(GameMode.Client);
            }
        }
    }
}
