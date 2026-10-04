# 🏝️ Islanded

<div align="center">

![Unity](https://img.shields.io/badge/Unity-6000.0%2B%20URP-black?style=for-the-badge&logo=unity)
![C#](https://img.shields.io/badge/C%23-239120?style=for-the-badge&logo=c-sharp&logoColor=white)
![Event](https://img.shields.io/badge/Event-GameJam%20Trabzon%20(36h)-blueviolet?style=for-the-badge)
![Status](https://img.shields.io/badge/Status-Completed-success?style=for-the-badge)

**A 3D post-apocalyptic third-person survival game created in 36 hours during GameJam Trabzon.**

[Story & Overview](#-story--overview) •
[Key Features](#-key-gameplay-features) •
[Controls](#-controls) •
[Architecture & Systems](#-architecture--systems) •
[Getting Started](#-getting-started) •
[The Team](#-the-team)

</div>

---

## 📖 Story & Overview

In a post-apocalyptic world where the oceans have turned lethal with toxic contamination and vegetation has rapidly mutated to adapt to the chaos, you find yourself stranded on a hazardous island.

Your only beacon of hope is a broken ship anchored offshore. To escape, you must brave the toxic atmosphere, scavenge sparse resources from the mutated wilderness, nourish yourself against starvation, repair the vessel's critical systems, and prepare for departure before time runs out.

> *"In this 36-hour sprint, quick decision-making and crisis management were the keys to bringing Islanded to life."*

---

## ⚡ Key Gameplay Features

### ⏳ 1. Race Against Time (Toxic Atmosphere)
* **Hazardous Environment:** The air across the island is saturated with toxic spores. You can only remain on the island for a limited duration before taking heavy atmospheric damage.
* **Safe Zone Sanctuary:** Your ship acts as a refuge where toxicity timers freeze or reset, allowing you to recuperate and plan your next expedition.
* **Survival Upgrades:** Unlock exploration abilities such as *Deep Breath (Derin Nefes)* and *Time Bender (Zaman Bükücü)* to expand your exploration window.

### 🍖 2. Survival & Hunger Management
* **Dual Vitality System:** Manage both **Health** and **Hunger (Tokluk)**. Running out of food causes gradual starvation, directly draining your hit points.
* **Foraging & Hunting:** Scavenge for mushrooms (*Mantar*), harvest wheat (*Buğday*), and track sheep (*Koyun*).
* **Nourishment Multipliers:** Upgrade skills like *Nutritious Meal (Besleyici Öğün)* to boost nutrition yield from gathered rations by 50%.

### 🛠️ 3. The Escape Plan (Ship Restoration)
* **Modular Repair Stations:** Fix essential components of your vessel using raw materials—wood planks (*Tahta*), nails (*Çivi*), and stones (*Taş*).
* **Wear & Dynamic Leaks:** Resting in bed triggers day transitions that consume hunger and periodically cause new hull leaks, requiring proactive maintenance.
* **Hull Reinforcements:** Visit the Carpenter Station to build *Sturdy Hull (Sağlam Gövde)* and *Armored Bow (Zırhlı Pruva)*, reinforcing the ship against leaks.

### 🌊 4. Environmental Hazards & AI
* **Poisoned Waters:** The toxic sea deals instant, continuous damage upon contact. Staying on dry land or boarding the ship is essential for survival.
* **Patrol AI:** Roaming wildlife and creatures patrol the terrain with dynamic waypoints, randomized pauses, and alertness behaviors.

---

## 🎮 Controls

| Action | Input (Keyboard / Mouse) |
| :--- | :--- |
| **Move** | <kbd>W</kbd> <kbd>A</kbd> <kbd>S</kbd> <kbd>D</kbd> |
| **Look / Camera** | <kbd>Mouse</kbd> |
| **Sprint** | <kbd>Left Shift</kbd> |
| **Jump** | <kbd>Space</kbd> |
| **Interact / Repair / Sleep** | <kbd>E</kbd> |
| **Consume Food / UI Actions** | <kbd>Left Mouse Click</kbd> on UI buttons |
| **Restart Scene (Game Over)** | On-screen Restart Button |

---

## 🏗️ Architecture & Systems

The project is built on **Unity 6 (URP)** using modular C# MonoBehaviour components:

```
Assets/Scripts/
├── SurvivalManager.cs       # Core gameplay loop: health, hunger decay, toxicity timers & game over
├── ShipManager.cs           # Ship integrity, leak tracking, repair state & escape triggers
├── RepairStation.cs         # Modular interactable stations with resource checks and stage visuals
├── BedInteraction.cs        # Day transition, sleep hunger cost, island respawning & random wear
├── GlobalInventory.cs       # Centralized resource inventory & real-time UI synchronization
├── EatFoodManager.cs        # Food consumption logic with passive skill multipliers
├── KasifManager.cs          # Explorer upgrades (Gas Mask visual, satchel capacity, time extenders)
├── MarangozManager.cs       # Carpenter upgrades (Hull armor, bow armor & durability)
├── ButtonInfoHandler.cs     # Interactive upgrade button hover and tooltip presentation
├── ProximityPanel.cs        # Proximity-based UI trigger system for interactive stations
├── CollectibleItem.cs       # World resource pickups (floating bobbing animations & collection)
├── SurfaceScatterSpawner.cs # Procedural scatter spawner distributing flora & resources on terrain
├── ObjectSpawner.cs         # Configurable randomized prefab spawner
├── PatrolAI.cs              # Autonomous waypoint-based patrol system with randomized wait times
├── PlayerHealth.cs          # Player health data & slider UI bindings
├── WaterDamage.cs           # Toxic water trigger collider dealing damage over time
├── FootstepManager.cs       # Surface-aware audio footstep player
├── RandomSoundPlayer.cs     # Ambient sound player with randomized intervals
└── SceneController.cs       # Scene management and reload utilities
```

---

## 🚀 Getting Started

### Prerequisites
* **Unity Version:** Unity 6 (`6000.0` or higher) with **Universal Render Pipeline (URP)**.
* **Input System:** Unity New Input System package installed and enabled.

### Running in Unity Editor
1. Clone the repository:
   ```bash
   git clone https://github.com/Edyboziron/islanded.git
   cd islanded
   ```
2. Open the project folder in **Unity Hub** (select Unity 6000.x).
3. In the Project window, navigate to `Assets/Scenes/SampleScene.unity` and open it.
4. Press the **Play** button in the Unity Editor toolbar.

### Standalone Build
A pre-built Windows standalone version is included in the [`zehirli ada/`](file:///D:/UnityDosya/jam/jam/islanded/zehirli%20ada) directory:
* Run `zehirli ada/gemi.exe` directly on 64-bit Windows systems.

---

## 👥 The Team

Created in **36 hours** during **GameJam Trabzon**:

* **Enes Bozdemir** — *Game Development & Programming*  
  GitHub: [@Edyboziron](https://github.com/Edyboziron)
  
* **Deniz Mirik** — *3D Art & Modeling*  
  GitHub: [@DenizMirik7](https://github.com/DenizMirik7)

---

## 📄 License & Attribution

* Third-party character controllers, audio clips, and UI packages used during the game jam belong to their respective creators under Unity Asset Store licenses.
* Original source code and 3D models developed for Islanded are available for educational and portfolio demonstration.
