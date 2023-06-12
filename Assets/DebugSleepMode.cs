using System.Collections;
using UnityEngine;

public class DebugSleepMode : MonoBehaviour
{
    public float turnOffSleepTime = 10f;

    IEnumerator Start()
    {
        yield return new WaitForSeconds(1f);
        DeviceManager.SetScreenAndSleepTimeouts(SleepTimeout.NeverSleep);

        StartCoroutine(TurnOffInfiniteSleep());
    }


    IEnumerator TurnOffInfiniteSleep()
    {
        float sleepInCountdown = turnOffSleepTime;
        while (sleepInCountdown > 0)
        {
            Debug.Log($"DebugSleepMode: Will reset sleep & screen timeouts back to normal in {sleepInCountdown} seconds");
            sleepInCountdown--;
            yield return new WaitForSeconds(1);
        }

        DeviceManager.SetScreenAndSleepTimeouts(3);
    }
}