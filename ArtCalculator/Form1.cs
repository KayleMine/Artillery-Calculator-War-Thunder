using ArtCalculator.libs;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using static ArtCalculator.libs.Win32Helper;

namespace ArtCalculator
{
    public partial class Form1 : Form
    {
        #region Defines
        private const int MOD_ALT = 0x0001;
        private const int WM_HOTKEY = 0x0312;
        private const int HOTKEY_ID = 1;
        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;


        private Point firstClick = Point.Empty; // First Point
        private Point secondClick = Point.Empty; // Second Point

        private double gridDist;
        private double targetDistance;

        private readonly Calc calculator = new Calc();
        private readonly Drawin drawer = new Drawin();
        private Keys boundHotkey;

        private int cornerRadius = 20;
        #endregion

        #region Override
        protected override void WndProc(ref Message m)
        {
            if (m.Msg == WM_HOTKEY)
            {
                int id = m.WParam.ToInt32();
                if (id == HOTKEY_ID)
                {
                    if (this.Visible) { this.Hide(); } else { this.Show(); }
                }
            }

            base.WndProc(ref m);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            GraphicsPath path = new GraphicsPath();
            path.AddArc(0, 0, cornerRadius * 2, cornerRadius * 2, 180, 90); // Top-left corner
            path.AddArc(Width - cornerRadius * 2, 0, cornerRadius * 2, cornerRadius * 2, 270, 90); // Top-right corner
            path.AddArc(Width - cornerRadius * 2, Height - cornerRadius * 2, cornerRadius * 2, cornerRadius * 2, 0, 90); // Bottom-right corner
            path.AddArc(0, Height - cornerRadius * 2, cornerRadius * 2, cornerRadius * 2, 90, 90); // Bottom-left corner
            path.CloseFigure();
            Region = new Region(path);
        }
        #endregion

        #region Keybind
        private void Keybinder_Click(object sender, EventArgs e)
        {
            KeyBinder.Text = "Press any key...";
            KeyBinder.KeyDown += Keybinder_KeyDown;
        }

        private void Keybinder_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control || e.Shift || e.Alt)
            {
                KeyBinder.Text = "Not allowed.";
                return;
            }

            U_HK(Handle, HOTKEY_ID);
            boundHotkey = e.KeyCode;
            KeyBinder.Text = boundHotkey.ToString();
            R_HK(Handle, HOTKEY_ID, MOD_ALT, (int)boundHotkey);
        }
        #endregion

        #region Draw part
        private void Screenshot_Click(object sender, EventArgs e)
        {
            int widthDivisor = int.Parse(textBox3.Text);
            int heightDivisor = int.Parse(textBox1.Text);
            int zoom = int.Parse(textBox4.Text);

            Scrnsht.CaptureAndDisplayScreenshot(DrawLayer, widthDivisor, heightDivisor, zoom);
        }

        private void DrawLayer_Click(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;

            if (firstClick == Point.Empty)
                firstClick = me.Location;
            else if (secondClick == Point.Empty)
                secondClick = me.Location;

            if (firstClick != Point.Empty && secondClick != Point.Empty)
            {
                using (Graphics g = DrawLayer.CreateGraphics())
                {
                    if (gridDist == 0)
                        DrawGrid(g);
                    else
                        DrawDistance(g);
                }
                firstClick = secondClick = Point.Empty;
            }

            UpdateViewPointStatus();
        }

        private void DrawGrid(Graphics g)
        {
            gridDist = calculator.CalculateDistance(firstClick, secondClick);
            label1.Text = "Grid Size: " + Math.Round(gridDist);
            drawer.WriteGridLine(g, firstClick, secondClick, "Grid Size");
        }

        private void DrawDistance(Graphics g)
        {
            targetDistance = calculator.CalculateDistance(firstClick, secondClick);
            string azimuth = "Zn: " + calculator.CalculateAzimuth(firstClick, secondClick).ToString("F2");
            string arrivalTime = "Time: " + (calculator.CalculateDistanceInGame(double.Parse(textBox2.Text), gridDist, targetDistance) / double.Parse(textBox5.Text)).ToString("F2") + "s";
            string distance = "Dist: " + Math.Round(calculator.CalculateDistanceInGame(double.Parse(textBox2.Text), gridDist, targetDistance));
            DataBox.Text = distance + Environment.NewLine + azimuth + Environment.NewLine + arrivalTime;
            drawer.WriteDistanceLine(g, firstClick, secondClick, distance, azimuth, arrivalTime);
        }

 
        private void ResetGrid_Click(object sender, EventArgs e)
        {
            gridDist = 0;
            label1.BackColor = Color.Red;
            label1.Text = "Grid Size: ";
            firstClick = Point.Empty;
            secondClick = Point.Empty;
            DrawLayer.Invalidate();
        }

        private void UpdateViewPointStatus()
        {
            label11.BackColor = (firstClick == Point.Empty) ? Color.Red : Color.Green;
            label11.Text = (firstClick == Point.Empty) ? "P1" : "P2";
        }

        private void ResetLines_Click(object sender, EventArgs e)
        {
            UpdateViewPointStatus();
            firstClick = Point.Empty;
            secondClick = Point.Empty;
            DrawLayer.Invalidate();
        }
        #endregion

        #region Form related
        public Form1()
        {
            InitializeComponent();
            SetFormRoundCorners();
            UpdateViewPointStatus();
            R_HK(Handle, HOTKEY_ID, MOD_ALT, (int)Keys.T);
            FormClosing += (s, e) => { U_HK(Handle, HOTKEY_ID); };
        }

        private void siticoneRoundedButton1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void SetFormRoundCorners()
        {
            FormBorderStyle = FormBorderStyle.None;
            DoubleBuffered = true;
        }

        private void TranspSlider_ValueChanged(object sender, EventArgs e)
        {
            double opacityValue = (double)TranspSlider.Value / 100;
            if (opacityValue < 0.3)
            {
                opacityValue = 0.3;
                TranspSlider.Value = (int)(opacityValue * 100);
            }
            Opacity = opacityValue;
            Invalidate();
        }
        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }
        #endregion

    }
}
