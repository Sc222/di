﻿using    FractalPainting.Infrastructure.Common;
using    FractalPainting.Solved.Step09.Infrastructure.UiActions;

namespace    FractalPainting.Solved.Step09.App.Actions
{
	public    class    ImageSettingsAction    :    IUiAction
	{
	                private    readonly    IImageHolder    imageHolder;
	                private    readonly    ImageSettings    imageSettings;

	                public    ImageSettingsAction(IImageHolder    imageHolder,
                                                ImageSettings    imageSettings)
	                {
	                                this.imageHolder    =    imageHolder;
	                                this.imageSettings    =    imageSettings;
	                }

		public    string    Category    =>    "Настройки";
		public    string    Name    =>    "Изображение...";
		public    string    Description    =>    "Размеры    изображения";

		public    void    Perform()
		{
			SettingsForm.For(imageSettings).ShowDialog();
			imageHolder.RecreateImage(imageSettings);
		}
	}
}