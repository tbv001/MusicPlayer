# Music Player

**Music Player** is a mod for **Zumbi Blocks 2** that allows players to add custom music for various game states (Menu, Active Waves, and Boss Fights).

## Features

- Play your own `.mp3` music files during gameplay.
- Adjust the music volume through the BepInEx configuration file.

## Installation

Download the latest version of the plugin at the [releases](https://github.com/tbv001/MusicPlayer/releases) page, and then copy the contents of the zip file into your BepInEx `plugins` folder:

```
Zumbi Blocks 2 Open Alpha\BepInEx\plugins\MusicPlayer
```

## Usage

The plugin looks for specific filenames in the mod's `Music` folder:

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

On first launch with the mod installed, a configuration file is generated at:

```
Zumbi Blocks 2 Open Alpha\BepInEx\config\com.theblackvoid.musicplayer.cfg
```

## License

This project is licensed under the **MIT License** - see the [LICENSE](https://github.com/tbv001/MusicPlayer/blob/main/LICENSE) file for details.
