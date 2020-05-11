﻿using OWML.Common.Menus;
using OWML.ModHelper.Events;
using UnityEngine.UI;

namespace OWML.ModHelper.Menus
{
    public abstract class ModInputField<T> : ModInput<T>
    {
        public IModButton Button { get; }

        protected readonly IModInputMenu InputMenu;
        protected readonly TwoButtonToggleElement ToggleElement;

        protected ModInputField(TwoButtonToggleElement toggle, IModMenu menu, IModInputMenu inputMenu) : base(toggle, menu)
        {
            ToggleElement = toggle;
            InputMenu = inputMenu;

            Button = new ModButton(toggle.GetValue<Button>("_buttonTrue"), menu);
            Button.OnClick += Open;

            var noButton = ToggleElement.GetValue<Button>("_buttonFalse");
            noButton.transform.parent.gameObject.SetActive(false);

            var buttonParent = Button.Button.transform.parent;
            var layoutGroup = buttonParent.parent.GetComponent<HorizontalLayoutGroup>();
            layoutGroup.childControlWidth = true;
            layoutGroup.childForceExpandWidth = true;

            buttonParent.GetComponent<LayoutElement>().preferredWidth = 100;
        }

        protected abstract void Open();

    }
}
