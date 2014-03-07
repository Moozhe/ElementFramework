namespace ElementFrameworkTest
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
            this.components = new System.ComponentModel.Container();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.testToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.test2ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.panel1 = new System.Windows.Forms.Panel();
            this.type_txt = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.name_txt = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.rotateOriginY_txt = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.rotateOriginX_txt = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.rotateAngle_txt = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.scaleY_txt = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.scaleX_txt = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.translateY_txt = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.translateX_txt = new System.Windows.Forms.TextBox();
            this.selectParent_btn = new System.Windows.Forms.Button();
            this.elementContainer = new ElementFrameworkTest.TestContainer();
            this.selectPrevious_btn = new System.Windows.Forms.Button();
            this.contextMenuStrip1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.testToolStripMenuItem,
            this.test2ToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(68, 48);
            // 
            // testToolStripMenuItem
            // 
            this.testToolStripMenuItem.Name = "testToolStripMenuItem";
            this.testToolStripMenuItem.Size = new System.Drawing.Size(67, 22);
            // 
            // test2ToolStripMenuItem
            // 
            this.test2ToolStripMenuItem.Name = "test2ToolStripMenuItem";
            this.test2ToolStripMenuItem.Size = new System.Drawing.Size(67, 22);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.selectPrevious_btn);
            this.panel1.Controls.Add(this.selectParent_btn);
            this.panel1.Controls.Add(this.type_txt);
            this.panel1.Controls.Add(this.label10);
            this.panel1.Controls.Add(this.name_txt);
            this.panel1.Controls.Add(this.label9);
            this.panel1.Controls.Add(this.label8);
            this.panel1.Controls.Add(this.rotateOriginY_txt);
            this.panel1.Controls.Add(this.label7);
            this.panel1.Controls.Add(this.label5);
            this.panel1.Controls.Add(this.rotateOriginX_txt);
            this.panel1.Controls.Add(this.label6);
            this.panel1.Controls.Add(this.rotateAngle_txt);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.scaleY_txt);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.scaleX_txt);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.translateY_txt);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.translateX_txt);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(813, 59);
            this.panel1.TabIndex = 2;
            // 
            // type_txt
            // 
            this.type_txt.Enabled = false;
            this.type_txt.Location = new System.Drawing.Point(56, 7);
            this.type_txt.Name = "type_txt";
            this.type_txt.ReadOnly = true;
            this.type_txt.Size = new System.Drawing.Size(248, 20);
            this.type_txt.TabIndex = 18;
            this.type_txt.TabStop = false;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(16, 10);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(34, 13);
            this.label10.TabIndex = 17;
            this.label10.Text = "Type:";
            // 
            // name_txt
            // 
            this.name_txt.Enabled = false;
            this.name_txt.Location = new System.Drawing.Point(56, 33);
            this.name_txt.Name = "name_txt";
            this.name_txt.ReadOnly = true;
            this.name_txt.Size = new System.Drawing.Size(200, 20);
            this.name_txt.TabIndex = 16;
            this.name_txt.TabStop = false;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(12, 35);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(38, 13);
            this.label9.TabIndex = 15;
            this.label9.Text = "Name:";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(385, 36);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(15, 13);
            this.label8.TabIndex = 14;
            this.label8.Text = "y:";
            // 
            // rotateOriginY_txt
            // 
            this.rotateOriginY_txt.Enabled = false;
            this.rotateOriginY_txt.Location = new System.Drawing.Point(402, 33);
            this.rotateOriginY_txt.Name = "rotateOriginY_txt";
            this.rotateOriginY_txt.Size = new System.Drawing.Size(27, 20);
            this.rotateOriginY_txt.TabIndex = 2;
            this.rotateOriginY_txt.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.rotateOriginY_txt.TextChanged += new System.EventHandler(this.rotateOriginY_txt_TextChanged);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(337, 36);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(15, 13);
            this.label7.TabIndex = 12;
            this.label7.Text = "x:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(262, 36);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(72, 13);
            this.label5.TabIndex = 11;
            this.label5.Text = "Rotate Origin:";
            // 
            // rotateOriginX_txt
            // 
            this.rotateOriginX_txt.Enabled = false;
            this.rotateOriginX_txt.Location = new System.Drawing.Point(354, 33);
            this.rotateOriginX_txt.Name = "rotateOriginX_txt";
            this.rotateOriginX_txt.Size = new System.Drawing.Size(27, 20);
            this.rotateOriginX_txt.TabIndex = 1;
            this.rotateOriginX_txt.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.rotateOriginX_txt.TextChanged += new System.EventHandler(this.rotateOriginX_txt_TextChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(310, 10);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(72, 13);
            this.label6.TabIndex = 9;
            this.label6.Text = "Rotate Angle:";
            // 
            // rotateAngle_txt
            // 
            this.rotateAngle_txt.Enabled = false;
            this.rotateAngle_txt.Location = new System.Drawing.Point(388, 7);
            this.rotateAngle_txt.Name = "rotateAngle_txt";
            this.rotateAngle_txt.Size = new System.Drawing.Size(41, 20);
            this.rotateAngle_txt.TabIndex = 0;
            this.rotateAngle_txt.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.rotateAngle_txt.TextChanged += new System.EventHandler(this.rotateAngle_txt_TextChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(549, 36);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(44, 13);
            this.label3.TabIndex = 7;
            this.label3.Text = "ScaleY:";
            // 
            // scaleY_txt
            // 
            this.scaleY_txt.Enabled = false;
            this.scaleY_txt.Location = new System.Drawing.Point(599, 33);
            this.scaleY_txt.Name = "scaleY_txt";
            this.scaleY_txt.Size = new System.Drawing.Size(41, 20);
            this.scaleY_txt.TabIndex = 6;
            this.scaleY_txt.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.scaleY_txt.TextChanged += new System.EventHandler(this.scaleY_txt_TextChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(549, 10);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(44, 13);
            this.label4.TabIndex = 5;
            this.label4.Text = "ScaleX:";
            // 
            // scaleX_txt
            // 
            this.scaleX_txt.Enabled = false;
            this.scaleX_txt.Location = new System.Drawing.Point(599, 7);
            this.scaleX_txt.Name = "scaleX_txt";
            this.scaleX_txt.Size = new System.Drawing.Size(41, 20);
            this.scaleX_txt.TabIndex = 5;
            this.scaleX_txt.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.scaleX_txt.TextChanged += new System.EventHandler(this.scaleX_txt_TextChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(435, 36);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(61, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "TranslateY:";
            // 
            // translateY_txt
            // 
            this.translateY_txt.Enabled = false;
            this.translateY_txt.Location = new System.Drawing.Point(502, 33);
            this.translateY_txt.Name = "translateY_txt";
            this.translateY_txt.Size = new System.Drawing.Size(41, 20);
            this.translateY_txt.TabIndex = 4;
            this.translateY_txt.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.translateY_txt.TextChanged += new System.EventHandler(this.translateY_txt_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(435, 10);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(61, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "TranslateX:";
            // 
            // translateX_txt
            // 
            this.translateX_txt.Enabled = false;
            this.translateX_txt.Location = new System.Drawing.Point(502, 7);
            this.translateX_txt.Name = "translateX_txt";
            this.translateX_txt.Size = new System.Drawing.Size(41, 20);
            this.translateX_txt.TabIndex = 3;
            this.translateX_txt.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.translateX_txt.TextChanged += new System.EventHandler(this.translateX_txt_TextChanged);
            // 
            // selectParent_btn
            // 
            this.selectParent_btn.Location = new System.Drawing.Point(646, 7);
            this.selectParent_btn.Name = "selectParent_btn";
            this.selectParent_btn.Size = new System.Drawing.Size(75, 46);
            this.selectParent_btn.TabIndex = 19;
            this.selectParent_btn.Text = "Select Parent";
            this.selectParent_btn.UseVisualStyleBackColor = true;
            this.selectParent_btn.Click += new System.EventHandler(this.selectParent_btn_Click);
            // 
            // elementContainer
            // 
            this.elementContainer.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(127)))), ((int)(((byte)(127)))));
            this.elementContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.elementContainer.Location = new System.Drawing.Point(0, 59);
            this.elementContainer.Name = "elementContainer";
            this.elementContainer.Size = new System.Drawing.Size(813, 459);
            this.elementContainer.TabIndex = 1;
            this.elementContainer.SizeChanged += new System.EventHandler(this.testContainer_SizeChanged);
            this.elementContainer.MouseMove += new System.Windows.Forms.MouseEventHandler(this.testContainer_MouseMove);
            this.elementContainer.MouseUp += new System.Windows.Forms.MouseEventHandler(this.testContainer_MouseUp);
            // 
            // selectPrevious_btn
            // 
            this.selectPrevious_btn.Location = new System.Drawing.Point(727, 7);
            this.selectPrevious_btn.Name = "selectPrevious_btn";
            this.selectPrevious_btn.Size = new System.Drawing.Size(75, 46);
            this.selectPrevious_btn.TabIndex = 20;
            this.selectPrevious_btn.Text = "Select Previous";
            this.selectPrevious_btn.UseVisualStyleBackColor = true;
            this.selectPrevious_btn.Click += new System.EventHandler(this.selectPrevious_btn_Click);
            // 
            // TestForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(813, 518);
            this.Controls.Add(this.elementContainer);
            this.Controls.Add(this.panel1);
            this.Name = "TestForm";
            this.Text = "TestForm";
            this.contextMenuStrip1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem testToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem test2ToolStripMenuItem;
        private TestContainer elementContainer;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox scaleY_txt;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox scaleX_txt;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox translateY_txt;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox translateX_txt;
        private System.Windows.Forms.TextBox type_txt;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox name_txt;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox rotateOriginY_txt;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox rotateOriginX_txt;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox rotateAngle_txt;
        private System.Windows.Forms.Button selectParent_btn;
        private System.Windows.Forms.Button selectPrevious_btn;
    }
}