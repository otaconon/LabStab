using System;
using System.Threading.Tasks;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using UnityEngine;

namespace Networking {
    public class RelayManager : MonoBehaviour {
        private async void Start() {
            try {
                await UnityServices.InitializeAsync();

                if (!AuthenticationService.Instance.IsSignedIn) {
                    await AuthenticationService.Instance.SignInAnonymouslyAsync();
                }
            }
            catch (Exception e) {
                Debug.LogError(e);
            }
        }

        public static async Task<string> CreateRelay() {
            try {
                Allocation allocation = await RelayService.Instance.CreateAllocationAsync(3);

                string joinCode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);
                Debug.Log($"Relay Created! Join Code: {joinCode}");

                NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(
                    allocation.ToRelayServerData("dtls")
                );

                return joinCode;
            }
            catch (RelayServiceException e) {
                Debug.LogError(e);
                return null;
            }
        }

        public static async Task<bool> JoinRelay(string joinCode) {
            try {
                Debug.Log($"Joining Relay with code: {joinCode}");

                JoinAllocation joinAllocation = await RelayService.Instance.JoinAllocationAsync(joinCode);

                NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(
                    joinAllocation.ToRelayServerData("dtls")
                );

                return true;
            }
            catch (RelayServiceException e) {
                Debug.LogError(e);
                return false;
            }
        }
    }
}