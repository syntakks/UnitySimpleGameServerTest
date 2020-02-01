using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SocketIO; 

namespace Project.Networking
{
    public class NetworkClient : SocketIOComponent
    {
        [Header("Network Client")]
        [SerializeField] private Transform networkContainer;

        private Dictionary<string, GameObject> serverObjects; 

        public override void Start()
        {
            base.Start();
            Initialize(); 
            SetupEvents(); 
        }

        public override void Update()
        {
            base.Update(); 
        }

        private void Initialize()
        {
            serverObjects = new Dictionary<string, GameObject>(); 
        }

        private void SetupEvents()
        {
            On("open", (E) => {
                Debug.Log("Connection made to the server!"); 
            });

            On("register", (E) => {
                string id = E.data["id"].ToString().Trim('"'); 
                Debug.LogFormat("Register: ClientID: ({0})", id); 
            });

            On("spawn", (E) => {
                // Handling spawning all players
                // Passed data
                string id = E.data["id"].ToString().Trim('"');

                GameObject go = new GameObject("Server ID: " + id);
                go.transform.SetParent(networkContainer); // Keep it organized. 
                serverObjects.Add(id, go); // Add to current roster. 
            });

            On("disconnected", (E) => {
                string id = E.data["id"].ToString().Trim('"');
                GameObject go = serverObjects[id];
                Destroy(go);
                serverObjects.Remove(id); 
            });
        }
    }
}

