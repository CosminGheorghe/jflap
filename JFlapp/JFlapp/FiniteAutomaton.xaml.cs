using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.Script.Serialization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Xml;
using System.Xml.Serialization;

namespace JFlapp
{
    public partial class FiniteAutomaton : Window
    {
        private enum MyAction
        { None, DrawState, DrawTransition, DeleteShape, MoveObject }

        private MyAction action = MyAction.None;

        private int noPoints;
        private Border shapeConnect1;
        private Border shapeConnect2;
        double x1, x2, y1, y2;
        Point point1, point2;

        private int stateCounter = 0;
        private readonly List<int> deletedStates = new List<int>();
        
        Border moveState = null;
        private readonly List<Line> moveTransitions1 = new List<Line>();
        private readonly List<Line> moveTransitions2 = new List<Line>();
        List<SaveObject> save = new List<SaveObject>();


        public FiniteAutomaton()
        {
            InitializeComponent();
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            this.Owner.Show();
        }

        private void MoveObject(object sender, RoutedEventArgs e)
        {
            action = MyAction.MoveObject;
        }

        private void DrawCircle(object sender, RoutedEventArgs e)
        {
            action = MyAction.DrawState;
        }

        private void DeleteShape(object sender, RoutedEventArgs e)
        {
            action = MyAction.DeleteShape;
        }

        private void DrawLine(object sender, RoutedEventArgs e)
        {
            action = MyAction.DrawTransition;
        }

        private void CanvasMouseMove(object sender, MouseEventArgs e)
        {
            if(action == MyAction.MoveObject && moveState!= null)
            {
                Canvas.SetLeft(moveState, e.GetPosition(myCanvas).X - 30);
                Canvas.SetTop(moveState, e.GetPosition(myCanvas).Y - 30);
                foreach (Line line in moveTransitions1)
                {
                    line.X1 = Canvas.GetLeft(moveState) + 30;
                    line.Y1 = Canvas.GetTop(moveState) + 30;
                }
                foreach (Line line in moveTransitions2)
                {
                    line.X2 = Canvas.GetLeft(moveState) + 30;
                    line.Y2 = Canvas.GetTop(moveState) + 30;
                }  
            }
        }

        private void CanvasMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            moveState = null;
            moveTransitions1.Clear();
            moveTransitions2.Clear();
        }

        private void SaveAsPNG(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = "PNG File (*.png)|*.png",
                InitialDirectory = @"c:\Desktop"
            };
            if (saveFileDialog.ShowDialog() == true)
            {
                Rect bounds = VisualTreeHelper.GetDescendantBounds(myCanvas);
                double dpi = 96d;

                RenderTargetBitmap rtb = new RenderTargetBitmap((int)bounds.Width, (int)bounds.Height, dpi, dpi, System.Windows.Media.PixelFormats.Default);

                DrawingVisual dv = new DrawingVisual();
                using (DrawingContext dc = dv.RenderOpen())
                {
                    VisualBrush vb = new VisualBrush(myCanvas);
                    dc.DrawRectangle(vb, null, new Rect(new Point(), bounds.Size));
                }

                rtb.Render(dv);

                BitmapEncoder pngEncoder = new PngBitmapEncoder();
                
                pngEncoder.Frames.Add(BitmapFrame.Create(rtb));
                try
                {
                    System.IO.MemoryStream ms = new System.IO.MemoryStream();

                    pngEncoder.Save(ms);
                    ms.Close();

                    System.IO.File.WriteAllBytes(saveFileDialog.FileName, ms.ToArray());
                }
                catch (Exception err)
                {
                    MessageBox.Show(err.ToString(), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void SaveContent(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = "JSON File (*.json)|*.json",
                InitialDirectory = @"c:\Desktop"
            };

            string json = new JavaScriptSerializer().Serialize(save);

            if (saveFileDialog.ShowDialog() == true)
            {
                try
                {
                    System.IO.MemoryStream ms = new System.IO.MemoryStream();

                    System.IO.File.WriteAllText(saveFileDialog.FileName, json);
                }
                catch (Exception err)
                {
                    MessageBox.Show(err.ToString(), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void LoadJSON(object sender, RoutedEventArgs e)
        {
            OpenFileDialog theDialog = new OpenFileDialog();
            theDialog.Title = "Open JSON File";
            theDialog.Filter = "JSON files|*.json";
            theDialog.InitialDirectory = @"C:\Desktop";
            string jsonString = string.Empty;


            if (theDialog.ShowDialog() == true)
            {
                try
                {
                    Stream myStream;
                    if ((myStream = theDialog.OpenFile()) != null)
                    {

                        save.Clear();
                        myCanvas.Children.Clear();

                        using (StreamReader reader = new StreamReader(myStream))
                        {
                            jsonString = reader.ReadToEnd();
                        }

                        save = new JavaScriptSerializer().Deserialize<List<SaveObject>>(jsonString);

                        stateCounter = save[save.Count - 1].value + 1;
                        foreach (var item in save)
                        {
                            drawState(item.value, item.X, item.Y);

                            foreach (var it in item.connections)
                            {
                                foreach (var iterator in save)
                                {
                                    if (iterator.value == it)
                                    {
                                        drawLine(iterator.X, iterator.Y, item.X, item.Y);
                                    }
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: Could not read file from disk. Original error: " + ex.Message);
                }
            }

            

        }

        private void drawState(int actualState, double x, double y)
        {
            Border circle = new Border()
            {
                CornerRadius = new CornerRadius(50),
                Width = 60,
                Height = 60,
                Margin = new Thickness(10),
                Padding = new Thickness(0, 20, 0, 0),
                Background = Brushes.Yellow,
                BorderBrush = Brushes.Black,
                BorderThickness = new Thickness(1),
                Child = new TextBlock()
                {
                    Text = "s" + actualState,
                    FontSize = 14,
                    FontWeight = FontWeights.Bold,
                    HorizontalAlignment = HorizontalAlignment.Center
                },

            };

            Canvas.SetLeft(circle, x);
            Canvas.SetTop(circle, y);
            Canvas.SetZIndex(circle, 10);
            myCanvas.Children.Add(circle);
        }

        private void drawLine(double x1, double y1, double x2, double y2)
        {
            point2.X = x2 + 30;
            point2.Y = y2 + 30;

            Line transition = new Line()
            {
                Stroke = Brushes.Black,
                StrokeThickness = 3,
                X1 = x1 + 30,
                Y1 = y1 + 30,
                X2 = x2 + 30,
                Y2 = y2 + 30,
                StrokeStartLineCap = PenLineCap.Flat,
                StrokeEndLineCap = PenLineCap.Triangle,
            };

            myCanvas.Children.Add(transition);
            Canvas.SetZIndex(transition, 5);
        }


        private new void MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            switch (action)
            {
                case MyAction.DrawState:
                    {
                        int actualState;
                        SaveObject obj = new SaveObject();
                        if (deletedStates.Count == 0)
                        {
                            actualState = stateCounter++;
                            obj.value = actualState;
                        }
                        else
                        {
                            actualState = deletedStates.Min();
                            deletedStates.Remove(actualState);
                        }
                        Border circle = new Border()
                        {
                            CornerRadius = new CornerRadius(50),
                            Width = 60,
                            Height = 60,
                            Margin = new Thickness(10),
                            Padding = new Thickness(0, 20, 0, 0),
                            Background = Brushes.Yellow,
                            BorderBrush = Brushes.Black,
                            BorderThickness = new Thickness(1),
                            Child = new TextBlock()
                            {
                                Text = "s" + actualState,
                                FontSize = 14,
                                FontWeight = FontWeights.Bold,
                                HorizontalAlignment = HorizontalAlignment.Center
                            },
                        };

                        obj.X = e.GetPosition(myCanvas).X - 30;
                        obj.Y = e.GetPosition(myCanvas).Y - 30;
                        obj.connections = new List<int>();

                        save.Add(obj);

                        Canvas.SetLeft(circle, e.GetPosition(myCanvas).X - 30);
                        Canvas.SetTop(circle, e.GetPosition(myCanvas).Y - 30);
                        Canvas.SetZIndex(circle, 10);
                        myCanvas.Children.Add(circle);
                        break;
                    }
                    
                case MyAction.DrawTransition:
                    {
                        if (noPoints == 0)
                        {
                            Point point = e.GetPosition((Canvas)sender);
                            HitTestResult hitResult = VisualTreeHelper.HitTest(myCanvas, point);
                            if(hitResult.VisualHit is Border)
                            {
                                shapeConnect1 = hitResult.VisualHit as Border;
                            }
                            else if(hitResult.VisualHit is TextBlock)
                            {
                                shapeConnect1 = (Border)((TextBlock)hitResult.VisualHit).Parent;
                            }

                            if (shapeConnect1 != default(Border))
                            {
                                noPoints++;
                                x1 = Canvas.GetLeft(shapeConnect1);
                                y1 = Canvas.GetTop(shapeConnect1);
                                point1 = point;
                                point1.X = x1 + 30;
                                point1.Y = y1 + 30;
                            }
                        }
                        else
                        {
                            Point point = e.GetPosition((Canvas)sender);
                            HitTestResult hitResult = VisualTreeHelper.HitTest(myCanvas, point);
                            if (hitResult.VisualHit is Border)
                            {
                                shapeConnect2 = hitResult.VisualHit as Border;
                            }
                            else if (hitResult.VisualHit is TextBlock)
                            {
                                shapeConnect2 = (Border)((TextBlock)hitResult.VisualHit).Parent;
                            }

                            if (shapeConnect2 != default(Border))
                            {
                                noPoints++;
                                x2 = Canvas.GetLeft(shapeConnect2);
                                y2 = Canvas.GetTop(shapeConnect2);
                                point2 = point;
                                point2.X = x2 + 30;
                                point2.Y = y2 + 30;

                                Line transition = new Line()
                                {
                                    Stroke = Brushes.Black,
                                    StrokeThickness = 3,
                                    X1 = point1.X,
                                    Y1 = point1.Y,
                                    X2 = point2.X,
                                    Y2 = point2.Y,
                                    StrokeStartLineCap = PenLineCap.Flat,
                                    StrokeEndLineCap = PenLineCap.Triangle,
                                };

                                myCanvas.Children.Add(transition);
                                Canvas.SetZIndex(transition, 1);
                            }
                            else
                            {
                                shapeConnect1.Background = Brushes.Orange;
                            }
                            noPoints = 0;
                            int i = 0;
                            while (i < save.Count())
                            {
                                var sh1 = shapeConnect1.Child as TextBlock;
                                var sh2 = shapeConnect2.Child as TextBlock;
                                if (save[i].value == (sh1.Text[1] - '0'))
                                {
                                    save[i].connections.Add(sh2.Text[1] - '0');
                                    save[sh2.Text[1] - '0'].connections.Add(i);
                                }
                                i++;
                            }
                        }
                        break;
                    }
                case MyAction.DeleteShape:
                    {
                        Point pt = e.GetPosition((Canvas)sender);

                        HitTestResult result = VisualTreeHelper.HitTest(myCanvas, pt);
                        if (result != null)
                        {
                            TextBlock child;
                            if (result.VisualHit is Border border)
                            {
                                child = border.Child as TextBlock;
                                string numericString = child.Text.Remove(0, 1);

                                deletedStates.Add(int.Parse(numericString));
                                SaveObject toDelete = null;
                                foreach(var item in save)
                                {
                                    if(item.value == int.Parse(numericString))
                                    {
                                        toDelete = item;
                                    }
                                }
                                if(toDelete!=null)
                                {
                                    save.Remove(toDelete);
                                }
                                save.Remove(toDelete);
                                List<Line> transitionsToDelete = new List<Line>();
                                foreach (var v in myCanvas.Children)
                                {
                                    if (v is Line line)
                                    {
                                        if (((line.X1 - 30 == Canvas.GetLeft(border)) && (line.Y1 - 30 == Canvas.GetTop(border))) ||
                                            ((line.X2 - 30 == Canvas.GetLeft(border)) && (line.Y2 - 30 == Canvas.GetTop(border))))
                                        {
                                           transitionsToDelete.Add(line);
                                        }
                                    }
                                }

                                foreach(var transition in transitionsToDelete)
                                {
                                    myCanvas.Children.Remove(transition);
                                }

                                myCanvas.Children.Remove(border);
                            }
                            else if (result.VisualHit is TextBlock)
                            {
                                child = result.VisualHit as TextBlock;
                                Border state = (Border)child.Parent;
                                string numericString = child.Text.Remove(0, 1);

                                deletedStates.Add(int.Parse(numericString));
                                SaveObject toDelete = null;
                                foreach (var item in save)
                                {
                                    if (item.value == int.Parse(numericString))
                                    {
                                        toDelete = item;
                                    }
                                }
                                if (toDelete != null)
                                {
                                    save.Remove(toDelete);
                                }
                                save.Remove(toDelete);
                                List<Line> transitionsToDelete = new List<Line>();

                                foreach (var v in myCanvas.Children)
                                {
                                    if (v is Line line)
                                    {
                                        if (((line.X1 - 30 == Canvas.GetLeft(state)) && (line.Y1 - 30 == Canvas.GetTop(state))) ||
                                            ((line.X2 - 30 == Canvas.GetLeft(state)) && (line.Y2 - 30 == Canvas.GetTop(state))))
                                        {
                                            transitionsToDelete.Add(line);
                                        }
                                    }
                                }

                                foreach (var transition in transitionsToDelete)
                                {
                                    myCanvas.Children.Remove(transition);
                                }

                                myCanvas.Children.Remove(state);
                            }
                            else if (result.VisualHit is Line)
                            {
                                myCanvas.Children.Remove(result.VisualHit as Line);
                            }
                        }
                        break;
                    }
                case MyAction.MoveObject:
                    {
                        Point pt = e.GetPosition((Canvas)sender);
                        HitTestResult result = VisualTreeHelper.HitTest(myCanvas, pt);
                        
                        if(result.VisualHit is Border state)
                        {
                            moveState = state;
                        }
                        else if(result.VisualHit is TextBlock stateLabel)
                        {
                            moveState = (Border)stateLabel.Parent;
                        }
                        
                        if(moveState != null)
                        {
                            foreach(var v in myCanvas.Children)
                            {
                                if(v is Line line)
                                {
                                    if((line.X1 - 30 == Canvas.GetLeft(moveState)) && (line.Y1 - 30 == Canvas.GetTop(moveState)))
                                    {
                                        moveTransitions1.Add(line);
                                    }
                                    else if((line.X2 - 30 == Canvas.GetLeft(moveState)) && (line.Y2 - 30 == Canvas.GetTop(moveState)))
                                    {
                                        moveTransitions2.Add(line);
                                    }
                                }
                            }
                        }

                        break;
                    }
                default:
                    return;
            }
        }
    }
}
