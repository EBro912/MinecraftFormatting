# MinecraftFormatting
 Adds Minecraft chat formatting to the in-game text chat in Lethal Company.

## Usage
To format your chat, type the ampersand `&` symbol and then a specific letter/number to set the current color or format. Colors and formats will persist in your message until modified by another formatter. You can find a list of formats [here](https://minecraft.fandom.com/wiki/Formatting_codes). The magic formatter (`&k`) is not supported.

An example is `&aHello!`, which will send the text `Hello!` in a lime green color.

It should be noted that the mod does not modify the chat range mechanic of the game, so players far away from you will still not be able to see your chat messages.

## Installation
Requires the latest version of [BepInEx 5](https://github.com/BepInEx/BepInEx). After BepInEx has been installed, drag `MinecraftFormatting.dll` into the `BepInEx/plugins` folder in the game's root directory.
