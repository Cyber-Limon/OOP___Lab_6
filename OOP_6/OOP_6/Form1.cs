using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlTypes;
using System.Diagnostics.Eventing.Reader;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OOP_6
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private class Distance
        {
            public double dist(double x1, double y1, double x2, double y2)
            {
                return Math.Sqrt(Math.Pow(x2 - x1, 2) + Math.Pow(y2 - y1, 2));
            }
        }

        private class Area
        {
            Distance D = new Distance();

            public double S(double x1, double y1, double x2, double y2, double x3, double y3)
            {
                double a = D.dist(x1, y1, x2, y2);
                double b = D.dist(x2, y2, x3, y3);
                double c = D.dist(x3, y3, x1, y1);

                double p = (a + b + c) / 2;
                double s = Math.Sqrt(p * (p - a) * (p - b) * (p - c));

                return s;
            }
        }

        private class Draw
        {
            Bitmap map;
            Graphics graphics;
            Pen pen = new Pen(Color.Blue, 1f);

            public Draw()
            {
                Rectangle rectangle = Screen.PrimaryScreen.Bounds;
                map = new Bitmap(rectangle.Width, rectangle.Height);
                graphics = Graphics.FromImage(map);
            }

            public Bitmap GetBitmap()
            {
                return map;
            }

            public Pen GetPen()
            {
                return pen;
            }

            public void DrawRectangle(Pen pen, float x, float y, float r1, float r2)
            {
                graphics.DrawRectangle(pen, x, y, r1, r2);
            }

            public void DrawEllipse(Pen pen, float x, float y, float r1, float r2)
            {
                graphics.DrawEllipse(pen, x, y, r1, r2);
            }

            public void DrawTriangle(Pen pen, float x1, float y1, float x2, float y2)
            {
                graphics.DrawLine(pen, (x1 + x2) / 2, y1, x1, y2);
                graphics.DrawLine(pen, x1, y2, x2, y2);
                graphics.DrawLine(pen, x2, y2, (x1 + x2) / 2, y1);
            }

            public void DrawLine(Pen pen, float x1, float y1, float x2, float y2)
            {
                graphics.DrawLine(pen, x1, y1, x2, y2);
            }

            public void DrawArrayFigures(ArrayFigures arrayFigures)
            {
                for (int i = 0; i < arrayFigures.GetSize(); i++)
                    arrayFigures.GetFigure(i).Drawing();
            }

            public void NotSelectAll(ArrayFigures arrayFigures)
            {
                for (int i = 0; i < arrayFigures.GetSize(); i++)
                    arrayFigures.GetFigure(i).NotSelect();
            }

            public void Clear()
            {
                graphics.Clear(Color.White);
            }
        }

        private class Figure
        {
            protected Draw draw;
            protected Pen pen = new Pen(Color.Blue, 1f);
            protected Pen pen_select = new Pen(Color.Blue, 3f);
            protected float x1, y1, x2, y2;
            protected bool select = true;

            public Figure(Point[] points, Draw draw, PictureBox pictureBox)
            {
                StartBorder(pictureBox, points);
                x1 = Math.Min(points[0].X, points[1].X);
                y1 = Math.Min(points[0].Y, points[1].Y);
                x2 = Math.Max(points[0].X, points[1].X);
                y2 = Math.Max(points[0].Y, points[1].Y);
                this.draw = draw;
                pen.Color = pen_select.Color = draw.GetPen().Color;
            }

            virtual public void Drawing() { return; }

            public void Select()
            {
                select = true;
            }

            public void NotSelect()
            {
                select = false;
            }

            public bool GetSelect()
            {
                if (select)
                    return true;
                else
                    return false;
            }

            virtual public bool Click(int x, int y) { return false; }

            public void ChangeColor(Color color)
            {
                pen.Color = pen_select.Color = color;
            }

            public virtual void ChangeSizeBig(PictureBox pictureBox)
            {
                x1--;
                y1--;
                x2++;
                y2++;
                if (Border(pictureBox))
                {
                    ChangeSizeSmall(pictureBox);
                }
            }

            public virtual void ChangeSizeSmall(PictureBox pictureBox)
            {
                if (x1 + 1 < x2 - 1 && y1 + 1 < y2 - 1)
                {
                    x1++;
                    y1++;
                    x2--;
                    y2--;
                }
            }

            public void ChangePositionLeft(PictureBox pictureBox)
            {                
                x1--;
                x2--;
                if (Border(pictureBox))
                {
                    x1++;
                    x2++;
                }
            }

            public void ChangePositionRight(PictureBox pictureBox)
            {                
                x1++;
                x2++;
                if (Border(pictureBox))
                {
                    x1--;
                    x2--;
                }
            }

            public void ChangePositionUp(PictureBox pictureBox)
            {                
                y1--;
                y2--;                
                if (Border(pictureBox))
                {
                    y1++;
                    y2++;
                }
            }

            public void ChangePositionDown(PictureBox pictureBox)
            {
                y1++;
                y2++;
                if (Border(pictureBox))
                {
                    y1--;
                    y2--;
                }
            }

            public bool Border(PictureBox pictureBox)
            {
                bool trespass = false;

                if (x1 <= 0) trespass = true;
                if (y1 <= 0) trespass = true;
                if (x1 >= pictureBox.Width - 1) trespass = true;
                if (y1 >= pictureBox.Height ) trespass = true;
                if (x2 <= 0) trespass = true;
                if (y2 <= 0) trespass = true;
                if (x2 >= pictureBox.Width) trespass = true;
                if (y2 >= pictureBox.Height) trespass = true;

                return trespass;
            }

            public virtual void StartBorder(PictureBox pictureBox, Point[] points)
            {
                if (points[1].X <= 0) points[1].X = 1;
                if (points[1].Y <= 0) points[1].Y = 1;
                if (points[1].X >= pictureBox.Width) points[1].X = pictureBox.Width - 1;
                if (points[1].Y >= pictureBox.Height) points[1].Y = pictureBox.Height - 1;
            }
        }

        private class MyRectangle : Figure
        {
            public MyRectangle(Point[] points, Draw draw, PictureBox pictureBox) : base(points, draw, pictureBox) {  }
            
            public override void Drawing()
            {
                if (select)
                    draw.DrawRectangle(pen_select, x1, y1, x2 - x1, y2 - y1);
                else
                    draw.DrawRectangle(pen, x1, y1, x2 - x1, y2 - y1);
            }

            public override bool Click(int x, int y)
            {
                if (x1 <= x && x <= x2 && y1 <= y && y <= y2)
                    return true;
                else
                    return false;
            }
        }

        private class Square : MyRectangle
        {
            public Square(Point[] points, Draw draw, PictureBox pictureBox) : base (points, draw, pictureBox)
            {
                x1 = points[1].X - 50;
                y1 = points[1].Y - 50;
                x2 = points[1].X + 50;
                y2 = points[1].Y + 50;
            }

            public override void StartBorder(PictureBox pictureBox, Point[] points)
            {
                if (points[1].X <= 50) points[1].X = 51;
                if (points[1].Y <= 50) points[1].Y = 51;
                if (points[1].X >= pictureBox.Width - 50) points[1].X = pictureBox.Width - 51;
                if (points[1].Y >= pictureBox.Height - 50) points[1].Y = pictureBox.Height - 51;
            }
        }

        private class Ellipse : Figure
        {
            public Ellipse(Point[] points, Draw draw, PictureBox pictureBox) : base(points, draw, pictureBox) { }

            public override void Drawing()
            {
                if (select)
                    draw.DrawEllipse(pen_select, x1, y1, x2 - x1, y2 - y1);
                else 
                    draw.DrawEllipse(pen, x1, y1, x2 - x1, y2 - y1);
            }

            public override bool Click(int x, int y)
            {
                Distance D = new Distance();

                float X = x2 - x1;
                float Y = y2 - y1;

                float a = Math.Max(X, Y) / 2;
                float b = Math.Min(X, Y) / 2;

                double c = Math.Sqrt(a * a - b * b);

                float x0 = (x1 + x2) / 2;
                float y0 = (y1 + y2) / 2;
                double c1 = x0 - c;
                double c2 = x0 + c;

                double l = D.dist(x, y, c1, y0) + D.dist(x, y, c2, y0);

                if (Y > X) 
                {
                    c1 = y0 - c;
                    c2 = y0 + c;

                    l = D.dist(x, y, x0, c1) + D.dist(x, y, x0, c2);
                }

                if (l <= 2 * a)
                    return true;
                else
                    return false;
            }
        }

        private class Circle : Ellipse
        {
            public Circle(Point[] points, Draw draw, PictureBox pictureBox) : base(points, draw, pictureBox)
            {
                x1 = points[1].X - 50;
                y1 = points[1].Y - 50;
                x2 = points[1].X + 50;
                y2 = points[1].Y + 50;
            }

            public override void StartBorder(PictureBox pictureBox, Point[] points)
            {
                if (points[1].X <= 50) points[1].X = 51;
                if (points[1].Y <= 50) points[1].Y = 51;
                if (points[1].X >= pictureBox.Width - 50) points[1].X = pictureBox.Width - 51;
                if (points[1].Y >= pictureBox.Height - 50) points[1].Y = pictureBox.Height - 51;
            }
        }

        private class Triangle : Figure
        {
            public Triangle(Point[] points, Draw draw, PictureBox pictureBox) : base (points, draw, pictureBox) { }

            public override void Drawing()
            {
                if (select)
                    draw.DrawTriangle(pen_select, x1, y1, x2, y2);
                else
                    draw.DrawTriangle(pen, x1, y1, x2, y2);
            }

            public override bool Click(int x, int y)
            {
                double x1 = (this.x1 + this.x2) / 2;
                double y1 = this.y1;
                double x2 = this.x1;
                double y2 = this.y2;
                double x3 = this.x2;
                double y3 = this.y2;

                Area area = new Area();
                
                double S = area.S(x1, y1, x2, y2, x3, y3); 
                double S1 = area.S(x, y, x2, y2, x3, y3);
                double S2 = area.S(x1, y1, x, y, x3, y3);
                double S3 = area.S(x1, y1, x2, y2, x, y);

                if (S1 + S2 + S3 >= S - 1 && S1 + S2 + S3 <= S + 1)
                    return true;
                else
                    return false;
            }
        } 

        private class Line : Figure
        {
            public Line(Point[] points, Draw draw, PictureBox pictureBox) : base (points, draw, pictureBox) 
            {
                x1 = points[0].X;
                y1 = points[0].Y;
                x2 = points[1].X;
                y2 = points[1].Y;
            }

            public override void Drawing()
            {
                if (select)
                    draw.DrawLine(pen_select, x1, y1, x2, y2);
                else
                    draw.DrawLine(pen, x1, y1, x2, y2);
            }

            public override bool Click(int x, int y)
            {
                Distance D = new Distance();

                double l1 = D.dist(x, y, x2, y2);
                double l2 = D.dist(x1, y1, x, y);
                double l = D.dist(x1, y1, x2, y2);

                if (l1 + l2 >= l - 1 && l1 + l2 <= l + 1)
                    return true;
                else
                    return false;
            }

            public override void ChangeSizeBig(PictureBox pictureBox)
            {
                float k, b;

                k = (y2 - y1) / (x2 - x1);
                b = y1 - k * x1;

                if (x1 < x2)
                {
                    x1--;
                    y1 = k * x1 + b;
                    x2++;
                    y2 = k * x2 + b;
                    if (Border(pictureBox))
                    {
                        x1++;
                        y1 = k * x1 + b;
                        x2--;
                        y2 = k * x2 + b;
                    }                   
                }

                else
                {                    
                    x1++;
                    y1 = k * x1 + b;
                    x2--;
                    y2 = k * x2 + b;
                    if (Border(pictureBox))
                    {
                        x1--;
                        y1 = k * x1 + b;
                        x2++;
                        y2 = k * x2 + b;
                    }
                }
            }

            public override void ChangeSizeSmall(PictureBox pictureBox1)
            {
                float k, b;

                k = (y2 - y1) / (x2 - x1);
                b = y1 - k * x1;

                if (x1 < x2)
                {
                    if (x1 + 1 < x2 - 1)
                    {
                        x1++;
                        y1 = k * x1 + b;
                        x2--;
                        y2 = k * x2 + b;
                    }
                }

                else
                {
                    if (x2 + 1 < x1 - 1)
                    {
                        x1--;
                        y1 = k * x1 + b;
                        x2++;
                        y2 = k * x2 + b;
                    }
                }
            }
        }

        private class ArrayFigures
        {
            private int size;
            private Figure[] figures;

            public ArrayFigures()
            {
                size = 0;
                figures = new Figure[size];
            }

            public int GetSize()
            {
                return size;
            }

            public Figure GetFigure(int index)
            {
                return figures[index];
            }

            public void push_back(Figure data)
            {
                size++;
                Figure[] figures = new Figure[size];
                figures[size - 1] = data;

                for (int i = 0; i < size - 1; i++)
                    figures[i] = this.figures[i];

                this.figures = figures;
            }

            public void seizure_with_deletion(int index)
            {
                size--;
                Figure[] figures = new Figure[size];

                for (int i = 0; i < index; i++)
                    figures[i] = this.figures[i];

                for (int i = index; i < size; i++)
                    figures[i] = this.figures[i + 1];

                this.figures = figures;
            }
        }

        Draw draw = new Draw();
        ArrayFigures arrayFigures = new ArrayFigures();

        private bool PressMouse = false;
        private bool flag = false;

        private Point[] points = new Point[2];

        public void NewFigure(Point[] points)
        {
            Figure figure;

            if (radioButtonRectangle.Checked)
                figure = new MyRectangle(points, draw, pictureBox1);

            else if (radioButtonSquare.Checked)
                figure = new Square(points, draw, pictureBox1);

            else if (radioButtonEllipse.Checked)
                figure = new Ellipse(points, draw, pictureBox1);

            else if (radioButtonCircle.Checked)
                figure = new Circle(points, draw, pictureBox1);

            else if(radioButtonTriangle.Checked)
                figure = new Triangle(points, draw, pictureBox1);

            else
                figure = new Line(points, draw, pictureBox1);

            figure.Drawing();

            if (flag)
                arrayFigures.push_back(figure);
        }

        private void button_color_selection_Click(object sender, EventArgs e)
        {
            if (sender == button_color_selection)
            {
                colorDialog1.ShowDialog();
                button_color_selection.BackColor = colorDialog1.Color;
            }
            
            draw.GetPen().Color = ((Button)sender).BackColor;

            for (int i = 0; i < arrayFigures.GetSize(); i++)
                if (arrayFigures.GetFigure(i).GetSelect())
                    arrayFigures.GetFigure(i).ChangeColor(((Button)sender).BackColor);

            draw.Clear();
            draw.DrawArrayFigures(arrayFigures);
            pictureBox1.Image = draw.GetBitmap();
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            bool SelectFigure = false;
            int k = 0;

            for (int i = 0; i < arrayFigures.GetSize(); i++)
            {
                if (arrayFigures.GetFigure(i).Click(e.X, e.Y))
                {
                    k++;
                    SelectFigure = true;

                    if (checkBox1.Checked)
                    {
                        arrayFigures.GetFigure(i).Select();

                        if (!checkBox2.Checked)
                        {
                            break;
                        }
                    }

                    else
                    {
                        if (!(checkBox2.Checked && k > 1))
                        {
                            draw.NotSelectAll(arrayFigures);
                        }

                        arrayFigures.GetFigure(i).Select();
                    }
                }
            }

            if (!SelectFigure)
            {
                draw.NotSelectAll(arrayFigures);
                PressMouse = true;
                points[0] = new Point(e.X, e.Y);
                points[1] = new Point(e.X, e.Y);
            }

            else
            {
                draw.Clear();
                draw.DrawArrayFigures(arrayFigures);
                pictureBox1.Image = draw.GetBitmap();
            }
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (PressMouse)
            {
                flag = false;
                draw.Clear();
                draw.DrawArrayFigures(arrayFigures);
                points[1] = new Point(e.X, e.Y);
                NewFigure(points);
                pictureBox1.Image = draw.GetBitmap();
            }
        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            if (PressMouse)
            {
                flag = true;
                PressMouse = false;
                if (points[0] != points[1])
                    NewFigure(points);
                else
                {
                    if (arrayFigures.GetSize() > 0)
                    {
                        arrayFigures.GetFigure(arrayFigures.GetSize() - 1).Select();
                        draw.Clear();
                        draw.DrawArrayFigures(arrayFigures);
                        pictureBox1.Image = draw.GetBitmap();
                    }
                }
            }
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.ControlKey)
                checkBox1.Checked = true;

            if (e.KeyCode == Keys.Delete)
            {
                for (int i = 0; i < arrayFigures.GetSize();)
                {
                    if (arrayFigures.GetFigure(i).GetSelect())
                        arrayFigures.seizure_with_deletion(i);
                    else
                        i++;
                }

                if (arrayFigures.GetSize() > 0)
                    arrayFigures.GetFigure(arrayFigures.GetSize() - 1).Select();
            }

            if (e.KeyCode == Keys.NumPad4)
            {
                for (int i = 0; i < arrayFigures.GetSize(); i++)
                    if (arrayFigures.GetFigure(i).GetSelect())
                        arrayFigures.GetFigure(i).ChangePositionLeft(pictureBox1);
            }

            if (e.KeyCode == Keys.NumPad6)
            {
                for (int i = 0; i < arrayFigures.GetSize(); i++)
                    if (arrayFigures.GetFigure(i).GetSelect())
                        arrayFigures.GetFigure(i).ChangePositionRight(pictureBox1);
            }

            if (e.KeyCode == Keys.NumPad8)
            {
                for (int i = 0; i < arrayFigures.GetSize(); i++)
                    if (arrayFigures.GetFigure(i).GetSelect())
                        arrayFigures.GetFigure(i).ChangePositionUp(pictureBox1);
            }

            if (e.KeyCode == Keys.NumPad2)
            {
                for (int i = 0; i < arrayFigures.GetSize(); i++)
                    if (arrayFigures.GetFigure(i).GetSelect())
                        arrayFigures.GetFigure(i).ChangePositionDown(pictureBox1);
            }

            if (e.KeyCode == Keys.Oemplus)
            {
                for (int i = 0; i < arrayFigures.GetSize(); i++)
                    if (arrayFigures.GetFigure(i).GetSelect())
                        arrayFigures.GetFigure(i).ChangeSizeBig(pictureBox1);
            }

            if (e.KeyCode == Keys.OemMinus)
            {
                for (int i = 0; i < arrayFigures.GetSize(); i++)
                    if (arrayFigures.GetFigure(i).GetSelect())
                        arrayFigures.GetFigure(i).ChangeSizeSmall(pictureBox1);
            }

            draw.Clear();
            draw.DrawArrayFigures(arrayFigures);
            pictureBox1.Image = draw.GetBitmap();
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.ControlKey)
                checkBox1.Checked = false;
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox2.Checked) { checkBox2.Text = "Несколько"; }
            else { checkBox2.Text = "Один"; }
        }
    }
}
