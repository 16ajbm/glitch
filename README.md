# Glitch

## Setup

Steps:

- Add the file [pre-push](Hooks/pre-push) to your `.git/hooks` folder within the project directory

## Version Control

We will be using GitHub for version control for the [project](https://github.com/16ajbm/Glitch).

We will be following the [Git Flow](https://gamemakerblog.com/2023/07/13/how-to-use-git-flow-for-game-development/) branching model.

## Changelog

All notable changes to this project will be documented in [CHANGELOG.md]

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.1.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## Project Organization

```
Assets
+--- Docs # Concept art, etc.
+--- Prefabs
+--- Scenes
| +--- Sandbox # Folder for prototype scenes
+--- Scripts
+--- Settings
+--- Shaders
+--- Sprites
```

## Making a Change

When contributing to the repository there are a few things to keep in mind:

1. Your rpository is [setup](#setup) correctly
2. You are working on a feature branch and it is brought into `develop` via a Pull Request
3. Your files are well-formatted, linted and tests pass (ideally we will have automated tools to ensure this, but it is always good practice)

## Resources

- [Unity's Best Practices for Project Organization](https://unity.com/resources/best-practices-version-control-unity-6)
- [How to Git properly for Game Dev - A beginner's quick guide](https://youtu.be/ZvXMn9aPyZI)
