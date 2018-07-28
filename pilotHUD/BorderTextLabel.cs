/*
Original code taken from here:
https://github.com/fengdingfeilong/HUD
*/

/*
The MIT License (MIT)

Copyright (c) 2016 fengdingfeilong

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE. 
*/
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace pilotHUD
{
  public class BorderTextLabel : System.Windows.Controls.Label
  {
    private static void Redraw(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
      ((BorderTextLabel)d).InvalidateVisual();
    }
    public string Text
    {
      get { return (string)GetValue(TextProperty); }
      set { SetValue(TextProperty, value); }
    }
    public static readonly DependencyProperty TextProperty = DependencyProperty.Register("Text", typeof(string), typeof(BorderTextLabel), new FrameworkPropertyMetadata(string.Empty, Redraw));

    public Brush Stroke
    {
      get { return (Brush)GetValue(StrokeProperty); }
      set { SetValue(StrokeProperty, value); }
    }
    public static readonly DependencyProperty StrokeProperty = DependencyProperty.Register("Stroke", typeof(Brush), typeof(BorderTextLabel), new FrameworkPropertyMetadata(Brushes.Black, Redraw));

    public double StrokeThickness
    {
      get { return (double)GetValue(StrokeThicknessProperty); }
      set { SetValue(StrokeThicknessProperty, value); }
    }
    public static readonly DependencyProperty StrokeThicknessProperty = DependencyProperty.Register("StrokeThickness", typeof(double), typeof(BorderTextLabel), new FrameworkPropertyMetadata((double)1, Redraw));

    protected override void OnRender(DrawingContext drawingContext)
    {
      base.OnRender(drawingContext);
      FormattedText formattedText = new FormattedText(this.Text, CultureInfo.CurrentCulture, FlowDirection, new Typeface(FontFamily, FontStyle, FontWeight, FontStretch), FontSize, this.Foreground);
      if (double.IsNaN(this.Width))
        this.Width = formattedText.Width;
      if (double.IsNaN(this.Height))
        this.Height = formattedText.Height;
      Point startp = new Point(0, 0);
      if (this.HorizontalContentAlignment == HorizontalAlignment.Right) startp.X = this.Width - formattedText.Width;
      if (this.HorizontalContentAlignment == HorizontalAlignment.Center) startp.X = (this.Width - formattedText.Width) / 2;
      if (this.VerticalContentAlignment == VerticalAlignment.Bottom) startp.X = this.Height - formattedText.Height;
      if (this.VerticalContentAlignment == VerticalAlignment.Center) startp.X = (this.Height - formattedText.Height) / 2;
      var textgeometry = formattedText.BuildGeometry(startp);
      drawingContext.DrawGeometry(this.Foreground, new Pen(Stroke, StrokeThickness), textgeometry);
    }

  }
}
