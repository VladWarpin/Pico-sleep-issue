using System;
using Unity.XR.PXR;
using UnityEngine;


/// <summary>
/// Helper class to initialize & bind Pico system,
/// only need to be used in case we want to manipulate Pico system related stuff
/// </summary>
public class PicoSystemInitializer : MonoBehaviour
{
    #region Singelton
    private static PicoSystemInitializer instance;
    public static PicoSystemInitializer Instance
    {
        get
        {
            if (instance == null)
            {
                GameObject container = new GameObject("PicoSystemInitializer");
                instance = container.AddComponent<PicoSystemInitializer>();
                DontDestroyOnLoad(container);
            }

            return instance;
        }
    }
    #endregion

    public event Action OniIitialized;
    public bool IsInitialized { get; private set; }
    
    // Initializing and bind Service, the objectname refers to name of the object which is used to receive callback.
    private void Awake()
    {
        PXR_System.InitSystemService(gameObject.name);
        PXR_System.BindSystemService();
    }

    // For callback of Service binding, this method must be present,
    // which will be called after Service binding success
    public void toBServiceBind(string s)
    {
        Debug.Log($"Pico system bind success. {s}");
        IsInitialized = true;
        OniIitialized?.Invoke();
    }

    // Unbind the Service
    private void OnDestory()
    {
        PXR_System.UnBindSystemService();
    }
    // Add 4 callback methods to allow corresponding callback can be received.
    private void BoolCallback(string value)
    {
        if (PXR_Plugin.System.BoolCallback != null) PXR_Plugin.System.BoolCallback(bool.Parse(value));
        PXR_Plugin.System.BoolCallback = null;
    }
    private void IntCallback(string value)
    {
        if (PXR_Plugin.System.IntCallback != null) PXR_Plugin.System.IntCallback(int.Parse(value));
        PXR_Plugin.System.IntCallback = null;
    }
    private void LongCallback(string value)
    {
        if (PXR_Plugin.System.LongCallback != null) PXR_Plugin.System.LongCallback(int.Parse(value));
        PXR_Plugin.System.LongCallback = null;
    }
    private void StringCallback(string value)
    {
        if (PXR_Plugin.System.StringCallback != null) PXR_Plugin.System.StringCallback(value);
        PXR_Plugin.System.StringCallback = null;
    }
}