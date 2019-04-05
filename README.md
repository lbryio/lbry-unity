# lbry-unity

`lbry-unity` is an all-inclusive LBRY SDK for Unity.

This project is currently a work-in-progress.

See also https://github.com/lbryio/lbry-format

## Requirements

Dependencies: [NodeJS](https://nodejs.org/en/download/)
Verified in `Unity 2018.3` and `Unity 2019.1.0b8`, `Unity Hub` is recommended.
  
Does not work with `Unity 2017` and some prior alpha releases of `Unity 2019`.

## Usage

In your Unity project, import `lbry.unitypackage`.

`Assets` > `Import Package` > `Custom Package` > `lbry.unitypackage`

Package exports will be saved to `./Build/package.lbry`, these can be uploaded directly to LBRY.

## Limitations

Unity WebGL targets do not support threads (use co-routines instead) or some binary add-ons (must use addons that provide sources or WebASM targets)
