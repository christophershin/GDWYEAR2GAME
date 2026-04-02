using Unity.Netcode;
using UnityEngine;

public class ColorChanger : NetworkBehaviour
{
    // color options
    [SerializeField] private Vector3[] colors;
    
    private MaterialPropertyBlock _propBlock;
    private Renderer _renderer;
    private static readonly int ColorPropertyID = Shader.PropertyToID("_Vector3");

    private NetworkVariable<Vector3> _coll = new NetworkVariable<Vector3>(new Vector3(1f, 1f, 1f));
    
    public override void OnNetworkSpawn()
    {
        if (_renderer == null) _renderer = GetComponent<Renderer>();
        if (_propBlock == null) _propBlock = new MaterialPropertyBlock();

        UpdateShader(_coll.Value);

        _coll.OnValueChanged += (oldVal, newVal) => UpdateShader(newVal);

        if (IsServer)
        {
            // float red = Random.Range(0f, 4f);
            // float green = Random.Range(0f, 4f);
            // float blue = Random.Range(0f, 4f);
            
            // The Server instance of this script pulls from the shared manager
            
            _coll.Value = colors[ColorManager.GetCurrentColor()];
        }
    }
    
    private void UpdateShader(Vector3 newColorVector)
    {
        if (_renderer == null) _renderer = GetComponent<Renderer>();
        if (_propBlock == null) _propBlock = new MaterialPropertyBlock();

        _renderer.GetPropertyBlock(_propBlock);
        _propBlock.SetVector(ColorPropertyID, newColorVector);
        _renderer.SetPropertyBlock(_propBlock);
    }
}