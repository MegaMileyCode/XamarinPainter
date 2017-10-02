//
// PainterTestbed.FormsPage.xaml.cs
//
// Author:
//     Miley Hollenberg
//
// Copyright (c) 2017 Nitrocrime
//
//
using System;
using System.Diagnostics;
using System.IO;
using Painter.Forms;
using PainterTestbed;
using Xamarin.Forms;
using Painter.Interfaces;

namespace PainterTestbed.Forms
{
	public partial class PainterTestbed_FormsPage : ContentPage
	{
        public PainterTestbed_FormsPage()
		{
			InitializeComponent();

            painterView.StrokeColor = new Painter.Abstractions.Color(0, 1, 0, 1);
			painterView.Initialized += (sender, e) =>
			{
				painterView.LoadImage("background.jpg", true, Painter.Abstractions.Scaling.Absolute_Fit);
			};
            painterView.FinishedStrokeEvent = PainterView_GetFinishedData;
        }

        private void PainterView_GetFinishedData(object sender, EventArgs e)
        {
            // Done with saving etc
            // do what you want..
        }

        private async void SaveJson(object sender, System.EventArgs e)
		{
			var data = await painterView.GetJson();
			await DependencyService.Get<ISaveAndLoad>().SaveTextAsync("image.json", data);
		}

        private async void SaveImage()
        {
            IPainterExport export = DependencyService.Get<IPainterExport>();
            var test = DependencyService.Get<ISaveAndLoad>().GetFileBinary("/storage/emulated/0/Android/data/com.gappless.forms.droid.test/files/Pictures/Gappless/camera_69687f19-e233-4a4d-b0d8-8fee8f54917b", false);

            //var strokes = painterView.GetStrokes();
            var data = await export.ExportCurrentImage((int)painterView.Width, (int)painterView.Height, painterView.GetStrokes(), Painter.Abstractions.Scaling.Absolute_Fit, Painter.Abstractions.ExportFormat.Png, 80, new Painter.Abstractions.Color(1, 1, 1, 1), test);
            var base64 = Convert.ToBase64String(data);

            Debug.WriteLine(data.Length);
        }

		private void Clear(object sender, System.EventArgs e)
		{
			painterView.Clear();
		}

		private async void LoadJson(object sender, System.EventArgs e)
		{
			var data = await DependencyService.Get<ISaveAndLoad>().LoadTextAsync("image.json");
			painterView.LoadJson(data);
		}

		private void setRedColor(object sender, EventArgs e)
		{
			painterView.StrokeColor = new Painter.Abstractions.Color(1, 0, 0);
		}

		private void setGreenColor(object sender, EventArgs e)
		{
			painterView.StrokeColor = new Painter.Abstractions.Color(0, 1, 0);
		}

		private void setBlueColor(object sender, EventArgs e)
		{
			painterView.StrokeColor = new Painter.Abstractions.Color(0, 0, 1);
		}

		private void StepperChanged(object sender, EventArgs e)
		{
			painterView.StrokeThickness = (int)stepper.Value;
		}
	}
}
