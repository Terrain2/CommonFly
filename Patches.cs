using System;
using HarmonyLib;
using UnityEngine;
using UnityEngine.Audio;

namespace LibCommonFly
{
    [HarmonyPatch(typeof(PlayerMovement))]
    class CommonFlyMovement
    {
        [HarmonyPatch(nameof(PlayerMovement.Movement)), HarmonyPrefix]
        static bool Movement(PlayerMovement __instance, float x, float y)
        {
            __instance.playerCollider.enabled = !CommonFly.noclip || !CommonFly.flying;
            if (!CommonFly.flying) return true;
            if (__instance.grounded)
            {
                CommonFly.flying = false;
                return true;
            }
            __instance.rb.useGravity = false;
            __instance.UpdateCollisionChecks();
            __instance.x = x;
            __instance.y = y;
            if (__instance.dead) return false;
            __instance.CheckInput();

            var velocity = __instance.FindVelRelativeToLook();

            var v = 0f;
            if (CommonFly.flyDown) v -= 1f;
            if (CommonFly.flyUp) v += 1f;
            __instance.rb.velocity = new Vector3(__instance.rb.velocity.x, v * Mathf.Max(velocity.magnitude, 20f), __instance.rb.velocity.z);

            var moveSpeed = __instance.sprinting ? __instance.moveSpeed * 5f : __instance.moveSpeed;
            var forceZ = __instance.orientation.forward * y * moveSpeed * 0.02f;
            var forceX = __instance.orientation.right * x * moveSpeed * 0.02f;

            if ((x == 0 && y == 0) || Vector3.Dot((forceX + forceZ).normalized, __instance.rb.velocity.normalized) < 0)
            {
                __instance.rb.drag = 2f;
            }
            else
            {
                __instance.rb.drag = 0f;
            }

            __instance.rb.AddForce(forceZ);
            __instance.rb.AddForce(forceX);

            __instance.rb.AddForce(-__instance.orientation.forward * y * moveSpeed * 0.0002f);
            __instance.rb.AddForce(-__instance.orientation.right * x * moveSpeed * 0.0002f);
            return false;
        }

        [HarmonyPatch(nameof(PlayerMovement.crouching), MethodType.Getter), HarmonyPrefix]
        static bool crouching(bool __result)
        {
            if (CommonFly.flying)
            {
                __result = false;
                return false;
            }
            return true;
        }

        [HarmonyPatch(nameof(PlayerMovement.CheckInput)), HarmonyPostfix]
        static void CheckInput(PlayerMovement __instance)
        {
            if (CommonFly.flying) __instance.maxSpeed = float.PositiveInfinity;
        }
    }

    [HarmonyPatch(typeof(PlayerInput))]
    class CommonFlyInput
    {
        [HarmonyPatch(nameof(PlayerInput.MyInput)), HarmonyPostfix]
        static void MyInput()
        {
            CommonFly.flyUp = Input.GetKey(Main.up.Value);
            CommonFly.flyDown = Input.GetKey(Main.down.Value);
        }
    }

    [HarmonyPatch(typeof(MuckSettings.Settings))]
    class CommonFlySettings
    {
        [HarmonyPatch(nameof(MuckSettings.Settings.Controls)), HarmonyPostfix]
        static void Controls(MuckSettings.Settings.Page page)
        {
            page.AddControlSetting("Fly Up", Main.up);
            page.AddControlSetting("Fly Down", Main.down);
        }
    }
}