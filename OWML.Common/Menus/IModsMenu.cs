﻿#pragma warning disable 1591

namespace OWML.Common.Menus
{
    public interface IModsMenu : IModPopupMenu
    {
        void AddMod(IModData modData, IModBehaviour mod);
        IModConfigMenu GetModMenu(IModBehaviour modBehaviour);
        void Initialize(IModMainMenu mainMenu);
        void Initialize(IModPauseMenu pauseMenu);
    }
}
