# Spectre.Console.Terminal

A terminal abstraction with platform specific drivers.

## Features

- [x] STDIN
  - [x] Read
  - [x] Get encoding
  - [ ] Set encoding
  - [x] Redirect to custom reader
  - [x] Is handle redirected?
- [ ]  STDOUT/STDERR
  - [x] Write
  - [x] Get encoding
  - [ ] Set encoding
  - [x] Redirect to custom writer
  - [x] Is handle redirected?
- [x] Raw mode
- [ ] Signals
  - [ ] SIGINT
  - [ ] SIGQUIT
- [ ] Window
  - [ ] Get width
  - [ ] Get height
  - [ ] Auto refresh size

## ANSI emulation (Windows)

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
- [ ] SGR (Selection graphic rendition)
- [x] SCP (Save current cursor position)
- [x] RCP (Restore saved cursor position)

## Acknowledgement

Inspired by [system-terminal](https://github.com/alexrp/system-terminal) written by Alex Rønne Petersen.