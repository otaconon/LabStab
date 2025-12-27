using System;
using System.Collections.Generic;
using Managers;
using Maze;
using Networking;
using Unity.Netcode;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Player {
    public class SpawnSystem : NetworkBehaviour {
        [SerializeField] private GameObject _playerPrefab;
        [SerializeField] private MazeMesh _mazeMesh;
        [SerializeField] private Texture2D _mazeTexture;

        private void Start() {
            GameManager.Instance.GameStarted += OnGameStart;
        }

        public override void OnNetworkSpawn() {
            Debug.Log("On Network Spawn called");
            if (IsHost) {
                NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnected;
            }
        }

        public override void OnNetworkDespawn() {
            if (NetworkManager.Singleton != null) {
                NetworkManager.Singleton.OnClientConnectedCallback -= OnClientConnected;
            }
        }

        private void OnClientConnected(ulong clientId) {
            GameObject playerObj = Instantiate(_playerPrefab, new Vector3(0, 0, 0), Quaternion.identity);
            playerObj.GetComponent<NetworkObject>().SpawnAsPlayerObject(clientId);
            GameManager.Instance.AddPlayer(playerObj);
        }

        private void OnGameStart() {
            foreach (GameObject player in GameManager.Instance.Players) {
                SpawnPlayer(player); 
            }
        }

        public void SpawnPlayer(GameObject playerObj) {
            var spawnPositions = new List<Vector2Int>();
            for (var row = 0; row < _mazeTexture.height; row++) {
                for (var col = 0; col < _mazeTexture.width; col++) {
                    if (_mazeTexture.GetPixel(col, row).grayscale > 0.5f) {
                        spawnPositions.Add(new Vector2Int(col, row));
                        Debug.Log("Empty pixel: " + col + ", " + row);
                    }
                }
            }

            Vector2 spawnPixel = spawnPositions[Random.Range(0, spawnPositions.Count)];
            Debug.Log("Spawn pixel: " + spawnPixel);

            float scale = _mazeMesh.Scale;
            float halfUnit = scale / 2.0f;

            float worldX = (spawnPixel.x * scale) + halfUnit;
            float worldZ = (spawnPixel.y * scale) + halfUnit;

            var cc = playerObj.GetComponent<CharacterController>();
            if (cc != null) {
                cc.enabled = false;
                playerObj.transform.position = new Vector3(worldX, 0, worldZ);
                cc.enabled = true;
            }
        }
    }
}