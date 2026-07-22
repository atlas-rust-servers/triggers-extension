# Triggers Framework
Triggers framework for [Rust](https://store.steampowered.com/app/252490/Rust/) using the [Oxide/uMod](https://umod.org) or [Carbon](https://carbonmod.gg) extension platforms, exposing base objects to handle player collisions.

## Dependencies
1. [Gizmos Framework](https://github.com/ilovepatatos-rust/gizmos-extension)
2. [Ilovepatatos Framework](https://github.com/ilovepatatos-rust/ilovepatatos-extension)

## Getting Started
Download the artifact for your extension platform from the latest release, along with the matching-platform dependency artifacts listed above.

### Oxide/uMod
1. Download `Oxide.Ext.TriggersExt.dll`.
2. Put it and the Oxide dependency DLLs into the `RustDedicated_Data\Managed` folder.
3. Restart the server.

### Carbon
1. Download `Carbon.Ext.TriggersExt.dll`.
2. Put it and the Carbon dependency DLLs into the `carbon\extensions` folder.
3. Restart the server.
