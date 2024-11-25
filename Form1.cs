namespace WinFS_DZ_9
{
    public partial class Form1 : Form
    {
        private List<Drawable> objects = new List<Drawable>();
        private Color currentColor = Color.Black;
        private Font currentFont = new Font("Arial", 12);
        private string currentAction = "Rectangle";
        private Point startPoint;
        private bool isDrawing = false;
        public Form1()
        {
            InitializeComponent();
            InitializeObjects();
            this.DoubleBuffered = true;
        }
        private void InitializeObjects()
        {
            this.Text = "Простий графічний редактор";
            this.Size = new Size(800, 600);

            var btnRectangle = new Button { Text = "Прямокутник", Location = new Point(10, 10) };
            var btnEllipse = new Button { Text = "Еліпс", Location = new Point(120, 10) };
            var btnLine = new Button { Text = "Лінія", Location = new Point(230, 10) };
            var btnText = new Button { Text = "Текст", Location = new Point(340, 10) };
            var btnColor = new Button { Text = "Колір", Location = new Point(450, 10) };
            var btnFont = new Button { Text = "Шрифт", Location = new Point(560, 10) };
            var btnClear = new Button { Text = "Очистити", Location = new Point(670, 10) };

            btnRectangle.Click += (s, e) => currentAction = "Rectangle";
            btnEllipse.Click += (s, e) => currentAction = "Ellipse";
            btnLine.Click += (s, e) => currentAction = "Line";
            btnText.Click += (s, e) => currentAction = "Text";

            btnColor.Click += (s, e) =>
            {
                using var colorDialog = new ColorDialog();
                if (colorDialog.ShowDialog() == DialogResult.OK)
                {
                    currentColor = colorDialog.Color;
                }
            };

            btnFont.Click += (s, e) =>
            {
                using var fontDialog = new FontDialog();
                if (fontDialog.ShowDialog() == DialogResult.OK)
                {
                    currentFont = fontDialog.Font;
                }
            };

            btnClear.Click += (s, e) =>
            {
                objects.Clear();
                this.Invalidate();
            };

            this.Controls.AddRange(new Control[] { btnRectangle, btnEllipse, btnLine, btnText, btnColor, btnFont, btnClear });
            this.MouseDown += Form_MouseDown;
            this.MouseUp += Form_MouseUp;
            this.MouseMove += Form_MouseMove;
            this.Paint += Form_Paint;
        }
        private void Form_MouseDown(object sender, MouseEventArgs e)
        {
            startPoint = e.Location;
            isDrawing = true;
        }
        private void Form_MouseUp(object sender, MouseEventArgs e)
        {
            if (!isDrawing) return;

            Point endPoint = e.Location;
            switch (currentAction)
            {
                case "Rectangle":
                    objects.Add(new DrawableRectangle(startPoint, endPoint, currentColor));
                    break;
                case "Ellipse":
                    objects.Add(new DrawableEllipse(startPoint, endPoint, currentColor));
                    break;
                case "Line":
                    objects.Add(new DrawableLine(startPoint, endPoint, currentColor));
                    break;
                case "Text":
                    objects.Add(new DrawableText("Текст", e.Location, currentColor, currentFont));
                    break;
            }

            isDrawing = false;
            this.Invalidate();
        }
        private void Form_MouseMove(object sender, MouseEventArgs e)
        {
            if (isDrawing && (currentAction == "Rectangle" || currentAction == "Ellipse" || currentAction == "Line"))
            {
                this.Invalidate();
            }
        }
        private void Form_Paint(object sender, PaintEventArgs e)
        {
            foreach (var obj in objects)
            {
                obj.Draw(e.Graphics);
            }
        }
    }
    public abstract class Drawable
    {
        public abstract void Draw(Graphics g);
    }
    public class DrawableRectangle : Drawable
    {
        private Rectangle rect;
        private Color color;
        public DrawableRectangle(Point start, Point end, Color color)
        {
            this.color = color;
            this.rect = new Rectangle(Math.Min(start.X, end.X), Math.Min(start.Y, end.Y),
                                      Math.Abs(start.X - end.X), Math.Abs(start.Y - end.Y));
        }
        public override void Draw(Graphics g)
        {
            using var pen = new Pen(color);
            g.DrawRectangle(pen, rect);
        }
    }
    public class DrawableEllipse : Drawable
    {
        private Rectangle rect;
        private Color color;
        public DrawableEllipse(Point start, Point end, Color color)
        {
            this.color = color;
            this.rect = new Rectangle(Math.Min(start.X, end.X), Math.Min(start.Y, end.Y),
                                      Math.Abs(start.X - end.X), Math.Abs(start.Y - end.Y));
        }
        public override void Draw(Graphics g)
        {
            using var pen = new Pen(color);
            g.DrawEllipse(pen, rect);
        }
    }
    public class DrawableLine : Drawable
    {
        private Point start;
        private Point end;
        private Color color;

        public DrawableLine(Point start, Point end, Color color)
        {
            this.start = start;
            this.end = end;
            this.color = color;
        }

        public override void Draw(Graphics g)
        {
            using var pen = new Pen(color);
            g.DrawLine(pen, start, end);
        }
    }
    public class DrawableText : Drawable
    {
        private string text;
        private Point location;
        private Color color;
        private Font font;
        public DrawableText(string text, Point location, Color color, Font font)
        {
            this.text = text;
            this.location = location;
            this.color = color;
            this.font = font;
        }
        public override void Draw(Graphics g)
        {
            using var brush = new SolidBrush(color);
            g.DrawString(text, font, brush, location);
        }
    }
}
