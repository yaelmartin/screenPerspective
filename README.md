# screenPerspective

v1.0

## **Introduction**

screenPerspective is a Unity package that enables real-time Virtual Production in Unity, supporting one or multiple **flat** screens. It utilizes Unity's physical camera lens shift properties to create an anamorphic projection.

[![Video preview](https://github.com/user-attachments/assets/eba24914-41b2-4cb4-b726-d225ad8b8459)](https://youtu.be/flpAcYQN8oo "Virtual production in Unity, CAVE perspective corrected displays with tracked camera")

The project can easily be adapted to work with any low-latency 6DOF tracking devices.
By default, it includes an example using SteamVR tracker and Unity XR Interaction Toolkit (XRI).

See [https://github.com/yaelmartin/viveTrackersUnityXRI](https://github.com/yaelmartin/viveTrackersUnityXRI) for more info.

## Features:

- Correct perspective mapping between your screen(s) and a real tracked camera
- Simulator scene for experiencing the project without a 6DOF tracker
- Support for up to 8 flat screens/projectors
- SteamVR demo using a single tracker and screen
- Adaptable with various tracking solutions

## Use Cases

- Real-time Virtual Production
- CAVE (Cave Automatic Virtual Environment) systems
- Rendering scenes on screens in Unity with known screens and camera positions

## Getting Started

### Simulator Mode

1. Open the `ScreenPerspective_Simulator` scene and enter Play mode.
2. Navigate using right-click and WASD keys.
3. Each screen requires its own CameraRigPerspective prefab, which can be freely positioned and rotated.

The simulator works with render textures instead of display outputs. To add a new screen in the simulator:

1. Place a CameraRigPerspective prefab inside ScreenScaleMultiplier
2. Create a new render texture and material
3. Apply the material to ScreenMesh
4. Select the PhysicalCamera in your new CameraRigPerspective and set your render texture as its Output Texture
5. Adjust screen size using ScreenWidthM and ScreenHeightM fields (in meters)
6. Click "Apply Screen Dimensions"

You can scale up or down the whole setup with ScreenScaleMultiplier.

**Note**: The simulator uses FreeFlyCamera.cs by [Sergey Stafeev](https://assetstore.unity.com/packages/tools/camera/free-fly-camera-140739). For pitch or roll rotations of ScreenScaleMultiplier, consider placing the ExternalCamera at the scene root.

### Using a SteamVR Tracker and Real Screen

1. Measure your physical screen's width and height
2. Attach your tracker to your phone/camera
3. Open the `ScreenPerspective_SteamVR-Tracker` scene
4. Select CameraRigPerspective and update ScreenWidthM and ScreenHeightM
5. Click "Apply Screen Dimensions"
6. The pink T-shaped "Spawn" object indicates where your tracker should be relative to your screen during calibration
7. Press "R" to toggle the calibration panel in Play mode

**Important Notes**:

- For multiple SteamVR trackers, specify which to use in ScreenScaleMultiplier_SteamVR-Tracker → MovableAreaTracker → OriginRoomScale → VTSingle. (Use a preset like TPDCamera for its TrackedPoseDriver component)
- Adjust the HEADTRACKER child of VTSingle to match your real camera's nodal point for best results
- For calibration details, see [viveTrackersUnityXRI Calibration](https://github.com/yaelmartin/viveTrackersUnityXRI?tab=readme-ov-file#calibration)

### Using multiple screens

- Make sure to place each CameraRigPerspective accurately (3D scanning can be helpful)
- Use the correct display output for each physical camera in your rigs. You can use [this tool to see the order Unity uses](https://github.com/yaelmartin/unityTargetDisplayOrderDetector)
- Place a ActivateDisplays prefab in your scene

### Integrating Other Tracking Methods

1. Refer to your device's documentation for Unity integration
2. Implement a method to align your Unity scene with your physical setup
3. Place your custom offset calibration solution as a child of ScreenScaleMultiplier
4. For each CameraRigPerspective, link the Transform representing your camera's nodal point to the EyePosition field in the inspector

**Credits**

- HTCViveTrackerProfile.cs from [Vive's forums](https://forum.htc.com/topic/14370-tutorial-openxr-pc-vr-how-to-use-vive-tracker/?do=findComment&comment=55772) and haptics from [https://github.com/mbennett12/ViveTrackerHapticOpenXR](https://github.com/mbennett12/ViveTrackerHapticOpenXR)
- FreeFlyCamera.cs from [Sergey Stafeev](https://assetstore.unity.com/packages/tools/camera/free-fly-camera-140739)
