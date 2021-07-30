# Spectre.Terminals

A terminal abstraction with platform specific drivers.

## Disclaimer
This is a work in progress, and usage is not yet recommended.  
Things will change, move around and break.

## Acknowledgement

Inspired by [system-terminal](https://github.com/alexrp/system-terminal) written by Alex Rønne Petersen.

## Features

- [x] **Windows**
  - [x] STDIN
    - [x] Read Key
    - [x] Read Single Character (if raw mode is enabled)
    - [x] Read Line
    - [x] Get encoding
    - [x] Set encoding
    - [x] Redirect to custom reader
    - [x] Is handle redirected?
  - [x]  STDOUT/STDERR
    - [x] Write
    - [x] Get encoding
    - [x] Set encoding
    - [x] Redirect to custom writer
    - [x] Is handle redirected?
  - [x] Raw mode (enable/disable)
  - [x] Signals
    - [x] SIGINT (CTRL+C)
    - [x] SIGQUIT (CTRL+BREAK)
  - [x] Window
    - [x] Get width
    - [x] Get height
  - [x] VT/ANSI emulation (Windows only)
    - [x] CUU (Cursor up)
    - [x] CUD (Cursor down)
    - [x] CUF (Cursor forward)
    - [x] CUB (Cursor back)
    - [x] CNL (Cursor next line)
    - [x] CPL (Cursor previous line)
    - [x] CHA (Cursor horizontal absolute)
    - [X] CUP (Cursor position)
    - [X] ED (Erase in display)
    - [X] EL (Erase in line)
    - [x] SGR (Selection graphic rendition)
    - [x] SCP (Save current cursor position)
    - [x] RCP (Restore saved cursor position)
    - [x] DECTCEM 25 (Hide cursor)
    - [x] DECTCEM 25 (Show cursor)
    - [x] DECSET 1049 (Enable alternative buffer)
    - [x] DECSET 1049 (Disable alternative buffer)

- [x] **Linux**
  - [ ] STDIN
    - [ ] Read Key
    - [ ] Read Single Character
    - [ ] Read Line
    - [x] Get encoding
    - [x] Set encoding (NOT SUPPORTED)
    - [x] Redirect to custom reader
    - [x] Is handle redirected?
  - [x]  STDOUT/STDERR
    - [x] Write
    - [x] Get encoding
    - [x] Set encoding (NOT SUPPORTED)
    - [x] Redirect to custom writer
    - [x] Is handle redirected?
  - [x] Raw mode (enable/disable)
  - [x] Signals
    - [x] SIGINT
    - [x] SIGQUIT
  - [x] Window
    - [x] Get width
    - [x] Get height

- [x] **macOS**
  - [ ] STDIN
    - [ ] Read Key
    - [ ] Read Single Character
    - [ ] Read Line
    - [x] Get encoding
    - [x] Set encoding (NOT SUPPORTED)
    - [x] Redirect to custom reader
    - [x] Is handle redirected?
  - [x]  STDOUT/STDERR
    - [x] Write
    - [x] Get encoding
    - [x] Set encoding (NOT SUPPORTED)
    - [x] Redirect to custom writer
    - [x] Is handle redirected?
  - [x] Raw mode (enable/disable)
  - [x] Signals
    - [x] SIGINT
    - [x] SIGQUIT
  - [x] Window
    - [x] Get width
    - [x] Get height