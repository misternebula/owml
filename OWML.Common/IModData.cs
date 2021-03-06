﻿namespace OWML.Common
{
    public interface IModData
    {
        IModManifest Manifest { get; }
        IModConfig Config { get; }
        IModConfig DefaultConfig { get; }

        void ResetConfig();
    }
}