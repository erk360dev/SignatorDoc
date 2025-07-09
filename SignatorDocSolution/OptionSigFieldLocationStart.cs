using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace SignatorDocSolution
{
    /// <summary>
    /// CLASS_ID=08;
    /// </summary>
    class OptionSigFieldLocationStart : Panel
    {
        private Button btn_StartTopLeft;
        private Button btn_StartTopCenter;
        private Button btn_StartTopRight;
        private Button btn_StartMiddleLeft;
        private Button btn_StartMiddleCenter;
        private Button btn_StartMiddleRight;
        private Button btn_StartBottomLeft;
        private Button btn_StartBottomCenter;
        private Button btn_StartBottomRight;

        private System.Drawing.Size sizeOptions = new System.Drawing.Size(40, 40);

        public OptionSigFieldLocationStart() {
 
            this.btn_StartTopLeft = new Button();
            this.btn_StartTopLeft.Size = this.sizeOptions;
            this.btn_StartTopLeft.Location = new System.Drawing.Point(1, 1);
            this.btn_StartTopLeft.Text = string.Empty;
            this.btn_StartTopLeft.Name = "btn_StartTopLeft";

            this.btn_StartTopCenter = new Button();
            this.btn_StartTopCenter.Size = this.sizeOptions;
            this.btn_StartTopCenter.Location = new System.Drawing.Point(this.btn_StartTopLeft.Location.X + this.btn_StartTopLeft.Size.Width, this.btn_StartTopLeft.Location.Y);
            this.btn_StartTopCenter.Text = string.Empty;
            this.btn_StartTopCenter.Name = "btn_StartTopCenter";

            this.btn_StartTopRight = new Button();
            this.btn_StartTopRight.Size = this.sizeOptions;
            this.btn_StartTopRight.Location = new System.Drawing.Point(this.btn_StartTopCenter.Location.X + this.btn_StartTopCenter.Size.Width, this.btn_StartTopCenter.Location.Y );
            this.btn_StartTopRight.Text = string.Empty;
            this.btn_StartTopRight.Name = "btn_StartTopRight";

            this.btn_StartMiddleLeft = new Button();
            this.btn_StartMiddleLeft.Size = this.sizeOptions;
            this.btn_StartMiddleLeft.Location = new System.Drawing.Point(this.btn_StartTopLeft.Location.X , this.btn_StartTopRight.Location.Y + this.btn_StartTopRight.Size.Height);
            this.btn_StartMiddleLeft.Text = string.Empty;
            this.btn_StartMiddleLeft.Name = "btn_StartMiddleLeft";

            this.btn_StartMiddleCenter = new Button();
            this.btn_StartMiddleCenter.Size = this.sizeOptions;
            this.btn_StartMiddleCenter.Location = new System.Drawing.Point(this.btn_StartMiddleLeft.Location.X + this.btn_StartMiddleLeft.Size.Width, this.btn_StartMiddleLeft.Location.Y);
            this.btn_StartMiddleCenter.Text = string.Empty;
            this.btn_StartMiddleCenter.Focus();
            this.btn_StartMiddleCenter.Name = "btn_StartMiddleCenter";

            this.btn_StartMiddleRight = new Button();
            this.btn_StartMiddleRight.Size = this.sizeOptions;
            this.btn_StartMiddleRight.Location = new System.Drawing.Point(this.btn_StartMiddleCenter.Location.X + this.btn_StartMiddleCenter.Size.Width, this.btn_StartMiddleCenter.Location.Y);
            this.btn_StartMiddleRight.Text = string.Empty;
            this.btn_StartMiddleRight.Name = "btn_StartMiddleRight";

            this.btn_StartBottomLeft = new Button();
            this.btn_StartBottomLeft.Size = this.sizeOptions;
            this.btn_StartBottomLeft.Location = new System.Drawing.Point(this.btn_StartTopLeft.Location.X, this.btn_StartMiddleRight.Location.Y + this.btn_StartMiddleRight.Size.Height);
            this.btn_StartBottomLeft.Text = string.Empty;
            this.btn_StartBottomLeft.Name = "btn_StartBottomLeft";

            this.btn_StartBottomCenter = new Button();
            this.btn_StartBottomCenter.Size = this.sizeOptions;
            this.btn_StartBottomCenter.Location = new System.Drawing.Point(this.btn_StartTopLeft.Location.X + this.btn_StartTopLeft.Size.Width, this.btn_StartBottomLeft.Location.Y);
            this.btn_StartBottomCenter.Text = string.Empty;
            this.btn_StartBottomCenter.Name = "btn_StartBottomCenter";

            this.btn_StartBottomRight = new Button();
            this.btn_StartBottomRight.Size = this.sizeOptions;
            this.btn_StartBottomRight.Location = new System.Drawing.Point(this.btn_StartBottomCenter.Location.X + this.btn_StartBottomCenter.Size.Width, this.btn_StartBottomLeft.Location.Y);
            this.btn_StartBottomRight.Text = string.Empty;
            this.btn_StartBottomRight.Name = "btn_StartBottomRight";

            this.Size = new System.Drawing.Size((3 * this.sizeOptions.Width)+3, (3 * this.sizeOptions.Height)+3);
            this.BorderStyle= System.Windows.Forms.BorderStyle.FixedSingle;

            this.Controls.Add(this.btn_StartMiddleCenter);

            this.Controls.Add(this.btn_StartTopLeft);
            this.Controls.Add(this.btn_StartTopCenter);
            this.Controls.Add(this.btn_StartTopRight);

            this.Controls.Add(this.btn_StartMiddleLeft);       
            this.Controls.Add(this.btn_StartMiddleRight);

            this.Controls.Add(this.btn_StartBottomLeft);
            this.Controls.Add(this.btn_StartBottomCenter);
            this.Controls.Add(this.btn_StartBottomRight);
            this.btn_StartMiddleCenter.Focus();

        }

        public void setLocation(System.Drawing.Point location) {
            this.Location = location;
        }

        public void setClickEvent(EventHandler event_Handler) {
            this.btn_StartTopLeft.Click += event_Handler;
            this.btn_StartTopCenter.Click += event_Handler;
            this.btn_StartTopRight.Click += event_Handler;
            this.btn_StartMiddleLeft.Click += event_Handler;
            this.btn_StartMiddleCenter.Click += event_Handler;
            this.btn_StartMiddleRight.Click += event_Handler;
            this.btn_StartBottomLeft.Click += event_Handler;
            this.btn_StartBottomCenter.Click += event_Handler;
            this.btn_StartBottomRight.Click += event_Handler;
        }


    }
}
