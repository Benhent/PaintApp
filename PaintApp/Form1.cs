using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using static System.Windows.Forms.DataFormats;

namespace PaintApp
{
    public partial class Form1 : Form
    {
        Bitmap bm = new Bitmap(2000, 1000);
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
            this.Width = 1500;
            this.Height = 830;
            bm = new Bitmap(drawPanel.Width, drawPanel.Height);
            g = Graphics.FromImage(bm);
            g.Clear(Color.White);
            g = drawPanel.CreateGraphics();
            drawPanel.Image = bm;


            // làm mượt nét vẽ
            g.SmoothingMode = SmoothingMode.AntiAlias;

            // làm cho phân đuôi nét vẽ được bo tròn lại
            pen.StartCap = pen.EndCap = LineCap.Round;   // bút
            era.StartCap = era.EndCap = LineCap.Round;   // tẩy
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

            using (g = Graphics.FromImage(bm))
            {
                if (index == 10)     // hình tròn
                {
                    g.DrawEllipse(pen, cx, cy, sx, sy);
                }
                if (index == 11)     // hình chữ nhật
                {
                    g.DrawRectangle(pen, cx, cy, sx, sy);
                }
                if (index == 12)     // đường thẳng
                {
                    g.DrawLine(pen, cx, cy, x, y);
                }
                if (index == 13)    // hình tam giác
                {
                    // tính chiều cao của tam giác
                    int height = (int)(Math.Sqrt(3) / 2 * (x - cx));

                    // xác định các đỉnh của tam giác
                    Point vertex1 = new Point(cx, cy + height);
                    Point vertex2 = new Point(cx + (x - cx), cy + height);
                    Point vertex3 = new Point(cx + (x - cx) / 2, cy);

                    // vẽ
                    g.DrawPolygon(pen, new Point[] { vertex1, vertex2, vertex3 });
                }
            }

            drawPanel.Invalidate();

        }

        private void drawPanel_MouseMove(object sender, MouseEventArgs e)
        {
            if (drawing)
            {
                using (Graphics g = Graphics.FromImage(bm))
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
                drawPanel.Invalidate();
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
                case 10:
                    this.Cursor = Cursors.Cross;
                    break;
                case 11:
                    this.Cursor = Cursors.Cross;
                    break;
                case 12:
                    this.Cursor = Cursors.Cross;
                    break;
                case 13:
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
                if (index == 10)
                {
                    g.DrawEllipse(pen, cx, cy, sx, sy);
                }
                if (index == 11)
                {
                    g.DrawRectangle(pen, cx, cy, sx, sy);
                }
                if (index == 12)
                {
                    g.DrawLine(pen, cx, cy, x, y);
                }
                if (index == 13)
                {
                    int height = (int)(Math.Sqrt(3) / 2 * (x - cx));
                    Point vertex1 = new Point(cx, cy + height);
                    Point vertex2 = new Point(cx + (x - cx), cy + height);
                    Point vertex3 = new Point(cx + (x - cx) / 2, cy);
                    g.DrawPolygon(pen, new Point[] { vertex1, vertex2, vertex3 });
                }
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

        private void drawPanel_MouseClickfortools(object sender, MouseEventArgs e) // chức năng cho tool
        {
            if (currentTool == SelectedTool.Bucket)         // bucket 
            {
                Color targetColor = GetPixelColor((int)e.X, (int)e.Y);
                FloodFill(bm, (int)e.X, (int)e.Y, targetColor, pen.Color);

                drawPanel.Invalidate();
            }
            if (currentTool == SelectedTool.ColorPicker)    // color picker
            {
                // Get the color of the pixel at the clicked position
                Color pickedColor = GetPixelColor((int)e.X, (int)e.Y);

                // Set the picked color as the current drawing color
                pen.Color = pickedColor;

                // Update the color in the color picker tool
                pictureBox1.BackColor = pickedColor;
            }
            if (currentTool == SelectedTool.Text)
            {
                using (Graphics g = Graphics.FromImage(bm))
                {
                    // Prompt the user for text input
                    string userInput = Microsoft.VisualBasic.Interaction.InputBox("Enter text:", "Text Tool", "");

                    if (!string.IsNullOrEmpty(userInput))
                    {
                        Font font = new Font("Arial", 16);
                        SolidBrush brush = new SolidBrush(pen.Color);
                        StringFormat drawFormat = new StringFormat();

                        // Draw the text at the clicked position
                        g.DrawString(userInput, font, brush, e.Location, drawFormat);

                        drawPanel.Invalidate();
                    }
                }
            }
        }

        private Color GetPixelColor(int x, int y)                   // kiểm tra màu hiện tại
        {
            if (x >= 0 && x < bm.Width && y >= 0 && y < bm.Height)
            {
                return bm.GetPixel(x, y);
            }

            return Color.Empty;
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

            drawPanel.MouseClick += drawPanel_MouseClickfortools;
        }

        private void FloodFill(Bitmap bmp, int x, int y, Color targetColor, Color replacementColor) // kiểm tra vùng tô màu và tô màu
        {
            Stack<(int, int)> stack = new Stack<(int, int)>();
            stack.Push((x, y));

            while (stack.Count > 0)
            {
                (int currentX, int currentY) = stack.Pop();

                if (currentX >= 0 && currentX < bmp.Width && currentY >= 0 && currentY < bmp.Height)
                {
                    if (bmp.GetPixel(currentX, currentY) == targetColor)
                    {
                        bmp.SetPixel(currentX, currentY, replacementColor);

                        stack.Push((currentX - 1, currentY));
                        stack.Push((currentX + 1, currentY));
                        stack.Push((currentX, currentY - 1));
                        stack.Push((currentX, currentY + 1));
                    }
                }
            }
        }

        private void tool_picker_Click(object sender, EventArgs e)
        {
            SetCurrentTool(SelectedTool.ColorPicker);
            ResetPictureBoxColors();
            PictureBox pictureBox = (PictureBox)sender;
            pictureBox.BackColor = Color.LightCyan;
            pictureBox.BorderStyle = BorderStyle.FixedSingle;

            drawPanel.MouseClick += drawPanel_MouseClickfortools;
        }

        private void tool_clear_Click(object sender, EventArgs e)
        {
            SetCurrentTool(SelectedTool.Clear);
            ResetPictureBoxColors();
            PictureBox pictureBox = (PictureBox)sender;
            pictureBox.BackColor = Color.LightCyan;
            pictureBox.BorderStyle = BorderStyle.FixedSingle;

            using (Graphics g = Graphics.FromImage(bm))
            {
                g.Clear(Color.White);
            }

            drawPanel.Invalidate();
        }

        private void tool_text_Click(object sender, EventArgs e)
        {
            SetCurrentTool(SelectedTool.Text);
            ResetPictureBoxColors();
            PictureBox pictureBox = (PictureBox)sender;
            pictureBox.BackColor = Color.LightCyan;
            pictureBox.BorderStyle = BorderStyle.FixedSingle;

            drawPanel.MouseClick -= drawPanel_MouseClickfortools;

        }
        //----------------------------------------------------------------





        // shape----------------------------------------------------------
        private void sh_ht_Click(object sender, EventArgs e)    // hình tròn
        {
            index = 10;
            ResetPictureBoxColors();
        }

        private void sh_hcn_Click(object sender, EventArgs e)   // hình chữ nhật
        {
            index = 11;
            ResetPictureBoxColors();
        }

        private void sh_line_Click(object sender, EventArgs e)  // đường thẳng
        {
            index = 12;
            ResetPictureBoxColors();
        }
        private void sh_htg_Click(object sender, EventArgs e)   // hình tam giác
        {
            index = 13;
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
        private void savefile_Click(object sender, EventArgs e)   // save file
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Jpeg Image|*.jpg|Bitmap Image *.bmp|";
            saveFileDialog.Title = "Save an Image File";

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                string fileName = saveFileDialog.FileName;

                using (FileStream fs = new FileStream(fileName, FileMode.Create))
                {
                    switch (saveFileDialog.FilterIndex)
                    {
                        case 1:
                            bm.Save(fs, ImageFormat.Jpeg);
                            break;
                        case 2:
                            bm.Save(fs, ImageFormat.Bmp);
                            break;
                    }
                }
            }
        }

        private void exitfile_Click(object sender, EventArgs e)     // exit  
        {
            if (bm != null)
            {
                DialogResult result = MessageBox.Show("bạn có muốn lưu lại file trước khi thoát?", "Thoát", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    // lưu trước khi thoát
                    SaveFileDialog saveFileDialog = new SaveFileDialog();
                    saveFileDialog.Filter = "Jpeg Image|*.jpg|Bitmap Image *.bmp|";
                    saveFileDialog.Title = "Lưu file trước khi thoát";

                    if (saveFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        string fileName = saveFileDialog.FileName;

                        using (FileStream fs = new FileStream(fileName, FileMode.Create))
                        {
                            switch (saveFileDialog.FilterIndex)
                            {
                                case 1:
                                    bm.Save(fs, ImageFormat.Jpeg);
                                    break;
                                case 2:
                                    bm.Save(fs, ImageFormat.Bmp);
                                    break;
                            }
                        }
                    }
                }
                else if (result == DialogResult.No)
                {
                    // chọn không lưu thì tiến hành thoát
                    Close();
                }
                // nếu bấm cancle thì không làm gì cả
            }
            else
            {
                // không có gì thì sẽ tự thoát
                Close();
            }
        }

        private void openfile_Click(object sender, EventArgs e)     // open file
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Image Files (*.bmp;*.jpg;*.png)|*.bmp;*.jpg;*.png|All files (*.*)|*.*";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    // load hình ảnh được chọn vào drawpanel
                    try
                    {
                        Bitmap loadedImage = new Bitmap(openFileDialog.FileName);
                        bm = new Bitmap(loadedImage);
                        g = Graphics.FromImage(bm);
                        drawPanel.Image = bm;
                        g.SmoothingMode = SmoothingMode.AntiAlias;
                        drawPanel.Invalidate();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error loading the image: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void newfile_Click(object sender, EventArgs e)      // new file
        {
            if (bm != null)
            {
                DialogResult result = MessageBox.Show("Bạn có muốn lưu những thay đổi của bạn?", "Save", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    // lưu file trước khi tạo file mới
                    SaveFileDialog saveFileDialog = new SaveFileDialog();
                    saveFileDialog.Filter = "Jpeg Image|*.jpg|Bitmap Image *.bmp|";
                    saveFileDialog.Title = "Lưu lại tiến độ công việc của bạn";

                    if (saveFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        string fileName = saveFileDialog.FileName;

                        using (FileStream fs = new FileStream(fileName, FileMode.Create))
                        {
                            switch (saveFileDialog.FilterIndex)
                            {
                                case 1:
                                    bm.Save(fs, ImageFormat.Jpeg);
                                    break;
                                case 2:
                                    bm.Save(fs, ImageFormat.Bmp);
                                    break;
                            }
                        }
                    }
                }
                else if (result == DialogResult.Cancel)
                {
                    // Nếu chọn cancle thì không làm gì cả
                    return;
                }
            }

            // tạo bitmap mới và xóa bitmap hiện tại
            bm = new Bitmap(drawPanel.Width, drawPanel.Height);
            g = Graphics.FromImage(bm);
            g.Clear(Color.White);
            drawPanel.Image = bm;
        }
        //-------------------------------------------------------------------
    }
}