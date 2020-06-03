﻿#pragma warning disable 1591

namespace OWML.Common.Menus
{
    public interface IModTabMenu : IModPopupMenu
    {
        void Initialize(TabButton tabButton);
        TabButton TabButton { get; }
        new IModTabMenu Copy();
        new IModTabMenu Copy(string title);
        new string Title { get; set; }
    }
}