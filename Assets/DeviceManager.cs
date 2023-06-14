using Unity.XR.PXR;
using UnityEngine;

public static class DeviceManager
{
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

        if (PicoSystemInitializer.Instance.IsInitialized)
            SetScreenTimeout();
        else
            PicoSystemInitializer.Instance.OniIitialized += SetScreenTimeout;

        void SetScreenTimeout()
        {
            PicoSystemInitializer.Instance.OniIitialized -= SetScreenTimeout;

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

            // Setting screen timeout and sleep
            PXR_System.PropertySetSleepDelay(sleepTimeOut);
            PXR_System.PropertySetScreenOffDelay(screenTimeOut, ScreenSetCallback);

            // print get values (After)
            systemSleepDelay = PXR_System.GetSleepDelay();
            systemsScreenOffDelay = PXR_System.GetScreenOffDelay();
            Debug.Log($"{after} Getting device sleep delay: {systemSleepDelay}");
            Debug.Log($"{after} Getting device screen off delay {systemsScreenOffDelay}"); ;
        }

        void ScreenSetCallback(int i)
        {
            string result = "";
            switch (i)
            {
                case 0:
                    result = "Successfully set";
                    break;
                case 1:
                    result = "Failed to set";
                    break;
                case 10:
                    result = "The screen off timeout should not be greater than the system sleep timeout";
                    break;
                default:
                    break;
            }
            Debug.Log("Screen set callback returned: " + result);
        }
    }
}