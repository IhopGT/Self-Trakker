using System;
using System.Collections.Specialized;
using System.IO;
using System.Net;
using BepInEx;
using HarmonyLib;
using Photon.Pun;

namespace selftracker
{
    [BepInPlugin("com.Trakker.gorillatag.IhopTrakker", "selftracker", "1.0.0")]
    public class Plugin : BaseUnityPlugin
    {
        private bool sentBefore;
        private string trackerPersonName = "IhopGT"; // add your name
        private string webhookUrl = "Your_WebhookHere";// your webhook
        private const string LogFilePath = @"C:\Program Files (x86)\Steam\steamapps\common\Gorilla Tag\error_log.txt";

        private void Start()
        {
            SendNotification($"{trackerPersonName} has started the game!");
        }

        private void OnEnable()
        {
            HarmonyPatches.ApplyHarmonyPatches();
        }

        private void OnDisable()
        {
            HarmonyPatches.RemoveHarmonyPatches();
        }

        private void Update()
        {
            if (PhotonNetwork.InRoom && !sentBefore)
            {
                sentBefore = true;
                SendNotification($"{trackerPersonName} is in code {PhotonNetwork.CurrentRoom.Name} : {PhotonNetwork.CurrentRoom.PlayerCount}/10 players\nIs Public? **{PhotonNetwork.CurrentRoom.IsVisible}**");
            }
            else if (!PhotonNetwork.InRoom && sentBefore)
            {
                sentBefore = false;
                SendNotification($"{trackerPersonName} has left that code!");
            }
        }

        private void SendNotification(string message)
        {
            try
            {
                using (var client = new WebClient())
                {
                    var values = new NameValueCollection
                    {
                        { "content", message }
                    };

                    client.UploadValues(webhookUrl, values);
                }
            }
            catch (Exception ex)
            {
                string errorMessage = $"Error sending message to Discord: {ex.Message}";
                LogErrorToFile(errorMessage);
            }
        }

        private void LogErrorToFile(string errorMessage)
        {
            try
            {
                Directory.CreateDirectory(Path.GetDirectoryName(LogFilePath));
                File.AppendAllText(LogFilePath, $"{DateTime.Now}: {errorMessage}\n");
            }
            catch (Exception ex)
            {
                UnityEngine.Debug.LogError($"Failed to write error to file: {ex.Message}");
            }
        }
    }
}
