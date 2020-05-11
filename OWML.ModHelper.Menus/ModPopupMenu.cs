﻿using System;
using OWML.Common;
using OWML.Common.Menus;
using OWML.ModHelper.Events;
using UnityEngine;
using UnityEngine.UI;

namespace OWML.ModHelper.Menus
{
    public class ModPopupMenu : ModMenu, IModPopupMenu
    {
        public event Action OnOpen;
        public event Action OnClose;

        public bool IsOpen { get; private set; }

        private Text _title;
        public string Title
        {
            get => _title.text;
            set => _title.text = value;
        }

        private readonly IModConsole _console;

        public ModPopupMenu(IModConsole console) : base(console)
        {
            _console = console;
        }

        public override void Initialize(Menu menu, LayoutGroup layoutGroup)
        {
            base.Initialize(menu, layoutGroup);
            _title = Menu.GetComponentInChildren<Text>();
            var localizedText = _title.GetComponent<LocalizedText>();
            if (localizedText != null)
            {
                Title = UITextLibrary.GetString(localizedText.GetValue<UITextType>("_textID"));
                GameObject.Destroy(localizedText);
            }
            Menu.OnActivateMenu += OnActivateMenu;
            Menu.OnDeactivateMenu += OnDeactivateMenu;
        }

        private void OnDeactivateMenu()
        {
            IsOpen = false;
            OnClose?.Invoke();
        }

        private void OnActivateMenu()
        {
            IsOpen = true;
            OnOpen?.Invoke();
        }

        public virtual void Open()
        {
            if (Menu == null)
            {
                _console.WriteLine("Warning: can't open menu, it doesn't exist.");
                return;
            }
            SelectFirst();
            Menu.EnableMenu(true);
        }

        public void Close()
        {
            if (Menu == null)
            {
                _console.WriteLine("Warning: can't close menu, it doesn't exist.");
                return;
            }
            Menu.EnableMenu(false);
        }

        public void Toggle()
        {
            if (IsOpen)
            {
                Close();
            }
            else
            {
                Open();
            }
        }

        public IModPopupMenu Copy()
        {
            if (Menu == null)
            {
                _console.WriteLine("Warning: can't copy menu, it doesn't exist.");
                return null;
            }
            var menu = GameObject.Instantiate(Menu, Menu.transform.parent);
            var modMenu = new ModPopupMenu(_console);
            modMenu.Initialize(menu);
            return modMenu;
        }

        public IModPopupMenu Copy(string title)
        {
            var copy = Copy();
            copy.Title = title;
            return copy;
        }

    }
}
