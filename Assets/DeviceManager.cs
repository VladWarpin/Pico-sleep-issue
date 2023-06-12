using System;
using System.Collections;
using Unity.XR.PXR;
using UnityEngine;


public static class DeviceManager
{
    private static bool picoSystemBound = false;

    /// <summary>
    /// Will set screen and sleep timeouts on pico device through pico plugin
    /// </summary>
    /// <param name="timeout">Time in seconds</param>
    public static void SetScreenAndSleepTimeouts(int timeout)
    {
        if (Application.isEditor)
            return;


        var sleepTimeOut = timeout == -1 ?
            SleepDelayTimeEnum.NEVER :
            SleepDelayTimeEnum.FIFTEEN; // minimum available value is 15 minutes


        var screenTimeOut = timeout < 0 ? ScreenOffDelayTimeEnum.NEVER :
                            timeout > 0 && timeout < 4 ? ScreenOffDelayTimeEnum.THREE :
                            timeout > 3 && timeout < 11 ? ScreenOffDelayTimeEnum.TEN :
                            timeout > 10 && timeout < 31 ? ScreenOffDelayTimeEnum.THIRTY :
                            timeout > 30 && timeout < 61 ? ScreenOffDelayTimeEnum.SIXTY :
                            timeout > 60 && timeout < 301 ? ScreenOffDelayTimeEnum.THREE_HUNDRED :
                              /*else*/                      ScreenOffDelayTimeEnum.SIX_HUNDRED;

        CoroutineHelper.InitPicoSystem((result) =>
        {
            string before = "(Before)";
            string after = "(After)";

            // print get values (Before)
            var systemSleepDelay = PXR_System.GetSleepDelay();
            var systemsScreenOffDelay = PXR_System.GetScreenOffDelay();
            Debug.Log($"{before} Getting device sleep delay: {systemSleepDelay}");
            Debug.Log($"{before} Getting device screen off delay {systemsScreenOffDelay}");


            // print set values
            Debug.Log($"Setting device sleep delay to: {sleepTimeOut}");
            Debug.Log($"Setting device screen off delay to {screenTimeOut}");


            // Setting screen timeout and sleep calues
            PXR_System.PropertySetSleepDelay(sleepTimeOut);
            PXR_System.PropertySetScreenOffDelay(screenTimeOut, null);


            // print get values (After)
            systemSleepDelay = PXR_System.GetSleepDelay();
            systemsScreenOffDelay = PXR_System.GetScreenOffDelay();
            Debug.Log($"{after} Getting device sleep delay: {systemSleepDelay}");
            Debug.Log($"{after} Getting device screen off delay {systemsScreenOffDelay}");
        });

    }


    #region Helper
    public static class CoroutineHelper
    {
        public static IEnumerator InitPicoSystemCoroutine(Action<bool> result)
        {
            bool initResult = true;

            try
            {
                if (picoSystemBound)
                {
                    PXR_System.UnBindSystemService();
                    picoSystemBound = false;
                }
                PXR_System.InitSystemService(CoroutineManager.Instance.name);
                PXR_System.BindSystemService();
                picoSystemBound = true;
            }
            catch (Exception e)
            {
                Debug.LogError("Init pico system service failed " + e);
                initResult = false;
            }
            finally
            {
                string resultText = initResult ? "successful" : "unsuccessful";
                Debug.Log($"Init pico system was {resultText}!");
            }

            // give it some time to init
            yield return new WaitForSeconds(1f);
            result?.Invoke(initResult);
        }

        public static void InitPicoSystem(Action<bool> result) =>
            CoroutineManager.Instance.StartCoroutine(InitPicoSystemCoroutine(result));
    }
    #endregion
    #region Coroutine Manger
    public class CoroutineManager : MonoBehaviour
    {
        private static CoroutineManager instance;

        public static CoroutineManager Instance
        {
            get
            {
                if (instance == null)
                {
                    GameObject container = new GameObject("CoroutineManager");
                    instance = container.AddComponent<CoroutineManager>();
                    DontDestroyOnLoad(container);
                }

                return instance;
            }
        }
    }
    #endregion
}