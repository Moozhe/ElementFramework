﻿namespace EmailWidgetTest
{
    partial class TestForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.elementContainer = new ElementFramework.ElementContainer();
            this.SuspendLayout();
            // 
            // elementContainer
            // 
            this.elementContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.elementContainer.Location = new System.Drawing.Point(0, 0);
            this.elementContainer.Name = "elementContainer";
            this.elementContainer.Size = new System.Drawing.Size(471, 384);
            this.elementContainer.TabIndex = 0;
            // 
            // TestForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(471, 384);
            this.Controls.Add(this.elementContainer);
            this.Name = "TestForm";
            this.Text = "Email Widget Test";
            this.ResumeLayout(false);

        }

        #endregion

        private ElementFramework.ElementContainer elementContainer;
    }
}

