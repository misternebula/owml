﻿using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;
using OWML.Common;
using OWML.Common.Menus;
using OWML.ModHelper.Events;
using UnityEngine;
using UnityEngine.UI;

namespace OWML.ModHelper.Menus
{
    public class ModMenu : IModMenu
    {
        public event Action OnInit;

        public Menu Menu { get; protected set; }
        public List<IModButton> Buttons { get; private set; }
        public List<IModToggleInput> ToggleInputs { get; private set; }
        public List<IModSliderInput> SliderInputs { get; private set; }
        public List<IModTextInput> TextInputs { get; private set; }
        public List<IModNumberInput> NumberInputs { get; private set; }

        private readonly IModConsole _console;
        private LayoutGroup _layoutGroup;

        public ModMenu(IModConsole console)
        {
            _console = console;
        }

        public virtual void Initialize(Menu menu)
        {
            var root = menu.GetValue<GameObject>("_selectableItemsRoot") ?? menu.GetValue<GameObject>("_menuActivationRoot");
            var layoutGroup = root.GetComponent<LayoutGroup>() ?? root.GetComponentInChildren<LayoutGroup>();
            Initialize(menu, layoutGroup);
        }

        public virtual void Initialize(Menu menu, LayoutGroup layoutGroup)
        {
            Menu = menu;
            _layoutGroup = layoutGroup;
            Buttons = Menu.GetComponentsInChildren<Button>().Select(x => new ModButton(x, this)).Cast<IModButton>().ToList();
            ToggleInputs = Menu.GetComponentsInChildren<TwoButtonToggleElement>().Select(x => new ModToggleInput(x, this)).Cast<IModToggleInput>().ToList();
            SliderInputs = Menu.GetComponentsInChildren<SliderElement>().Select(x => new ModSliderInput(x, this)).Cast<IModSliderInput>().ToList();
            TextInputs = new List<IModTextInput>();
            NumberInputs = new List<IModNumberInput>();
        }

        public IModButton GetButton(string title)
        {
            var button = Buttons.FirstOrDefault(x => x.Title == title || x.Button.name == title);
            if (button == null)
            {
                _console.WriteLine("Warning: no button found with title or name: " + title);
            }
            return button;
        }

        public IModButton AddButton(IModButton button)
        {
            return AddButton(button, button.Index);
        }

        public virtual IModButton AddButton(IModButton button, int index)
        {
            var transform = button.Button.transform;
            var scale = transform.localScale;
            transform.parent = _layoutGroup.transform;
            button.Index = index;
            button.Initialize(this);
            Buttons.Add(button);
            button.Button.transform.localScale = scale;
            return button;
        }

        public IModToggleInput GetToggleInput(string title)
        {
            return ToggleInputs.FirstOrDefault(x => x.Title == title || x.Element.name == title);
        }

        public IModToggleInput AddToggleInput(IModToggleInput input)
        {
            return AddToggleInput(input, input.Index);
        }

        public IModToggleInput AddToggleInput(IModToggleInput input, int index)
        {
            ToggleInputs.Add(input);
            AddInput(input, index);
            return input;
        }

        public IModSliderInput GetSliderInput(string title)
        {
            return SliderInputs.FirstOrDefault(x => x.Title == title || x.Element.name == title);
        }

        public IModSliderInput AddSliderInput(IModSliderInput input)
        {
            return AddSliderInput(input, input.Index);
        }

        public IModSliderInput AddSliderInput(IModSliderInput input, int index)
        {
            SliderInputs.Add(input);
            AddInput(input, index);
            return input;
        }

        public IModTextInput GetTextInput(string title)
        {
            return TextInputs.FirstOrDefault(x => x.Title == title || x.Element.name == title);
        }

        public IModTextInput AddTextInput(IModTextInput input)
        {
            return AddTextInput(input, input.Index);
        }

        public IModTextInput AddTextInput(IModTextInput input, int index)
        {
            TextInputs.Add(input);
            AddInput(input, index);
            return input;
        }

        public IModNumberInput GetNumberInput(string title)
        {
            return NumberInputs.FirstOrDefault(x => x.Title == title || x.Element.name == title);
        }

        public IModNumberInput AddNumberInput(IModNumberInput input)
        {
            return AddNumberInput(input, input.Index);
        }

        public IModNumberInput AddNumberInput(IModNumberInput input, int index)
        {
            NumberInputs.Add(input);
            AddInput(input, index);
            return input;
        }

        private void AddInput<T>(IModInput<T> input, int index)
        {
            var transform = input.Element.transform;
            var scale = transform.localScale;
            transform.parent = _layoutGroup.transform;
            input.Index = index;
            input.Initialize(this);
            input.Element.transform.localScale = scale;
        }

        public object GetInputValue(string key)
        {
            var slider = GetSliderInput(key);
            if (slider != null)
            {
                return slider.Value;
            }
            var toggle = GetToggleInput(key);
            if (toggle != null)
            {
                return toggle.Value;
            }
            var textInput = GetTextInput(key);
            if (textInput != null)
            {
                return textInput.Value;
            }
            var numberInput = GetNumberInput(key);
            if (numberInput != null)
            {
                return numberInput.Value;
            }
            _console.WriteLine("Error: no input found with name " + key);
            return null;
        }

        public void SetInputValue(string key, object value)
        {
            var slider = GetSliderInput(key);
            if (slider != null)
            {
                var val = value is JObject obj ? obj["value"] : value;
                slider.Value = Convert.ToSingle(val);
                return;
            }
            var toggle = GetToggleInput(key);
            if (toggle != null)
            {
                var val = value is JObject obj ? obj["value"] : value;
                toggle.Value = Convert.ToBoolean(val);
                return;
            }
            var textInput = GetTextInput(key);
            if (textInput != null)
            {
                var val = value is JObject obj ? obj["value"] : value;
                textInput.Value = Convert.ToString(val);
                return;
            }
            var numberInput = GetNumberInput(key);
            if (numberInput != null)
            {
                var val = value is JObject obj ? obj["value"] : value;
                numberInput.Value = Convert.ToSingle(val);
                return;
            }
            _console.WriteLine("Error: no input found with name " + key);
        }

        protected void InvokeOnInit()
        {
            OnInit?.Invoke();
        }

        public void SelectFirst()
        {
            var firstSelectable = Menu.GetComponentInChildren<Selectable>();
            Locator.GetMenuInputModule().SelectOnNextUpdate(firstSelectable);
            Menu.SetSelectOnActivate(firstSelectable);
        }

        public void UpdateNavigation()
        {
            var selectables = Menu.GetComponentsInChildren<TooltipSelectable>()
                .Select(x => x.GetComponent<Selectable>())
                .Where(x => x != null).ToList();
            for (var i = 0; i < selectables.Count; i++)
            {
                var upIndex = (i - 1 + selectables.Count) % selectables.Count;
                var downIndex = (i + 1) % selectables.Count;
                var navigation = selectables[i].navigation;
                navigation.selectOnUp = selectables[upIndex];
                navigation.selectOnDown = selectables[downIndex];
                selectables[i].navigation = navigation;
            }
        }

    }
}
