using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace pilotHUD
{
  /// <summary>
  /// Interaction logic for hudControl.xaml
  /// </summary>
  public partial class hudControl : UserControl
  {
    public hudControl()
    {
      InitializeComponent();
    }
    private static void GestureChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
      ((hudControl)d).InvalidateVisual();
    }

    public double RollAngle
    {
      get { return (double)GetValue(RollAngleProperty); }
      set { SetValue(RollAngleProperty, value); }
    }
    public static readonly DependencyProperty RollAngleProperty =
        DependencyProperty.Register("RollAngle", typeof(double), typeof(hudControl), new FrameworkPropertyMetadata((double)0, GestureChangedCallback));

    public double PitchAngle
    {
      get { return (double)GetValue(PitchAngleProperty); }
      set { SetValue(PitchAngleProperty, value); }
    }
    public static readonly DependencyProperty PitchAngleProperty =
        DependencyProperty.Register("PitchAngle", typeof(double), typeof(hudControl), new FrameworkPropertyMetadata((double)0, GestureChangedCallback));

    public double YawAngle
    {
      get { return (double)GetValue(YawAngleProperty); }
      set { SetValue(YawAngleProperty, value); }
    }
    public static readonly DependencyProperty YawAngleProperty =
        DependencyProperty.Register("YawAngle", typeof(double), typeof(hudControl), new FrameworkPropertyMetadata((double)0, GestureChangedCallback));

    private const int VERTICAL_DEG_TO_DISP = 26;
    private const int YAW_COMPASS_DEG_TO_DISP = 26;

    void DrawGroundAndSky(double pitchDeg)
    {
      double vertPixelsPerDeg = Grid_Viewport.ActualHeight / VERTICAL_DEG_TO_DISP;

      double offset = pitchDeg * vertPixelsPerDeg;

      // want to always make sure the canvas is filled by the sky and/or ground rectangles
      // - therefore, create them oversized and limit max offset, to prevent any issues
      // with silly window sizes/aspect ratios
      double maxDim = Grid_Viewport.ActualWidth;
      if (Grid_Viewport.ActualHeight > maxDim)
      {
        maxDim = Grid_Viewport.ActualHeight;
      }
      if (offset > maxDim)
      {
        offset = maxDim;
      }
      else if (offset < -maxDim)
      {
        offset = -maxDim;
      }
      const double OVERSIZE_RATIO = 5;
      double rectDimension = maxDim * OVERSIZE_RATIO;

      var gndRect = new Rectangle();
      gndRect.Stroke = new SolidColorBrush(Colors.Orange);
      gndRect.Fill = new SolidColorBrush(Colors.Orange);
      gndRect.Width = rectDimension;
      gndRect.Height = rectDimension;
      Canvas.SetLeft(gndRect, -maxDim);
      Canvas.SetTop(gndRect, offset);

      var skyRect = new Rectangle();
      skyRect.Stroke = new SolidColorBrush(Colors.Blue);
      skyRect.Fill = new SolidColorBrush(Colors.Blue);
      skyRect.Width = rectDimension;
      skyRect.Height = rectDimension;
      Canvas.SetLeft(skyRect, -maxDim);
      Canvas.SetBottom(skyRect, -offset);

      Canvas_HUD_RollRotation.Children.Add(gndRect);
      Canvas_HUD_RollRotation.Children.Add(skyRect);

    }

    private void DrawMajorPitchTick(double offset, double val, bool dispTxt)
    {
      Line ln = new Line();
      ln.X1 = 0;
      ln.X2 = 160;
      ln.Y1 = 0;
      ln.Y2 = 0;
      ln.Stroke = Brushes.White;
      ln.StrokeThickness = 2;
      Canvas.SetLeft(ln, -80);
      Canvas.SetTop(ln, -offset);
      Canvas_PitchIndicator.Children.Add(ln);

      if (val != 0)
      { 
        Line left = new Line();
        left.X1 = 0;
        left.X2 = 0;
        left.Y1 = 0;
        left.Y2 = 7;
        left.Stroke = Brushes.White;
        left.StrokeThickness = 2;
        Canvas.SetLeft(left, -80);
        Canvas.SetTop(left, -offset);

        Line right = new Line();
        right.X1 = 0;
        right.X2 = 0;
        right.Y1 = 0;
        right.Y2 = 7;
        right.Stroke = Brushes.White;
        right.StrokeThickness = 2;
        Canvas.SetRight(right, -80);
        Canvas.SetTop(right, -offset);

        if (val < 0)
        {
          left.Y2 = -left.Y2;
          right.Y2 = -right.Y2;
        }
        Canvas_PitchIndicator.Children.Add(left);
        Canvas_PitchIndicator.Children.Add(right);
      }

      if (true == dispTxt)
      {
        var txtBlkL = new BorderTextLabel();
        txtBlkL.Stroke = Brushes.DimGray;
        txtBlkL.HorizontalContentAlignment = HorizontalAlignment.Left;
        txtBlkL.Text = val.ToString("##0");
        txtBlkL.Foreground = Brushes.White;
        txtBlkL.FontSize = 16;
        txtBlkL.FontWeight = FontWeights.Bold;
        Canvas.SetTop(txtBlkL, -offset - 13);
        Canvas.SetLeft(txtBlkL, -160);
        Canvas_PitchIndicator.Children.Add(txtBlkL);

        var txtBlkR = new BorderTextLabel();
        txtBlkR.Stroke = Brushes.DimGray;
        txtBlkR.HorizontalContentAlignment = HorizontalAlignment.Right;
        txtBlkR.Text = val.ToString("##0");
        txtBlkR.Foreground = Brushes.White;
        txtBlkR.FontSize = 16;
        txtBlkR.FontWeight = FontWeights.Bold;
        Canvas.SetTop(txtBlkR, -offset - 13);
        Canvas.SetRight(txtBlkR, -160);
        Canvas_PitchIndicator.Children.Add(txtBlkR);

      }

    }

    private void DrawMinorPitchTick(double offset, double val)
    {
      Line ln = new Line();
      ln.X1 = 0;
      ln.X2 = 80;
      ln.Y1 = 0;
      ln.Y2 = 0;
      ln.Stroke = Brushes.White;
      ln.StrokeThickness = 1;
      Canvas.SetLeft(ln, -40);
      Canvas.SetTop(ln, -offset);
      Canvas_PitchIndicator.Children.Add(ln);

      if (val != 0)
      {
        Line left = new Line();
        left.X1 = 0;
        left.X2 = 0;
        left.Y1 = 0;
        left.Y2 = 5;
        left.Stroke = Brushes.White;
        left.StrokeThickness = 1;
        Canvas.SetLeft(left, -40);
        Canvas.SetTop(left, -offset);

        Line right = new Line();
        right.X1 = 0;
        right.X2 = 0;
        right.Y1 = 0;
        right.Y2 = 5;
        right.Stroke = Brushes.White;
        right.StrokeThickness = 1;
        Canvas.SetRight(right, -40);
        Canvas.SetTop(right, -offset);

        if (val < 0)
        {
          left.Y2 = -left.Y2;
          right.Y2 = -right.Y2;
        }
        Canvas_PitchIndicator.Children.Add(left);
        Canvas_PitchIndicator.Children.Add(right);
      }
    }

    private void DrawPitchTicks(double pitchDeg, double rollAngle)
    {
      double vertPixelsPerDeg = Grid_Viewport.ActualHeight / VERTICAL_DEG_TO_DISP;
      
      double zeroOffset = -(pitchDeg * vertPixelsPerDeg);

      // the pitch indicator grid is only a percentage of the overall viewport height
      // - need to account for this (so pitch '0' indicator lines up with ground/sky border)
      double gridOffset = ((Grid_Viewport.ActualHeight - Grid_PitchIndicator.ActualHeight) / 2.0) * 
        Math.Cos(rollAngle * Math.PI / 180.0);

      zeroOffset += gridOffset;

      DrawMajorPitchTick(zeroOffset, 0, false);

      for (int i = 1; i < 10; i++)
      {
        double pitchVal = i * 10;
        double offset = (pitchVal * vertPixelsPerDeg) + zeroOffset;
        DrawMajorPitchTick(offset, pitchVal, true);

        offset -= (5 * vertPixelsPerDeg);
        DrawMinorPitchTick(offset, pitchVal - 5);

        offset = -(pitchVal * vertPixelsPerDeg) + zeroOffset;
        DrawMajorPitchTick(offset, -pitchVal, true);

        offset += (5 * vertPixelsPerDeg);
        DrawMinorPitchTick(offset, -pitchVal + 5);
      }
    }

    private void DrawCompass(double yawDeg)
    {
      double wl = Grid_Compass.ActualWidth;
 
      double horzPixelsPerDeg = wl / YAW_COMPASS_DEG_TO_DISP;

      double startYaw = yawDeg - (YAW_COMPASS_DEG_TO_DISP / 2.0);
      int roundedStart = (int)Math.Ceiling(startYaw);

      // this is the x co-ord of the left-most tick to be displayed displayed
      double tickOffset = (roundedStart - startYaw) * horzPixelsPerDeg;

      for (int i = 0; i < YAW_COMPASS_DEG_TO_DISP; i++)
      {
        if (0 == ((i + roundedStart) % 2))
        {
          Line tl = new Line();
          tl.X1 = tickOffset + (i * horzPixelsPerDeg);
          tl.X2 = tl.X1;

          if (0 == ((i + roundedStart) % 10))
          {
            tl.Y1 = 21;
            var ticktext = new BorderTextLabel();
            ticktext.FontFamily = new FontFamily("Courier New");
            ticktext.Stroke = Brushes.White;
            ticktext.FontSize = 14;
            int txt = (i + roundedStart);
            if (txt < 0)
            {
              txt += 360;
            }
            txt /= 10;
            ticktext.Text = txt.ToString("D2");
            ticktext.Foreground = Brushes.White;
            Canvas.SetTop(ticktext, 2);
            Canvas.SetLeft(ticktext, tl.X1 - 10);
            Canvas_Compass.Children.Add(ticktext);

          }
          else
          {
            tl.Y1 = 25;
          }
          tl.Y2 = 30;
          tl.Stroke = Brushes.White;
          tl.StrokeThickness = 1;
          Canvas_Compass.Children.Add(tl);
        }
      }

    }

    private void DrawHeading(double yawDeg)
    {
      yawDeg = yawDeg % 360;

      if (yawDeg < 0)
      {
        yawDeg += 360;
      }

      int yawInt = (int)yawDeg;
      if (360 == yawInt)
      {
        yawInt = 0;
      }

      double left = (Grid_Viewport.ActualWidth / 2) - 30;

      string hdgStr = "HDG ";
      if (yawInt < 100)
      {
        hdgStr += " ";
      }
      if (yawInt < 10)
      {
        hdgStr += " ";
      }

      var heading = new BorderTextLabel();
      heading.FontFamily = new FontFamily("Courier New");
      heading.Stroke = Brushes.White;
      heading.FontSize = 12;
 
      heading.Text = hdgStr + ((int)yawDeg).ToString();
      heading.Foreground = Brushes.White;

      Border border = new Border();
      border.Child = heading;

      border.BorderThickness = new Thickness(1);
      border.BorderBrush = new SolidColorBrush(Colors.White);
      Canvas.SetTop(border, 44);
      Canvas.SetLeft(border, left);
      Canvas_HUD.Children.Add(border);

      Line leftLn = new Line();
      leftLn.X1 = (Grid_Viewport.ActualWidth / 2) - 15;
      leftLn.X2 = (Grid_Viewport.ActualWidth / 2);
      leftLn.Y1 = 44;
      leftLn.Y2 = 30;
      leftLn.Stroke = Brushes.White;
      leftLn.StrokeThickness = 1;
      Canvas_HUD.Children.Add(leftLn);

      Line rightLn = new Line();
      rightLn.X1 = (Grid_Viewport.ActualWidth / 2) + 15;
      rightLn.X2 = (Grid_Viewport.ActualWidth / 2);
      rightLn.Y1 = 44;
      rightLn.Y2 = 30;
      rightLn.Stroke = Brushes.White;
      rightLn.StrokeThickness = 1;
      Canvas_HUD.Children.Add(rightLn);

    }

    protected override void OnRender(DrawingContext drawingContext)
    {
      base.OnRender(drawingContext);

      Canvas_HUD_RollRotation.Children.Clear();
      Canvas_PitchIndicator.Children.Clear();
      Canvas_HUD.Children.Clear();
      Canvas_Compass.Children.Clear();

      double pitchDeg = PitchAngle;
      if (pitchDeg == 90.0)
      {
        pitchDeg = 89.99;
      }
      else if (pitchDeg == -90.0)
      {
        pitchDeg = -89.99;
      }

      DrawGroundAndSky(pitchDeg);
      DrawPitchTicks(pitchDeg, RollAngle);
      DrawCompass(YawAngle);
      DrawHeading(YawAngle);

      Canvas_HUD_RollRotation.RenderTransform = new RotateTransform(-RollAngle);
      Canvas_PitchIndicator.RenderTransform = new RotateTransform(-RollAngle);


    }
  }
}
