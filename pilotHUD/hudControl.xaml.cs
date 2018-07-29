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

    private const int VERTICAL_DEG_TO_DISP    = 36;
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

      LinearGradientBrush gndGradBrush =
    new LinearGradientBrush();
      gndGradBrush.StartPoint = new Point(0, 0);
      gndGradBrush.EndPoint = new Point(0, 1);
      gndGradBrush.GradientStops.Add(
          new GradientStop(Color.FromRgb(173, 104, 13), 0.0));
      gndGradBrush.GradientStops.Add(
          new GradientStop(Color.FromRgb(247, 147, 17), 0.25));

      LinearGradientBrush skyGradBrush =
    new LinearGradientBrush();
      skyGradBrush.StartPoint = new Point(0, 0);
      skyGradBrush.EndPoint = new Point(0, 1);
      skyGradBrush.GradientStops.Add(
          new GradientStop(Color.FromRgb(3, 84, 196), 0.75));
      skyGradBrush.GradientStops.Add(
          new GradientStop(Color.FromRgb(2, 147, 247), 1));

      var gndRect = new Rectangle();
      gndRect.Fill = gndGradBrush;
      gndRect.Width = rectDimension;
      gndRect.Height = rectDimension;
      Canvas.SetLeft(gndRect, -maxDim);
      Canvas.SetTop(gndRect, offset);

      var skyRect = new Rectangle();
      skyRect.Fill = skyGradBrush;
      skyRect.Width = rectDimension;
      skyRect.Height = rectDimension;
      Canvas.SetLeft(skyRect, -maxDim);
      Canvas.SetBottom(skyRect, -offset);

      var line = new Line();
      line.X1 = -rectDimension;
      line.X2 = rectDimension;
      line.Y1 = offset;
      line.Y2 = offset;
      line.Stroke = Brushes.White;
      line.StrokeThickness = 2;


      Canvas_Background.Children.Add(gndRect);
      Canvas_Background.Children.Add(skyRect);
      Canvas_Background.Children.Add(line);

    }

    private void DrawMajorPitchTick(double offset, double val, bool dispTxt)
    {
      Line lnL = new Line();
      lnL.X1 = 0;
      lnL.X2 = 40;
      lnL.Y1 = 0;
      lnL.Y2 = 0;
      lnL.Stroke = Brushes.White;
      lnL.StrokeThickness = 2;
      Canvas.SetLeft(lnL, -80);
      Canvas.SetTop(lnL, -offset);
      Canvas_PitchIndicator.Children.Add(lnL);

      Line lnR = new Line();
      lnR.X1 = 0;
      lnR.X2 = 40;
      lnR.Y1 = 0;
      lnR.Y2 = 0;
      lnR.Stroke = Brushes.White;
      lnR.StrokeThickness = 2;
      Canvas.SetLeft(lnR, 40);
      Canvas.SetTop(lnR, -offset);
      Canvas_PitchIndicator.Children.Add(lnR);

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
        Canvas.SetLeft(txtBlkL, -120);
        Canvas_PitchIndicator.Children.Add(txtBlkL);

        var txtBlkR = new BorderTextLabel();
        txtBlkR.Stroke = Brushes.DimGray;
        txtBlkR.HorizontalContentAlignment = HorizontalAlignment.Right;
        txtBlkR.Text = val.ToString("##0");
        txtBlkR.Foreground = Brushes.White;
        txtBlkR.FontSize = 16;
        txtBlkR.FontWeight = FontWeights.Bold;
        Canvas.SetTop(txtBlkR, -offset - 13);
        Canvas.SetRight(txtBlkR, -120);
        Canvas_PitchIndicator.Children.Add(txtBlkR);

      }

    }

    private void DrawMinorPitchTick(double offset, double val)
    {
      Line lnL = new Line();
      lnL.X1 = 0;
      lnL.X2 = 25;
      lnL.Y1 = 0;
      lnL.Y2 = 0;
      lnL.Stroke = Brushes.White;
      lnL.StrokeThickness = 1;
      Canvas.SetLeft(lnL, -60);
      Canvas.SetTop(lnL, -offset);
      Canvas_PitchIndicator.Children.Add(lnL);

      Line lnR = new Line();
      lnR.X1 = 0;
      lnR.X2 = 25;
      lnR.Y1 = 0;
      lnR.Y2 = 0;
      lnR.Stroke = Brushes.White;
      lnR.StrokeThickness = 1;
      Canvas.SetLeft(lnR, 35);
      Canvas.SetTop(lnR, -offset);
      Canvas_PitchIndicator.Children.Add(lnR);

      if (val != 0)
      {
        Line left = new Line();
        left.X1 = 0;
        left.X2 = 0;
        left.Y1 = 0;
        left.Y2 = 5;
        left.Stroke = Brushes.White;
        left.StrokeThickness = 1;
        Canvas.SetLeft(left, -60);
        Canvas.SetTop(left, -offset);

        Line right = new Line();
        right.X1 = 0;
        right.X2 = 0;
        right.Y1 = 0;
        right.Y2 = 5;
        right.Stroke = Brushes.White;
        right.StrokeThickness = 1;
        Canvas.SetRight(right, -60);
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

      double left = (Grid_Compass.ActualWidth / 2) - 30;

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
      Canvas_Compass.Children.Add(border);

      Line leftLn = new Line();
      leftLn.X1 = (Grid_Compass.ActualWidth / 2) - 15;
      leftLn.X2 = (Grid_Compass.ActualWidth / 2);
      leftLn.Y1 = 44;
      leftLn.Y2 = 30;
      leftLn.Stroke = Brushes.White;
      leftLn.StrokeThickness = 1;
      Canvas_Compass.Children.Add(leftLn);

      Line rightLn = new Line();
      rightLn.X1 = (Grid_Compass.ActualWidth / 2) + 15;
      rightLn.X2 = (Grid_Compass.ActualWidth / 2);
      rightLn.Y1 = 44;
      rightLn.Y2 = 30;
      rightLn.Stroke = Brushes.White;
      rightLn.StrokeThickness = 1;
      Canvas_Compass.Children.Add(rightLn);

    }

    private void DrawRollTick(double circleRad, double rollAngle, bool isLarge)
    {
      Line line = new Line();
      line.X1 = 0;
      line.X2 = 0;
      if (true == isLarge)
      {
        line.Y1 = -24;
      }
      else
      {
        line.Y1 = -12;
      }
      line.Y2 = 0;
      line.Stroke = Brushes.White;
      line.StrokeThickness = 2;
      Canvas.SetTop(line, -circleRad);
      line.RenderTransform = new RotateTransform(rollAngle, 0, circleRad);
      Canvas_HUD.Children.Add(line);
    }

    private void DrawZeroRollTick(double circleRad)
    {
      Polygon triangle = new Polygon();
      triangle.Stroke = Brushes.White;
      triangle.Fill = Brushes.White;
      triangle.StrokeThickness = 1;
      triangle.HorizontalAlignment = HorizontalAlignment.Left;
      triangle.VerticalAlignment = VerticalAlignment.Center;
      Point Point1 = new Point(0, 0);
      Point Point2 = new Point(12, -16);
      Point Point3 = new Point(-12, -16);
      PointCollection pc = new PointCollection();
      pc.Add(Point1);
      pc.Add(Point2);
      pc.Add(Point3);
      triangle.Points = pc;

      Canvas.SetTop(triangle, -circleRad);
      Canvas_HUD.Children.Add(triangle);
    }

    private void DrawRollIndicator(double circleRad, double rollAngle)
    {
      Polygon triangle = new Polygon();
      triangle.Stroke = Brushes.White;
      triangle.Fill = Brushes.White;
      triangle.StrokeThickness = 1;
      triangle.HorizontalAlignment = HorizontalAlignment.Left;
      triangle.VerticalAlignment = VerticalAlignment.Center;
      Point triP1 = new Point(0, 0);
      Point triP2 = new Point(9, 12);
      Point triP3 = new Point(-9, 12);
      PointCollection pc = new PointCollection();
      pc.Add(triP1);
      pc.Add(triP2);
      pc.Add(triP3);
      triangle.Points = pc;

      Polygon trapezoid = new Polygon();
      trapezoid.Stroke = Brushes.White;
      trapezoid.Fill = Brushes.White;
      trapezoid.StrokeThickness = 1;
      trapezoid.HorizontalAlignment = HorizontalAlignment.Left;
      trapezoid.VerticalAlignment = VerticalAlignment.Center;
      Point trapP1 = new Point(-12, 16);
      Point trapP2 = new Point(12, 16);
      Point trapP3 = new Point(15, 20);
      Point trapP4 = new Point(-15, 20);
      PointCollection pcTrap = new PointCollection();
      pcTrap.Add(trapP1);
      pcTrap.Add(trapP2);
      pcTrap.Add(trapP3);
      pcTrap.Add(trapP4);
      trapezoid.Points = pcTrap;

      triangle.RenderTransform = new RotateTransform(rollAngle, 0, circleRad);
      trapezoid.RenderTransform = new RotateTransform(rollAngle, 0, circleRad);

      Canvas.SetTop(triangle, -circleRad);
      Canvas.SetTop(trapezoid, -circleRad);
      Canvas_HUD.Children.Add(triangle);
      Canvas_HUD.Children.Add(trapezoid);

    }

    private void DrawRoll(double rollAngle)
    {
      double circleRad = Grid_Viewport.ActualHeight / 3;

      List<KeyValuePair<double, bool>> tickList = new List<KeyValuePair<double, bool>>();
      tickList.Add(new KeyValuePair<double, bool>(10, false));
      tickList.Add(new KeyValuePair<double, bool>(20, false));
      tickList.Add(new KeyValuePair<double, bool>(30, true));
      tickList.Add(new KeyValuePair<double, bool>(45, false));
      tickList.Add(new KeyValuePair<double, bool>(60, true));

      DrawZeroRollTick(circleRad);
      for (int i = 0; i < tickList.Count; i++)
      {
        DrawRollTick(circleRad, tickList[i].Key, tickList[i].Value);
        DrawRollTick(circleRad, -tickList[i].Key, tickList[i].Value);
      }

      DrawRollIndicator(circleRad, rollAngle);
    }

    private void DrawAircraft()
    {
      double segmentLength = 6;
      Polyline waterline = new Polyline();
      waterline.Stroke = Brushes.White;
      waterline.StrokeThickness = 2;

      Point p1 = new Point(-4 * segmentLength, 0);
      Point p2 = new Point(-2 * segmentLength, 0);
      Point p3 = new Point(-segmentLength, segmentLength);
      Point p4 = new Point(0, 0);
      Point p5 = new Point(segmentLength, segmentLength);
      Point p6 = new Point(2 * segmentLength, 0);
      Point p7 = new Point(4 * segmentLength, 0);

      PointCollection pc = new PointCollection();
      pc.Add(p1);
      pc.Add(p2);
      pc.Add(p3);
      pc.Add(p4);
      pc.Add(p5);
      pc.Add(p6);
      pc.Add(p7);

      waterline.Points = pc;
      Canvas_HUD.Children.Add(waterline);
    }

    protected override void OnRender(DrawingContext drawingContext)
    {
      base.OnRender(drawingContext);

      Canvas_Background.Children.Clear();
      Canvas_PitchIndicator.Children.Clear();
      Canvas_HUD.Children.Clear();
      Canvas_Compass.Children.Clear();

      DrawGroundAndSky(PitchAngle);
      DrawPitchTicks(PitchAngle, RollAngle);
      DrawCompass(YawAngle);
      DrawHeading(YawAngle);
      DrawRoll(RollAngle);
      DrawAircraft();

      Canvas_Background.RenderTransform = new RotateTransform(-RollAngle);
      Canvas_PitchIndicator.RenderTransform = new RotateTransform(-RollAngle);
    }

  }
}
