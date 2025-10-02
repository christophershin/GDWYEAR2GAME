using UnityEngine;
using Unity.Netcode.Components;


[DisallowMultipleComponent]
public class ClientNetworkScript : NetworkTransform
{
    protected override bool OnIsServerAuthoritative()
    {
        return false;
    }
}
