# Common Fly

This is a helper mod that implements a fly mode that other plugins can hook into and enable/disable. This mod handles the movement and inputs, but not enabling/disabling the fly mode. **By itself, this plugin does NOTHING.** (other than add 2 settings that are ignored)

## Usage

First, add a ``[BepInDependency("Terrain.CommonFly")]`` annotation to your plugin.

- To enable/disable flying, simply set the ``CommonFly.flying`` property to true/false.
- There's also a ``CommonFly.noclip`` property, which prevents the fly mode from suddenly ending when you hit solid objects. Specifically, it completely disables your player collider client-side.

Both of these are auto properties instead of fields for your patching convenience, because then you can use something like ``[HarmonyPatch(typeof(CommonFly), nameof(CommonFly.flying), MethodType.Getter)]`` if you need to change the value when it's used (which is like 3-4 times every physics frame or something actually).

The other two properties ``CommonFly.flyUp`` and ``CommonFly.flyDown`` are the actual inputs sent by the player. You probably shouldn't mess with their setters, but you can [Reverse Patch](https://harmony.pardeike.net/articles/reverse-patching.html) them if you really want to anyways.

---

Icon made by [Shocho](https://thenounproject.com/search/?q=flight&i=1606615). I haven't modified the icon outside of the preview options The Noun Project provides.