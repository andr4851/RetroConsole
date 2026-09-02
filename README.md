# RetroConsole

A runtime debug console for Unity with a Unix-like command syntax.

RetroConsole gives you an in-game terminal window for inspecting and manipulating your project while it runs. It ships with a set of built-in commands, basic file and directory utilities, and a small API for writing your own commands.

> **Pre-release (0.5.1).** The package is usable and stable enough for day-to-day debugging, but some parts are still being polished. Expect changes to the API before 1.0.

---

## Features

- Draggable, resizable terminal window with a full input/output buffer
- Built-in file and directory utilities — read, create, delete
- Command history persisted between sessions
- Familiar Unix-like feel: prompt format, flags, output stream
- A small, explicit API for adding your own commands
- Command registration through a ScriptableObject — no changes to the console core

---

## Requirements

- Unity 6 (developed and tested on `6000.0.58f2`)
- TextMeshPro

---

## Installation

1. Download the latest `.unitypackage` from the [Releases](../../releases) page
2. In Unity: **Assets → Import Package → Custom Package…**
3. Select the downloaded file and import

---

## Quick start

1. Open `Assets/RetroConsole/Scenes/Demo.unity` to see a working setup
2. To add the console to your own scene, drop the `Terminal` prefab (`Assets/RetroConsole/Prefabs/Windows/`) into a Canvas
3. The window requires a **Workzone** object to support its full functionality — window dragging, resizing and status bar. See how it is wired up in the demo scene

---

## Built-in commands

| Command | Description |
|---|---|
| `help` | Lists available commands with their descriptions |
| `clear` | Clears the current terminal buffer |
| `history` | Prints the contents of `historyrc` |
| `gamedata` | Prints developer, product name, engine version and build info |
| `lock` | Sets the current buffer to read-only |
| `fullscreen` | Toggles the terminal window between windowed and fullscreen |
| `close` | Closes the current terminal window |
| `exit` | Terminates the terminal window |

Shipped as external commands (prefabs, registered through `ExternalCommands`):

| Command | Description |
|---|---|
| `echo` | Writes input text to standard output |
| `cat` | Writes the contents of a file or input stream to standard output |
| `mk` | Creates a file |
| `rm` | Removes a file or directory |
| `test` | Demo command — does nothing, used for testing and demonstration |

---

## How it works

The console is a linear command processor. It takes user input, runs the matching command, and writes the result back into the buffer.

Commands come in two kinds:

- **Built-in** — handled inside the console core (`clear`, `history`, `exit`, `lock`, and so on)
- **External** — separate `MonoBehaviour` scripts on prefabs, written against the API and registered in an `ExternalCommands` asset

Every command follows the same path: **enter → run → exit**. You decide what happens at each step, but control always returns to the terminal master when the command finishes.

---

## Writing your own command

### 1. Create the script

Inherit from `TerminalCommand` and implement `IOrder`:

```csharp
using UnityEngine;
using RetroConsole.Console;
using RetroConsole.Extented;

namespace MyGame.Commands
{
    [AddComponentMenu("RetroConsole/Terminal/Hello")]
    public class Hello : TerminalCommand, IOrder
    {
        public override void Init()
        {
            buffer.PrintLine("Hello from a custom command!");
            OnExit();
        }

        public override void OnExit()
        {
            buffer.SetOrder(master);
            buffer.SetFormat($"unity@{Application.productName}");
        }
    }
}
```

`Assets/RetroConsole/Scripts/Terminal/API/TerminalCommand.cs` is the reference implementation — it doubles as a working template you can copy from.

### 2. Register it

1. Create a prefab with your script attached
2. Open your `ExternalCommands` asset (`Assets/RetroConsole/Presets/ExternalCommands.asset`)
3. Add an entry: the command name as typed by the user, the prefab, and a description shown by `help`

---

## API reference

| Method | When it runs |
|---|---|
| `Init()` | On command initialization — the entry point |
| `OnInputEnter(string input)` | After the user submits input requested by the command |
| `OnExit()` | When the command finishes. Must hand control back to the terminal master |
| `OnArrowUp()` / `OnArrowDown()` | On up / down arrow key press |

You rarely need to override all of them. For most commands `Init()` and `OnExit()` are enough, and `OnInputEnter` can be skipped entirely — use it only when your command needs to prompt the user for something.

### Available members

| Member | Purpose |
|---|---|
| `input` | The raw command line as entered |
| `separatedinput` | The command line split into tokens — use this to read flags and arguments |
| `buffer` | The terminal buffer. `PrintLine()`, `InsertInput()`, `SetFormat()`, `SetOrder()` |
| `master` | The terminal master. Pass it to `buffer.SetOrder()` in `OnExit()` to return control |
| `format` | The prompt string shown while the command is active |

### Handling flags

`separatedinput` holds the tokenized command line, so a call like:

```
rm -r -f myfolder
```

arrives as `["rm", "-r", "-f", "myfolder"]` — parse it however your command needs.

---

## Package layout

```
Assets/RetroConsole/
├── Prefabs/
│   ├── TermialCommands/     # command prefabs
│   └── Windows/             # Terminal window and base window prefabs
├── Presets/
│   └── ExternalCommands.asset
├── Resources/               # fonts, sprites, audio
├── Scenes/
│   └── Demo.unity           # working example
├── Scripts/
│   ├── Misc/                # constants, tokenizer, filesystem helpers, workzone
│   ├── Terminal/
│   │   ├── API/             # IOrder, TerminalCommand, ExternalCommands
│   │   └── Commands/        # built-in external commands
│   └── WindowBaseLogic/     # window move, resize, status bar
└── ThirdParty/
    └── TextMeshPro/         # modified TMP_InputField
```

---

## Third-party code

`Assets/RetroConsole/ThirdParty/TextMeshPro/TMP_InputFieldMod.cs` is a fork of Unity's `TMP_InputField`.

The fork exists because the console needs access to the input field's internal editing methods (`Backspace`, `Delete` and similar), which have no public or protected accessor. Working around this from the outside proved fragile, so the class was forked with minimal changes — access modifiers only, no logic changes.

This file is distributed under the Unity Companion License. See `Assets/RetroConsole/ThirdParty/LICENSE.md`. The rest of the package is MIT.

## Known limitations

- Console files (`historyrc`, `shellrc`, `bufferrc`) are written to
  `Application.dataPath`. On builds installed to a protected location
  this will fail — moving to `Application.persistentDataPath` is planned.
- Path separators are currently hardcoded for Windows.

---

## License

MIT, except for the contents of `Assets/RetroConsole/ThirdParty/` — see above.
