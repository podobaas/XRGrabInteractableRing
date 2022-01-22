<p align="center">
<a href="https://imgbb.com/"><img src="https://i.ibb.co/Tb19xnc/XR-Grab-Interacteble-Ring-13.png" alt="XR-Grab-Interacteble-Ring-13" border="0"></a>
</p>

<h2 align="center">

<img alt="GitHub" src="https://img.shields.io/github/license/podobaas/DevToAPI?color=green&style=flat">
</h2>

## Demo
[![Demo](https://img.youtube.com/vi/-FZZ0IZzTEY/0.jpg)](https://www.youtube.com/embed/-FZZ0IZzTEY)

## Installation
* Install XR Interaction Toolkit from Unity Package Manager
* Install XR-Plugin Management
* Customize your project for the target VR
* Download asset from releases on github
* Add component XRGrabInteractableRing on your interactable object

## Properties
| Name  | Value |
| ------------- | ------------- |
| Camera | Rig camera (main camera) |
| Model Prefab | Ring model from Prefabs folder or your custom model|
| Color | Model color |
| Transform type  | Self: The ring will be placed in the center of the object. Custom: Custom attach transform |
| Attach Transform  | The transform that is used as the attach point for Interactables. Only for Custom transform type. |
| Show On Selected  | Show on selected object or not  |
| Layer Mask  | Layer Mask for raycast  |
| Threshold Distance  | Maximum distance to display the ring  |
| Min Scale | Minimum scale model |
| Max Scale  | Maximum scale model|
| Speed  | Speed animation |
| Duration | Duration animation |
| Event OnShow | Called when the model appears on the object  |
| Event OnHide | Called when the model disappears on the object  |


## References
+ [LICENSE](LICENSE)