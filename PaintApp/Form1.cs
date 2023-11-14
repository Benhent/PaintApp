using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using static System.Windows.Forms.DataFormats;

namespace PaintApp
{
    public partial class Form1 : Form
    {
        Bitmap bm;
        Graphics g;
        bool drawing = false;
        Point startPoint;  // điểm bắt đầu vẽ
        Pen pen = new Pen(Color.Black, 2);   // khai báo bút
        Pen era = new Pen(Color.White, 8);   // khai báo tẩy
        int index = 0;
        int x, y, sx, sy, cx, cy;

        private enum SelectedTool
        {
            Pencil,
            Bucket,
            Text,
            Eraser,
            ColorPicker,
            Clear
        }
        private SelectedTool currentTool = SelectedTool.Pencil;

        public Form1()
        {
            InitializeComponent();
            bm = new Bitmap(drawPanel.Width, drawPanel.Height);
            g = Graphics.FromImage(bm);
            g.Clear(Color.White);
            drawPanel.Image = bm;
            g = drawPanel.CreateGraphics();


            // làm mượt nét vẽ
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            // làm cho phân đuôi nét vẽ được bo tròn lại
            pen.StartCap = pen.EndCap = System.Drawing.Drawing2D.LineCap.Round;   // bút
            era.StartCap = era.EndCap = System.Drawing.Drawing2D.LineCap.Round;   // tẩy
        }




        // drawPanel------------------------------------------------------
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

            if (index == 7)     // hình tròn
            {
                g.DrawEllipse(pen, cx, cy, sx, sy);
            }
            if (index == 8)     // hình chữ nhật
            {
                g.DrawRectangle(pen, cx, cy, sx, sy);
            }
            if (index == 9)     // đường thẳng
            {
                g.DrawLine(pen, cx, cy, x, y);
            }
        }

        private void drawPanel_MouseMove(object sender, MouseEventArgs e)
        {
            if (drawing)
            {
                if (index == 1)     // bút
                {
                    Point endPoint = e.Location;  // điểm kết thúc vẽ
                    g.DrawLine(pen, startPoint, endPoint);
                    startPoint = endPoint;
                }
                if (index == 2)     // tẩy
                {
                    Point endPoint = e.Location;
                    g.DrawLine(era, startPoint, endPoint);
                    startPoint = endPoint;
                }
                if (index == 0)
                {
                    Point endPoint = e.Location;  // điểm kết thúc vẽ
                    g.DrawLine(pen, startPoint, endPoint);
                    startPoint = endPoint;
                }
            }
            //drawPanel.Refresh();

            x = e.X;
            y = e.Y;
            sx = e.X - cx;
            sy = e.Y - cy;
        }

        private void drawPanel_MouseEnter(object sender, EventArgs e)
        {
            switch (currentTool)
            {
                case SelectedTool.Pencil:
                    this.Cursor = new Cursor("Cursor\\Pencil.cur");
                    break;
                case SelectedTool.Bucket:
                    this.Cursor = new Cursor("Cursor\\Bucket.cur");
                    break;
                case SelectedTool.Text:
                    this.Cursor = Cursors.Cross;
                    break;
                case SelectedTool.Eraser:
                    this.Cursor = new Cursor("Cursor\\Eraser.cur");
                    break;
                case SelectedTool.ColorPicker:
                    this.Cursor = new Cursor("Cursor\\Color-picker.cur");
                    break;
                case SelectedTool.Clear:
                    this.Cursor = Cursors.Default;
                    break;
                default:
                    this.Cursor = new Cursor("Cursor\\Pencil.cur");
                    break;
            }

            switch (index)
            {
                case 7:
                    this.Cursor = Cursors.Cross;
                    break;
                case 8:
                    this.Cursor = Cursors.Cross;
                    break;
                case 9:
                    this.Cursor = Cursors.Cross;
                    break;
            }
        }

        private void drawPanel_MouseLeave(object sender, EventArgs e)
        {
            this.Cursor = Cursors.Default;
        }

        private void drawPanel_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            if (drawing)
            {
                if (index == 7)
                {
                    g.DrawEllipse(pen, cx, cy, sx, sy);
                }
                if (index == 8)
                {
                    g.DrawRectangle(pen, cx, cy, sx, sy);
                }
                if (index == 9)
                {
                    g.DrawLine(pen, cx, cy, x, y);
                }
            }
        }

        private void drawPanel_MouseClick(object sender, MouseEventArgs e)
        {
            if (index == 3)
            {

            }
        }
        //----------------------------------------------------------------





        //tool------------------------------------------------------------
        private void ResetPictureBoxColors() // khi bấm picturebox khác thì tự set lại màu nền và viền
        {
            foreach (PictureBox pictureBox in tableTool.Controls)
            {
                pictureBox.BackColor = SystemColors.ButtonHighlight; // Đặt lại màu nền về màu gốc
                pictureBox.BorderStyle = BorderStyle.None;
            }
        }

        private void SetCurrentTool(SelectedTool tool)
        {
            currentTool = tool;
        }

        private void tool_pencil_Click(object sender, EventArgs e)  // bút 
        {
            index = 1;
            SetCurrentTool(SelectedTool.Pencil);
            ResetPictureBoxColors();
            PictureBox pictureBox = (PictureBox)sender;
            pictureBox.BackColor = Color.LightCyan;
            pictureBox.BorderStyle = BorderStyle.FixedSingle;
        }

        private void tool_eraser_Click(object sender, EventArgs e)  // tẩy
        {
            index = 2;
            SetCurrentTool(SelectedTool.Eraser);
            ResetPictureBoxColors();
            PictureBox pictureBox = (PictureBox)sender;
            pictureBox.BackColor = Color.LightCyan;
            pictureBox.BorderStyle = BorderStyle.FixedSingle;
        }


        private void tool_bucket_Click(object sender, EventArgs e)  // tô màu nền
        {

            index = 3;
            SetCurrentTool(SelectedTool.Bucket);
            ResetPictureBoxColors();
            PictureBox pictureBox = (PictureBox)sender;
            pictureBox.BackColor = Color.LightCyan;
            pictureBox.BorderStyle = BorderStyle.FixedSingle;
        }

        private void tool_picker_Click(object sender, EventArgs e)
        {
            SetCurrentTool(SelectedTool.ColorPicker);
            ResetPictureBoxColors();
            PictureBox pictureBox = (PictureBox)sender;
            pictureBox.BackColor = Color.LightCyan;
            pictureBox.BorderStyle = BorderStyle.FixedSingle;
        }

        private void tool_clear_Click(object sender, EventArgs e)
        {
            SetCurrentTool(SelectedTool.Clear);
            ResetPictureBoxColors();
            PictureBox pictureBox = (PictureBox)sender;
            pictureBox.BackColor = Color.LightCyan;
            pictureBox.BorderStyle = BorderStyle.FixedSingle;
            drawPanel.Refresh();
        }
        //----------------------------------------------------------------





        // shape----------------------------------------------------------
        private void sh_ht_Click(object sender, EventArgs e)    // hình tròn
        {
            index = 7;
            ResetPictureBoxColors();
        }

        private void sh_hcn_Click(object sender, EventArgs e)   // hình chữ nhật
        {
            index = 8;
            ResetPictureBoxColors();
        }

        private void sh_line_Click(object sender, EventArgs e)  // đường thẳng
        {
            index = 9;
            ResetPictureBoxColors();
        }
        //----------------------------------------------------------------





        // Size-----------------------------------------------------------
        private void PenSize_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (PenSize.SelectedItem != null)    //kiểm tra xem combobox PenSize có trống không
            {
                // nếu không trống thì sẽ thay đổi kích cỡ bút đã được cài đặt sẵn giá trị
                if (PenSize.Text == "2px")
                {
                    pen.Width = 2;
                }
                if (PenSize.Text == "4px")
                {
                    pen.Width = 4;
                }
                if (PenSize.Text == "6px")
                {
                    pen.Width = 6;
                }
                if (PenSize.Text == "8px")
                {
                    pen.Width = 8;
                }
                if (PenSize.Text == "10px")
                {
                    pen.Width = 10;
                }
            }
        }

        private void PenSize_DrawItem(object sender, DrawItemEventArgs e)
        {
            e.DrawBackground();                                         // vẽ background ---->|
            e.DrawFocusRectangle();  // cho background chạy theo từng mục trong combobox <----|  

            if (e.Index >= 0)
            {
                ImageList imageList1 = new ImageList();
                //imageList.Images.Add("imageKey", Image.FromFile("imagepath"));
                imageList1.Images.Add("2px", Image.FromFile("line2px.jpg"));
                imageList1.Images.Add("4px", Image.FromFile("line4px.jpg"));
                imageList1.Images.Add("6px", Image.FromFile("line6px.jpg"));
                imageList1.Images.Add("8px", Image.FromFile("line8px.jpg"));
                imageList1.Images.Add("10px", Image.FromFile("line10px.jpg"));

                ComboBox comboBox = (ComboBox)sender;
                string text = comboBox.GetItemText(comboBox.Items[e.Index]);        // lấy item cho từng mục trong combobox

                // canh chỉnh chữ và hình trong background 
                Rectangle imageRect = new Rectangle(e.Bounds.Right - 85, e.Bounds.Top, 100, 24);
                Rectangle textRect = new Rectangle(e.Bounds.Left, e.Bounds.Top, e.Bounds.Width - 20, e.Bounds.Height);

                int imageIndex = 0;    // để xuất hình theo thứ tự trong ImageList1
                switch (text)
                {
                    case "2px":
                        imageIndex = 0;
                        break;
                    case "4px":
                        imageIndex = 1;
                        break;
                    case "6px":
                        imageIndex = 2;
                        break;
                    case "8px":
                        imageIndex = 3;
                        break;
                    case "10px":
                        imageIndex = 4;
                        break;
                    default:
                        break;
                }

                // vẽ hình ảnh và chữ cho từng mục
                e.Graphics.DrawImage(imageList1.Images[imageIndex], imageRect);
                e.Graphics.DrawString(text, comboBox.Font, Brushes.Black, textRect);
            }
        }
        //----------------------------------------------------------------





        // Color----------------------------------------------------------
        private void pictureBox13_Click(object sender, EventArgs e)
        {
            PictureBox p = (PictureBox)sender; // lấy thông tin màu trong picturebox
            pen.Color = p.BackColor;   // thay đổi màu bút vẽ khi chọn màu

            if (pictureBox1 != null)
            {
                pictureBox1.BackColor = p.BackColor; // thay đổi màu trong ô pictureBox1(ô color)
            }
        }

        private void pictureBox12_Click(object sender, EventArgs e)
        {
            PictureBox p = (PictureBox)sender;
            pen.Color = p.BackColor;

            if (pictureBox1 != null)
            {
                pictureBox1.BackColor = p.BackColor;
            }
        }

        private void pictureBox11_Click(object sender, EventArgs e)
        {
            PictureBox p = (PictureBox)sender;
            pen.Color = p.BackColor;

            if (pictureBox1 != null)
            {
                pictureBox1.BackColor = p.BackColor;
            }
        }

        private void pictureBox10_Click(object sender, EventArgs e)
        {
            PictureBox p = (PictureBox)sender;
            pen.Color = p.BackColor;

            if (pictureBox1 != null)
            {
                pictureBox1.BackColor = p.BackColor;
            }
        }

        private void pictureBox9_Click(object sender, EventArgs e)
        {
            PictureBox p = (PictureBox)sender;
            pen.Color = p.BackColor;

            if (pictureBox1 != null)
            {
                pictureBox1.BackColor = p.BackColor;
            }
        }

        private void pictureBox8_Click(object sender, EventArgs e)
        {
            PictureBox p = (PictureBox)sender;
            pen.Color = p.BackColor;

            if (pictureBox1 != null)
            {
                pictureBox1.BackColor = p.BackColor;
            }
        }

        private void pictureBox7_Click(object sender, EventArgs e)
        {
            PictureBox p = (PictureBox)sender;
            pen.Color = p.BackColor;

            if (pictureBox1 != null)
            {
                pictureBox1.BackColor = p.BackColor;
            }
        }

        private void pictureBox6_Click(object sender, EventArgs e)
        {
            PictureBox p = (PictureBox)sender;
            pen.Color = p.BackColor;

            if (pictureBox1 != null)
            {
                pictureBox1.BackColor = p.BackColor;
            }
        }

        private void pictureBox5_Click(object sender, EventArgs e)
        {
            PictureBox p = (PictureBox)sender;
            pen.Color = p.BackColor;

            if (pictureBox1 != null)
            {
                pictureBox1.BackColor = p.BackColor;
            }
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            PictureBox p = (PictureBox)sender;
            pen.Color = p.BackColor;

            if (pictureBox1 != null)
            {
                pictureBox1.BackColor = p.BackColor;
            }
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            PictureBox p = (PictureBox)sender;
            pen.Color = p.BackColor;

            if (pictureBox1 != null)
            {
                pictureBox1.BackColor = p.BackColor;
            }
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            PictureBox p = (PictureBox)sender;
            pen.Color = p.BackColor;

            if (pictureBox1 != null)
            {
                pictureBox1.BackColor = p.BackColor;
            }
        }

        private void pictureBox14_Click(object sender, EventArgs e)
        {
            ColorDialog colorDialog = new ColorDialog();     // Mở một hộp thoại màu để cho người dùng chọn màu muốn tô
            if (colorDialog.ShowDialog() == DialogResult.OK)
            {
                Color selectedColor = colorDialog.Color;

                // Đặt màu cho bút vẽ
                pen.Color = selectedColor;

                // Đặt màu cho pictureBox1
                pictureBox1.BackColor = selectedColor;
            }
        }
        //----------------------------------------------------------------




        //---------------------------FILE---------------------------------
        //save------------------------------------------------------------
        private void savefile_Click(object sender, EventArgs e)
        {
            var sfd = new SaveFileDialog();
            sfd.Filter = "Image(*.jpg) |*.jpg|(*.*|*.*";
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                Bitmap btm = bm.Clone(new Rectangle(0, 0, drawPanel.Width, drawPanel.Height), bm.PixelFormat);
                btm.Save(sfd.FileName, ImageFormat.Jpeg);
            }
        }

        private void exitfile_Click(object sender, EventArgs e)
        {
            index = 10;
            Close();
        }

        private void openfile_Click(object sender, EventArgs e)
        {
            var ofd = new OpenFileDialog();
            ofd.Filter = "Image(*.jpg) |*.jpg|(*.*|*.*";
            if (ofd.ShowDialog() == DialogResult.OK)
            {

                drawPanel.Image = new Bitmap(ofd.FileName);

                // Add the new control to its parent's controls collection
                this.Controls.Add(drawPanel);
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (index == 10)
            {
                if (drawPanel != null)
                {
                    if (MessageBox.Show("Bạn có muốn lưu file", "Thông báo", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.Cancel)
                        e.Cancel = true;
                }
                else
                {
                    if (MessageBox.Show("Bạn muốn thoát chương trình", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Cancel)
                        e.Cancel = true;
                }
            }
        }

        private void Form1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.V && ModifierKeys == Keys.Control)
            {
                Bitmap bm = new Bitmap(drawPanel.Width, drawPanel.Height); ;
                // Paste hình ảnh từ clipboard vào Bitmap
                Image img = Clipboard.GetImage();
                Graphics.FromImage(bm).DrawImage(img, Point.Empty);
                drawPanel.SizeMode = PictureBoxSizeMode.StretchImage;
                drawPanel.Image = bm;
            }

        }
        //-------------------------------------------------------------------
    }
}