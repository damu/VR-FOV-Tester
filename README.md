# VR FOV Tester

Find out your personal Field of View (FOV) values with a specific VR headset. The values you can get are horizontal FOV (HFOV), vertical FOV (VFOV) and even asymmetric vertical FOV values like for example 40 top and 50 bottom which results in a total vertical FOV of 90. While developing this tool I found out that I see more to the bottom than to the top with every tested headset so I added that feature.

## Downloads

* PC: [VR_FOV_Tester_1.0_PC.zip](https://github.com/damu/VR-FOV-Tester/releases/download/v1.0/VR_FOV_Tester_1.0_PC.zip)
* Quest standalone: [VR_FOV_Tester_1.0_Quest.apk](https://github.com/damu/VR-FOV-Tester/releases/download/v1.0/VR_FOV_Tester_1.0_Quest.apk)
* Pico standalone: [VR_FOV_Tester_1.0_Pico.apk](https://github.com/damu/VR-FOV-Tester/releases/download/v1.0/VR_FOV_Tester_1.0_Pico.apk)

## How to use

* Put on your headset in a normal usage position. So no extra pressing or holding with your hands to get a bigger number.  
* Look straight ahead at the X to get proper values for a normal usage case. When you look to the edge your FOV will be a bit smaller because the position of your pupil changes.  
* Use any thumbstick to adjust the horizontal and vertical FOV. Move the black bars until you can barely see them anymore.  
* Since the black bars are hard to see at the corner of your eyes when you look straight ahead you can hold any face button (like A, B, X or Y) to make them blink.  
* If you also have the effect that one vertical bar disappears while the other is still quite visible you can use the asymetric mode with the trigger and grip buttons to move the lower bar independently. In asymmetric mode the values are displayed as  
```VFOV top + VFOV bottom = total VFOV```
* Press the thumbstick to leave the asymetric FOV mode.
* The "Rendered HFOV" and "Rendered VFOV" values are the FOV values reported by the headset (the camera in Unity). I'm not sure if those are accurate and don't know how headsets with not full stereo overlap like Pimax are reported.

Usage example:
<video src='https://user-images.githubusercontent.com/11298027/212571677-b2a115cf-decc-4b38-b466-b92651383c2e.mp4' width=300/>

## Supported Headsets

Currently you can get:
* a PC VR (Open XR) Windows program. Runs fine with Steam VR and over Virtual Desktop for example.
* a standalone Quest APK which should run on Quest 1, 2, Pro and potential future headsets (tested on Quest 2)
* a standalone Pico APK which should run on Pico 3, 4 and potential future headsets (tested on Pico 4)

## How to install / run

The Windows program can just be run like any other PC VR program. When using Virtual Desktop you probably have to first start SteamVR from within Virtual Desktop. Depending on your settings it might run directly on Oculus or WMR or whatever without SteamVR, it should not require SteamVR since it's using OpenXR.

To install the APK on Pico simply connect your Pico to your PC via USB, copy the APK file to the Pico (for example into the Download folder), install it by using the Pico File Manager and then you can find it under Apps -> Unknown.  
Here a video where I copied the APK onto the Pico while using my desktop in VR with Virtual Desktop:
<video src='https://user-images.githubusercontent.com/11298027/212573567-2d458025-5768-485f-9215-28ba322f3565.mp4' width=300/>

To install the APK on Quest or as an alternative way for Pico you can use SideQuest or something similar.

Since this is Free Software (Open Source) you could also run it out of the Unity editor or build it for other platforms.

## Motivation

The motivation to make this program came from confusion about the Pico 4 values. I used a FOV testing environment in Steam VR Home but that is not such a great tool. My values seemed to be notably below the FOV values that other users reported. By now I'm relatively sure that it is headshape dependent. Some users eyes probably have a bigger distance to the Pico 4 lenses due to the facial gasket. This should also be the case with other headsets and might explain why some Pimax 8KX reviewers complain about distortions while others are fine with the same headset. Near and far sightedness might also play a role.

## My values with various VR Headsets

|               |  Quest 2  | Pico 4 | Rift S |
| ------------- | --------: | -----: | -----: |
| HFOV          | 84        | 94     | 84     |
| VFOV          | 82        | 92     | 87     |
| Upper VFOV    | 34        | 45     | 39     |
| Lower VFOV    | 48        | 47     | 48     |
| Rendered HFOV | 95        | 99     | 94     |
| Rendered VFOV | 99        | 103    | 98     |
| | <img width="200px" src="https://user-images.githubusercontent.com/11298027/213967660-422a293d-f35b-4380-b53d-b86c455c9a08.jpg" /> | <img width="200px" src="https://user-images.githubusercontent.com/11298027/213967693-bd3e3914-c435-4550-afa3-68219e38ddb6.jpeg" /> | <img width="200px" src="https://user-images.githubusercontent.com/11298027/213967727-4faad146-9720-4aa0-ab0f-c3b0adb7252e.jpeg" /> |


![OculusScreenshot1674446743](https://user-images.githubusercontent.com/11298027/213967727-4faad146-9720-4aa0-ab0f-c3b0adb7252e.jpeg)

