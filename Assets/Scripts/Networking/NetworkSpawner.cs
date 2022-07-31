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
    public int MOUSE_X;
    public int MOUSE_Y;
    public Quaternion PLAYER_FORWARD;
}

public class NetworkSpawner : MonoBehaviour, INetworkRunnerCallbacks
{

    private NetworkRunner _runner;

    [SerializeField] private NetworkPrefabRef _playerPrefab;
    [SerializeField] private NetworkPrefabRef _ballPrefab;
    private Dictionary<PlayerRef, NetworkObject> _spawnedCharacters = new Dictionary<PlayerRef, NetworkObject>();

    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
    {
        // Create a unique position for the player
        Vector3 spawnPosition = GetComponent<PlayerSpawnPointManagerPrototype>().GetNextSpawnPoint(_runner, player).position;
        //runner.Spawn(_ballPrefab, new Vector3(0, 0, 0), Quaternion.identity, player);
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
        var frameworkInput = new NetworkInputPrototype();

        if (Input.GetKey(KeyCode.W))
        {
            frameworkInput.Buttons |= NetworkInputPrototype.BUTTON_FORWARD;
        }

        if (Input.GetKey(KeyCode.S))
        {
            frameworkInput.Buttons |= NetworkInputPrototype.BUTTON_BACKWARD;
        }

        if (Input.GetKey(KeyCode.A))
        {
            frameworkInput.Buttons |= NetworkInputPrototype.BUTTON_LEFT;
        }

        if (Input.GetKey(KeyCode.D))
        {
            frameworkInput.Buttons |= NetworkInputPrototype.BUTTON_RIGHT;
        }

        if (Input.GetKey(KeyCode.Space))
        {
            frameworkInput.Buttons |= NetworkInputPrototype.BUTTON_JUMP;
        }

        if (Input.GetKey(KeyCode.C))
        {
            frameworkInput.Buttons |= NetworkInputPrototype.BUTTON_CROUCH;
        }

        if (Input.GetKey(KeyCode.E))
        {
            frameworkInput.Buttons |= NetworkInputPrototype.BUTTON_ACTION1;
        }

        if (Input.GetKey(KeyCode.Q))
        {
            frameworkInput.Buttons |= NetworkInputPrototype.BUTTON_ACTION2;
        }

        if (Input.GetKey(KeyCode.F))
        {
            frameworkInput.Buttons |= NetworkInputPrototype.BUTTON_ACTION3;
        }

        if (Input.GetKey(KeyCode.G))
        {
            frameworkInput.Buttons |= NetworkInputPrototype.BUTTON_ACTION4;
        }

        if (Input.GetKey(KeyCode.R))
        {
            frameworkInput.Buttons |= NetworkInputPrototype.BUTTON_RELOAD;
        }

        if (Input.GetMouseButton(0))
        {
            frameworkInput.Buttons |= NetworkInputPrototype.BUTTON_FIRE;
        }

        var localView = LocalPlayer.getView();
        if (localView != null)
            frameworkInput.cameraRotationX = localView.getXRotation();

        input.Set(frameworkInput);
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
