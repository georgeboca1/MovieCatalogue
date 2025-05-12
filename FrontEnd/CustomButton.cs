using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace FancyControls
{
    public class IconPickerButton : Button
    {
        private Image _buttonIcon;
        private int _iconPadding = 5; // Padding around the icon
        private Color _hoverBackColor = Color.LightGray;
        private Color _pressedBackColor = Color.DarkGray;
        private Color _disabledBackColor = Color.Gainsboro;
        private Color _borderColor = Color.Gray;
        private int _borderThickness = 1;

        private ButtonState _currentState = ButtonState.Normal;

        // Enum to manage visual state for custom drawing
        private enum ButtonState
        {
            Normal,
            Hover,
            Pressed,
            Disabled
        }

        #region Properties

        [Category("Appearance")]
        [Description("The icon to display on the button.")]
        public Image ButtonIcon
        {
            get { return _buttonIcon; }
            set
            {
                _buttonIcon = value;
                this.Invalidate(); // Redraw the button when icon changes
            }
        }

        [Category("Appearance")]
        [Description("Padding around the icon within the button.")]
        [DefaultValue(5)]
        public int IconPadding
        {
            get { return _iconPadding; }
            set
            {
                _iconPadding = Math.Max(0, value); // Ensure non-negative
                this.Invalidate();
            }
        }

        [Category("Appearance")]
        [Description("Background color when the mouse hovers over the button.")]
        [DefaultValue(typeof(Color), "LightGray")]
        public Color HoverBackColor
        {
            get { return _hoverBackColor; }
            set { _hoverBackColor = value; this.Invalidate(); }
        }

        [Category("Appearance")]
        [Description("Background color when the button is pressed.")]
        [DefaultValue(typeof(Color), "DarkGray")]
        public Color PressedBackColor
        {
            get { return _pressedBackColor; }
            set { _pressedBackColor = value; this.Invalidate(); }
        }

        [Category("Appearance")]
        [Description("Background color when the button is disabled.")]
        [DefaultValue(typeof(Color), "Gainsboro")]
        public Color DisabledBackColor
        {
            get { return _disabledBackColor; }
            set { _disabledBackColor = value; this.Invalidate(); }
        }

        [Category("Appearance")]
        [Description("Color of the button's border.")]
        [DefaultValue(typeof(Color), "Gray")]
        public Color ButtonBorderColor // Renamed to avoid conflict
        {
            get { return _borderColor; }
            set { _borderColor = value; this.Invalidate(); }
        }

        [Category("Appearance")]
        [Description("Thickness of the button's border.")]
        [DefaultValue(1)]
        public int BorderThickness
        {
            get { return _borderThickness; }
            set { _borderThickness = Math.Max(0, value); this.Invalidate(); }
        }


        // Hide the original Text property from the designer and code completion
        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public override string Text
        {
            get { return base.Text; }
            set { base.Text = string.Empty; } // Always ensure text is empty
        }

        // The issue is that 'FlatAppearance' is not a standalone type but a property of the Button class in WinForms.  
        // The correct type is 'FlatButtonAppearance', which is part of the System.Windows.Forms namespace.  
        // The following fix updates the property type to resolve the error.  

        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public new FlatButtonAppearance FlatAppearance => base.FlatAppearance;

        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public new FlatStyle FlatStyle
        {
            get => base.FlatStyle;
            set => base.FlatStyle = value; // Can still set it, but UserPaint will override
        }


        #endregion

        public IconPickerButton()
        {
            // Use base.Text to set it, as our override prevents direct setting
            base.Text = string.Empty;
            this.Size = new Size(40, 40); // Default size suitable for an icon

            // Enable custom painting
            this.SetStyle(ControlStyles.UserPaint |
                          ControlStyles.AllPaintingInWmPaint |
                          ControlStyles.OptimizedDoubleBuffer |
                          ControlStyles.ResizeRedraw |
                          ControlStyles.SupportsTransparentBackColor, true);
            this.UpdateStyles();
        }

        protected override void OnPaint(PaintEventArgs pevent)
        {
            // Do not call base.OnPaint(pevent) if you want full control
            // base.OnPaint(pevent);

            Graphics g = pevent.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;

            Rectangle clientRect = this.ClientRectangle;
            Color currentBackColor = this.BackColor; // Default

            // Determine current state for drawing
            if (!this.Enabled)
            {
                _currentState = ButtonState.Disabled;
                currentBackColor = _disabledBackColor;
            }
            // Note: OnMouseDown/Up handles the Pressed state logic more accurately
            // So _currentState should already be set correctly by mouse events.

            switch (_currentState)
            {
                case ButtonState.Hover:
                    currentBackColor = _hoverBackColor;
                    break;
                case ButtonState.Pressed:
                    currentBackColor = _pressedBackColor;
                    break;
                case ButtonState.Disabled:
                    currentBackColor = _disabledBackColor;
                    break;
                case ButtonState.Normal:
                default:
                    currentBackColor = this.BackColor;
                    break;
            }


            // 1. Draw Background
            using (SolidBrush backBrush = new SolidBrush(currentBackColor))
            {
                g.FillRectangle(backBrush, clientRect);
            }

            // 2. Draw Border
            if (_borderThickness > 0)
            {
                using (Pen borderPen = new Pen(_borderColor, _borderThickness))
                {
                    // Adjust rectangle for border drawing to be inside
                    Rectangle borderRect = new Rectangle(clientRect.X, clientRect.Y,
                                                         clientRect.Width - _borderThickness,
                                                         clientRect.Height - _borderThickness);
                    g.DrawRectangle(borderPen, borderRect);
                }
            }

            // 3. Draw Icon
            if (_buttonIcon != null)
            {
                // Calculate icon drawing area, considering padding
                int paddedSize = _iconPadding * 2;
                int availableWidth = Math.Max(0, clientRect.Width - paddedSize - (_borderThickness * 2));
                int availableHeight = Math.Max(0, clientRect.Height - paddedSize - (_borderThickness * 2));

                if (availableWidth > 0 && availableHeight > 0)
                {
                    Rectangle iconRect = new Rectangle(
                        _iconPadding + _borderThickness,
                        _iconPadding + _borderThickness,
                        availableWidth,
                        availableHeight);

                    // Maintain aspect ratio
                    float Hratio = (float)iconRect.Height / _buttonIcon.Height;
                    float Wratio = (float)iconRect.Width / _buttonIcon.Width;
                    float scale = Math.Min(Hratio, Wratio); // Use the smaller ratio to fit

                    int newWidth = (int)(_buttonIcon.Width * scale);
                    int newHeight = (int)(_buttonIcon.Height * scale);

                    // Center the scaled icon within the iconRect
                    int dX = (iconRect.Width - newWidth) / 2;
                    int dY = (iconRect.Height - newHeight) / 2;

                    Rectangle destRect = new Rectangle(
                        iconRect.Left + dX,
                        iconRect.Top + dY,
                        newWidth,
                        newHeight);

                    if (this.Enabled)
                    {
                        g.DrawImage(_buttonIcon, destRect);
                    }
                    else
                    {
                        // Draw disabled version of the icon
                        // ControlPaint.DrawImageDisabled is simple. For more control, you might use a ColorMatrix.
                        ControlPaint.DrawImageDisabled(g, _buttonIcon, destRect.X, destRect.Y, currentBackColor);
                    }
                }
            }

            // 4. Draw Focus Rectangle (optional, but good for accessibility)
            if (this.Focused && this.ShowFocusCues && this.Enabled)
            {
                Rectangle focusRect = clientRect;
                focusRect.Inflate(-2 - _borderThickness, -2 - _borderThickness); // Ensure it's inside border and padding
                ControlPaint.DrawFocusRectangle(g, focusRect);
            }
        }

        protected override void OnMouseEnter(EventArgs e)
        {
            base.OnMouseEnter(e);
            if (this.Enabled)
            {
                _currentState = ButtonState.Hover;
                this.Invalidate();
            }
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            if (this.Enabled)
            {
                _currentState = ButtonState.Normal;
                this.Invalidate();
            }
        }

        protected override void OnMouseDown(MouseEventArgs mevent)
        {
            base.OnMouseDown(mevent);
            if (mevent.Button == MouseButtons.Left && this.Enabled)
            {
                _currentState = ButtonState.Pressed;
                this.Invalidate();
            }
        }

        protected override void OnMouseUp(MouseEventArgs mevent)
        {
            base.OnMouseUp(mevent);
            if (mevent.Button == MouseButtons.Left && this.Enabled)
            {
                // If mouse is still over the button, transition to Hover, else Normal
                _currentState = this.ClientRectangle.Contains(mevent.Location) ? ButtonState.Hover : ButtonState.Normal;
                this.Invalidate();
            }
        }

        protected override void OnEnabledChanged(EventArgs e)
        {
            base.OnEnabledChanged(e);
            _currentState = this.Enabled ? ButtonState.Normal : ButtonState.Disabled;
            this.Invalidate();
        }

        // Optional: Override IsInputKey to handle Enter/Space for clicks if needed
        // protected override bool IsInputKey(Keys keyData)
        // {
        //     if (keyData == Keys.Enter || keyData == Keys.Space) return true;
        //     return base.IsInputKey(keyData);
        // }

        // Optional: Override OnKeyDown/OnKeyUp for visual feedback on keyboard interaction
        // protected override void OnKeyDown(KeyEventArgs kevent)
        // {
        //     base.OnKeyDown(kevent);
        //     if ((kevent.KeyCode == Keys.Enter || kevent.KeyCode == Keys.Space) && this.Enabled)
        //     {
        //         _currentState = ButtonState.Pressed;
        //         Invalidate();
        //     }
        // }
        // protected override void OnKeyUp(KeyEventArgs kevent)
        // {
        //    base.OnKeyUp(kevent);
        //    if ((kevent.KeyCode == Keys.Enter || kevent.KeyCode == Keys.Space) && this.Enabled)
        //    {
        //        _currentState = ButtonState.Normal; // Or Hover if focused
        //        Invalidate();
        //        // PerformClick(); // Already handled by base Button
        //    }
        // }
    }
}