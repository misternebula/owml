﻿using System;
using System.Reflection;
using Harmony;
using OWML.Common;

namespace OWML.ModHelper.Events
{
    public class HarmonyHelper : IHarmonyHelper
    {
        private readonly IModLogger _logger;
        private readonly IModConsole _console;
        private readonly HarmonyInstance _harmony;

        public HarmonyHelper(IModLogger logger, IModConsole console)
        {
            _logger = logger;
            _console = console;
            _harmony = CreateInstance();
        }

        private HarmonyInstance CreateInstance()
        {
            HarmonyInstance harmony;
            try
            {
                _logger.Log("Creating harmony instance");
                harmony = HarmonyInstance.Create("com.alek.owml");
                _logger.Log("Created harmony instance");
            }
            catch (Exception ex)
            {
                _console.WriteLine($"Exception while creating harmony instance: {ex}");
                return null;
            }
            if (harmony == null)
            {
                _console.WriteLine("Error: harmony instance is null");
            }
            return harmony;
        }

        private MethodInfo GetMethod<T>(string methodName)
        {
            var targetType = typeof(T);
            try
            {
                _logger.Log($"Getting method {methodName} of {targetType.Name}");
                return targetType.GetAnyMethod(methodName);
            }
            catch (Exception ex)
            {
                _console.WriteLine($"Exception while getting method {methodName} of {targetType.Name}: {ex}");
                return null;
            }
        }

        /// <summary>Add prefix patch to the given method.</summary>
        /// <typeparam name="T">The type the method is in.</typeparam>
        /// <param name="methodName">The name of the method.</param>
        /// <param name="patchType">The type the patch is in.</param>
        /// <param name="patchMethodName">The name of the patch method.</param>
        public void AddPrefix<T>(string methodName, Type patchType, string patchMethodName)
        {
            AddPrefix(GetMethod<T>(methodName), patchType, patchMethodName);
        }

        /// <summary>Add prefix patch to the given method.</summary>
        /// <param name="original">The method to patch.</param>
        /// <param name="patchType">The type the patch is in.</param>
        /// <param name="patchMethodName">The name of the patch method.</param>
        public void AddPrefix(MethodInfo original, Type patchType, string patchMethodName)
        {
            var prefix = patchType.GetAnyMethod(patchMethodName);
            if (prefix == null)
            {
                _console.WriteLine($"Error in {nameof(AddPrefix)}: {patchType.Name}.{patchMethodName} is null");
                return;
            }
            Patch(original, prefix, null, null);
        }

        /// <summary>Add postfix patch to the given method.</summary>
        /// <typeparam name="T">The type the method is in.</typeparam>
        /// <param name="methodName">The name of the method.</param>
        /// <param name="patchType">The type the patch is in.</param>
        /// <param name="patchMethodName">The name of the patch method.</param>
        public void AddPostfix<T>(string methodName, Type patchType, string patchMethodName)
        {
            AddPostfix(GetMethod<T>(methodName), patchType, patchMethodName);
        }

        /// <summary>Add postfix patch to the given method.</summary>
        /// <param name="original">The method to patch.</param>
        /// <param name="patchType">The type the patch is in.</param>
        /// <param name="patchMethodName">The name of the patch method.</param>
        public void AddPostfix(MethodInfo original, Type patchType, string patchMethodName)
        {
            var postfix = patchType.GetAnyMethod(patchMethodName);
            if (postfix == null)
            {
                _console.WriteLine($"Error in {nameof(AddPostfix)}: {patchType.Name}.{patchMethodName} is null");
                return;
            }
            Patch(original, null, postfix, null);
        }

        /// <summary>Empty the given method.</summary>
        /// <typeparam name="T">The type the method is in.</typeparam>
        /// <param name="methodName">The name of the method.</param>
        public void EmptyMethod<T>(string methodName)
        {
            EmptyMethod(GetMethod<T>(methodName));
        }

        /// <summary>Empty the given method.</summary>
        /// <param name="methodInfo">The method to clear.</param>
        public void EmptyMethod(MethodInfo methodInfo)
        {
            Transpile(methodInfo, typeof(Patches), nameof(Patches.EmptyMethod));
        }

        /// <summary>Add transpile patch to the given method.</summary>
        /// <typeparam name="T">The type the method is in.</typeparam>
        /// <param name="methodName">The name of the method.</param>
        /// <param name="patchType">The type the patch is in.</param>
        /// <param name="patchMethodName">The name of the patch method.</param>
        public void Transpile<T>(string methodName, Type patchType, string patchMethodName)
        {
            Transpile(GetMethod<T>(methodName), patchType, patchMethodName);
        }

        /// <summary>Add transpile patch to the given method.</summary>
        /// <param name="original">The method to patch.</param>
        /// <param name="patchType">The type the patch is in.</param>
        /// <param name="patchMethodName">The name of the patch method.</param>
        public void Transpile(MethodInfo original, Type patchType, string patchMethodName)
        {
            var patchMethod = patchType.GetAnyMethod(patchMethodName);
            if (patchMethod == null)
            {
                _console.WriteLine($"Error in {nameof(Transpile)}: {patchType.Name}.{patchMethodName} is null");
                return;
            }
            Patch(original, null, null, patchMethod);
        }

        private void Patch(MethodInfo original, MethodInfo prefix, MethodInfo postfix, MethodInfo transpiler)
        {
            if (original == null)
            {
                _console.WriteLine($"Error in {nameof(Patch)}: original MethodInfo is null");
                return;
            }
            var prefixMethod = prefix == null ? null : new HarmonyMethod(prefix);
            var postfixMethod = postfix == null ? null : new HarmonyMethod(postfix);
            var transpilerMethod = transpiler == null ? null : new HarmonyMethod(transpiler);
            var fullName = $"{original.DeclaringType}.{original.Name}";
            try
            {
                _harmony.Patch(original, prefixMethod, postfixMethod, transpilerMethod);
                _logger.Log($"Patched {fullName}!");
            }
            catch (Exception ex)
            {
                _console.WriteLine($"Exception while patching {fullName}: {ex}");
            }
        }

    }
}
