# Pico-sleep-issue

[PICO Unity Integration SDK](https://developer-global.pico-interactive.com/sdk?deviceId=1&platformId=1&itemId=12) v2.1.3 is used  

Test project to prove the issue where pico device won't apply sleep & screen timeout settings when device is NOT head-mounted.

Settings are set via `PXR_System.PropertySetSleepDelay()` and `PXR_System.PropertySetScreenOffDelay()` using pico integration

## How to reproduce:

1. Build (or take already provided ***TestBuild.apk***) and sideload to pico device. 
2. The device sleep and screen timeout settings will be set to NEVER
3. Put the device off, place it in such a way so that you can observe whenever the device sceen is ON or OFF without picking it up
4. After 10 seconds, the script will set the device sleep and screen timeout settings back to its minimum supported values (usually 30 seconds for screen off delay and 15 minutes for sleep delay)
5. After 30 seconds, check the device screen (while device being unmounted)  

**Expected result:** Device screen shoud go off after 30 seconds

**Actual result:** Device screen will never go off (new applied settings won't take effect)

6. Mount the device, then put if off and the previously applies settings will take effect (screen will go off in 30 seconds).