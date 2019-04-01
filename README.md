# lbry-unity

`lbry-unity` is an all-inclusive LBRY SDK for Unity.

This project is currently a work-in-progress.

See also https://github.com/lbryio/lbry-format

## Requirements

Dependencies: `NodeJS`  
Verified in `Unity 2018.3`. Does not work with `Unity 2017` or `Unity 2019` (in Beta as of writing)

## Usage

In your Unity project, import `lbry.unitypackage`.

## Limitations

Unity WebGL targets do not support threads (use co-routines instead) or some binary add-ons (must use addons that provide sources or WebASM targets)
