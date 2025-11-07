# River Crossing — Unity Game

A family-friendly 3D river crossing puzzle game built with Unity. The objective is to transport characters across a river using a boat while following level-specific constraints (e.g., never leaving certain characters together unattended). The project contains multiple levels (classic missionaries & cannibals, jealous husbands, and other variations) with reusable boat and character controllers, particle effects, and sound.

## Highlights / Features

- Multiple puzzle level types (Missionaries & Cannibals, Jealous Husbands, Custom scenarios)
- Boat physics and rotation controller
- Character grouping and crossing rules implemented per level
- Audio (background music, paddle splash), particle effects for water interactions
- Prefabs for boat, characters, and UI elements for fast iteration

## Recommended Unity Version

- Unity 2020.x / 2021.x / 2022.x (project is compatible with Unity 2020+). If you encounter package or API issues, open the project using the Unity Hub and allow it to upgrade packages if prompted.

## How to open the project

1. Install Unity with a compatible editor version (see above).
2. Open Unity Hub, choose "Add" and select the project folder: `RiverCrossingGameUnity`.
3. Open the project in the Hub. Wait for the Editor to import assets and compile scripts.
4. Open the main Scenes folder (Assets/Scenes) and select a scene to play. The project may include scenes for different levels; the scene names typically indicate the level.

## Play in the Editor

- Press the Play button in the Unity Editor to run the currently open scene.
- Use the Scene Hierarchy to locate player/boat prefabs and test components live.

## Controls

- Mouse / Touch (mobile): Tap characters to select and place them in the boat; tap the "Go" button to cross the boat.
- UI: On-screen buttons for "Start Level", "Next Level", and "Try again".

## Project structure (notable folders & files)

- Assets/
  - Scenes/ — game scenes for different levels
  - Prefabs/ — boat, characters, UI prefabs
  - Scripts/ — gameplay logic and controllers
    - CannibalsMissionariesController.cs — logic for the Missionaries & Cannibals puzzle
    - Level2RiverController3D.cs — sample 3D river level controller
    - Level4JealousHusbandsController3D.cs — jealous husbands level implementation
    - boatRotation.cs — boat rotation and animation helper
    - BackgroundMusic.cs — persistent background audio manager
    - SplashParticle.cs, PaddleSplash.cs — water and paddle FX
    - Couple.cs — helper class for paired characters
  - Materials/, Models/, Animations/ — visual assets used across scenes

Files and scripts may have slightly different names in the Assets root — use the Project window to inspect components attached to GameObjects in each scene.

## Gameplay & Mechanics

- Objective: Move all required characters from the start bank to the goal bank using the boat while obeying level-specific rules.
- Boat capacity: Varies by level; default boat scripts enforce a maximum passenger count.
- Constraint rules: Example — in the white shirt & gray shirt level, gray shirt's cannot outnumber white shirt's on either bank.
- Level controllers monitor win/lose conditions and trigger UI feedback or scene transitions.

## Building for platforms

1. In Unity, open File > Build Settings.
2. Select target platform (PC/Mac/Linux, Android, iOS) and add the desired scenes to the Build Scenes list.
3. Configure Player Settings (icon, bundle id, resolution, input settings) as needed.
4. Click Build or Build and Run.

For mobile builds, ensure you have the required platform modules installed in Unity Hub (Android / iOS). For iOS builds, open the generated Xcode project and build from Xcode.

## Debugging & Troubleshooting

- Script compile errors: Open the Console window (Window > General > Console) to see stack traces. Double-click an error to open the offending script.
- Missing references: Check Inspector for missing script or prefab links and reassign the correct components or prefabs.
- Audio not playing: Confirm `BackgroundMusic` is on a GameObject in the starting scene and has "Play On Awake" enabled if desired.
- Scene-specific problems: Inspect the level controller attached to the main controller GameObject for null references or misconfigured rules.

## Extending the game

- Add a new level:
  - Duplicate an existing level scene and modify the starting bank composition.
  - Create or reuse a LevelController script and configure rules and boat capacity.
  - Add the new scene to the Build Settings.

- Add new character types:
  - Create a new prefab with animations and assign the appropriate tag or script (e.g., Couple.cs) if special logic applies.
  - Update level controllers to include the new character in rule checks.

## Contribution

Contributions, bug reports, and improvement suggestions are welcome.

- To contribute: fork the repo, make changes on a feature branch, and open a pull request with a concise description of changes.
- Please keep gameplay logic and UI changes separated and include screenshots or short notes describing the behavior change.
Perfect — here’s a clean section you can add to your **README.md** under a new heading like **“🧭 Contribution Guide”** or **“Contributing”** 👇

---

## 🧭 Contribution Guide

We welcome contributions to improve the River Crossing Game!

### 🔹 How to Contribute

1. Check the [Issues](../../issues) page for open tasks or feature bounties.
2. Comment on an issue to claim it before starting work.
3. Fork the repo
4. Clone from the forked to your own machine
5. Create a feature branch:

   ```bash
   git checkout -b feature/your-feature-name
   ```
6. Commit clean, modular code and push your branch.
7. Submit a **Pull Request** referencing the issue number.
   Example:

   > Resolves #12

---

### 💰 Bounty System

* Some issues include **ETB rewards** for completed features.
* Rewards are given to the **first successfully merged pull request** meeting all acceptance criteria.
* Be sure to review the **Acceptance Criteria** listed in the issue before starting.

---

### 🧩 Code Style

* Follow Unity and C# best practices.
* Keep code readable and modular.
* Use meaningful commit messages and clear PR descriptions.

## Assets & Licensing

- Many assets in `Assets/` appear to be project-specific; verify third-party assets for their respective licenses before redistribution.
- If you add external assets (models, sounds, textures), include license text in `Assets/ThirdPartyLicenses` or a similar folder.

## Known issues

- Project may require minor package upgrades on newer Unity versions.
- Some prefab names or scene references may differ between versions; check the Console for missing references after opening the project.
- There are many unnecessary files, folders and code are included and we believe those should be removed.
- Since we're just experimenting thing may not working as expected 

## Contact

For questions or assistance, open an issue on the repository or contact the maintainer via email (esubalew.a2009@gmail.com).

---

Enjoy building and improving the River Crossing puzzle game!
