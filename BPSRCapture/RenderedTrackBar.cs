using System.ComponentModel;
using System.Windows.Forms.VisualStyles;

namespace BPSRCapture
{
    [DesignerCategory("Code")]
    partial class RenderedTrackBar : Control
    {
        private int _currentValue = 50;
        private const int ThumbWidth = 10;

        public RenderedTrackBar()
        {
            SetStyle(
                ControlStyles.AllPaintingInWmPaint |
                ControlStyles.UserPaint |
                ControlStyles.DoubleBuffer |
                ControlStyles.ResizeRedraw,
                true);
        }

        [Category("Behavior"), DefaultValue(50), Description("現在の値")]
        public int CurrentValue
        {
            get => _currentValue;
            set
            {
                int newValue = Math.Max(Minimum, Math.Min(Maximum, value));
                if (_currentValue != newValue)
                {
                    int oldValue = _currentValue;
                    _currentValue = newValue;
                    Invalidate();
                    ValueChanged?.Invoke(this, new ValueChangeEventArgs(oldValue, newValue));
                }
            }
        }

        [Category("Behavior"), DefaultValue(0)]
        public int Minimum { get; set; } = 0;

        [Category("Behavior"), DefaultValue(100)]
        public int Maximum { get; set; } = 100;

        [Category("Behavior"), DefaultValue(10)]
        public int TickFrequency { get; set; } = 10;

        // TickStyle を EdgeStyle に変換（デザイナー対応）
        private EdgeStyle EdgeStyle
        {
            get
            {
                return TickStyle switch
                {
                    TickStyle.TopLeft => EdgeStyle.Sunken,
                    TickStyle.BottomRight => EdgeStyle.Sunken,
                    TickStyle.Both => EdgeStyle.Sunken,
                    _ => EdgeStyle.Sunken
                };
            }
        }

        // デザイナー用に TickStyle を公開（シリアル化可能）
        [Category("Appearance"), DefaultValue(TickStyle.BottomRight)]
        public TickStyle TickStyle { get; set; } = TickStyle.BottomRight;

        [Category("Behavior")]
        public event ValueChangeEventHandler ValueChanged;

        protected override void OnPaint(PaintEventArgs e)
        {
            if (!TrackBarRenderer.IsSupported)
            {
                base.OnPaint(e);
                return;
            }

            Graphics g = e.Graphics;

            // トラック
            Rectangle trackRect = new Rectangle(ThumbWidth / 2, Height / 2 - 2, Width - ThumbWidth, 4);
            TrackBarRenderer.DrawHorizontalTrack(g, trackRect);

            // つまみ
            Rectangle thumbRect = CalculateThumbRect();
            var state = Enabled ? TrackBarThumbState.Normal : TrackBarThumbState.Disabled;
            TrackBarRenderer.DrawHorizontalThumb(g, thumbRect, state);

            // 目盛りの描画（正しいオーバーロード使用）
            if (TickStyle != TickStyle.None && Maximum > Minimum)
            {
                int numTicks = (Maximum - Minimum) / TickFrequency + 1;
                Rectangle ticksRect = new Rectangle(0, 0, Width, Height);
                TrackBarRenderer.DrawHorizontalTicks(g, ticksRect, numTicks, EdgeStyle);
            }
        }

        private Rectangle CalculateThumbRect()
        {
            int range = Maximum - Minimum;
            int pos = range > 0 ? (int)((double)(CurrentValue - Minimum) / range * (Width - ThumbWidth)) : 0;
            return new Rectangle(pos, 0, ThumbWidth, Height);
        }

        // マウス操作
        protected override void OnMouseDown(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                UpdateValueFromPoint(e.Location);
                Capture = true;
            }
            base.OnMouseDown(e);
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (Capture)
            {
                UpdateValueFromPoint(e.Location);
            }
            base.OnMouseMove(e);
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            Capture = false;
            base.OnMouseUp(e);
        }

        private void UpdateValueFromPoint(Point pt)
        {
            int range = Maximum - Minimum;
            int newValue = range > 0
                ? Minimum + (int)((double)(pt.X - ThumbWidth / 2) / (Width - ThumbWidth) * range)
                : Minimum;
            CurrentValue = Math.Max(Minimum, Math.Min(Maximum, newValue));
        }
    }
}