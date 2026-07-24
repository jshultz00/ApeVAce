# Ape V Ace

A 2-player asymmetric online game built in **Unity 2021.3.8f1**.

One player is **the Ape** (King Kong) climbing to the top of the Empire State
Building. The other is **the Pilot**, flying a fighter/biplane and trying to
shoot the ape down before it reaches the summit. Two win conditions, one tower.

> **Origins:** This project began as a VR (Oculus/OpenXR) student project called
> "KingKong." It is being converted into a **non-VR PC game** that keeps the same
> core concept and the Photon online multiplayer, but replaces all VR input with
> keyboard/gamepad controls. The original VR sprint log is preserved in git history.

## Status

Converting from VR to non-VR. The repository has been cleaned of the committed
player build, VR-only scripts, and demo/sample content.

- **Networking:** Photon PUN, online (two separate PCs). *Kept.*
- **Art:** current Ape / plane / city models are placeholders, to be replaced.
- **Controls:** VR input is being torn out and rewritten for keyboard/gamepad.

## Gameplay scripts

| Script | Role | State |
| --- | --- | --- |
| `Assets/Scripts/MonkeyBehavior.cs` | Ape health, height %, win/lose, `onHit` | Health/UI logic kept; movement being rewritten (was VR hand-grab) |
| `Assets/Scripts/towerBehavior.cs` | Procedural ledge generation on the tower | Kept |
| `Assets/Scripts/ProjectileScript.cs` | Physics projectile + damage on hit | Kept |
| `Assets/Scripts/UISpriteBehavior.cs` | Simple UI sprite animation | Kept |
| `Assets/Scripts/MenuController.cs` | Character selection | Kept |
| `Assets/Scripts/NetworkManager.cs` | Photon connect / rooms / team select | Kept |
| `Assets/Scripts/NetworkPlayerManager.cs` | Local player instance across scene loads | Kept |
| `Assets/Scripts/DisableCamera.cs` | Enables only the local player's camera | To adapt for non-VR cameras |
| `Assets/Plane/Scripts/planeController.cs` | Flight physics, overheating gun, respawn | Flight/gun math kept; VR input being rewritten |
| `Assets/Plane/Scripts/joystickInteractor.cs` | VR joystick interactable | To be removed in the rewrite |

## Roadmap

**Rewrite phase (current):**
1. **Ape controls** — replace VR hand-over-hand ledge grabbing with a
   keyboard/gamepad climbing mechanic.
2. **Pilot controls** — keep the flight physics and overheating gun; swap the
   VR joystick/throttle for keyboard+mouse or gamepad.
3. **Cameras** — a third-person chase camera per player (each on their own PC).
4. Drop the XR/Oculus/OpenXR packages once no script references them.

**Later:**
- Replace placeholder art (ape, plane, cityscape / Empire State Building).
- Main menu, HUD, and match flow polish.

## Requirements

- Unity **2021.3.8f1**
- A Photon PUN App ID (for online play)
