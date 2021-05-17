﻿namespace Microsoft.Maui.Handlers
{
	public partial class PickerHandler
	{
		public static PropertyMapper<IPicker, PickerHandler> PickerMapper = new PropertyMapper<IPicker, PickerHandler>(ViewHandler.ViewMapper)
		{
			[nameof(IPicker.CharacterSpacing)] = MapCharacterSpacing,
			[nameof(IPicker.Font)] = MapFont,
			[nameof(IPicker.SelectedIndex)] = MapSelectedIndex,
			[nameof(IPicker.TextColor)] = MapTextColor,
			[nameof(IPicker.Title)] = MapTitle,
			[nameof(IPicker.HorizontalTextAlignment)] = MapHorizontalTextAlignment,
			Actions =
			{
				["Reload"] = MapReload,
			}
		};

		public PickerHandler() : base(PickerMapper)
		{

		}

		public PickerHandler(PropertyMapper mapper) : base(mapper)
		{

		}
	}
}