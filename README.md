# Glitch

## Version Control

We will be using GitHub for version control for the [project](https://github.com/16ajbm/Glitch).

We will be following the [Git Flow](https://gamemakerblog.com/2023/07/13/how-to-use-git-flow-for-game-development/) branching model.

### Important Repository Rules
I have to write these out because they cannot be enforced by GitHub unless we have a Team membership. They are:
- **DO NOT** push directly to `main`
- **DO NOT** push directly to `develop`
- **DO NOT** merge Pull Requests without approval/review from another team member
- Be careful and consider what you are intending before using `--force` on any Git command, that can get messy **very** fast

For more information, see:

- [Unity's Best Practices for Project Organization](https://unity.com/resources/best-practices-version-control-unity-6)
- [How to Git properly for Game Dev - A beginner's quick guide](https://youtu.be/ZvXMn9aPyZI)

## Project Organization

TODO:

- Determine file structure
- Determine file naming scheme
- Determine code naming scheme

## Prototype

Digital prototypes can be found under

```bash
Assets/Scenes/Prototypes/
```

All UI prototypes are in the UI folder and divided into subfolders.

### UI - Bounce Prototype

Doesn't use dynamic beat detection. Instead it relies on BPM of the music playing to calculate beat timing and place the sprites for the whole and half beats. Music used is free to use for game development. It is from the album by One Man Symphony called Collateral Damage. It is track 15, CyberRace Theme 02. It can be found on [bandcamp](https://onemansymphony.bandcamp.com/album/collateral-damage-free).