namespace PaintApp
{
    public partial class Form1 : Form
    {
        Graphics g;
        bool drawing = false;
        Point startPoint;
        Pen pen = new Pen(Color.Black, 2);
        Pen era = new Pen(Color.White, 8);
        int index;
        int x, y, sx, sy, cx, cy;

        public Form1()
        {
            InitializeComponent();

            g = drawPanel.CreateGraphics();
            g.Clear(Color.White);

            // làm mượt nét vẽ
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            // làm cho phân đuôi nét vẽ được bo tròn lại
            pen.StartCap = pen.EndCap = System.Drawing.Drawing2D.LineCap.Round;   // bút
            era.StartCap = era.EndCap = System.Drawing.Drawing2D.LineCap.Round;   // tẩy
        }

        private void drawPanel_MouseDown(object sender, MouseEventArgs e)
        {
            drawing = true;
            startPoint = e.Location;

            cx = e.X;
            cy = e.Y;
        }

        private void drawPanel_MouseUp(object sender, MouseEventArgs e)
        {
            drawing = false;

            sx = x - cx;
            sy = y - cy;

            if (index == 3)
            {
                g.DrawEllipse(pen, cx, cy, sx, sy);
            }
            if (index == 4)
            {
                g.DrawRectangle(pen, cx, cy, sx, sy);
            }
            if (index == 5)
            {
                g.DrawLine(pen, cx, cy, x, y);
            }
        }

        private void drawPanel_MouseMove(object sender, MouseEventArgs e)
        {
            if (drawing)
            {
                if (index == 1)
                {
                    Point endPoint = e.Location;
                    g.DrawLine(pen, startPoint, endPoint);
                    startPoint = endPoint;
                }
                if (index == 2)
                {
                    Point endPoint = e.Location;
                    g.DrawLine(era, startPoint, endPoint);
                    startPoint = endPoint;
                }
            }
            //drawPanel.Refresh();

            x = e.X;
            y = e.Y;
            sx = e.X - cx;
            sy = e.Y - cy;
        }

        private void tool_pencil_Click(object sender, EventArgs e)
        {
            index = 1;
        }

        private void tool_eraser_Click(object sender, EventArgs e)
        {
            index = 2;
        }

        private void pictureBox16_Click(object sender, EventArgs e)
        {
            index = 3;
        }

        private void pictureBox15_Click(object sender, EventArgs e)
        {
            index = 4;
        }

        private void pictureBox24_Click(object sender, EventArgs e)
        {
            index = 5;
        }

        private void drawPanel_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            if (drawing)
            {
                if (index == 3)
                {
                    g.DrawEllipse(pen, cx, cy, sx, sy);
                }
                if (index == 4)
                {
                    g.DrawRectangle(pen, cx, cy, sx, sy);
                }
                if (index == 5)
                {
                    g.DrawLine(pen, cx, cy, x, y);
                }
            }
        }
    }
}