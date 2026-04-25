# Music Player

A [BepInEx](https://github.com/BepInEx/BepInEx) 5 plugin for **Zumbi Blocks 2** that allows players to add custom music for various game states (Menu, Active Waves, and Boss Fights).

## Features

- **Customizable Music**: Play your own `.mp3` files during gameplay.
- **Configurable Volume**: Adjust custom music volume directly from the BepInEx configuration.

## Installation

Download the latest version of the plugin at the [releases](https://github.com/tbv001/MusicPlayer/releases) page, and then copy the contents of the zip file into your BepInEx `plugins` folder:

```
Zumbi Blocks 2 Open Alpha\BepInEx\plugins\MusicPlayer
```

## Usage

The plugin looks for specific filenames in the `Music` folder:

| Game State | Expected Filename |
| :--- | :--- |
| Main Menu | `Menu.mp3` |
| Wave Tier 1 | `Wave1.mp3` |
| Wave Tier 2 | `Wave2.mp3` |
| Wave Tier 3 | `Wave3.mp3` |
| Boss Riot | `BossRiot.mp3` |
| Boss Queen | `BossQueen.mp3` |
| Boss Reaper | `BossReaper.mp3` |

## Configuration

On first launch, a configuration file is generated at:

```
Zumbi Blocks 2 Open Alpha\BepInEx\config\com.theblackvoid.musicplayer.cfg
```

## License

This project is licensed under the **MIT License** - see the [LICENSE](https://github.com/tbv001/MusicPlayer/blob/main/LICENSE) file for details.
